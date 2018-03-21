using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

namespace User_App
{
    public partial class Form1 : Form
    {
        [XmlArray("Chambers")]
        [XmlArrayItem("Chamber")]
        private Chamber[] chambers;
        public Form1()
        {
            InitializeComponent();
            //DeserialiseProcessorOutput(callProcessor("produceGraph 1 \"2018-03-07 03:23:00\" \"2018-03-07 03:53:00\" false false"));
            DeserialiseProcessorOutput(callProcessor("getEnv"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            String xml = "";
            BuildGraphFromXML(xml);
        }

        void BuildGraphFromXML(String xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataSet[]));
            TextReader reader = new StringReader(xml);
            FileStream loadStream = new FileStream("C:\\Users\\Raife\\test.xml", FileMode.Open, FileAccess.Read);
            Console.WriteLine(loadStream.ToString());
            DataSet[] data = (DataSet[])serializer.Deserialize(loadStream);
            liveChart.Series.Add("Temperature");
            liveChart.Series.Add("Humidity");
            liveChart.Series.Add("Pressure");
            liveChart.Series["Temperature"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            liveChart.Series["Temperature"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            liveChart.Series["Humidity"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            liveChart.Series["Humidity"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            liveChart.Series["Pressure"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            liveChart.Series["Pressure"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

            liveChart.Series["Temperature"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            liveChart.Series["Humidity"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            liveChart.Series["Pressure"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            for (int j = 0; j < data[0].Data.Length; j++)
            {
                liveChart.Series["Temperature"].Points.AddXY(data[0].Data[j].Timestamp, data[0].Data[j].Reading);
            }
            for (int j = 0; j < data[1].Data.Length; j++)
            {
                liveChart.Series["Humidity"].Points.AddXY(data[1].Data[j].Timestamp, data[0].Data[j].Reading);
            }
            for (int j = 0; j < data[2].Data.Length; j++)
            {
                liveChart.Series["Pressure"].Points.AddXY(data[2].Data[j].Timestamp, data[0].Data[j].Reading);
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chamberPickerBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //reload chart
        }

        private void liveChartTab_Click(object sender, EventArgs e)
        {

        }

        private void customChart_Click(object sender, EventArgs e)
        {

        }

        private String callProcessor(string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"C:\Users\Raife\source\repos\Final-Year-Project\ProcessingApplication\ProcessingApplication\bin\Debug\ProcessingApplication.exe";
            start.UseShellExecute = false;
            start.Arguments = args;
            start.RedirectStandardOutput = true;
            String result;
            using(Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    result = reader.ReadToEnd();
                    //debugBox.Text = result;
                }
            }
            return result;
        }

        private void DeserialiseProcessorOutput(String input)
        {
            if(input.Contains("<Success value=\"True\" />"))
            {
                debugBox.Text = "deserialisable";
                input = input.Replace("<Success value=\"True\" />", null);
                debugBox.Text += input;
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Chamber[]));
                    System.IO.StringReader reader = new System.IO.StringReader(input);
                    chambers = (Chamber[]) serializer.Deserialize(reader);
                    debugBox.Text = chambers[0].Name;
                }
                catch (Exception e)
                {
                    debugBox.Text = e.ToString();
                }
            }
            else
            {
                //do not attempt deserialisation - show error box
                debugBox.Text = "ERROR";
            }
        }
    }
}
