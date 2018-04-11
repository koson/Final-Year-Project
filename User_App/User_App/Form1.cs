using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

namespace User_App
{
    public partial class MainForm : Form
    {
        [XmlArray("Chambers")]
        [XmlArrayItem("Chamber")]
        private Chamber[] chambers;

        private ChamberForm chamberForm;
        private SensorForm sensorForm;
        private DeleteForm deleteForm;

        private int liveChartRange = 1;
        private System.Timers.Timer liveChartTimer;
        public MainForm()
        {
            InitializeComponent();
            liveChartTimer = new System.Timers.Timer
            {
                Interval = 10000
            };
            UpdateEnvironment();
        }

        private void UpdateEnvironment()
        {
            DeserialiseProcessorOutput(CallProcessor("getEnv"), "getEnv");
            liveChartPicker.DisplayMember = "Text";
            liveChartPicker.ValueMember = "Value";
            customChartPicker.DisplayMember = "Text";
            customChartPicker.ValueMember = "Value";

            List<Object> items = new List<Object>();
            for (int i = 0; i < chambers.Length; i++)
            {
                items.Add(new { Text = chambers[i].Name, Value = chambers[i] });
            }

            liveChartPicker.DataSource = items;
            customChartPicker.DataSource = items;
        }
        private void ChildForm_Disposed(object sender, EventArgs e) //change from disposed to disposed after successful submit
        {
            Task update = new Task(() => UpdateEnvironment());
            update.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            liveChartPicker.DisplayMember = "Text";
            liveChartPicker.ValueMember = "Value";
            customChartPicker.DisplayMember = "Text";
            customChartPicker.ValueMember = "Value";

            List<Object> items = new List<Object>();
            for (int i = 0; i < chambers.Length; i++)
            {
                items.Add(new { Text = chambers[i].Name, Value = chambers[i] });
            }

            liveChartPicker.DataSource = items;
            customChartPicker.DataSource = items;
            liveChartTimer.Start();
            liveChartTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            DataSet[] customChartData = null;
            Chamber c = (Chamber)GetCustomChartPickerValue();
            DateTime startDate = GetStartDatePickerValue();
            DateTime endDate = GetEndDatePickerValue();
            Boolean averageValues = GetCustomChartAverage();

            String args = "produceGraph " + c.ID + " \"" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" \"" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" " + averageValues.ToString().ToLower() + " false";
            Thread customChartThread = new Thread(new ThreadStart(UpdateCustomChart));
            customChartThread.Start();
        }

        private void UpdateCustomChart()
        {
            DataSet[] customChartData = null;
            Chamber c = (Chamber)GetCustomChartPickerValue();
            DateTime startDate = GetStartDatePickerValue();
            DateTime endDate = GetEndDatePickerValue();
            Boolean averageValues = GetCustomChartAverage();

            String args = "produceGraph " + c.ID + " \"" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" \"" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" " + averageValues + " false";
            customChartData = DeserialiseProcessorOutput(CallProcessor(args));
            if (customChartData != null)
            {
                SetCustomChart(customChartData);
            }
        }

        private void chamberPickerBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshLiveChart();
        }

