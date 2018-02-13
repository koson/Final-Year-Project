using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace ProcessingApplication
{
   public class DataSet
    {
        [XmlArray("DataItems")]
        [XmlArrayItem("DataItem")]
        public DataItem[] Data { get; set; }

        [XmlAttribute("SensorID")]
        public int SensorID { get; set; }

        public DataSet(DataItem[] newData, int sensorID)
        {
            Data = newData;
            SensorID = sensorID;
        }

        public DataSet()
        {
            Data = new DataItem[0];
        }

        public void AddDataItem(DataItem d)
        {
            List<DataItem> data = Data.ToList();
            data.Add(d);
            Data = data.ToArray();
        }
    }
}
