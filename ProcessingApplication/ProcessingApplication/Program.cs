using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Xml.Serialization;

namespace ProcessingApplication
{
    class Program
    {
        private string databaseHost;
        private string databaseName;
        private string databaseUser;
        private string databasePassword;
        private string databaseTimeout;
        [XmlArray("Chambers")]
        [XmlArrayItem("Chamber")]
        private Chamber[] chambers;
        static void Main(string[] args)
        {
            Program p = new Program();
            p.GetEnvironment();
            string s = p.ProduceGraphData(p.chambers[0], DateTime.Parse("2018-01-31 12:52:00"), DateTime.Parse("2018-01-31 12:54:00"));
            Console.Write(s);
            Console.Read();
           /* if(args[0] == "getenv")
            {
                Console.Write(s);
            }
            else
            {
                Console.WriteLine("Wrong usage");
            }*/
        }

        void PrintUsageText() //prints help guide on command line
        {
            Console.WriteLine();
        }

        void GetEnvironment() //get all chambers and sensors in database (no actual reportable data)
        {
            ReadProgramConfig();
            chambers = GetChamberList();
            LoadAllSensors();
        }
        Sensor[] GetSensorsForChamber(Chamber chamber)
        {
            //get list of all sensors for a chamber
            List<Sensor> sensors = new List<Sensor>();
            string connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "SELECT * FROM Sensor WHERE Chamber_ID=" + chamber.ID + ";";
                SqlCommand getData = new SqlCommand(query, connection);
                var returned = getData.ExecuteReader();
                while (returned.Read())
                {
                    sensors.Add(new Sensor(
                        returned.GetInt32(0),
                        returned.GetString(1),
                        returned.GetInt32(2),
                        returned.GetInt32(3),
                        returned.GetInt32(5),
                        returned.GetDouble(6),
                        returned.GetDouble(7),
                        returned.GetInt32(8)));
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return sensors.ToArray();
        }

        void LoadAllSensors()
        {
            for(int i = 0; i < chambers.Length; i++)
            {
                chambers[i].sensors = GetSensorsForChamber(chambers[i]);
            }
        }
        Chamber[] GetChamberList()
        {
            //get list of all chambers
            List<Chamber> chambers = new List<Chamber>();
            string connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "SELECT * FROM Chamber;";
                SqlCommand getData = new SqlCommand(query, connection);
                var returned = getData.ExecuteReader();
                while (returned.Read())
                {
                    chambers.Add(new Chamber(returned.GetInt32(0), returned.GetString(1), returned.GetString(2), null));
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return chambers.ToArray();
        }

        DataSet GetDataForSensor(Sensor s, DateTime startTime, DateTime endTime)
        {
            //get all data for a sensor in a timerange
            List<DataItem> returnedData = new List<DataItem>();
            string connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "SELECT * FROM Data_Record WHERE Sensor_ID='" + s.ID + "' AND Record_Time BETWEEN '" + startTime.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + endTime.ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY Record_Time ASC;";
                SqlCommand getData = new SqlCommand(query, connection);
                var returned = getData.ExecuteReader();
                while (returned.Read())
                {
                    DataItem item = new DataItem(
                        returned.GetInt32(0), 
                        returned.GetDouble(1), 
                        returned.GetDouble(2), 
                        DateTime.Parse(returned.GetSqlDateTime(3).ToString()), 
                        returned.GetInt32(4));
                    returnedData.Add(item);
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return new DataSet(returnedData.ToArray(), s.ID);
        }

        void GetCalibrationRecord(Sensor s)
        {
           // get most recent calibration data for a given sensor
        }

        void ProduceCalibratedDataSet()
        {
            //adjust value from DB to calibrated values using given cal data
        }
        
        Chamber GetChamberByID(int chamberID)
        {
            Chamber c = null;
            for(int i = 0; i < chambers.Length; i++)
            {
                if (chambers[i].ID.Equals(chamberID))
                {
                    c = chambers[i];
                }
            }
            return c;
        }

        String ProduceGraphData(Chamber c, DateTime start, DateTime end)
        {
            DataSet[] allTempSensors = new DataSet[c.GetNumberOfSensors(0)];
            DataSet[] allPressureSensors = new DataSet[c.GetNumberOfSensors(1)];
            DataSet[] allHumiditySensors = new DataSet[c.GetNumberOfSensors(2)];

            for (int i = 0; i < c.sensors.Length; i++)
            {
                switch (c.sensors[i].SensorType)
                {
                    case 0: //temperature
                        allTempSensors[i] = GetDataForSensor(c.sensors[i], start, end);
                        break;
                    case 1: //pressure
                        allPressureSensors[i] = GetDataForSensor(c.sensors[i], start, end);
                        break;
                    case 2: //humidity
                        allHumiditySensors[i] = GetDataForSensor(c.sensors[i], start, end);
                        break;
                    default: //throw error
                        throw new Exception();
                        //break;
                }
            }

            DataSet[] finalValues = new DataSet[3]; //3 sets of mean values to plot on graph
            if(allTempSensors.Length != 0)
            {
                finalValues[0] = ProduceMeanValues(allTempSensors);
            }
            if(allHumiditySensors.Length != 0)
            {
                finalValues[1] = ProduceMeanValues(allHumiditySensors);
            }
            if(allPressureSensors.Length != 0)
            {
                finalValues[2] = ProduceMeanValues(allPressureSensors);
            } 

            //steps
            //1. with given data, select all sensors and all data for the sensors within the timerange - done
            //2. produce calibrated data for each data set by getting latest calibration records for each sensor
            //3. produce one piece of data for each time interval (one temp, one pressure, one humidity) - done(ish)
            //4. serialize information using buildXML - done
            //5. return the XML - done
            String serialized = BuildXML(finalValues);
            return serialized;
        }

        DataSet ProduceMeanValues(DataSet[] dataToAverage) //mean of all values for certain sensor type per minute within the time range
        {
            DataSet meanValues = new DataSet();
            DateTime earliestTime = dataToAverage[0].Data[0].Timestamp.AddSeconds(-(dataToAverage[0].Data[0].Timestamp.Second)); //earliest whole minute
            DateTime latestTime = dataToAverage[0].Data[dataToAverage[0].Data.Length-1].Timestamp.AddSeconds(60- dataToAverage[0].Data[dataToAverage[0].Data.Length - 1].Timestamp.Second); //latest whole minute
            TimeSpan minutesDifference = latestTime.Subtract(earliestTime); //amount of minutes in between
            Console.WriteLine(earliestTime);
            Console.WriteLine(latestTime);
            Console.WriteLine(minutesDifference.TotalMinutes);

            for(int i = 0; i < minutesDifference.TotalMinutes; i++) //calculate mean value for each minute
            {
                List<Double> valuesToMean = new List<Double>();
                for (int j = 0; j < dataToAverage.Length; j++)
                {
                    for (int k = 0; k < dataToAverage[j].Data.Length; k++)
                    {
                        if (dataToAverage[j].Data[k].Timestamp < (earliestTime.AddMinutes(i+1)) && dataToAverage[j].Data[k].Timestamp >= earliestTime.AddMinutes(i)) //search all data for each sensor and add any within minute range to valuesToMean
                        {
                            Console.WriteLine(dataToAverage[j].Data[k].Timestamp + ": " + dataToAverage[j].Data[k].Reading);
                            valuesToMean.Add(dataToAverage[j].Data[k].Reading);
                        }
                    }
                }

                Double mean = Enumerable.Average(valuesToMean); //average values
                meanValues.AddDataItem(new DataItem(mean, earliestTime.AddMinutes(i))); //add to array of points to plot on graph (gets returned by function)
                Console.WriteLine(mean);
            }

            return meanValues;
        }

        void ExportToExcel()
        {
            //export result of ProduceGraphData() to an excel sheet
        }

        void produceSensorCalibrationInfo(int sensorID)
        {
            //given a sensor ID, look up latest calibration record and produce calibration value (amount to add to each reading due to sensor inaccuracy)
        }

        void ConvertToFarenheit()
        {
            //convert temperature values to farenheit
        }

        String BuildXML(Object o) //serialize an object to XML for command line output
        {
            XmlSerializer serializer = new XmlSerializer(o.GetType());
            System.IO.StringWriter writer = new System.IO.StringWriter();
            serializer.Serialize(writer, o);
            return writer.ToString();
        }

        void ReadProgramConfig()
        {
            try
            {
                databaseHost = ConfigurationManager.AppSettings.Get("DatabaseHost");
                databaseName = ConfigurationManager.AppSettings.Get("DatabaseName");
                databaseUser = ConfigurationManager.AppSettings.Get("DatabaseUser");
                databasePassword = ConfigurationManager.AppSettings.Get("DatabasePassword");
                databaseTimeout = ConfigurationManager.AppSettings.Get("DatabaseConnectionTimeout");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}