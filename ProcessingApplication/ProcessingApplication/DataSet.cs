using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace ProcessingApplication
{
    /// <summary>
    /// Class used to represent sets of data for a sensor
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
        /// <param name="newData">the array of data items this set holds</param>
        /// <param name="sensorID">the ID of the sensor this set is associated with</param>
        public DataSet(DataItem[] newData, int sensorID)
        {
            Data = newData;
            SensorID = sensorID;
        }

        /// <summary>
        /// Blank constructor used to initialise dataset before data is added
        /// </summary>
        public DataSet()
        {
            Data = new DataItem[0];
        }

        /// <summary>
        /// Method to add data items to the dataset
        /// </summary>
        /// <param name="d">the data item to add</param>
        public void AddDataItem(DataItem d)
        {
            List<DataItem> data = Data.ToList();
            data.Add(d);
            Data = data.ToArray();
        }
    }
}
