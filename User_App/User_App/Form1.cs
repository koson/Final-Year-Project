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

namespace User_App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            chart.Series.Add("Temperature");
            chart.Series.Add("Humidity");
            chart.Series.Add("Pressure");
            chart.Series["Temperature"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            chart.Series["Temperature"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            chart.Series["Humidity"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            chart.Series["Humidity"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            chart.Series["Pressure"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            chart.Series["Pressure"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

            chart.Series["Temperature"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart.Series["Humidity"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart.Series["Pressure"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            for (int j = 0; j < data[0].Data.Length; j++)
            {
                chart.Series["Temperature"].Points.AddXY(data[0].Data[j].Timestamp, data[0].Data[j].Reading);
            }
            for (int j = 0; j < data[1].Data.Length; j++)
            {
                chart.Series["Humidity"].Points.AddXY(data[1].Data[j].Timestamp, data[0].Data[j].Reading);
            }
            for (int j = 0; j < data[2].Data.Length; j++)
            {
                chart.Series["Pressure"].Points.AddXY(data[2].Data[j].Timestamp, data[0].Data[j].Reading);
            }
        }

    }
}
