using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;

namespace Collector
{
    class Program
    {
        private string databaseHost;
        private string databaseName;
        private string databaseUser;
        private string databasePassword;
        private string databaseTimeout;
        private int sensorPollInterval;
        private Sensor[] sensors;

        static void Main(string[] args) //method only has test statements rn
        {
            Program program = new Program();
            program.ReadGeneralConfig();
            program.sensors = program.ReadSensorConfig();
            System.Timers.Timer pollTimer = new System.Timers.Timer();
            pollTimer.Interval = program.sensorPollInterval;
            pollTimer.Start();
            pollTimer.Elapsed += new System.Timers.ElapsedEventHandler(program.OnTimer);
            Console.ReadLine();
        }
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            Task[] tasks = new Task[sensors.Length];
            for (int i = 0; i < sensors.Length; i++)
            {
                Console.WriteLine("Value of i is: " + i.ToString());
                Console.WriteLine(sensors[i].Address);
                Sensor s = sensors[i];
                tasks[i] = Task.Factory.StartNew(() => PollSensor(s));
            }
        }

        void PollSensor(Sensor sensor)
        {
            var result = ReadSensor(sensor);
            SendToDatabase(sensor, result);
        }

        Tuple<double, double, String> ReadSensor(Sensor sensor) //tuple allows to return multiple values
        {
            byte[] rawData = RequestData(sensor);
            double regValue = CalculateRegisterValue(rawData);
            //double voltage = CalculateVoltage(regValue);
            double sensorReading = GetSensorReading(regValue, sensor);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");
            Console.WriteLine(timestamp);
            var toReturn = Tuple.Create(regValue, sensorReading, timestamp);
            return toReturn;
        }

        ushort CalculateRegisterValue(byte[] rawData)
        { //calculates register value based off of bits in packet (uses little endian)
            ushort regValue;
            byte[] toAdd = new byte[2]; //always 2 bytes of data returned (modmux modules use 12 bits to send value)
            toAdd = new byte[2] { rawData[10], rawData[9] }; //always 9th and 10th bit for modmux devices
            regValue = BitConverter.ToUInt16(toAdd, 0); //concatenate bits to form 16 bit word
            Console.WriteLine("Register value is: " + regValue.ToString()); //for test purposes
            return regValue;
        }

        double CalculateVoltage(double regValue)
        { //calculates voltage based off register value received
            double voltage = regValue / 409.5; //http://www.proconel.com/Industrial-Data-Acquisition-Products/MODBUS-TCP-I-O-Modules/PM8AI-V-ISO---8-Voltage-Input-Module-Fully-Isolate.aspx states that 819 = 2v
            Console.WriteLine("Voltage is: " + voltage.ToString()); //for test purposes
            return voltage;
        }

        double GetSensorReading(double regValue, Sensor s)
        {
            double reading = 0;
            switch (s.SensorType)
            {
                case 0:
                    reading = CalculateTemperature(regValue);
                    break;
                case 1:
                    reading = CalculatePressure(regValue, s.Scale, s.Offset);
                    break;
                case 2:
                    reading = CalculateHumidity(regValue, s.Scale, s.Offset);
                    break;
                default:
                    //unknown - throw error
                    break;
            }
            return reading;
        }

        double CalculateHumidity(double regValue, double scale, double offset)
        {
            double humidity = (regValue*scale) + offset;
            Console.WriteLine("Humidity is: " + humidity);
            return humidity;
        }

        double CalculatePressure(double regValue, double scale, double offset)
        {
            double pressure = (regValue*scale) + offset;
            Console.WriteLine("pressure is: " + pressure);
            return pressure;
        }

        double CalculateTemperature(double regValue)
        {
            double temperature = regValue/10;
            Console.WriteLine("Temperature is: " + temperature);
            return temperature;
        }

        byte[] RequestData(Sensor sensor)
        { //sends request to a modmux device. returns modmux response
            byte upperTransIdentifier = 0b0;
            byte transIdentifier = (byte)0;
            byte protocolIdentifier = 0b0;
            byte upperHeaderLength = 0b0;
            byte lowerHeaderLength = 0b110;
            byte unitIdentifier = 0b1;
            byte functionCode = 0b100;
            byte[] received;
            byte register = (byte)sensor.Register;
            TcpClient client = new TcpClient(sensor.Address, sensor.Port); //add try-catch
            NetworkStream nwStream = client.GetStream();
            byte[] request = new byte[] { upperTransIdentifier, transIdentifier, protocolIdentifier, protocolIdentifier,
        upperHeaderLength, lowerHeaderLength, unitIdentifier, functionCode, 0b0, register, 0b0, 0b1};
            nwStream.Write(request, 0, request.Length);
            received = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(received, 0, client.ReceiveBufferSize);
            client.Close();
            return received;
        }

        Sensor[] ReadSensorConfig()
        { //reads configuration from the sensors table in the DB
            List<Sensor> sensors = new List<Sensor>();
            string connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "SELECT * FROM Sensor WHERE Calibration_Sensor=0;"; //select all sensors not used for calibration
                SqlCommand getSensors = new SqlCommand(query, connection);
                var returned = getSensors.ExecuteReader();
                while (returned.Read())
                {
                    sensors.Add(new Sensor(returned.GetInt32(0), returned.GetString(1), returned.GetInt32(2), returned.GetInt32(3), returned.GetInt32(5), returned.GetDouble(6), returned.GetDouble(7), returned.GetInt32(8)));
                }
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return sensors.ToArray();
        }

        void ReadGeneralConfig()
        {
            try
            {
                databaseHost = ConfigurationManager.AppSettings.Get("DatabaseHost");
                databaseName = ConfigurationManager.AppSettings.Get("DatabaseName");
                databaseUser = ConfigurationManager.AppSettings.Get("DatabaseUser");
                databasePassword = ConfigurationManager.AppSettings.Get("DatabasePassword");
                databaseTimeout = ConfigurationManager.AppSettings.Get("DatabaseConnectionTimeout");
                sensorPollInterval = int.Parse(ConfigurationManager.AppSettings.Get("SensorPollInterval"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void SendToDatabase(Sensor sensor, Tuple<double, double, String> result)
        {
            string connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                String insertStatement = "INSERT INTO Data_Record (Computed_Value, Register_Value, Record_Time, Sensor_ID) " +
                                     "Values ('" + result.Item2 + "','" + result.Item1 + "','" + result.Item3 + "'," + sensor.ID + ")";
                SqlCommand insertData = new SqlCommand(insertStatement, connection);
                insertData.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}

