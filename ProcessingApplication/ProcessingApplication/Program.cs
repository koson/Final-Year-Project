using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
using System.Xml.Serialization;
using Excel = Microsoft.Office.Interop.Excel;

namespace ProcessingApplication
{
    public class Program
    {
        private string databaseHost;
        private string databaseName;
        private string databaseUser;
        private string databasePassword;
        private string databaseTimeout;
        [XmlArray("Chambers")]
        [XmlArrayItem("Chamber")]
        private Chamber[] chambers;
        private String connectionString;
        bool debug;
        bool interactive;

        public static void Main(string[] args)
        {
            Program p = new Program();
            p.Initialise();
            p.GetEnvironment();
            p.HandleArguments(args);
        }

        public void HandleArguments(string[] args)
        {
            int offset = 0;

            if (args.Length > 1)
            {
                if (args[0] == "-v" ) //verbose mode
                {
                    offset++;
                    interactive = true;
                    debug = true;
                }
            }
            switch (args[0 + offset])
            {

                case "getEnv":
                    Console.Write(BuildXML(chambers, true));
                    break;

                case "produceGraph":
                    string filename = null;
                    int chamberID = 0;
                    DateTime startDate = DateTime.Now;
                    DateTime endDate = DateTime.Now;
                    bool averageValues = false;         //placeholder values
                    bool exportToExcel = false;

                    bool parseSuccessful = false; //set to true if command line arguments are correctly parsed

                    try
                    {
                        chamberID = int.Parse(args[1 + offset]);
                        startDate = DateTime.Parse(args[2 + offset]);
                        endDate = DateTime.Parse(args[3 + offset]);
                        averageValues = Boolean.Parse(args[4 + offset]);
                        exportToExcel = Boolean.Parse(args[5 + offset]);
                        if (exportToExcel)
                        {
                            filename = args[6 + offset];
                        }
                        if (startDate < endDate)
                        {
                            parseSuccessful = true;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch (Exception e)
                    {
                        if (interactive || debug)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        Chamber c = GetChamberByID(chamberID);
                        if (c != null)
                        {
                            DataSet[] graphData = ProduceGraphData(GetChamberByID(chamberID), startDate, endDate, averageValues);
                            if (exportToExcel)
                            {
                                ExportToExcel(graphData, filename, GetChamberByID(chamberID).Description);
                                Console.Write(BuildXML(null, true));
                            }
                            else
                            {
                                Console.Write(BuildXML(graphData, true));
                            }
                        }
                        else
                        {
                            BuildXML(null, false);
                        }

                    }
                    break;

                case "addChamber":
                    String chamberDescription = null;
                    String chamberName = null; //placeholder values
                    parseSuccessful = false;

                    try
                    {
                        chamberDescription = args[1 + offset];
                        chamberName = args[2 + offset];
                        parseSuccessful = true;
                    }
                    catch (Exception e)
                    {
                        if (interactive || debug)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(BuildXML(e, false));
                    }
                    if (parseSuccessful)
                    {
                        Console.Write(BuildXML(null, AddChamber(chamberName, chamberDescription)));
                    }
                    break;

                case "editChamber":
                    chamberID = 0;
                    chamberName = null;
                    chamberDescription = null;
                    parseSuccessful = false;

                    try
                    {
                        chamberID = int.Parse(args[1 + offset]);
                        chamberDescription = args[3 + offset];
                        chamberName = args[2 + offset];
                        parseSuccessful = true;
                    }
                    catch (Exception e)
                    {
                        if (interactive || debug)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        Console.Write(BuildXML(null, EditChamber(chamberID, chamberName, chamberDescription)));
                    }
                    break;

                case "removeChamber":
                    chamberID = 0;
                    parseSuccessful = false;
                    char confirm = ' ';
                    try
                    {
                        chamberID = int.Parse(args[1 + offset]);
                        parseSuccessful = true;
                    }
                    catch (Exception e)
                    {
                        if (interactive || debug)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        if(interactive || debug)
                        {
                            while (confirm != 'y' || confirm != 'n')
                            {
                                Console.WriteLine("Warning - this action will remove all associated sensors and the collected data for each sensor as well");
                                Console.WriteLine("Are you sure you want to remove chamber " + chamberID + ": " + GetChamberByID(chamberID).Name + "?");
                                confirm = Console.Read().ToString().ElementAt(0);
                            }
                            if (confirm == 'y')
                            {
                                Console.Write(BuildXML(null, RemoveChamber(chamberID)));
                            }
                        }
                        else
                        {
                            Console.Write(BuildXML(null, RemoveChamber(chamberID)));
                        }
                        
                    }
                    break;

                case "addSensor":
                    String sensorAddress = null;
                    int sensorPort = 0;
                    int sensorType = 0;
                    chamberID = 0;
                    int sensorRegister = 0;
                    double sensorScale = 0;
                    double sensorOffset = 0;
                    String sensorDescription = null;
                    bool sensorEnabled = true;
                    parseSuccessful = false;

                    try
                    {
                        sensorAddress = args[1 + offset];
                        sensorPort = int.Parse(args[2 + offset]);
                        sensorType = int.Parse(args[3 + offset]);
                        chamberID = int.Parse(args[4 + offset]);
                        sensorRegister = int.Parse(args[5 + offset]);
                        sensorScale = double.Parse(args[6 + offset]);
                        sensorOffset = double.Parse(args[7 + offset]);
                        sensorDescription = args[8 + offset];
                        sensorEnabled = bool.Parse(args[9 + offset]);
                        parseSuccessful = true;
                    }
                    catch (Exception e)
                    {
                        if (interactive || debug)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        Console.Write(BuildXML(null, AddModbusSensor(sensorAddress, sensorPort, sensorType, chamberID, sensorRegister, sensorScale, sensorOffset, sensorDescription, sensorEnabled)));
                    }
                    break;

                case "editSensor":
                    int sensorID = 0;
                    sensorDescription = null;
                    sensorEnabled = true;
                    sensorType = 0;
                    chamberID = 0;
                    sensorAddress = null;
                    sensorPort = 0;
                    sensorRegister = 0;
                    sensorScale = 0;
                    sensorOffset = 0;
                    parseSuccessful = false;

                    try
                    {
                        sensorID = int.Parse(args[1 + offset]);
                        sensorDescription = args[2 + offset];
                        sensorEnabled = bool.Parse(args[3 + offset]);
                        sensorType = int.Parse(args[4 + offset]);
                        chamberID = int.Parse(args[5 + offset]);
                        sensorAddress = args[6 + offset];
                        sensorPort = int.Parse(args[7 + offset]);
                        sensorRegister = int.Parse(args[8 + offset]);
                        sensorScale = double.Parse(args[9 + offset]);
                        sensorOffset = double.Parse(args[10 + offset]);
                        parseSuccessful = true;
                    }
                    catch (Exception e)
                    {
                        if (interactive || debug)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        Console.Write(BuildXML(null, EditModbusSensor(sensorID, sensorDescription, sensorEnabled, sensorType, chamberID, sensorAddress, sensorPort, sensorRegister, sensorScale, sensorOffset)));
                    }
                    break;

                case "removeSensor":
                    sensorID = 0;
                    confirm = ' ';
                    parseSuccessful = false;
                    try
                    {
                        sensorID = int.Parse(args[1 + offset]);
                        parseSuccessful = true;
                    }
                    catch (Exception e)
                    {
                        if (interactive || debug)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        if(interactive || debug)
                        {
                            while (confirm != 'y' || confirm != 'n')
                            {
                                Console.WriteLine("Warning - this action will remove all collected data for the sensor as well");
                                Console.WriteLine("Are you sure you want to permanently remove sensor " + sensorID + ": " + GetSensorByID(sensorID).Description + "?");
                                confirm = Console.Read().ToString().ElementAt(0);
                            }
                            if (confirm == 'y')
                            {
                                Console.Write(BuildXML(null, RemoveModbusSensor(sensorID)));
                            }
                        }
                        else
                        {
                            Console.Write(BuildXML(null, RemoveModbusSensor(sensorID)));
                        }
                        
                    }
                    break;

                case "help":
                    if (args.Length == (2 + offset))
                    {
                        PrintUsageText(args[1 + offset]);
                    }
                    else
                    {
                        PrintUsageText("");
                    }
                    break;

                default:
                    PrintUsageText("");
                    break;
            }
        }

        public void Initialise()
        {
            ReadProgramConfig();
            interactive = false;
            debug = false;
            connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
        }

        public void PrintUsageText(string commandName) //prints help guide on command line
        {
            switch (commandName)
            {
                case "general":
                    Console.WriteLine("USAGE: ProcessingApplication.exe [-v] [command] [options]");
                    Console.WriteLine("\n");
                    Console.WriteLine("Commands:");
                    Console.WriteLine(" - getEnv: Retrieve all chambers and sensors from database");
                    Console.WriteLine(" - produceGraph: Produce an XML representation of graph data");
                    Console.WriteLine(" - addChamber: Create a new chamber in the database");
                    Console.WriteLine(" - editChamber: Edit an existing chamber in the database");
                    Console.WriteLine(" - removeChamber: Remove a chamber from the database (will also delete associated sensors and collected data)");
                    Console.WriteLine(" - addSensor: Create a new sensor in the database");
                    Console.WriteLine(" - editSensor: Edit an existing sensor in the database");
                    Console.WriteLine(" - removeSensor: remove a sensor from the database (will also delete collected data for this sensor)");


                    break;

                case "produceGraph":
                    Console.WriteLine("USAGE: ProcessingApplication.exe produceGraph chamberID startDate endDate averageValues exportToExcel [filename]");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - chamberID: The ID of the chamber you want to graph data from");
                    Console.WriteLine(" - startDate: The date you want the graph to start from (format is \"yyyy-MM-dd hh:mm:ss\")");
                    Console.WriteLine(" - endDate: The date you want the graph to end at (format is \"yyyy-MM-dd hh:mm:ss\")");
                    Console.WriteLine(" - averageValues: If you want graph data to be averaged for one value every minute (true/false)");
                    Console.WriteLine(" - exportToExcel: If you want the graph to be saved to an Excel spreadsheet (true/false)");
                    Console.WriteLine(" - filename: full path to the sheet you want to save. Only used when exportToExcel is true");
                    break;

                case "addChamber":
                    Console.WriteLine("USAGE: ProcessingApplication.exe addChamber name description");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - name: The name of the new chamber");
                    Console.WriteLine(" - description: The description for the new chamber");
                    break;

                case "editChamber":
                    Console.WriteLine("USAGE: ProcessingApplication.exe editChamber ID description name");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - ID: The ID of the existing chamber to edit");
                    Console.WriteLine(" - description: The new description for the chamber");
                    Console.WriteLine(" - name: The new name for the chamber");
                    break;

                case "removeChamber":
                    Console.WriteLine("USAGE: ProcessingApplication.exe removeChamber ID");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - ID: The ID of the chamber to remove");
                    break;

                case "addSensor":
                    Console.WriteLine("USAGE: ProcessingApplication.exe addSensor address port type chamberID register scale offset description enabled");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - address: The IP address of the new sensor");
                    Console.WriteLine(" - port: The network port of the new sensor");
                    Console.WriteLine(" - type: The type of the new sensor");
                    Console.WriteLine(" - chamberID: The ID of the chamber the new sensor belongs to");
                    Console.WriteLine(" - register: The modbus register of the new sensor");
                    Console.WriteLine(" - scale: The scale value of the new sensor");
                    Console.WriteLine(" - offset: The offset value of the new sensor");
                    Console.WriteLine(" - description: The description of the new sensor");
                    Console.WriteLine(" - enabled: If data should be collected from the new sensor");
                    break;

                case "editSensor":
                    Console.WriteLine("USAGE: ProcessingApplication.exe editSensor ID description enabled type chamberID address port register scale offset");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - address: The ID of the sensor to edit");
                    Console.WriteLine(" - description: The description of the new sensor");
                    Console.WriteLine(" - enabled: If data should be collected from the new sensor");
                    Console.WriteLine(" - type: The type of the new sensor");
                    Console.WriteLine(" - chamberID: The ID of the chamber the new sensor belongs to");
                    Console.WriteLine(" - address: The IP address of the new sensor");
                    Console.WriteLine(" - port: The network port of the new sensor");
                    Console.WriteLine(" - register: The modbus register of the new sensor");
                    Console.WriteLine(" - scale: The scale value of the new sensor");
                    Console.WriteLine(" - offset: The offset value of the new sensor");
                    
                    break;

                case "removeSensor":
                    Console.WriteLine("USAGE: ProcessingApplication.exe removeSensor ID");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - ID: The ID of the sensor to remove");
                    break;
                default:
                    Console.WriteLine("USAGE: ProcessingApplication.exe [-v] [command] [options]");
                    Console.WriteLine("\n");
                    Console.WriteLine("Commands:");
                    Console.WriteLine(" - getEnv: Retrieve all chambers and sensors from database");
                    Console.WriteLine(" - produceGraph: Produce an XML representation of graph data");
                    Console.WriteLine(" - addChamber: Create a new chamber in the database");
                    Console.WriteLine(" - editChamber: Edit an existing chamber in the database");
                    Console.WriteLine(" - removeChamber: Remove a chamber from the database (will also delete associated sensors and collected data)");
                    Console.WriteLine(" - addSensor: Create a new sensor in the database");
                    Console.WriteLine(" - editSensor: Edit an existing sensor in the database");
                    Console.WriteLine(" - removeSensor: remove a sensor from the database (will also delete collected data for this sensor)");
                    break;
            }
            
        }

        public void GetEnvironment() //get all chambers and sensors in database (no actual reportable data)
        {
            chambers = GetChamberList();
            LoadAllSensors();
        }
        public Sensor[] GetSensorsForChamber(Chamber chamber)
        {
            //get list of all sensors for a chamber
            List<Sensor> sensors = new List<Sensor>();
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
                connection2.Close();
            }
            catch (Exception e)
            {
                if(interactive || debug)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            finally
            {
                GC.Collect();
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
        public Chamber[] GetChamberList()
        {
            //get list of all chambers
            List<Chamber> chambers = new List<Chamber>();
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
                if(interactive || debug)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            finally
            {
                GC.Collect();
            }
            return chambers.ToArray();
        }

        public DataSet GetDataForSensor(Sensor s, DateTime startTime, DateTime endTime)
        {
            //get all data for a sensor in a timerange
            List<DataItem> returnedData = new List<DataItem>();
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
                if (interactive || debug)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            finally
            {
                GC.Collect();
            }
            return new DataSet(returnedData.ToArray(), s.ID);
        }
        
        public Chamber GetChamberByID(int chamberID)
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

        public Sensor GetSensorByID(int sensorID)
        {
            Sensor s = null;
            for(int i = 0; i < chambers.Length; i++)
            {
                for(int j = 0; j < chambers[i].sensors.Length; j++)
                {
                    if (chambers[i].sensors[j].ID.Equals(sensorID))
                    {
                        s = chambers[i].sensors[j];
                    }
                }
            }
            return s;
        }

        DataSet[] ProduceGraphData(Chamber c, DateTime start, DateTime end, Boolean average)
        {
            DataSet tempDataSet = new DataSet();
            List<DataSet> finalValues = new List<DataSet>();
            for (int i = 0; i < c.sensors.Length; i++)
            {
                tempDataSet = GetDataForSensor(c.sensors[i], start, end);
                if(tempDataSet.Data.Length != 0 && average)
                {
                    tempDataSet = ProduceMeanValues(tempDataSet);  
                }
                tempDataSet.SensorID = c.sensors[i].ID;
                finalValues.Add(tempDataSet);
            }
            return finalValues.ToArray();
        }

        //change to allow custom time intervals to average (based off user input in GUI)?
        public DataSet ProduceMeanValues(DataSet dataToAverage) //mean of all values for certain sensor type per minute within the time range
        {
            DataSet meanValues = new DataSet();
            DateTime earliestTime = dataToAverage.Data[0].Timestamp.AddSeconds(-(dataToAverage.Data[0].Timestamp.Second)); //earliest whole minute
            DateTime latestTime = dataToAverage.Data[dataToAverage.Data.Length-1].Timestamp.AddSeconds(60- dataToAverage.Data[dataToAverage.Data.Length - 1].Timestamp.Second); //latest whole minute
            TimeSpan minutesDifference = latestTime.Subtract(earliestTime); //amount of minutes in between
            if (debug)
            {
                Console.WriteLine(earliestTime);
                Console.WriteLine(latestTime);
                Console.WriteLine(minutesDifference.TotalMinutes);
            }

            for(int i = 0; i < minutesDifference.TotalMinutes; i++) //calculate mean value for each minute
            {
                List<Double> valuesToMean = new List<Double>();
                for (int j = 0; j < dataToAverage.Data.Length; j++)
                {
                    if (dataToAverage.Data[j].Timestamp < (earliestTime.AddMinutes(i+1)) && dataToAverage.Data[j].Timestamp >= earliestTime.AddMinutes(i)) //search all data for each sensor and add any within minute range to valuesToMean
                    {
                        if (debug)
                        {
                            Console.WriteLine(dataToAverage.Data[j].Timestamp + ": " + dataToAverage.Data[j].Reading);
                        }
                        valuesToMean.Add(dataToAverage.Data[j].Reading);
                    }
                }

                Double mean = Enumerable.Average(valuesToMean); //average values
                meanValues.AddDataItem(new DataItem(mean, earliestTime.AddMinutes(i))); //add to array of points to plot on graph (gets returned by function)
                if (debug)
                {
                    Console.WriteLine(mean);
                }
            }

            return meanValues;
        }

        public void ExportToExcel(DataSet[] chartData, String filename, String chamberName)  //experimental version to test using 3 different series of time
        {
            //create workbook
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Worksheet xlWorkSheet2;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlApp.DisplayAlerts = false;
            xlApp.Visible = false;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = "Data";
            xlWorkSheet2 = xlWorkBook.Sheets.Add(misValue, misValue, 1, misValue) as Excel.Worksheet;
            xlWorkSheet2.Name = "Chart";

            //add data
            xlWorkSheet.Cells[1, 1] = "Time";
            DateTime[] dates = SortDates(chartData);
            //add times
            for(int i = 0; i < dates.Length; i++)
            {
                xlWorkSheet.Cells[(i+2), 1] = dates[i].ToString();
            }

            //add data to sheet where timestamp matches time
            for(int i = 0; i < chartData.Length; i++)
            {
                int sensorID = chartData[i].SensorID;
                int sensorType = GetSensorByID(chartData[i].SensorID).SensorType;
                switch (sensorType)
                {
                    case 0:
                        xlWorkSheet.Cells[1, (i + 2)] = "Temperature Sensor " + sensorID.ToString();
                        break;

                    case 1:
                        xlWorkSheet.Cells[1, (i + 2)] = "Pressure Sensor " + sensorID.ToString();
                        break;

                    case 2:
                        xlWorkSheet.Cells[1, (i + 2)] = "Humidity Sensor " + sensorID.ToString();
                        break;
                }
                for (int j = 0; j < chartData[i].Data.Length; j++)
                {
                    for(int k = 0; k < dates.Length; k++)
                    {  
                        if (String.Equals(dates[k].ToString(), chartData[i].Data[j].Timestamp.ToString()))
                        {
                            if (debug)
                            {
                                Console.WriteLine("Date is: " + dates[k].ToString());
                                Console.WriteLine("Cell is: [" + (k + 2) + "," + (i + 2) + "]");
                                Console.WriteLine("Reading is: " + chartData[i].Data[j].Reading);
                            }
                            xlWorkSheet.Cells[(k+2), (i+2)] = chartData[i].Data[j].Reading;
                            k = dates.Length;
                        }
                    }
                }
            }

            Excel.Range allDataRange = xlWorkSheet.UsedRange;
           // allDataRange.Sort(allDataRange.Columns[1], Excel.XlSortOrder.xlAscending, misValue, misValue, Excel.XlSortOrder.xlAscending, misValue, Excel.XlSortOrder.xlAscending, Excel.XlYesNoGuess.xlYes); //sort by time, excluding first row (header)

            Excel.ChartObjects xlCharts = xlWorkSheet2.ChartObjects(Type.Missing);
            Excel.ChartObject myChart = xlCharts.Add(0, 0, 900, 500);
            Excel.Chart chartPage = myChart.Chart;
            chartPage.HasTitle = true;
            chartPage.ChartTitle.Text = "<JOBNUM-LINENUM> <CUSTOMER> - " + chamberName +  " Chart";
            chartPage.SetSourceData(allDataRange, misValue);
            chartPage.ChartType = Excel.XlChartType.xlLine;
            Excel.Axis xAxis = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            xAxis.CategoryType = Excel.XlCategoryType.xlCategoryScale;
            xAxis.HasTitle = true;
            xAxis.AxisTitle.Text = "Time";
            xAxis.TickLabelPosition = Excel.XlTickLabelPosition.xlTickLabelPositionLow;
            xAxis.TickLabelSpacing = (int)(dates.Length / 15.11);
            xAxis.TickMarkSpacing = (int)(dates.Length / 15.11);
            xAxis.HasMajorGridlines = true;

            Excel.Axis yAxis1 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            yAxis1.HasTitle = true;
            yAxis1.AxisTitle.Text = "Temperature (°C) & Humidity (%)";
            yAxis1.AxisTitle.Orientation = Excel.XlOrientation.xlUpward;
            yAxis1.MaximumScale = 100;
            yAxis1.MinimumScale = -40;

            Boolean pressureData = false;
            //make all pressure data bound to secondary axis
            for (int i = 0; i < chartData.Length; i++)
            {
                String cellValue = (string)(xlWorkSheet.Cells[1, (i + 2)] as Excel.Range).Value;
                if (cellValue == ("Pressure Sensor " + chartData[i].SensorID))
                {
                    chartPage.SeriesCollection(("Pressure Sensor " + chartData[i].SensorID)).AxisGroup = Excel.XlAxisGroup.xlSecondary;
                    if (debug)
                    {
                        Console.WriteLine("Pressure Data");
                    }
                    pressureData = true;
                }
            }

            if (pressureData)
            {
                Excel.Axis yAxis2 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary);
                yAxis2.HasTitle = true;
                yAxis2.AxisTitle.Text = "Pressure (mbar) (a)";
                yAxis2.AxisTitle.Orientation = Excel.XlOrientation.xlUpward;
                yAxis2.MinimumScale = 0;
                yAxis2.MaximumScale = 1000;
            }

            xlWorkBook.SaveAs(filename, Excel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            GC.Collect();
        }

        public DateTime[] SortDates(DataSet[] chartData)
        {
            List<DateTime> dates = new List<DateTime>();
            for(int i = 0; i < chartData.Length; i++)
            {
                for(int j = 0; j < chartData[i].Data.Length; j++)
                {
                    if (!dates.Contains(chartData[i].Data[j].Timestamp)) //add each date only once
                    {
                        dates.Add(chartData[i].Data[j].Timestamp);
                    }
                }
            }
            dates.Sort();
            return dates.ToArray();
        }

        public Boolean AddChamber(String name, String description)
        {
            String query = "INSERT INTO Chamber (Name, Description) VALUES (@Name, @Description);";
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Name", System.Data.SqlDbType.VarChar);
            parameters[0].Value = name;
            parameters[1] = new SqlParameter("@Description", System.Data.SqlDbType.VarChar);
            parameters[1].Value = description;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            for (int i = 0; i < parameters.Length; i++)
            {
                cmd.Parameters.Add(parameters[i]);
            }
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                if (debug)
                {
                    Console.WriteLine(e.ToString());
                }
                return false;
            }
            finally
            {
                GC.Collect();
            }
        }

        public Boolean AddModbusSensor(String address, int port, int type, int chamberID, int register, double scale, double offset, String description, Boolean enabled) //add sensor to database
        {
            if(GetChamberByID(chamberID) != null)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlParameter[] parameters = new SqlParameter[9];
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
                parameters[8] = new SqlParameter("@Enabled", System.Data.SqlDbType.Bit);
                if (enabled)
                {
                    parameters[8].Value = 1;
                }
                else
                {
                    parameters[8].Value = 0;
                }

                String query = "INSERT INTO Modbus_Info (IP_Address, Network_Port, Register, Scale, Offset) VALUES (@IP, @Port, @Register, @Scale, @Offset);";
                SqlCommand command = connection.CreateCommand();
                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.Add(parameters[i]);
                }
                try
                {
                    connection.Open();
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                    query = "SELECT TOP 1 Modbus_Info_ID FROM Modbus_Info ORDER BY Modbus_Info_ID DESC;"; //get last inserted modbus info record - the one that was just made
                    command.CommandText = query;
                    int modbusID = (int)command.ExecuteScalar();
                    query = "INSERT INTO Sensor (Name, Sensor_Enabled, Sensor_Type, Chamber_ID, Modbus_Info_ID) VALUES (@Description, @Enabled,  @Type, @ChamberID, " + modbusID.ToString() + ");";
                    command.CommandText = query;
                    command.ExecuteNonQuery();
                    command.Dispose();
                    connection.Close();
                    return true;
                }
                catch (Exception e)
                {
                    if (debug)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return false;
                }
                finally
                {
                    GC.Collect();
                }
            }
            else
            {
                return false;
            }
        }

        public Boolean RemoveModbusSensor(int sensorID) //delete Modbus_Info data as well
        {
            DeleteDataForSensor(sensorID);
            String getModbusInfoID = "SELECT Modbus_Info_ID FROM Sensor WHERE Sensor_ID = @ID;";
            String removeModBusInfo = "DELETE FROM Modbus_Info WHERE Modbus_Info_ID = @ModbusID;";
            String removeSensor = "DELETE FROM Sensor WHERE Sensor_ID = @ID";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlParameter id = new SqlParameter("@ID", System.Data.SqlDbType.Int);
            SqlParameter modbusID = new SqlParameter("@ModbusID", System.Data.SqlDbType.Int);
            id.Value = sensorID;
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = getModbusInfoID;
            cmd.Parameters.Add(id);
            try
            {
                connection.Open();
                modbusID.Value = cmd.ExecuteScalar();
                cmd.Parameters.Add(modbusID);
                cmd.CommandText = removeSensor;
                cmd.ExecuteNonQuery();
                cmd.CommandText = removeModBusInfo;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                connection.Close();
                return true;
            }catch(Exception e)
            {
                if (debug)
                {
                    Console.WriteLine(e.ToString());
                }
                return false;
            }
            finally
            {
                GC.Collect();
            }
        }

        public Boolean RemoveChamber(int chamberID)
        {
            if(GetChamberByID(chamberID) != null)
            {
                Sensor[] sensorsToDelete = GetSensorsForChamber(GetChamberByID(chamberID));
                SqlConnection connection = new SqlConnection(connectionString);
                String query = "DELETE FROM Chamber WHERE Chamber_ID = @ID";
                SqlParameter id = new SqlParameter("@ID", System.Data.SqlDbType.Int);
                id.Value = chamberID;
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.Parameters.Add(id);
                if (sensorsToDelete != null)
                {
                    for (int i = 0; i < sensorsToDelete.Length; i++)
                    {
                        Console.WriteLine("Removing sensor with ID" + sensorsToDelete[i].ID);
                        RemoveModbusSensor(sensorsToDelete[i].ID);
                    }
                }

                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    return true;
                }
                catch (Exception e)
                {
                    if (debug)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return false;
                }
                finally
                {
                    GC.Collect();
                }
            }
            else
            {
                return false;
            }
        }