        private String CallProcessor(string args)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = @"..\..\Resources\ProcessingApplication.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = args,
                RedirectStandardOutput = true
            };
            String result;
            using(Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        private Boolean DeserialiseProcessorOutput(String input, Boolean requireConfirmation)
        {
            if (input.Contains("<Success value=\"True\" />"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private DataSet[] DeserialiseProcessorOutput(string input)
        {
            if(input.Contains("<Success value=\"True\" />"))
            {
                input = input.Replace("<Success value=\"True\" />", null); //remove so we can deserialise XML string into object
                //debugBox2.Text = input;
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(DataSet[]));
                    System.IO.StringReader reader = new System.IO.StringReader(input);
                    return (DataSet[])serializer.Deserialize(reader);
                }
                catch (Exception e)
                {
                }
            }
            else
            {
                //do not attempt deserialisation - show error box
            }
            return null;
        }

        private void DeserialiseProcessorOutput(String input, String originalCommand)
        {
            if(input.Contains("<Success value=\"True\" />"))
            {
                input = input.Replace("<Success value=\"True\" />", null); //remove so we can deserialise XML string into object
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Chamber[]));
                    System.IO.StringReader reader = new System.IO.StringReader(input);
                    chambers = (Chamber[])serializer.Deserialize(reader);

                }
                catch (Exception e)
                {
                    //show error box
                }
            }
            else
            {
                //do not attempt deserialisation - show error box
            }
        }

        Chamber GetChamberByID(int chamberID)
        {
            Chamber c = null;
            for (int i = 0; i < chambers.Length; i++)
            {
                if (chambers[i].ID.Equals(chamberID))
                {
                    c = chambers[i];
                }
            }
            return c;
        }

        Sensor GetSensorByID(int sensorID)
        {
            Sensor s = null;
            for (int i = 0; i < chambers.Length; i++)
            {
                for (int j = 0; j < chambers[i].sensors.Length; j++)
                {
                    if (chambers[i].sensors[j].ID.Equals(sensorID))
                    {
                        s = chambers[i].sensors[j];
                    }
                }
            }
            return s;
        }

        private void RefreshLiveChart()
        {
            DataSet[] liveChartData = null;
            Chamber c = GetLiveChartPickerValue();
            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate.AddHours(-liveChartRange);
            Boolean averageValues = GetLiveChartAverage();

            String processorArgs = "produceGraph " + c.ID + " \"" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" \"" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" " + averageValues + " false";
            liveChartData = DeserialiseProcessorOutput(CallProcessor(processorArgs));
            if (liveChartData != null || liveChartData.Length != 0)
            {
                this.SetLiveChart(liveChartData);
            }
        }

        delegate Chamber GetLiveChartPickerValueCallback();
        private Chamber GetLiveChartPickerValue()
        {
            if (this.liveChartPicker.InvokeRequired)
            {
                GetLiveChartPickerValueCallback p = new GetLiveChartPickerValueCallback(GetLiveChartPickerValue);
                return (Chamber)this.Invoke(p, new object[] {});
            }
            else
            {
                return (Chamber)this.liveChartPicker.SelectedValue;
            }
        }

        delegate Boolean GetLiveChartAverageCallback();
        private Boolean GetLiveChartAverage()
        {
            if (this.liveChartAverage.InvokeRequired)
            {
                GetLiveChartAverageCallback p = new GetLiveChartAverageCallback(GetLiveChartAverage);
                return (Boolean)this.Invoke(p, new object[] { });
            }
            else
            {
                return (Boolean)this.liveChartAverage.Checked;
            }
        }

        delegate Boolean GetCustomChartAverageCallback();
        private Boolean GetCustomChartAverage()
        {
            if (this.customChartAverage.InvokeRequired)
            {
                GetCustomChartAverageCallback p = new GetCustomChartAverageCallback(GetLiveChartAverage);
                return (Boolean)this.Invoke(p, new object[] { });
            }
            else
            {
                return (Boolean)this.customChartAverage.Checked;
            }
        }

        delegate Chamber GetCustomChartPickerValueCallback();
        private Chamber GetCustomChartPickerValue()
        {
            if (this.customChartPicker.InvokeRequired)
            {
                GetCustomChartPickerValueCallback p = new GetCustomChartPickerValueCallback(GetCustomChartPickerValue);
                return (Chamber)this.Invoke(p, new object[] { });
            }
            else
            {
                return (Chamber)this.customChartPicker.SelectedValue;
            }
        }

        delegate DateTime GetStartDatePickerValueCallback();
        private DateTime GetStartDatePickerValue()
        {
            if (this.startDatePicker.InvokeRequired)
            {
                GetStartDatePickerValueCallback p = new GetStartDatePickerValueCallback(GetStartDatePickerValue);
                return (DateTime)this.Invoke(p, new object[] { });
            }
            else
            {
                return this.startDatePicker.Value;
            }
        }

        delegate DateTime GetEndDatePickerValueCallback();
        private DateTime GetEndDatePickerValue()
        {
            if (this.endDatePicker.InvokeRequired)
            {
                GetEndDatePickerValueCallback p = new GetEndDatePickerValueCallback(GetEndDatePickerValue);
                return (DateTime)this.Invoke(p, new object[] { });
            }
            else
            {
                return this.endDatePicker.Value;
            }
        }

        delegate void SetLiveChartCallback(DataSet[] data);
        private void SetLiveChart(DataSet[] data)
        {
            if (this.liveChart.InvokeRequired)
            {
                SetLiveChartCallback callback = new SetLiveChartCallback(SetLiveChart);
                this.Invoke(callback, new object[] {data });
            }
            else
            {
                this.liveChart.Series.Clear();
                for (int i = 0; i < data.Length; i++)
                {
                    switch (GetSensorByID(data[i].SensorID).SensorType)
                    {
                        case 0:
                            this.liveChart.Series.Add("Temperature Sensor " + data[i].SensorID);
                            this.liveChart.Series["Temperature Sensor " + data[i].SensorID].XValueType = ChartValueType.Time;
                            this.liveChart.Series["Temperature Sensor " + data[i].SensorID].YValueType = ChartValueType.Double;
                            this.liveChart.Series["Temperature Sensor " + data[i].SensorID].ChartType = SeriesChartType.Line;
                            for (int j = 0; j < data[i].Data.Length; j++)
                            {
                                this.liveChart.Series["Temperature Sensor " + data[i].SensorID].Points.AddXY(data[i].Data[j].Timestamp.ToString("yyyy-MM-dd HH:mm"), data[i].Data[j].Reading);
                            }
                            break;

                        case 1:
                            this.liveChart.Series.Add("Pressure Sensor " + data[i].SensorID);
                            this.liveChart.Series["Pressure Sensor " + data[i].SensorID].XValueType = ChartValueType.Time;
                            this.liveChart.Series["Pressure Sensor " + data[i].SensorID].YValueType = ChartValueType.Double;
                            this.liveChart.Series["Pressure Sensor " + data[i].SensorID].ChartType = SeriesChartType.Line;
                            this.liveChart.Series["Pressure Sensor " + data[i].SensorID].YAxisType = AxisType.Secondary;
                            for (int j = 0; j < data[i].Data.Length; j++)
                            {
                                this.liveChart.Series["Pressure Sensor " + data[i].SensorID].Points.AddXY(data[i].Data[j].Timestamp.ToString("yyyy-MM-dd HH:mm"), data[i].Data[j].Reading);
                            }
                            break;

                        case 2:
                            this.liveChart.Series.Add("Humidity Sensor " + data[i].SensorID);
                            this.liveChart.Series["Humidity Sensor " + data[i].SensorID].XValueType = ChartValueType.Time;
                            this.liveChart.Series["Humidity Sensor " + data[i].SensorID].YValueType = ChartValueType.Double;
                            this.liveChart.Series["Humidity Sensor " + data[i].SensorID].ChartType = SeriesChartType.Line;
                            for (int j = 0; j < data[i].Data.Length; j++)
                            {
                                this.liveChart.Series["Humidity Sensor " + data[i].SensorID].Points.AddXY(data[i].Data[j].Timestamp.ToString("yyyy-MM-dd HH:mm"), data[i].Data[j].Reading);
                            }
                            break;
                    }
                }
                this.liveChart.Update();
            }
            
        }

        delegate void SetCustomChartCallback(DataSet[] data);
        private void SetCustomChart(DataSet[] data)
        {
            if (this.customChart.InvokeRequired)
            {
                SetCustomChartCallback callback = new SetCustomChartCallback(SetCustomChart);
                this.Invoke(callback, new object[] { data });
            }
            else
            {
                this.customChart.Series.Clear();
                for(int i = 0; i < data.Length; i++)
                {
                    switch (GetSensorByID(data[i].SensorID).SensorType)
                    {
                        case 0:
                            this.customChart.Series.Add("Temperature Sensor " + data[i].SensorID);
                            this.customChart.Series["Temperature Sensor " + data[i].SensorID].XValueType = ChartValueType.DateTime;
                            this.customChart.Series["Temperature Sensor " + data[i].SensorID].YValueType = ChartValueType.Double;
                            this.customChart.Series["Temperature Sensor " + data[i].SensorID].ChartType = SeriesChartType.Line;
                            for (int j = 0; j < data[i].Data.Length; j++)
                            {
                                this.customChart.Series["Temperature Sensor " + data[i].SensorID].Points.AddXY(data[i].Data[j].Timestamp.ToString("yyyy-MM-dd HH:mm"), data[i].Data[j].Reading);
                            }
                            break;

                        case 1:
                            this.customChart.Series.Add("Pressure Sensor " + data[i].SensorID);
                            this.customChart.Series["Pressure Sensor " + data[i].SensorID].XValueType = ChartValueType.DateTime;
                            this.customChart.Series["Pressure Sensor " + data[i].SensorID].YValueType = ChartValueType.Double;
                            this.customChart.Series["Pressure Sensor " + data[i].SensorID].ChartType = SeriesChartType.Line;
                            this.customChart.Series["Pressure Sensor " + data[i].SensorID].YAxisType = AxisType.Secondary;
                            for (int j = 0; j < data[i].Data.Length; j++)
                            {
                                this.customChart.Series["Pressure Sensor " + data[i].SensorID].Points.AddXY(data[i].Data[j].Timestamp.ToString("yyyy-MM-dd HH:mm"), data[i].Data[j].Reading);
                            }
                            break;

                        case 2:
                            this.customChart.Series.Add("Humidity Sensor " + data[i].SensorID);
                            this.customChart.Series["Humidity Sensor " + data[i].SensorID].XValueType = ChartValueType.DateTime;
                            this.customChart.Series["Humidity Sensor " + data[i].SensorID].YValueType = ChartValueType.Double;
                            this.customChart.Series["Humidity Sensor " + data[i].SensorID].ChartType = SeriesChartType.Line;
                            for (int j = 0; j < data[i].Data.Length; j++)
                            {
                                this.customChart.Series["Humidity Sensor " + data[i].SensorID].Points.AddXY(data[i].Data[j].Timestamp.ToString("yyyy-MM-dd HH:mm"), data[i].Data[j].Reading);
                            }
                            break;
                    }
                }
                this.customChart.Update();
            }
        }

        private void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            RefreshLiveChart();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if(e.TabPage == liveChartTab)
            {
                liveChartTimer.Start();
            }
            else
            {
                liveChartTimer.Stop();
            }
        }

        private void tenSecondsItem_Click(object sender, EventArgs e)
        {
            liveChartTimer.Interval = 10000;
            liveChartTimer.Stop();
            liveChartTimer.Start();
        }

        private void twentySecondsItem_Click(object sender, EventArgs e)
        {
            liveChartTimer.Interval = 20000;
            liveChartTimer.Stop();
            liveChartTimer.Start();
        }

        private void thirtySecondsItem_Click(object sender, EventArgs e)
        {
            liveChartTimer.Interval = 30000;
            liveChartTimer.Stop();
            liveChartTimer.Start();
        }

        private void sixtySecondsItem_Click(object sender, EventArgs e)
        {
            liveChartTimer.Interval = 60000;
            liveChartTimer.Stop();
            liveChartTimer.Start();
        }

        private void oneHourRange_Click(object sender, EventArgs e)
        {
            liveChartRange = 1;
        }

        private void twoHourRange_Click(object sender, EventArgs e)
        {
            liveChartRange = 2;
        }

        private void sixHourRange_Click(object sender, EventArgs e)
        {
            liveChartRange = 6;
        }

        private void twelveHourRange_Click(object sender, EventArgs e)
        {
            liveChartRange = 12;
        }

        private void twentyFourHourRange_Click(object sender, EventArgs e)
        {
            liveChartRange = 24;
        }

        private void exportToExcelBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel Workbook|*.xlsm";
            saveFileDialog1.Title = "Save Custom Chart";
            saveFileDialog1.ShowDialog();
            if(saveFileDialog1.FileName != "")
            {
                Thread saveChart = new Thread(() => SaveCustomChart(saveFileDialog1.FileName));
                saveChart.Start();
            }
        }

        private void SaveCustomChart(String filename)
        {
            Chamber c = GetCustomChartPickerValue();
            DateTime startDate = GetStartDatePickerValue();
            DateTime endDate = GetEndDatePickerValue();
            Boolean averageValues = GetCustomChartAverage();
            String args = "produceGraph " + c.ID + " \"" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" \"" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" " + averageValues + " true \"" + filename + "\"";
            if(DeserialiseProcessorOutput(CallProcessor(args), true)){
                String message = "Chart saved to " + filename;
                String caption = "Info";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, buttons);
            }
            else
            {
                String message = "Error saving chart to " + filename;
                String caption = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, buttons);
            }
        }

        private void newChamberMenuButton_Click(object sender, EventArgs e)
        {
            chamberForm = new ChamberForm(chambers, false);
            chamberForm.Disposed += ChildForm_Disposed;
            chamberForm.BringToFront();
            chamberForm.TopMost = true;
            chamberForm.Focus();
            chamberForm.Show();
        }

        private void createNewSensorButton_Click(object sender, EventArgs e)
        {
            sensorForm = new SensorForm(chambers, false);
            sensorForm.Disposed += ChildForm_Disposed;
            sensorForm.BringToFront();
            sensorForm.TopMost = true;
            sensorForm.Focus();
            sensorForm.Show();
        }

        private void editExistingChamberButton_Click(object sender, EventArgs e)
        {
            chamberForm = new ChamberForm(chambers, true);
            chamberForm.Disposed += ChildForm_Disposed;
            chamberForm.BringToFront();
            chamberForm.TopMost = true;
            chamberForm.Focus();
            chamberForm.Show();
        }

        private void editSensorMenuButton_Click(object sender, EventArgs e)
        {
            sensorForm = new SensorForm(chambers, true);
            sensorForm.Disposed += ChildForm_Disposed;
            sensorForm.BringToFront();
            sensorForm.TopMost = true;
            sensorForm.Focus();
            sensorForm.Show();
        }

        private void deleteChamberMenuButton_Click(object sender, EventArgs e)
        {
            deleteForm = new DeleteForm(chambers, false);
            deleteForm.Disposed += ChildForm_Disposed;
            deleteForm.BringToFront();
            deleteForm.TopMost = true;
            deleteForm.Focus();
            deleteForm.Show();
        }

        private void deleteSensorMenuButton_Click(object sender, EventArgs e)
        {
            deleteForm = new DeleteForm(chambers, true);
            deleteForm.Disposed += ChildForm_Disposed;
            deleteForm.BringToFront();
            deleteForm.TopMost = true;
            deleteForm.Focus();
            deleteForm.Show();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //settings form if there's time
            throw new NotImplementedException();
        }
    }
}
