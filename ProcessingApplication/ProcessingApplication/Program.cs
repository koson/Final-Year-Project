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
        private String connectionString;
        bool debug;
        bool interactive;
        static void Main(string[] args)
        {
            Program p = new Program();
            p.Initialise();
            p.GetEnvironment();
            int offset = 0;

            if(args[0] == "-i") //interactive mode
            {
                offset++;
                p.interactive = true;
            }

            if(args[0] == "-d" || args[1] == "-d") //debug mode
            {
                p.debug = true;
                offset++;
            }

            switch (args[0+offset]) {

                case "getEnv":
                    Console.Write(p.BuildXML(p.chambers, true));
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
                        chamberID = int.Parse(args[1+offset]);
                        startDate = DateTime.Parse(args[2+offset]);
                        endDate = DateTime.Parse(args[3+offset]);
                        averageValues = Boolean.Parse(args[4+offset]);
                        exportToExcel = Boolean.Parse(args[5+offset]);
                        if (exportToExcel)
                        {
                            filename = args[6+offset];
                        }
                        if(startDate < endDate)
                        {
                            parseSuccessful = true;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    catch(Exception e)
                    {
                        if (p.interactive)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            p.PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(p.BuildXML(e, false));
                    }

                    if(parseSuccessful)
                    {
                        DataSet[] graphData = p.ProduceGraphData(p.GetChamberByID(chamberID), startDate, endDate, averageValues);
                        if (exportToExcel)
                        {
                            p.ExportToExcel(graphData, filename, p.GetChamberByID(chamberID).Description);
                            // write XML "Success" message to out - can be read by third party applications
                        }
                        else
                        {
                            Console.Write(p.BuildXML(graphData, true));
                        }
                    }
                    break;

                case "addChamber":
                    String chamberDescription = null;
                    String chamberLocation = null; //placeholder values
                    parseSuccessful = false;

                    try
                    {
                        chamberDescription = args[1+offset];
                        chamberLocation = args[2+offset];
                        parseSuccessful = true;
                    }
                    catch(Exception e)
                    {
                        if (p.interactive)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            p.PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(p.BuildXML(e, false));
                    }
                    if (parseSuccessful)
                    {
                        p.AddChamber(chamberLocation, chamberDescription);
                        //add XML success return and new chamber ID
                    }
                    break;

                case "editChamber":
                    chamberID = 0;
                    chamberLocation = null;
                    chamberDescription = null;
                    parseSuccessful = false;

                    try
                    {
                        chamberID = int.Parse(args[1+offset]);
                        chamberDescription = args[2+offset];
                        chamberLocation = args[3+offset];
                        parseSuccessful = true;
                    }catch(Exception e)
                    {
                        if (p.interactive)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            p.PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(p.BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        p.EditChamber(chamberID, chamberLocation, chamberDescription);
                    }
                    break;

                case "removeChamber":
                    chamberID = 0;
                    parseSuccessful = false;
                    char confirm = ' ';
                    try
                    {
                        chamberID = int.Parse(args[1+offset]);
                        parseSuccessful = true;
                    }catch(Exception e)
                    {
                        if (p.interactive)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            p.PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(p.BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        while(confirm != 'y' || confirm != 'n')
                        {
                            Console.WriteLine("Warning - this action will remove all associated sensors and the collected data for each sensor as well");
                            Console.WriteLine("Are you sure you want to remove chamber " + chamberID + ": " + p.GetChamberByID(chamberID).Description + "?");
                            confirm = (char)Console.Read();
                        }
                        if(confirm == 'y')
                        {
                            p.RemoveChamber(chamberID);
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
                    parseSuccessful = false;

                    try
                    {
                        sensorAddress = args[1+offset];
                        sensorPort = int.Parse(args[2+offset]);
                        sensorType = int.Parse(args[3+offset]);
                        chamberID = int.Parse(args[4+offset]);
                        sensorRegister = int.Parse(args[5+offset]);
                        sensorScale = double.Parse(args[6+offset]);
                        sensorOffset = double.Parse(args[7+offset]);
                        sensorDescription = args[8+offset];

                        parseSuccessful = true;
                    }catch(Exception e)
                    {
                        if (p.interactive)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            p.PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(p.BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        p.AddModbusSensor(sensorAddress, sensorPort, sensorType, chamberID, sensorRegister, sensorScale, sensorOffset, sensorDescription);
                    }
                    break;

                case "editSensor":
                    int sensorID = 0;
                    sensorDescription = null;
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
                        sensorID = int.Parse(args[1+offset]);
                        sensorDescription = args[2+offset];
                        sensorType = int.Parse(args[3+offset]);
                        chamberID = int.Parse(args[4+offset]);
                        sensorAddress = args[5+offset];
                        sensorPort = int.Parse(args[5+offset]);
                        sensorRegister = int.Parse(args[6+offset]);
                        sensorScale = double.Parse(args[7+offset]);
                        sensorOffset = double.Parse(args[8+offset]);
                        parseSuccessful = true;
                    }catch(Exception e)
                    {
                        if (p.interactive)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            p.PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(p.BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        p.EditModbusSensor(sensorID, sensorDescription, 0, sensorType, chamberID, sensorAddress, sensorPort, sensorRegister, sensorScale, sensorOffset);
                    }
                    break;

                case "removeSensor":
                    sensorID = 0;
                    confirm = ' ';
                    parseSuccessful = false;
                    try
                    {
                        sensorID = int.Parse(args[1+offset]);
                        parseSuccessful = true;
                    }catch(Exception e)
                    {
                        if (p.interactive)
                        {
                            Console.WriteLine("Oops, there was a problem with your synax. The proper usage for " + args[0 + offset] + " is:");
                            Console.WriteLine("\n");
                            p.PrintUsageText(args[0 + offset]);
                        }
                        Console.Write(p.BuildXML(e, false));
                    }

                    if (parseSuccessful)
                    {
                        while(confirm != 'y' || confirm != 'n')
                        {
                            Console.WriteLine("Warning - this action will remove all collected data for the sensor as well");
                            Console.WriteLine("Are you sure you want to permanently remove sensor " + sensorID + ": " + p.GetSensorByID(sensorID).Description + "?");
                            confirm = (char)Console.Read();
                        }
                        if(confirm == 'y')
                        {
                            p.RemoveModbusSensor(sensorID);
                        }
                    }
                    break;

                case "help":
                    if (args.Length == 2)
                    {
                        p.PrintUsageText(args[1+offset]);
                    }
                    else
                    {
                        p.PrintUsageText("");
                    }
                    break;

                default:
                    p.PrintUsageText("");
                    break;
            }
            Console.Read();
        }

        void Initialise()
        {
            ReadProgramConfig();
            interactive = false;
            debug = false;
            connectionString = "Data Source =" + databaseHost + "; Initial Catalog =" + databaseName + "; User ID ="
                + databaseUser + "; Password =" + databasePassword;
        }

        void PrintUsageText(string commandName) //prints help guide on command line
        {
            switch (commandName)
            {
                case "general":
                    Console.WriteLine("USAGE: ProcessingApplication.exe [-d] [command] [options]");
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
                    Console.WriteLine("USAGE: ProcessingApplication.exe addChamber description location");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - description: The description for the new chamber");
                    Console.WriteLine(" - location: The location of the new chamber");
                    break;

                case "editChamber":
                    Console.WriteLine("USAGE: ProcessingApplication.exe editChamber ID description location");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - ID: The ID of the existing chamber to edit");
                    Console.WriteLine(" - description: The new description for the chamber");
                    Console.WriteLine(" - location: The new location for the chamber");
                    break;

                case "removeChamber":
                    Console.WriteLine("USAGE: ProcessingApplication.exe removeChamber ID");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - ID: The ID of the chamber to remove");
                    break;

                case "addSensor":
                    Console.WriteLine("USAGE: ProcessingApplication.exe addSensor address port type chamberID register scale offset description");
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
                    break;

                case "editSensor":
                    Console.WriteLine("USAGE: ProcessingApplication.exe editSensor ID description type chamberID address port register scale offset");
                    Console.WriteLine("\n");
                    Console.WriteLine("Parameters:");
                    Console.WriteLine(" - address: The ID of the sensor to edit");
                    Console.WriteLine(" - description: The description of the new sensor");
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
                    Console.WriteLine("USAGE: ProcessingApplication.exe [-d] [-i] [command] [options]");
                    break;
            }
            
        }

        void GetEnvironment() //get all chambers and sensors in database (no actual reportable data)
        {
            chambers = GetChamberList();
            LoadAllSensors();
        }
        Sensor[] GetSensorsForChamber(Chamber chamber)
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
                Console.WriteLine(e.ToString());
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
        Chamber[] GetChamberList()
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
                Console.WriteLine(e.ToString());
            }
            finally
            {
                GC.Collect();
            }
            return chambers.ToArray();
        }

        DataSet GetDataForSensor(Sensor s, DateTime startTime, DateTime endTime)
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
                Console.WriteLine(e.ToString());
            }
            finally
            {
                GC.Collect();
            }
            return new DataSet(returnedData.ToArray(), s.ID);
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

        Sensor GetSensorByID(int sensorID)
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
            DataSet temperatureValues = new DataSet();
            DataSet pressureValues = new DataSet();
            DataSet humidityValues = new DataSet();
            for (int i = 0; i < c.sensors.Length; i++)
            {
                switch (c.sensors[i].SensorType)
                {
                    case 0: //temperature
                        DataSet tempAverage = GetDataForSensor(c.sensors[i], start, end);
                        if(tempAverage.Data.Length != 0 && average)
                        {
                            temperatureValues = ProduceMeanValues(tempAverage);
                            temperatureValues.SensorID = c.sensors[i].ID;
                        }
                        else
                        {
                            if (tempAverage.Data.Length != 0)
                            {
                                temperatureValues = GetDataForSensor(c.sensors[i], start, end);
                                temperatureValues.SensorID = c.sensors[i].ID;
                            }
                        }
                        break;
                    case 1: //pressure
                        DataSet pressureAverage = GetDataForSensor(c.sensors[i], start, end);
                        if(pressureAverage.Data.Length != 0 && average)
                        {
                            pressureValues = ProduceMeanValues(GetDataForSensor(c.sensors[i], start, end));
                            pressureValues.SensorID = c.sensors[i].ID;
                        }
                        else
                        {
                            if (pressureAverage.Data.Length != 0)
                            {
                                pressureValues = GetDataForSensor(c.sensors[i], start, end);
                                pressureValues.SensorID = c.sensors[i].ID;
                            }
                        }
                        break;
                    case 2: //humidity
                        DataSet humidityAverage = GetDataForSensor(c.sensors[i], start, end);
                        if(humidityAverage.Data.Length != 0 && average)
                        {
                            humidityValues = ProduceMeanValues(GetDataForSensor(c.sensors[i], start, end));
                            humidityValues.SensorID = c.sensors[i].ID;
                        }
                        else
                        {
                            if(humidityAverage.Data.Length != 0)
                            {
                                humidityValues = GetDataForSensor(c.sensors[i], start, end);
                                humidityValues.SensorID = c.sensors[i].ID;
                            }
                        } 
                        break;
                    default: //throw error
                        throw new Exception(); //find more specific error - pass error back to GUI or event log
                        //break;
                }
            }
            DataSet[] finalValues = new DataSet[3]; //3 sets of values to plot on graph
            finalValues[0] = temperatureValues;
            finalValues[1] = pressureValues;
            finalValues[2] = humidityValues; 
            return finalValues;
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

        void ExportToExcel(DataSet[] chartData, String filename, String chamberName)  //experimental version to test using 3 different series of time
        {
            //create workbook
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            Excel.Worksheet xlWorkSheet2;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlApp.Visible = false;
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = xlWorkBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = "Data";
            xlWorkSheet2 = xlWorkBook.Sheets.Add(misValue, misValue, 1, misValue) as Excel.Worksheet;
            xlWorkSheet2.Name = "Chart";

            //add data
            xlWorkSheet.Cells[1, 1] = "Time";
            xlWorkSheet.Cells[1, 2] = "Temperature";
            xlWorkSheet.Cells[1, 3] = "Humidity";
            xlWorkSheet.Cells[1, 4] = "Pressure";
            DateTime[] dates = sortDates(chartData);
            //add times
            for(int i = 0; i < dates.Length; i++)
            {
                xlWorkSheet.Cells[(i+2), 1] = dates[i].ToString();
            }

            //add data to sheet where timestamp matches time
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < chartData[i].Data.Length; j++)
                {
                    for(int k = 0; k < dates.Length; k++)
                    {
                        if (String.Equals(dates[k].ToString(), chartData[i].Data[j].Timestamp.ToString()))
                        {
                            Console.WriteLine("Date is: " + dates[k].ToString());
                            Console.WriteLine("Cell is: [" +(k+2) + "," + (i+2) + "]");
                            Console.WriteLine("Reading is: " + chartData[i].Data[j].Reading);
                            xlWorkSheet.Cells[(k+2), (i+2)] = chartData[i].Data[j].Reading;
                            k = dates.Length;
                        }
                    }
                }
            }

            Excel.Range allDataRange = xlWorkSheet.UsedRange;
            allDataRange.Sort(allDataRange.Columns[1], Excel.XlSortOrder.xlAscending, misValue, misValue, Excel.XlSortOrder.xlAscending, misValue, Excel.XlSortOrder.xlAscending, Excel.XlYesNoGuess.xlYes); //sort by time, excluding first row (header)

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

            Excel.Axis yAxis1 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            yAxis1.HasTitle = true;
            yAxis1.AxisTitle.Text = "Temperature (°C) & Humidity (%)";
            yAxis1.AxisTitle.Orientation = Excel.XlOrientation.xlUpward;

            chartPage.SeriesCollection(3).AxisGroup = Excel.XlAxisGroup.xlSecondary; //make presure a secondary axis and tie to third series (column D - pressure)

            Excel.Axis yAxis2 = (Excel.Axis)chartPage.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary);
            yAxis2.HasTitle = true;
            yAxis2.AxisTitle.Text = "Pressure (mbar) (a)";
            yAxis2.AxisTitle.Orientation = Excel.XlOrientation.xlUpward;

            xlWorkBook.SaveAs(filename, Excel.XlFileFormat.xlOpenXMLWorkbookMacroEnabled, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            releaseObject(xlWorkSheet);
            releaseObject(xlWorkBook);
            releaseObject(xlApp);
            GC.Collect();
        }

        DateTime[] sortDates(DataSet[] chartData)
        {
            List<DateTime> dates = new List<DateTime>();
            for(int i = 0; i < 3; i++)
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

        void AddChamber(String location, String description)
        {
            String query = "INSERT INTO Chamber (Location, Description) VALUES (@Location, @Description);";
            SqlParameter[] parameters = new SqlParameter[2];
            parameters[0] = new SqlParameter("@Location", System.Data.SqlDbType.VarChar);
            parameters[0].Value = location;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        void AddModbusSensor(String address, int port, int type, int chamberID, int register, double scale, double offset, String description) //add sensor to database
        {
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
                query = "INSERT INTO Sensor (Name, Calibration_Sensor, Sensor_Type, Chamber_ID, Modbus_Info_ID) VALUES (@Description, " + 0 + ", " +"@Type, @ChamberID, " + modbusID.ToString() + ");";
                command.CommandText = query;
                command.ExecuteNonQuery();
                command.Dispose();
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        void RemoveModbusSensor(int sensorID) //delete Modbus_Info data as well
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
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        void RemoveChamber(int chamberID)
        {
            Sensor[] sensorsToDelete = GetSensorsForChamber(GetChamberByID(chamberID));
            SqlConnection connection = new SqlConnection(connectionString);
            String query = "DELETE FROM Chamber WHERE Chamber_ID = @ID";
            SqlParameter id = new SqlParameter("@ID", System.Data.SqlDbType.Int);
            id.Value = chamberID;
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = query;
            cmd.Parameters.Add(id);
            for(int i = 0; i < sensorsToDelete.Length; i++)
            {
                Console.WriteLine("Removing sensor with ID" + sensorsToDelete[i].ID);
                RemoveModbusSensor(sensorsToDelete[i].ID);
            }
            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                connection.Close();
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                GC.Collect();
            }

        }

        void EditModbusSensor(int sensorID, String name, int cal, int type, int chamberID, String address, int port, int register, double scale, double offset)
        {
            String getModbusInfoID = "SELECT Modbus_Info_ID FROM Sensor WHERE Sensor_ID = @SensorID;";
            String updateSensor = "UPDATE Sensor SET Name = @Name, Calibration_Sensor = @Cal, Sensor_Type = @Type, Chamber_ID = @Chamber, Modbus_Info_ID = @ModbusID WHERE Sensor_ID = @SensorID;";
            String updateModbusInfo = "UPDATE Modbus_Info SET IP_Address = @Address, Network_Port = @Port, Register = @Register, Scale = @Scale, Offset = @Offset WHERE Modbus_Info_ID = @ModbusID;";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = connection.CreateCommand();
            SqlParameter[] parameters = new SqlParameter[11];
            parameters[0] = new SqlParameter("@SensorID", System.Data.SqlDbType.Int);
            parameters[0].Value = sensorID;
            parameters[1] = new SqlParameter("@Name", System.Data.SqlDbType.VarChar);
            parameters[1].Value = name;
            parameters[2] = new SqlParameter("@Cal", System.Data.SqlDbType.Bit);
            parameters[2].Value = cal;
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
            for (int i = 0; i < parameters.Length-1; i++)
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        void DeleteDataForSensor(int sensorID)
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        void EditChamber(int chamberID, String location, String description)
        {
            String query = "UPDATE Chamber SET Location = @Location, Description = @Description WHERE Chamber_ID = @ID;";
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@ID", System.Data.SqlDbType.Int);
            parameters[0].Value = chamberID;
            parameters[1] = new SqlParameter("@Location", System.Data.SqlDbType.VarChar);
            parameters[1].Value = location;
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
            }catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        DataSet ConvertToFarenheit(DataSet temperatureValues)
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
                Console.WriteLine(e.ToString());
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
                Console.WriteLine(e.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}