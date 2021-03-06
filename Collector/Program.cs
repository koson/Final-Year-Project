﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

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

        static void Main(string[] args) //test version for use before adding functionality to the service
        {
            Program program = new Program();
            program.ReadGeneralConfig();
            program.sensors = program.ReadSensorConfig();
            /* System.Timers.Timer pollTimer = new System.Timers.Timer();
             pollTimer.Interval = program.sensorPollInterval;
             pollTimer.Start();
             pollTimer.Elapsed += new System.Timers.ElapsedEventHandler(program.OnTimer);
             Console.ReadLine();*/
            Task.Factory.StartNew(() => program.PollModbusSensor(program.sensors[0]));
            Console.Read();
        }

        //repoll all sensors
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            Task[] tasks = new Task[sensors.Length];
            for (int i = 0; i < sensors.Length; i++)
            {
                Console.WriteLine(sensors[i].Address);
                Sensor s = sensors[i];
                if(s.Interface == "Modbus")
                {
                    tasks[i] = Task.Factory.StartNew(() => PollModbusSensor(s));
                }
            }
        }

        //start whole data process for one sensor
        void PollModbusSensor(Sensor sensor)
        {
            var result = ReadModbusSensor(sensor);
            if(result != null)
            {
                SendToDatabase(sensor, result);
            }
        }

        //request and receive data. Separate to database saving
        Tuple<double, double, String> ReadModbusSensor(Sensor sensor) //tuple allows to return multiple values
        {
            byte[] rawData = RequestData(sensor);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            for(int i = 0; i < rawData.Length; i++)
            {
                Console.Write(rawData[i].ToString());
            }
            Console.WriteLine(timestamp);
            if (rawData != null)
            {
                double regValue = CalculateRegisterValue(rawData);
                double sensorReading = GetModbusSensorReading(regValue, sensor);
                Console.WriteLine(timestamp);
                Console.WriteLine(regValue);
                var toReturn = Tuple.Create(regValue, sensorReading, timestamp);
                return toReturn;
            }
            else
            {
                return null;
            }
            
        }

        double CalculateRegisterValue(byte[] rawData)
        { //calculates register value based off of bits in packet (uses little endian)
            ushort regValue;
            byte[] toAdd = new byte[2]; //always 2 bytes of data returned (modmux modules use 12 bits to send value)
            toAdd = new byte[2] { rawData[10], rawData[9] }; //always 9th and 10th bit for modmux devices
            regValue = BitConverter.ToUInt16(toAdd, 0); //concatenate bits to form 16 bit word
            double converted = Convert.ToDouble(regValue);
            Console.WriteLine("Register value is: " + converted.ToString()); //for test purposes
            return converted;
        }

        //obsolete
        double CalculateVoltage(double regValue)
        { //calculates voltage based off register value received
            double voltage = regValue / 409.5; //http://www.proconel.com/Industrial-Data-Acquisition-Products/MODBUS-TCP-I-O-Modules/PM8AI-V-ISO---8-Voltage-Input-Module-Fully-Isolate.aspx states that 819 = 2v
            Console.WriteLine("Voltage is: " + voltage.ToString()); //for test purposes
            return voltage;
        }

        double GetModbusSensorReading(double regValue, Sensor s)
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

            try
            {
                TcpClient client = new TcpClient(sensor.Address, sensor.Port);
                NetworkStream nwStream = client.GetStream();
                byte[] request = new byte[] { upperTransIdentifier, transIdentifier, protocolIdentifier, protocolIdentifier,
        upperHeaderLength, lowerHeaderLength, unitIdentifier, functionCode, 0b0, register, 0b0, 0b1};
                nwStream.Write(request, 0, request.Length);
                received = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(received, 0, client.ReceiveBufferSize);
                client.Close();
                byte[] toReturn = received.Take(13).ToArray();
                return toReturn;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        Sensor[] ReadSensorConfig()
        { //reads configuration from the sensors table in the DB
            List<Sensor> sensors = new List<Sensor>();
            string connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlConnection connection2 = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                connection2.Open();
                string query = "SELECT * FROM Sensor WHERE Sensor_Enabled=1;";
                SqlCommand getSensors = new SqlCommand(query, connection);
                var returned = getSensors.ExecuteReader();
                while(returned.Read())
                {
                    if(returned.GetInt32(5) != 0){ //if it has a modbus connection entry - get it
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
                                returned.GetString(1),
                                "Modbus"));
                        }
                        returned2.Close();
                    }
                }
                connection.Close();
                connection2.Close();
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

