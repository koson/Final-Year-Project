using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProcessingApplication
{
    public class DataItem
    {
        [XmlAttribute("ID")]
        private int ID { get; set; }
        [XmlAttribute("Reading")]
        public double Reading { get; set; }
        [XmlAttribute("RegisterValue")]
        private double RegisterValue{ get; set; }
        [XmlAttribute("Timestamp")]
        public DateTime Timestamp { get; set; }
        [XmlAttribute("SensorID")]
        private int SensorID { get; set; }

        public DataItem(int newID, double newReading, double newRegValue, DateTime newTimestamp, int newSensorID)
        {
            ID = newID;
            Reading = newReading;
            RegisterValue = newRegValue;
            Timestamp = newTimestamp;
            SensorID = newSensorID;
        }

        public DataItem(double newReading, DateTime newTimestamp)
        {
            Reading = newReading;
            Timestamp = newTimestamp;
        }

        public DataItem()
        {
            //for serialization only
        }
    }
}
