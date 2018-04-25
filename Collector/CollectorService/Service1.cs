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
    /// <summary>
    /// Main windows service for collecting data from sensors
    /// </summary>
    public partial class Service1 : ServiceBase
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
       // private System.ComponentModel.IContainer components;
        private System.Diagnostics.EventLog eventLog;
        private int eventID = 1;
        private string databaseHost;
        private string databaseName;
        private string databaseUser;
        private string databasePassword;
        private string databaseTimeout;
        private int sensorPollInterval;
        private Sensor[] sensors;
        private System.Timers.Timer pollTimer;

        /// <summary>
        /// Constructor. initialises components and sets up event log reporting
        /// </summary>
        /// <param name="args">aruments passed to the service at startup (unused)</param>
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

        /// <summary>
        /// Main method for starting the service
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun = new ServiceBase[] { new Service1(args) };
            ServiceBase.Run(ServicesToRun);
        }

        /// <summary>
        /// Method called when service is started.
        /// sensor config read from database.
        /// timer initialised to call sensor polling method when elapsed.
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog.WriteEntry("Sensorcom collector starting", EventLogEntryType.Information, eventID++);
            ReadGeneralConfig();
            sensors = ReadSensorConfig();
            pollTimer = new System.Timers.Timer();
            pollTimer.Interval = sensorPollInterval;
            WhileRunning();
        }

        /// <summary>
        /// Method called when service is stopped.
        /// sensor polling timer stopped before service is stopped
        /// </summary>
        protected override void OnStop()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog.WriteEntry("Sensorcom collector stopping", EventLogEntryType.Information, eventID++);
            pollTimer.Stop();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /// <summary>
        /// Polls all sensors when timer elapses
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
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
            Task.WaitAll(tasks);
        }

        /// <summary>
        /// Method executed when service status changes to running.
        ///poll timer started here
        /// </summary>
        public void WhileRunning()
        {
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            pollTimer.Start();
            pollTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
        }

        /// <summary>
        /// Different service states
        /// </summary>
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

        /// <summary>
        /// object to represent service status
        /// </summary>
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

        /// <summary>
        /// Method for wrapping whole sensor communication process
        /// </summary>
        /// <param name="sensor">the sensor to collect data for</param>
        void PollModbusSensor(Sensor sensor)
        {
            var result = ReadModbusSensor(sensor);
            if(result != null)
            {
                SendToDatabase(sensor, result);
            }
        }

        /// <summary>
        /// Method for receiving data from a sensor
        /// </summary>
        /// <param name="sensor">sensor to communicate with</param>
        /// <returns>returns tuple containing register value, sensor reading and timestamp of data</returns>
        Tuple<double, double, String> ReadModbusSensor(Sensor sensor) //tuple allows to return multiple values
        {
            byte[] rawData = RequestData(sensor);
            if(rawData != null)
            {
                double regValue = CalculateRegisterValue(rawData);
                double sensorReading = GetSensorReading(regValue, sensor);
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var toReturn = Tuple.Create(regValue, sensorReading, timestamp);
                return toReturn;
            }
            return null;
        }

        /// <summary>
        /// Calculates register value based off raw TCP data received
        /// </summary>
        /// <param name="rawData">raw TCP data received from sensor</param>
        /// <returns>returns register value as double</returns>
        public double CalculateRegisterValue(byte[] rawData)
        { //calculates register value based off of bits in packet (uses little endian)
            ushort regValue;
            byte[] toAdd = new byte[2]; //always 2 bytes of data returned (modmux modules use 12 bits to send value)
            toAdd = new byte[2] { rawData[10], rawData[9] }; //always 9th and 10th bit for modmux devices
            regValue = BitConverter.ToUInt16(toAdd, 0); //concatenate bits to form 16 bit word
            return Convert.ToDouble(regValue);
        }

        //unused
        /*double CalculateVoltage(ushort regValue)
        { //calculates voltage based off register value received
            double voltage = regValue / 409.5; //http://www.proconel.com/Industrial-Data-Acquisition-Products/MODBUS-TCP-I-O-Modules/PM8AI-V-ISO---8-Voltage-Input-Module-Fully-Isolate.aspx states that 819 = 2v
            return voltage;
        }*/

            /// <summary>
            /// Calculates sensor reading based on sensor type
            /// </summary>
            /// <param name="regValue">calculated register value</param>
            /// <param name="sensor">sensor to calculate reading for</param>
            /// <returns>sensor reading</returns>
        public double GetSensorReading(double regValue, Sensor sensor)
        {
            double reading = 0;
            switch (sensor.SensorType)
            {
                case 0:
                    reading = CalculateTemperature(regValue);
                    break;
                case 1:
                    reading = CalculateHumidity(regValue, sensor.Scale, sensor.Offset);
                    break;
                case 2:
                    reading = CalculatePressure(regValue, sensor.Scale, sensor.Offset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Sensor.SensorType");
            }
            return reading;
        }

        /// <summary>
        /// Calculates humidity value based on register value, sensor scale and sensor offset
        /// </summary>
        /// <param name="regValue">calculated register value</param>
        /// <param name="scale">sensor scale</param>
        /// <param name="offset">sensor offset</param>
        /// <returns>returns humidity value as a double</returns>
        public double CalculateHumidity(double regValue, double scale, double offset)
        {
            //(regValue * scale) + offset
            double humidity = (regValue*scale) + offset;
            return humidity;
        }

        /// <summary>
        /// Calculates pressure value based on register value, sensor scale and sensor offset
        /// </summary>
        /// <param name="regValue">calculated register value</param>
        /// <param name="scale">sensor scale</param>
        /// <param name="offset">sensor offset</param>
        /// <returns>returns pressure value as a double</returns>
        public double CalculatePressure(double regValue, double scale, double offset)
        {
            //(regValue * scale) + offset
            double pressure = (regValue*scale) + offset;
            return pressure;
        }

        /// <summary>
        /// Calculates temperature reading by dividing register value by 10
        /// </summary>
        /// <param name="regValue">calculated register value</param>
        /// <returns>temperature reading as a double</returns>
        public double CalculateTemperature(double regValue)
        {
            double temperature = regValue/10;
            return temperature;
        }

        /// <summary>
        /// Method to send a TCP message to a sensor and receive the reply
        /// message constructed according to the modbus application protocol
        /// </summary>
        /// <param name="sensor">sensor to communicate with</param>
        /// <returns>raw TCP response as byte array</returns>
        public byte[] RequestData(Sensor sensor)
        { //sends request to a modmux device. returns modmux response
            byte upperTransIdentifier = 0b0;
            byte transIdentifier = (byte)0;
            byte protocolIdentifier = 0b0;
            byte upperHeaderLength = 0b0;
            byte lowerHeaderLength = 0b110;
            byte unitIdentifier = 0b1;
            byte functionCode = 0b100;
            byte register = (byte)sensor.Register;
            try
            {
                TcpClient client = new TcpClient(sensor.Address, sensor.Port); //add try-catch
                NetworkStream nwStream = client.GetStream();
                byte[] request = new byte[] { upperTransIdentifier, transIdentifier, protocolIdentifier, protocolIdentifier,
                upperHeaderLength, lowerHeaderLength, unitIdentifier, functionCode, 0b0, register, 0b0, 0b1};
                nwStream.Write(request, 0, request.Length);
                byte[] received = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(received, 0, client.ReceiveBufferSize);
                client.Close();
                byte[] toReturn = received.Take(13).ToArray();
                return toReturn;
            }
            catch (Exception e)
            {
                eventLog.WriteEntry("An error occured while reading data from a sensor. Error message is: " + e.ToString(), EventLogEntryType.Error, eventID++);
            }
            finally
            {
                GC.Collect();
            }
            return null;
        }

        /// <summary>
        /// Method for reading all active sensors from the database
        /// </summary>
        /// <returns>Array of sensor objects</returns>
        public Sensor[] ReadSensorConfig()
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
                    if(returned.GetInt32(5) != 0){ //if it has a modbus connection entry - get it and class it as a modbus sensor
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
                eventLog.WriteEntry("An error occured while reading sensor information from database. Error message is: " + e.ToString(), EventLogEntryType.Error, eventID++);
                throw new System.NullReferenceException();
            }
            finally
            {
                GC.Collect();
            }
            eventLog.WriteEntry("successfully read sensor configuration from database", EventLogEntryType.Information, eventID++);
            Sensor[] sensorArray = sensors.ToArray();
            if(sensorArray.Length == 0)
            {
                eventLog.WriteEntry("No sensors to poll. stopping", EventLogEntryType.Error, eventID++);
                throw new NullReferenceException();
            }
            return sensorArray;
        }

        /// <summary>
        /// Reads general configuration (database connection details, poll interval, etc) from app.config
        /// </summary>
        public void ReadGeneralConfig()
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
                throw new Exception(e.ToString());
            }
            finally
            {
                GC.Collect();
            }
            eventLog.WriteEntry("successfully read general configuration", EventLogEntryType.Information, eventID++);
        }

        /// <summary>
        /// Sends sensor reading to database
        /// </summary>
        /// <param name="sensor">the sensor object the reading was collected from</param>
        /// <param name="result">the reading, register value and timestamp of the collected data</param>
        public void SendToDatabase(Sensor sensor, Tuple<double, double, String> result)
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
                eventLog.WriteEntry("An error occured while sending information to the database. Error message is: " + e.ToString(), EventLogEntryType.Error, eventID++);
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// Used in unit tests
        /// </summary>
        public void SetValidGeneralConfigForTesting()
        {
            databaseHost = "169.254.121.230";
            databaseName = "Sensorcom";
            databaseUser = "sensorcom";
            databasePassword = "password";
            databaseTimeout = "10";
            sensorPollInterval = 1000;
        }

        /// <summary>
        /// Used in unit testing
        /// </summary>
        public void SetInvalidGeneralConfigForTesting()
        {
            databaseHost = "169.21.230";
            databaseName = "Sensorcom";
            databaseUser = "sensom";
            databasePassword = "pasord";
            databaseTimeout = "10";
            sensorPollInterval = 1000;
        }

    }
}
