using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using System.Threading;

/// <summary>
/// The user application
/// </summary>
namespace User_App
{
    /// <summary>
    /// The main class for the application. Main window for displaying graphs
    /// </summary>
    public partial class MainForm : Form
    {
        [XmlArray("Chambers")]
        [XmlArrayItem("Chamber")]
        private Chamber[] chambers;

        private ChamberForm chamberForm;
        private SensorForm sensorForm;
        private DeleteForm deleteForm;
        private String ProcessorFileName;

        private int liveChartRange = 1;
        private System.Timers.Timer liveChartTimer;

        /// <summary>
        /// Constructor method. Sets processor file path and initial live chart update interval
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            ProcessorFileName = @"..\..\Resources\ProcessingApplication.exe";
            liveChartTimer = new System.Timers.Timer
            {
                Interval = 10000
            };
        }

        /// <summary>
        /// Method for updating the contents of both charts chamber comboboxes
        /// </summary>
        private void UpdateEnvironment()
        {
            DeserialiseProcessorOutput(CallProcessor("getEnv"), "getEnv");
            List<Object> items = new List<Object>();
            for (int i = 0; i < chambers.Length; i++)
            {
                items.Add(new { Text = chambers[i].Name, Value = chambers[i] });
            }
            SetCustomChartPicker(items);
            SetLiveChartPicker(items);
        }

        /// <summary>
        /// Method handling the closure of smaller settings forms
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChildForm_Disposed(object sender, EventArgs e) //change from disposed to disposed after successful submit
        {
            Task update = new Task(() => UpdateEnvironment());
            update.Start();
        }

        /// <summary>
        /// Method called when form is loaded. Populates comboboxes with items and starts live chart timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateEnvironment();
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

