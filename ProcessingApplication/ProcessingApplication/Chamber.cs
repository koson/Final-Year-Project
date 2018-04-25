using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProcessingApplication
{
    /// <summary>
    /// Class used to represent chambers from the database
    /// </summary>
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

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="newID">the ID of the chamber</param>
        /// <param name="newName">the chamber name</param>
        /// <param name="newDescription">the chamber description</param>
        /// <param name="newSensors">the array of sensors associated with the chamber</param>
        public Chamber(int newID, String newName, String newDescription, Sensor[] newSensors)
        {
            ID = newID;
            Name = newName;
            Description = newDescription;
            sensors = newSensors;
        }

        /// <summary>
        /// Blank constructor used for XML serialisation
        /// </summary>
        public Chamber()
        {
            //for serialization only
        }

        /// <summary>
        /// Method to get the number of sensors of a given type
        /// </summary>
        /// <param name="typeOfSensor">the type of the sensor to count</param>
        /// <returns>the number of the sensors </returns>
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