        public Boolean EditModbusSensor(int sensorID, String name, bool enabled, int type, int chamberID, String address, int port, int register, double scale, double offset)
        {
            if(GetSensorByID(sensorID) != null)
            {
                String getModbusInfoID = "SELECT Modbus_Info_ID FROM Sensor WHERE Sensor_ID = @SensorID;";
                String updateSensor = "UPDATE Sensor SET Name = @Name, Sensor_Enabled = @Enabled, Sensor_Type = @Type, Chamber_ID = @ChamberID, Modbus_Info_ID = @ModbusID WHERE Sensor_ID = @SensorID;";
                String updateModbusInfo = "UPDATE Modbus_Info SET IP_Address = @Address, Network_Port = @Port, Register = @Register, Scale = @Scale, Offset = @Offset WHERE Modbus_Info_ID = @ModbusID;";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = connection.CreateCommand();
                SqlParameter[] parameters = new SqlParameter[11];
                parameters[0] = new SqlParameter("@SensorID", System.Data.SqlDbType.Int);
                parameters[0].Value = sensorID;
                parameters[1] = new SqlParameter("@Name", System.Data.SqlDbType.VarChar);
                parameters[1].Value = name;
                parameters[2] = new SqlParameter("@Enabled", System.Data.SqlDbType.Bit);
                if (enabled)
                {
                    parameters[2].Value = 1;
                }
                else
                {
                    parameters[2].Value = 0;
                }
                parameters[3] = new SqlParameter("@Type", System.Data.SqlDbType.Int);
                parameters[3].Value = type;
                parameters[4] = new SqlParameter("@ChamberID", System.Data.SqlDbType.Int);
                parameters[4].Value = chamberID;
                parameters[5] = new SqlParameter("@Address", System.Data.SqlDbType.VarChar);
                parameters[5].Value = address;
                parameters[6] = new SqlParameter("@Port", System.Data.SqlDbType.Int);
                parameters[6].Value = port;
                parameters[7] = new SqlParameter("@Register", System.Data.SqlDbType.Int);
                parameters[7].Value = register;
                parameters[8] = new SqlParameter("@Scale", System.Data.SqlDbType.Float);
                parameters[8].Value = scale;
                parameters[9] = new SqlParameter("@Offset", System.Data.SqlDbType.Float);
                parameters[9].Value = offset;
                parameters[10] = new SqlParameter("@ModbusID", System.Data.SqlDbType.Int);
                for (int i = 0; i < parameters.Length - 1; i++)
                {
                    cmd.Parameters.Add(parameters[i]);
                }
                cmd.CommandText = getModbusInfoID;
                try
                {
                    connection.Open();
                    parameters[10].Value = cmd.ExecuteScalar();
                    cmd.Parameters.Add(parameters[10]);
                    cmd.CommandText = updateSensor;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = updateModbusInfo;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    return true;
                }
                catch (Exception e)
                {
                    if (debug)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return false;
                }
                finally
                {
                    GC.Collect();
                }
            }
            else
            {
                return false;
            }
        }

