using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace User_App
{
    /// <summary>
    /// Class to represent sets of data retrieved from the database
    /// </summary>
   public class DataSet
    {
        [XmlArray("DataItems")]
        [XmlArrayItem("DataItem")]
        public DataItem[] Data { get; set; }

        [XmlAttribute("SensorID")]
        public int SensorID { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="newData">array of data items to store in dataset</param>
        /// <param name="sensorID">ID of the sensor this data belongs to</param>
        public DataSet(DataItem[] newData, int sensorID)
        {
            Data = newData;
            SensorID = sensorID;
        }

        /// <summary>
        /// Blank constructor used for serialisation
        /// </summary>
        public DataSet()
        {
            //Data = new DataItem[0];
        }

        /// <summary>
        /// Method for adding data items to the dataset
        /// </summary>
        /// <param name="d">The data item to be added</param>
        public void AddDataItem(DataItem d)
        {
            List<DataItem> data = Data.ToList();
            data.Add(d);
            Data = data.ToArray();
        }
    }
}
