using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.IO;
using System.Xml.Serialization;
using System.Data.SqlClient;
using System.Configuration;

namespace CollectorService
{
    public partial class Service1 : ServiceBase
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
        private System.ComponentModel.IContainer components;
        private System.Diagnostics.EventLog eventLog;
        private int eventID = 1;
        private List<Sensor> sensors = new List<Sensor>();
        private string databaseHost;
        private string databaseName;
        private string databaseUser;
        private string databasePassword;
        private string databaseTimeout;
        private int sensorPollInterval;

        public Service1(string[] args)
        {
            InitializeComponent();
            InitializeComponent();
            eventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("Sensorcom"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "Sensorcom", "Sensorcom");
            }
            eventLog.Source = "Sensorcom";
            eventLog.Log = "Sensorcom";
        }
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun = new ServiceBase[] { new Service1(args) };
            ServiceBase.Run(ServicesToRun);
        }

        protected override void OnStart(string[] args)
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog.WriteEntry("Sensorcom collector starting", EventLogEntryType.Information, eventID++);
            ReadGeneralConfig();
            //create thread for each sensor in sensors
            //use onTimer to poll all sensors every interval
            System.Timers.Timer pollTimer = new System.Timers.Timer();
            pollTimer.Interval = sensorPollInterval;
            pollTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            pollTimer.Start();

            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);  
        }

        protected override void OnStop()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog.WriteEntry("Sensorcom collector stopping", EventLogEntryType.Information, eventID++);

            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            //poll sensor again
        }
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        Tuple<double, double, String> PollSensor(Sensor sensor) //tuple allows to return multiple values
        {
            byte[] rawData = RequestData(sensor);
            double voltage = CalculateVoltage(CalculateRegisterValue(rawData));
            double sensorReading = GetSensorReading(voltage, sensor.SensorType);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");
            var toReturn = Tuple.Create(voltage, sensorReading, timestamp);
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

        double CalculateVoltage(ushort regValue)
        { //calculates voltage based off register value received
            double voltage = regValue / 409.5; //http://www.proconel.com/Industrial-Data-Acquisition-Products/MODBUS-TCP-I-O-Modules/PM8AI-V-ISO---8-Voltage-Input-Module-Fully-Isolate.aspx states that 819 = 2v
            Console.WriteLine("Voltage is: " + voltage.ToString()); //for test purposes
            return voltage;
        }

        double GetSensorReading(double voltage, int sensorType)
        {
            double reading = 0;
            switch (sensorType)
            {
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

        double CalculateHumidity(double voltage)
        {
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

        byte[] RequestData(Sensor sensor)
        { //sends request to a modmux device. returns modmux response
            byte upperTransIdentifier = 0b0;
            byte transIdentifier = (byte)0;
            byte protocolIdentifier = 0b0;
            byte upperHeaderLength = 0b0;
            byte lowerHeaderLength = 0b110;
            byte unitIdentifier = 0b1;
            byte functionCode = 0b100;
            byte register = (byte)sensor.Register;
            byte[] received = new byte[0];
            try
            {
                TcpClient client = new TcpClient(sensor.Address, sensor.Port); //add try-catch
                NetworkStream nwStream = client.GetStream();
                byte[] request = new byte[] { upperTransIdentifier, transIdentifier, protocolIdentifier, protocolIdentifier,
                upperHeaderLength, lowerHeaderLength, unitIdentifier, functionCode, 0b0, register, 0b0, 0b1};
                nwStream.Write(request, 0, request.Length);
                received = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(received, 0, client.ReceiveBufferSize);
                client.Close();
            }catch(Exception e)
            {
                eventLog.WriteEntry("An error occured while reading data from a sensor. Error message is: " + e.ToString(), EventLogEntryType.Error, eventID++);
            }
            return received;
        }

        void ReadSensorConfig()
        { //reads configuration from the sensors table in the DB
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                eventLog.WriteEntry("An error occured while reading sensor information from database. Error message is: " + e.ToString(), EventLogEntryType.Error, eventID++);
            }
            eventLog.WriteEntry("successfully read sensor configuration from database", EventLogEntryType.Information, eventID++);
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
                eventLog.WriteEntry("An error occured while reading general configuration. Error message is: " + e.ToString(), EventLogEntryType.Error, eventID++);
                Console.WriteLine(e.ToString());
            }
            eventLog.WriteEntry("successfully read general configuration", EventLogEntryType.Information, eventID++);
        }

        void SendToDatabase(Sensor sensor, Tuple<double, double, String> result)
        {
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
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                eventLog.WriteEntry("An error occured while sending information to the database. Error message is: " + e.ToString(), EventLogEntryType.Error, eventID++);
            }
        }
    }
}
