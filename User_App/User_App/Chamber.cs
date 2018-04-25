using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace User_App
{
    /// <summary>
    /// Class for representing chambers stored in the database
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
        /// <param name="newID">ID of the chamber</param>
        /// <param name="newName">Name of the chamber</param>
        /// <param name="newDescription">Description of the chamber</param>
        /// <param name="newSensors">Array of sensors which belong to the chamber</param>
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
        /// Returns the number of sensors associated with this chamber
        /// </summary>
        /// <param name="typeOfSensor">type of sensor to query</param>
        /// <returns>number of sensors of specified type</returns>
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