        public Boolean DeleteDataForSensor(int sensorID)
        {
            String dataRecordQuery = "DELETE FROM Data_Record where Sensor_ID = @ID;";
            String calibrationRecordQuery = "DELETE FROM Calibration_Record where Sensor_ID = @ID;";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlParameter id = new SqlParameter("@ID", System.Data.SqlDbType.Int);
            id.Value = sensorID;
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = dataRecordQuery;
            cmd.Parameters.Add(id);
            try
            {
                connection.Open();  
                cmd.ExecuteNonQuery(); //Data_Record delete
                cmd.CommandText = calibrationRecordQuery;
                cmd.ExecuteNonQuery(); //Calibration_Record delete
                cmd.Dispose();
                connection.Close();
                return true;
            }
            catch (Exception e)
            {
                if (debug)
                {
                    Console.WriteLine(e.ToString());
                }
                return false;
            }
            finally
            {
                GC.Collect();
            }
        }

        public Boolean EditChamber(int chamberID, String name, String description)
        {
            if(GetChamberByID(chamberID) != null)
            {
                String query = "UPDATE Chamber SET Name = @Name, Description = @Description WHERE Chamber_ID = @ID;";
                SqlParameter[] parameters = new SqlParameter[3];
                parameters[0] = new SqlParameter("@ID", System.Data.SqlDbType.Int);
                parameters[0].Value = chamberID;
                parameters[1] = new SqlParameter("@Name", System.Data.SqlDbType.VarChar);
                parameters[1].Value = name;
                parameters[2] = new SqlParameter("@Description", System.Data.SqlDbType.VarChar);
                parameters[2].Value = description;
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = query;
                for (int i = 0; i < parameters.Length; i++)
                {
                    cmd.Parameters.Add(parameters[i]);
                }
                try
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    connection.Close();
                    return true;
                }
                catch (Exception e)
                {
                    if (debug)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    return false;
                }
                finally
                {
                    GC.Collect();
                }
            }
            else
            {
                return false;
            }
            
        }

