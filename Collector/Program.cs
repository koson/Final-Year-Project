using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Configuration;

namespace Collector
{
    class Program
    {
        private List<Sensor> sensors = new List<Sensor>();
        private string databaseHost;
        private string databaseName;
        private string databaseUser;
        private string databasePassword;
        private string databaseTimeout;
        private int sensorPollInterval;

        static void Main(string[] args) //method only has test statements rn
        {
            Program program = new Program();
            program.ReadGeneralConfig();
            program.ReadSensorConfig();
            var result = program.PollSensor(program.sensors[0]);
            program.SendToDatabase(program.sensors[0], result);
            Console.Read();
        }

        Tuple<double, double, String> PollSensor(Sensor sensor) //tuple allows to return multiple values
        {
            byte[] rawData = RequestData(sensor);
            double voltage = CalculateVoltage(CalculateRegisterValue(rawData));
            double sensorReading = GetSensorReading(voltage, sensor.SensorType);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");
            var toReturn = Tuple.Create(voltage, sensorReading, timestamp);
            return toReturn;
        }

        ushort CalculateRegisterValue(byte[] rawData){ //calculates register value based off of bits in packet (uses little endian)
            ushort regValue;
            byte[] toAdd = new byte[2]; //always 2 bytes of data returned (modmux modules use 12 bits to send value)
            toAdd = new byte[2]{rawData[10], rawData[9]}; //always 9th and 10th bit for modmux devices
            regValue = BitConverter.ToUInt16(toAdd, 0); //concatenate bits to form 16 bit word
            Console.WriteLine("Register value is: " + regValue.ToString()); //for test purposes
            return regValue;
        }
          
        double CalculateVoltage(ushort regValue) { //calculates voltage based off register value received
            double voltage = regValue/409.5; //http://www.proconel.com/Industrial-Data-Acquisition-Products/MODBUS-TCP-I-O-Modules/PM8AI-V-ISO---8-Voltage-Input-Module-Fully-Isolate.aspx states that 819 = 2v
            Console.WriteLine("Voltage is: " + voltage.ToString()); //for test purposes
            return voltage;
        }
        
        double GetSensorReading(double voltage, int sensorType){
            double reading = 0;
            switch (sensorType) {
                case 0:
                    reading = CalculateTemperature(voltage);
                    break;
                case 1:
                    reading = CalculatePressure(voltage);
                    break;
                case 2:
                    reading = CalculateHumidity(voltage);
                    break;
                default:
                    //unknown - throw error
                    break;
            }
            return reading;
        }

        double CalculateHumidity(double voltage) {
            double humidity = voltage * 10;
            Console.WriteLine("Humidity: " + humidity + "%"); //for test purposes
            return humidity;
        }

        double CalculatePressure(double voltage)
        {
            //needs equation
            double pressure = 0;
            return pressure;
        }

        double CalculateTemperature(double voltage)
        {
            //needs equation
            double temperature = 0;
            return temperature;
        }

        byte[] RequestData(Sensor sensor) { //sends request to a modmux device. returns modmux response
            byte upperTransIdentifier = 0b0;
            byte transIdentifier = (byte)0;
            byte protocolIdentifier = 0b0;
            byte upperHeaderLength = 0b0;
            byte lowerHeaderLength = 0b110;
            byte unitIdentifier = 0b1;
            byte functionCode = 0b100;
            byte register = (byte) sensor.Register;
            TcpClient client = new TcpClient(sensor.Address, sensor.Port); //add try-catch
            NetworkStream nwStream = client.GetStream();
            byte[] request = new byte[] { upperTransIdentifier, transIdentifier, protocolIdentifier, protocolIdentifier,
                upperHeaderLength, lowerHeaderLength, unitIdentifier, functionCode, 0b0, register, 0b0, 0b1};
            nwStream.Write(request, 0, request.Length);
            byte[] received = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(received, 0, client.ReceiveBufferSize);
            client.Close();
            return received;
        }

        void ReadSensorConfig() { //reads configuration from the sensors table in the DB
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
                    sensors.Add(new Sensor(returned.GetString(1), returned.GetInt32(6
                        ), returned.GetInt32(0), returned.GetInt32(2), returned.GetInt32(3), returned.GetInt32(4)));
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        
        void SendToDatabase(Sensor sensor, Tuple<double,double, String> result) {
            string connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
            SqlConnection connection = new SqlConnection(connectionString);
            
            try
            {
                connection.Open();
                String insertStatement = "INSERT INTO Data_Record (Computed_Value, Voltage_Reading, Record_Time, Sensor_ID) " +
                                     "Values ('" + result.Item2 + "','" + result.Item1 + "','" + result.Item3 + "'," + sensor.ID + ")";
                SqlCommand insertData = new SqlCommand(insertStatement, connection);
                insertData.ExecuteNonQuery();
                connection.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }          
}

