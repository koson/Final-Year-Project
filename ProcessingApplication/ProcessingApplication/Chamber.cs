using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProcessingApplication
{
    public class Chamber
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }
        [XmlAttribute("Name")]
        public String Name { get; set; }
        [XmlAttribute("Description")]
        public String Description { get; set; }
        [XmlArray("Sensors")]
        [XmlArrayItem("Sensor")]
        public Sensor[] sensors { get; set; }

        public Chamber(int newID, String newName, String newDescription, Sensor[] newSensors)
        {
            ID = newID;
            Name = newName;
            Description = newDescription;
            sensors = newSensors;
        }

        public Chamber()
        {
            //for serialization only
        }

        public int GetNumberOfSensors(int typeOfSensor) //counts number of sensors of given type. e.g. temperature, humidity or pressure sensors.
        {
            int number = 0;
            for(int i = 0; i < sensors.Length; i++)
            {
                if(sensors[i].SensorType == typeOfSensor)
                {
                    number++;
                }
            }
            return number;
        }
    }
}
