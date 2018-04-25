using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProcessingApplication
{
    /// <summary>
    /// A class to represent records from the data_record table
    /// </summary>
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

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="newID">the ID of the data record</param>
        /// <param name="newReading">the reading value of the data record</param>
        /// <param name="newRegValue">the register value of the data record </param>
        /// <param name="newTimestamp">the timestamp value of the data record</param>
        /// <param name="newSensorID">the sensor ID of the data record</param>
        public DataItem(int newID, double newReading, double newRegValue, DateTime newTimestamp, int newSensorID)
        {
            ID = newID;
            Reading = newReading;
            RegisterValue = newRegValue;
            Timestamp = newTimestamp;
            SensorID = newSensorID;
        }

        /// <summary>
        /// Minimal constructor used to only store reading and timestamp
        /// </summary>
        /// <param name="newReading">the reading of the data record</param>
        /// <param name="newTimestamp">the timestamp of the data record</param>
        public DataItem(double newReading, DateTime newTimestamp)
        {
            Reading = newReading;
            Timestamp = newTimestamp;
        }

        /// <summary>
        /// blank constructor used for XML serialisation
        /// </summary>
        public DataItem()
        {
            //for serialization only
        }
    }
}