        public DataSet ConvertToFarenheit(DataSet temperatureValues)
        {
            for(int i = 0; i < temperatureValues.Data.Length; i++)
            {
                double temp = temperatureValues.Data[i].Reading * 1.8 + 32;
                temperatureValues.Data[i].Reading = temp;
            }

            return temperatureValues;
        }

        String BuildXML(Object o, bool success) //serialize an object to XML for command line output
        {
            System.IO.StringWriter writer = new System.IO.StringWriter();
            if (o != null && !(o is Exception))
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                serializer.Serialize(writer, o);
            }
            if(o is IndexOutOfRangeException)
            {
                Exception e = (Exception)o;
                System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(writer);
                xw.WriteStartElement("Exception");
                xw.WriteElementString("message", e.Message);
                xw.WriteElementString("source", e.Source);
            }
            writer.WriteLine();
            writer.WriteLine("<Success value=\"" + success + "\" />");
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
                if (debug)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            finally
            {
                GC.Collect();
            }
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception e)
            {
                obj = null;
                if (debug)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            finally
            {
                GC.Collect();
            }
        }

        public void SetConfigInUnitTests()
        {
            databaseHost = "169.254.121.230";
            databaseName = "Sensorcom";
            databaseUser = "sensorcom";
            databasePassword = "password";
            databaseTimeout = "10";
            interactive = false;
            debug = false;
            connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
        }

        public String GetConnectionString()
        {
            return connectionString;
        }

        public Boolean GetInteractive()
        {
            return interactive;
        }

        public Boolean GetDebug()
        {
            return debug;
        }

        public Chamber[] GetChambers()
        {
            return chambers;
        }
    }
}