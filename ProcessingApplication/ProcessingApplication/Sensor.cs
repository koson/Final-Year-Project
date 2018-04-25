using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProcessingApplication
{
    /// <summary>
    /// Class used to represent sensors stored in the database
    /// </summary>
    public class Sensor
    {
        [XmlAttribute("Address")]
        public string Address { get; set; }
        [XmlAttribute("Port")]
        public int Port { get; set; }
        [XmlAttribute("ID")]
        public int ID { get; set; }
        [XmlAttribute("Type")]
        public int SensorType { get; set; }
        [XmlAttribute("ChamberID")]
        public int ChamberID { get; set; }
        [XmlAttribute("Register")]
        public int Register { get; set; }
        [XmlAttribute("Scale")]
        public double Scale { get; set; }
        [XmlAttribute("Offset")]
        public double Offset { get; set; }
        [XmlAttribute("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="newID">ID of the sensor</param>
        /// <param name="newAddress">IP address of the sensor</param>
        /// <param name="newPort">Network port of the sensor</param>
        /// <param name="newRegister">register of the modbus device the sensor is connected to</param>
        /// <param name="newSensorType">type of sensor</param>
        /// <param name="newScale">sensor scale</param>
        /// <param name="newOffset">sensor offset</param>
        /// <param name="newChamberID">ID of the chamber the sensor is associated with</param>
        /// <param name="newDescription">Description (name) of the sensor</param>
        public Sensor(int newID, String newAddress, int newPort, int newRegister, int newSensorType, double newScale, double newOffset, int newChamberID, string newDescription)
        {
            ID = newID;
            Address = newAddress;
            Port = newPort;
            Register = newRegister;
            SensorType = newSensorType;
            Scale = newScale;
            Offset = newOffset;
            ChamberID = newChamberID;
            Description = newDescription;
        }

        /// <summary>
        /// Blank constructor used for XML serialisation
        /// </summary>
        public Sensor()
        {
            //for serialization only
        }
    }
}
