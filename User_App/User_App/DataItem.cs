using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace User_App
{
    /// <summary>
    /// Class to represent individual data records from the database
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
        /// <param name="newID">ID of the data record</param>
        /// <param name="newReading">sensor reading </param>
        /// <param name="newRegValue">raw sensor register value</param>
        /// <param name="newTimestamp">timestamp of data record</param>
        /// <param name="newSensorID">ID of the sensor which collected this data</param>
        public DataItem(int newID, double newReading, double newRegValue, DateTime newTimestamp, int newSensorID)
        {
            ID = newID;
            Reading = newReading;
            RegisterValue = newRegValue;
            Timestamp = newTimestamp;
            SensorID = newSensorID;
        }

        /// <summary>
        /// Alternate constructor
        /// </summary>
        /// <param name="newReading">sensor reading</param>
        /// <param name="newTimestamp">record timestamp</param>
        public DataItem(double newReading, DateTime newTimestamp)
        {
            Reading = newReading;
            Timestamp = newTimestamp;
        }

        /// <summary>
        /// Blank constructor used for XML serialisation
        /// </summary>
        public DataItem()
        {
            //for serialization only
        }
    }
}
