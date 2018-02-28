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
            string s = p.ProduceGraphData(p.chambers[0], DateTime.Parse("2018-02-21 05:50:10"), DateTime.Parse("2018-02-21 05:56:03"));
            Console.Write(s);
            //p.AddModbusSensor("169.254.228.122", 504, 0, 1, 1, 0, 0, "Test add sensor");
            //String sensors = p.BuildXML(p.GetSensorsForChamber(p.chambers[0]));
            //Console.WriteLine(sensors);
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
            SqlConnection connection2 = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                connection2.Open();
                string query = "SELECT * FROM Sensor WHERE Sensor.Chamber_ID=" + chamber.ID + ";";
                SqlCommand getData = new SqlCommand(query, connection);
                var returned = getData.ExecuteReader();
                while (returned.Read())
                {
                    if (returned.GetInt32(5) != 0)
                    { //if it has a modbus connection entry - get it
                        string query2 = "SELECT * FROM Modbus_Info WHERE Modbus_Info_ID=" + returned.GetInt32(5) + ";";
                        SqlCommand getSensors2 = new SqlCommand(query2, connection2);
                        var returned2 = getSensors2.ExecuteReader();
                        while (returned2.Read())
                        {
                            sensors.Add(new Sensor(
                                returned.GetInt32(0),
                                returned2.GetString(1),
                                returned2.GetInt32(2),
                                returned2.GetInt32(3),
                                returned.GetInt32(3),
                                returned2.GetDouble(4),
                                returned2.GetDouble(5),
                                returned.GetInt32(4),
                                returned.GetString(1)));
                        }
                        returned2.Close();
                        getSensors2.Dispose();
                    }
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
            DataSet temperatureValues = new DataSet();
            DataSet pressureValues = new DataSet();
            DataSet humidityValues = new DataSet();

            for (int i = 0; i < c.sensors.Length; i++)
            {
                switch (c.sensors[i].SensorType)
                {
                    case 0: //temperature
                        temperatureValues = ProduceMeanValues(GetDataForSensor(c.sensors[i], start, end));
                        temperatureValues.SensorID = c.sensors[i].ID;
                        break;
                    case 1: //pressure
                        pressureValues = ProduceMeanValues(GetDataForSensor(c.sensors[i], start, end));
                        pressureValues.SensorID = c.sensors[i].ID;
                        break;
                    case 2: //humidity
                        humidityValues = ProduceMeanValues(GetDataForSensor(c.sensors[i], start, end));
                        humidityValues.SensorID = c.sensors[i].ID;
                        break;
                    default: //throw error
                        throw new Exception();
                        //break;
                }
            }

            DataSet[] finalValues = new DataSet[3]; //3 sets of values to plot on graph
            finalValues[0] = temperatureValues;
            finalValues[1] = pressureValues;
            finalValues[2] = humidityValues; 

            //steps
            //1. with given data, select all sensors and all data for the sensors within the timerange - done
            //2. produce calibrated data for each data set by getting latest calibration records for each sensor
            //4. serialize information using buildXML - done
            //5. return the XML - done
            String serialized = BuildXML(finalValues);
            return serialized;
        }

        //change to allow custom time intervals to average (based off user input in GUI)?
        DataSet ProduceMeanValues(DataSet dataToAverage) //mean of all values for certain sensor type per minute within the time range
        {
            DataSet meanValues = new DataSet();
            DateTime earliestTime = dataToAverage.Data[0].Timestamp.AddSeconds(-(dataToAverage.Data[0].Timestamp.Second)); //earliest whole minute
            DateTime latestTime = dataToAverage.Data[dataToAverage.Data.Length-1].Timestamp.AddSeconds(60- dataToAverage.Data[dataToAverage.Data.Length - 1].Timestamp.Second); //latest whole minute
            TimeSpan minutesDifference = latestTime.Subtract(earliestTime); //amount of minutes in between
            Console.WriteLine(earliestTime);
            Console.WriteLine(latestTime);
            Console.WriteLine(minutesDifference.TotalMinutes);

            for(int i = 0; i < minutesDifference.TotalMinutes; i++) //calculate mean value for each minute
            {
                List<Double> valuesToMean = new List<Double>();
                for (int j = 0; j < dataToAverage.Data.Length; j++)
                {
                    if (dataToAverage.Data[j].Timestamp < (earliestTime.AddMinutes(i+1)) && dataToAverage.Data[j].Timestamp >= earliestTime.AddMinutes(i)) //search all data for each sensor and add any within minute range to valuesToMean
                    {
                        Console.WriteLine(dataToAverage.Data[j].Timestamp + ": " + dataToAverage.Data[j].Reading);
                        valuesToMean.Add(dataToAverage.Data[j].Reading);
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

        void AddModbusSensor(String address, int port, int type, int chamberID, int register, double scale, double offset, String description) //add sensor to database
        {
            string connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlParameter[] parameters = new SqlParameter[8];
            parameters[0] = new SqlParameter("@IP", System.Data.SqlDbType.VarChar);
            parameters[0].Value = address;
            parameters[1] = new SqlParameter("@Port", System.Data.SqlDbType.Int);
            parameters[1].Value = port;
            parameters[2] = new SqlParameter("@Type", System.Data.SqlDbType.Int);
            parameters[2].Value = type;
            parameters[3] = new SqlParameter("@ChamberID", System.Data.SqlDbType.Int);
            parameters[3].Value = chamberID;
            parameters[4] = new SqlParameter("@Register", System.Data.SqlDbType.Int);
            parameters[4].Value = register;
            parameters[5] = new SqlParameter("@Scale", System.Data.SqlDbType.Float);
            parameters[5].Value = scale;
            parameters[6] = new SqlParameter("@Offset", System.Data.SqlDbType.Float);
            parameters[6].Value = offset;
            parameters[7] = new SqlParameter("@Description", System.Data.SqlDbType.VarChar);
            parameters[7].Value = description;

            try
            {
                connection.Open();
                String query = "INSERT INTO Modbus_Info (IP_Address, Network_Port, Register, Scale, Offset) VALUES (@IP, @Port, @Register, @Scale, @Offset);";
                SqlCommand command = connection.CreateCommand();
                for(int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.Add(parameters[i]);
                }
                command.CommandText = query;
                command.ExecuteNonQuery();
                query = "SELECT TOP 1 Modbus_Info_ID FROM Modbus_Info ORDER BY Modbus_Info_ID DESC"; //get last inserted modbus info record - the one that was just made
                command.CommandText = query;
                Console.WriteLine(command.CommandText);
                int modbusID = (int)command.ExecuteScalar();
                Console.WriteLine("Modbus_Info_ID = " + modbusID);                
                query = "INSERT INTO Sensor (Name, Calibration_Sensor, Sensor_Type, Chamber_ID, Modbus_Info_ID) VALUES (@Description, " + 0 + ", " +"@Type, @ChamberID, " + modbusID.ToString() + ");";
                command.CommandText = query;
                Console.WriteLine(command.CommandText);
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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