        /// <summary>
        /// Validates custom chart options then calls chart update function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateButton_Click(object sender, EventArgs e)
        {
            DataSet[] customChartData = null;
            Chamber c = (Chamber)GetCustomChartPickerValue();
            DateTime startDate = GetStartDatePickerValue();
            DateTime endDate = GetEndDatePickerValue();
            Boolean averageValues = GetCustomChartAverage();
            if(ValidateDates(startDate, endDate))
            {
                String args = "produceGraph " + c.ID + " \"" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" \"" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" " + averageValues.ToString().ToLower() + " false";
                Task customChartTask = new Task(() => UpdateCustomChart());
                customChartTask.Start();
            }
            else
            {
                String message = "Start date cannot be after end date";
                String caption = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, buttons);
            }
            
        }

        /// <summary>
        /// Method for valudating dates from custom chart options.
        /// Ensures start date is before end date
        /// </summary>
        /// <param name="startDate">date from start date input</param>
        /// <param name="endDate">date from end date input</param>
        /// <returns>true if dates are valid</returns>
        private Boolean ValidateDates(DateTime startDate, DateTime endDate)
        {
            if(startDate > endDate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Method for updating the custom chart
        /// calls processing application and converts output into dataset
        /// dataset is passed to method which sets the custom chart data
        /// </summary>
        private void UpdateCustomChart()
        {
            DataSet[] customChartData = null;
            Chamber c = (Chamber)GetCustomChartPickerValue();
            DateTime startDate = GetStartDatePickerValue();
            DateTime endDate = GetEndDatePickerValue();
            Boolean averageValues = GetCustomChartAverage();

            String args = "produceGraph " + c.ID + " \"" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" \"" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" " + averageValues.ToString().ToLower() + " false";
            customChartData = DeserialiseProcessorOutput(CallProcessor(args));
            if (customChartData != null)
            {
                SetCustomChart(customChartData);
            }
        }

        /// <summary>
        /// Calls live chart refresh when new chamber is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chamberPickerBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Task t = new Task(() => this.RefreshLiveChart());
            t.Start();
        }

        /// <summary>
        /// Calls processing application with given arguments
        /// </summary>
        /// <param name="args">command line arguments for the proceesing application (as a Srtring)</param>
        /// <returns>XML output as a string</returns>
        private String CallProcessor(string args)
        {
            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = ProcessorFileName,
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

        /// <summary>
        /// Overload method for deserialising processor output as true/false response
        /// </summary>
        /// <param name="input">output of processor as XML string</param>
        /// <param name="requireConfirmation">boolean used to distinguish method as one used for true/false confirmations</param>
        /// <returns>parsed boolean value from processor output</returns>
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

        /// <summary>
        /// Method which deserialises processor output into a dataset
        /// </summary>
        /// <param name="input">output of processor as an XML string</param>
        /// <returns>an array of datasets parsed from the processor output</returns>
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

        /// <summary>
        /// Method for deserialising processor output into array of chambers
        /// method really needs renaming to distinguish from other two to ermove unnecessary parameters
        /// </summary>
        /// <param name="input">XML string output of processor</param>
        /// <param name="originalCommand">original command used when processor was called (somewhat irrelevant)</param>
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

        /// <summary>
        /// method to return chamber object for a given ID
        /// </summary>
        /// <param name="chamberID">the integer ID of the chamber to search for</param>
        /// <returns>the chamber object if it exists, null if chamber does not exist</returns>
        private Chamber GetChamberByID(int chamberID)
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

        /// <summary>
        /// Method to return sensor object for a given ID
        /// </summary>
        /// <param name="sensorID">the integer ID of the sensor to search for</param>
        /// <returns>sensor object if found, null if sensor does not exist</returns>
        private Sensor GetSensorByID(int sensorID)
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

        /// <summary>
        /// Method for refreshing the live chart.
        /// calls the processing application and calls the method for setting the chart data
        /// </summary>
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

        /// <summary>
        /// Delegate void for setting live chart combobox items
        /// </summary>
        /// <param name="items">list of items to show in the combobox</param>
        delegate void SetLiveChartPickerCallback(List<Object> items);
        /// <summary>
        /// method to set the items in the live chart combobox
        /// </summary>
        /// <param name="items">items to show in the combobox</param>
        private void SetLiveChartPicker(List<Object> items)
        {
            if (this.liveChartPicker.InvokeRequired)
            {
                SetLiveChartPickerCallback p = new SetLiveChartPickerCallback(SetLiveChartPicker);
               this.Invoke(p, new object[] { items });
            }
            else
            {
                this.liveChartPicker.DisplayMember = "Text";
                this.liveChartPicker.ValueMember = "Value";
                this.liveChartPicker.DataSource = items;
                this.liveChartPicker.Update();
            }
        }

        /// <summary>
        /// delegate void to set the custom chart combobox items
        /// </summary>
        /// <param name="items">items to show in the combobox</param>
        delegate void SetCustomChartPickerCallback(List<Object> items);
        /// <summary>
        /// Method to set the items in the custom chart combobox
        /// </summary>
        /// <param name="items">items to show in the combobox</param>
        private void SetCustomChartPicker(List<Object> items)
        {
            if (this.customChartPicker.InvokeRequired)
            {
                SetCustomChartPickerCallback p = new SetCustomChartPickerCallback(SetCustomChartPicker);
                this.Invoke(p, new object[] { items });
            }
            else
            {
                this.customChartPicker.DisplayMember = "Text";
                this.customChartPicker.ValueMember = "Value";
                this.customChartPicker.DataSource = items;
                this.customChartPicker.Update();
            }
        }

        /// <summary>
        /// Delegate method to retrieve the currently selected item in the live chart combobox
        /// </summary>
        /// <returns>the currently selected chamber object</returns>
        delegate Chamber GetLiveChartPickerValueCallback();
        /// <summary>
        /// method to select the current chamber in the live chart combobox
        /// </summary>
        /// <returns>the currently selected chamber object</returns>
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

        /// <summary>
        /// method to get the state of the live chart value average checkbox
        /// </summary>
        /// <returns>boolean true/false</returns>
        delegate Boolean GetLiveChartAverageCallback();
        /// <summary>
        /// method for getting the state of the live chart value average checkbox
        /// </summary>
        /// <returns>boolean true/false</returns>
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

        /// <summary>
        /// method to get the state of the custom chart value average checkbox
        /// </summary>
        /// <returns>boolean true/false</returns>
        delegate Boolean GetCustomChartAverageCallback();
        /// <summary>
        /// method for getting the state of the custom chart value average checkbox
        /// </summary>
        /// <returns>boolean true/false</returns>
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

        /// <summary>
        /// Delegate method to retrieve the currently selected item in the custom chart combobox
        /// </summary>
        /// <returns>the currently selected chamber object</returns>
        delegate Chamber GetCustomChartPickerValueCallback();
        /// <summary>
        /// method for getting the state of the custom chart combobox
        /// </summary>
        /// <returns>the currently selected chamber object</returns>
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

        /// <summary>
        /// method for getting the date in the start date box
        /// </summary>
        /// <returns>DateTime</returns>
        delegate DateTime GetStartDatePickerValueCallback();
        /// <summary>
        /// method for getting the date in the start date box
        /// </summary>
        /// <returns>DateTime</returns>
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
        /// <summary>
        /// method for getting the date in the end date box
        /// </summary>
        /// <returns>DateTime</returns>
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
        /// <summary>
        /// Method for setting the data shown of the live chart
        /// Appropriately generates labels based off sensor type and assigns to correct y axis
        /// </summary>
        /// <param name="data">dataset to display on the chart</param>
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
                            this.liveChart.Series.Add("Humidity Sensor " + data[i].SensorID);
                            this.liveChart.Series["Humidity Sensor " + data[i].SensorID].XValueType = ChartValueType.Time;
                            this.liveChart.Series["Humidity Sensor " + data[i].SensorID].YValueType = ChartValueType.Double;
                            this.liveChart.Series["Humidity Sensor " + data[i].SensorID].ChartType = SeriesChartType.Line;
                            for (int j = 0; j < data[i].Data.Length; j++)
                            {
                                this.liveChart.Series["Humidity Sensor " + data[i].SensorID].Points.AddXY(data[i].Data[j].Timestamp.ToString("yyyy-MM-dd HH:mm"), data[i].Data[j].Reading);
                            }
                            break;

                        case 2:
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
                    }
                }
                this.liveChart.Update();
            }
            
        }

        delegate void SetCustomChartCallback(DataSet[] data);
        /// <summary>
        /// Method for setting the data shown of the custom chart
        /// Appropriately generates labels based off sensor type and assigns to correct y axis
        /// </summary>
        /// <param name="data">dataset to display on the chart</param>
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
                            this.customChart.Series.Add("Humidity Sensor " + data[i].SensorID);
                            this.customChart.Series["Humidity Sensor " + data[i].SensorID].XValueType = ChartValueType.DateTime;
                            this.customChart.Series["Humidity Sensor " + data[i].SensorID].YValueType = ChartValueType.Double;
                            this.customChart.Series["Humidity Sensor " + data[i].SensorID].ChartType = SeriesChartType.Line;
                            this.customChart.Series["Humidity Sensor " + data[i].SensorID].YAxisType = AxisType.Secondary;
                            for (int j = 0; j < data[i].Data.Length; j++)
                            {
                                this.customChart.Series["Humidity Sensor " + data[i].SensorID].Points.AddXY(data[i].Data[j].Timestamp.ToString("yyyy-MM-dd HH:mm"), data[i].Data[j].Reading);
                            }
                            break;

                        case 2:
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
                    }
                }
                this.customChart.Update();
            }
        }

        /// <summary>
        /// Method to call when the live chart timer elapses
        /// calls update of the live chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            if(chambers.Length > 0)
            {
                RefreshLiveChart();
            }
        }

        /// <summary>
        /// Method which pauses the live chart timer if tab is switched to custom chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// updates timer interval to 10 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tenSecondsItem_Click(object sender, EventArgs e)
        {
            liveChartTimer.Interval = 10000;
            liveChartTimer.Stop();
            liveChartTimer.Start();
        }

        /// <summary>
        /// updates timer interval to 20 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void twentySecondsItem_Click(object sender, EventArgs e)
        {
            liveChartTimer.Interval = 20000;
            liveChartTimer.Stop();
            liveChartTimer.Start();
        }

        /// <summary>
        /// updates timer interval to 30 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void thirtySecondsItem_Click(object sender, EventArgs e)
        {
            liveChartTimer.Interval = 30000;
            liveChartTimer.Stop();
            liveChartTimer.Start();
        }

        /// <summary>
        /// updates timer interval to 60 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sixtySecondsItem_Click(object sender, EventArgs e)
        {
            liveChartTimer.Interval = 60000;
            liveChartTimer.Stop();
            liveChartTimer.Start();
        }

        /// <summary>
        /// updates live chart range to 1 hour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oneHourRange_Click(object sender, EventArgs e)
        {
            liveChartRange = 1;
        }

        /// <summary>
        /// updates live chart range to 2 hours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void twoHourRange_Click(object sender, EventArgs e)
        {
            liveChartRange = 2;
        }

        /// <summary>
        /// updates live chart range to 6 hours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sixHourRange_Click(object sender, EventArgs e)
        {
            liveChartRange = 6;
        }

        /// <summary>
        /// updates live chart range to 12 hours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void twelveHourRange_Click(object sender, EventArgs e)
        {
            liveChartRange = 12;
        }

        /// <summary>
        /// updates live chart range to 24 hours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void twentyFourHourRange_Click(object sender, EventArgs e)
        {
            liveChartRange = 24;
        }

        /// <summary>
        /// calls method to export custom chart to an excel spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportToExcelBtn_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel Workbook|*.xlsm";
            saveFileDialog1.Title = "Save Custom Chart";
            saveFileDialog1.ShowDialog();
            if(saveFileDialog1.FileName != "")
            {
                Task saveChart = new Task(() => SaveCustomChart(saveFileDialog1.FileName));
                saveChart.Start();
            }
        }

        /// <summary>
        /// Method for saving to an excel spreadsheet
        /// uses filename and all custom chart options as parameters fro the processing application
        /// processing application called to actually save the chart
        /// </summary>
        /// <param name="filename">Filename for the chart as a string</param>
        private void SaveCustomChart(String filename)
        {
            Chamber c = GetCustomChartPickerValue();
            DateTime startDate = GetStartDatePickerValue();
            DateTime endDate = GetEndDatePickerValue();
            Boolean averageValues = GetCustomChartAverage();
            if(ValidateDates(startDate, endDate))
            {
                String args = "produceGraph " + c.ID + " \"" + startDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" \"" + endDate.ToString("yyyy-MM-dd HH:mm:ss") + "\" " + averageValues + " true \"" + filename + "\"";
                if (DeserialiseProcessorOutput(CallProcessor(args), true))
                {
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
            else
            {
                String message = "Start date cannot be after end date";
                String caption = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, buttons);
            }
            
        }

        /// <summary>
        /// Method for displaying a new chamber form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newChamberMenuButton_Click(object sender, EventArgs e)
        {
            chamberForm = new ChamberForm(chambers, false);
            chamberForm.Disposed += ChildForm_Disposed;
            chamberForm.BringToFront();
            chamberForm.TopMost = true;
            chamberForm.Focus();
            chamberForm.Show();
        }

        /// <summary>
        /// Method for displaying a new sensor form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createNewSensorButton_Click(object sender, EventArgs e)
        {
            sensorForm = new SensorForm(chambers, false);
            sensorForm.Disposed += ChildForm_Disposed;
            sensorForm.BringToFront();
            sensorForm.TopMost = true;
            sensorForm.Focus();
            sensorForm.Show();
        }

        /// <summary>
        /// Method for displaying a new chamber form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editExistingChamberButton_Click(object sender, EventArgs e)
        {
            chamberForm = new ChamberForm(chambers, true);
            chamberForm.Disposed += ChildForm_Disposed;
            chamberForm.BringToFront();
            chamberForm.TopMost = true;
            chamberForm.Focus();
            chamberForm.Show();
        }

        /// <summary>
        /// Method for displaying a new sensor form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editSensorMenuButton_Click(object sender, EventArgs e)
        {
            sensorForm = new SensorForm(chambers, true);
            sensorForm.Disposed += ChildForm_Disposed;
            sensorForm.BringToFront();
            sensorForm.TopMost = true;
            sensorForm.Focus();
            sensorForm.Show();
        }

        /// <summary>
        /// Method for displaying a new delete form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteChamberMenuButton_Click(object sender, EventArgs e)
        {
            deleteForm = new DeleteForm(chambers, false);
            deleteForm.Disposed += ChildForm_Disposed;
            deleteForm.BringToFront();
            deleteForm.TopMost = true;
            deleteForm.Focus();
            deleteForm.Show();
        }

        /// <summary>
        /// Method for displaying a new delete form 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteSensorMenuButton_Click(object sender, EventArgs e)
        {
            deleteForm = new DeleteForm(chambers, true);
            deleteForm.Disposed += ChildForm_Disposed;
            deleteForm.BringToFront();
            deleteForm.TopMost = true;
            deleteForm.Focus();
            deleteForm.Show();
        }

        /// <summary>
        /// Method for quitting the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// Methof for diaplying a new settings form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //settings form if there's time
            throw new NotImplementedException();
        }
    }
}
