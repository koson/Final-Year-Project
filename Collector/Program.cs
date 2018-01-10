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
        [XmlArray("Sensors")]
        [XmlArrayItem("Sensors")]
        private List<Sensor> sensors = new List<Sensor>();
        private string databaseHost;
        private string databaseName;
        private string databaseUser;
        private string databasePassword;
        private string databaseTimeout;
        private string sensorListLocation;
        private int sensorPollInterval;

        static void Main(string[] args)
        {
            Program program = new Program();
            program.ReadGeneralConfig();
            program.ReadSensorConfig();
            program.PollSensor(program.sensors[2]);
            Console.Read();
        }

        double PollSensor(Sensor sensor)
        {
            byte[] rawData = RequestData(sensor);
            double sensorReading = GetSensorReading(CalculateVoltage(CalculateRegisterValue(rawData)), sensor.SensorType);
            return sensorReading;
        }

        ushort CalculateRegisterValue(byte[] rawData){ //calculates register value based off of bits in packet (uses little endian)
            ushort regValue;
            byte[] toAdd = new byte[2]; //always 2 bytes of data returned (modmux modules use 12 bits to send value)
            toAdd = new byte[2]{rawData[10], rawData[9]}; //always 9th and 10th bit for modmux devices
            regValue = BitConverter.ToUInt16(toAdd, 0); //concatenate bits to form 16 bit word
            Console.WriteLine("Register value is: " + regValue.ToString());
            return regValue;
        }

        double CalculateVoltage(ushort regValue) { //calculates voltage based off register value received
            double voltage = regValue/409.5; //http://www.proconel.com/Industrial-Data-Acquisition-Products/MODBUS-TCP-I-O-Modules/PM8AI-V-ISO---8-Voltage-Input-Module-Fully-Isolate.aspx states that 819 = 2v
            Console.WriteLine("Voltage is: " + voltage.ToString());
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
            Console.WriteLine("Humidity: " + humidity + "%");
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

        byte[] RequestData(Sensor sensor) { //reads data from a sensor
            byte upperTransIdentifier = 0b0;
            byte transIdentifier = (byte)0;
            byte protocolIdentifier = 0b0;
            byte upperHeaderLength = 0b0;
            byte lowerHeaderLength = 0b110;
            byte unitIdentifier = 0b1;
            byte functionCode = 0b100;
            byte register = (byte) sensor.Register;
            TcpClient client = new TcpClient(sensor.Address, sensor.Port);
            NetworkStream nwStream = client.GetStream();
            byte[] request = new byte[] { upperTransIdentifier, transIdentifier, protocolIdentifier, protocolIdentifier, upperHeaderLength, lowerHeaderLength, unitIdentifier, functionCode, 0b0, register, 0b0, 0b1};
            nwStream.Write(request, 0, request.Length);
            byte[] received = new byte[client.ReceiveBufferSize];
            Console.WriteLine(received.Length);
            int bytesRead = nwStream.Read(received, 0, client.ReceiveBufferSize);
            client.Close();
            return received;
        }

        void ReadSensorConfig() { //reads configuration from a file
            XmlSerializer configReader = new XmlSerializer(typeof(List<Sensor>));
            try
            {
                FileStream loadStream = new FileStream(sensorListLocation, FileMode.Open, FileAccess.Read);
                sensors = (List<Sensor>)configReader.Deserialize(loadStream);
                loadStream.Close();
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        void ReadGeneralConfig()
        {
            databaseHost = ConfigurationManager.AppSettings.Get("DatabaseHost");
            databaseName = ConfigurationManager.AppSettings.Get("DatabaseName");
            databaseUser = ConfigurationManager.AppSettings.Get("DatabaseUser");
            databasePassword = ConfigurationManager.AppSettings.Get("DatabasePassword");
            databaseTimeout = ConfigurationManager.AppSettings.Get("DatabaseConnectionTimeout");
            sensorListLocation = ConfigurationManager.AppSettings.Get("SensorFile");
            sensorPollInterval = int.Parse(ConfigurationManager.AppSettings.Get("SensorPollInterval"));
        }
        
        void SendToDatabase(Sensor sensor, double value) {
            SqlConnection connection = new SqlConnection("user id=" + databaseUser + ";" + "password=" + databasePassword + ";" + 
                "server=" + databaseHost + ";" + "Trusted_Connection=yes;" + "database=" + databaseName + ";" + "connection timeout=" + databaseTimeout);
            try
            {
                connection.Open();
                SqlCommand insertData = new SqlCommand("INSERT INTO Data (value, timestamp, Sensor_ID) " +
                                     "Values ('value', 'timestamp', 'sensor_ID)", connection);
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
