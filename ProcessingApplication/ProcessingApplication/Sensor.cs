using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProcessingApplication
{
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

        public Sensor(int newID, String newAddress, int newPort, int newRegister, int newSensorType, double newScale, double newOffset, int newChamberID)
        {
            ID = newID;
            Address = newAddress;
            Port = newPort;
            Register = newRegister;
            SensorType = newSensorType;
            Scale = newScale;
            Offset = newOffset;
            ChamberID = newChamberID;
        }

        public Sensor()
        {
            //for serialization only
        }
    }
}
