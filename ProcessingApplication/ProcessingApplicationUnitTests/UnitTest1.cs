using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProcessingApplication;

namespace ProcessingApplicationUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Initialise_Test()
        {
            //arrange
            Program p = new Program();
            String expectedConnectionString = "Data Source =; Initial Catalog =; User ID =; Password =";

            //act
            p.Initialise();
            String actual = p.GetConnectionString();

            //assert
            Assert.AreEqual(expectedConnectionString, actual);
            Assert.AreEqual(p.GetDebug(), false);
            Assert.AreEqual(p.GetInteractive(), false);
        }

        [TestMethod]
        public void GetSensorsForChamber_Test()
        {
            //arrange
            Program p = new Program();
            Chamber c = new Chamber();
            c.ID = 1;
            Sensor[] expected = new Sensor[0];
            
            //act
            var actual = p.GetSensorsForChamber(c);

            //assert
            CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(Sensor));
        }

        [TestMethod]
        public void GetChamberList_Test()
        {
            //arrange
            Program p = new Program();
            Chamber[] expected = new Chamber[0];

            //act
            var actual = p.GetChamberList();

            //assert
            CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(Chamber));
        }

        [TestMethod]
        public void GetDataForSensor_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            Sensor s = new Sensor(3, "192.168.0.13", 502, 1, 0, 0, 0, 1, "Temperature Sensor");
            DateTime startDate = DateTime.Parse("2018-03-26 13:00:00");
            DateTime endDate = DateTime.Parse("2018-03-26 13:00:10");
            DataSet expected = new DataSet();
            expected.SensorID = s.ID;
            expected.Data = new DataItem[]{
                new DataItem(3843, 18, 180, DateTime.Parse("2018-03-26 13:00:00"), 3),
                new DataItem(3846, 18, 180, DateTime.Parse("2018-03-26 13:00:01"), 3),
                new DataItem(3849, 18, 180, DateTime.Parse("2018-03-26 13:00:02"), 3),
                new DataItem(3852, 18, 180, DateTime.Parse("2018-03-26 13:00:03"), 3),
                new DataItem(3856, 18, 180, DateTime.Parse("2018-03-26 13:00:04"), 3),
                new DataItem(3858, 18, 180, DateTime.Parse("2018-03-26 13:00:05"), 3),
                new DataItem(3861, 18, 180, DateTime.Parse("2018-03-26 13:00:06"), 3),
                new DataItem(3864, 18, 180, DateTime.Parse("2018-03-26 13:00:07"), 3),
                new DataItem(3867, 18, 180, DateTime.Parse("2018-03-26 13:00:08"), 3),
                new DataItem(3870, 18, 180, DateTime.Parse("2018-03-26 13:00:09"), 3),
                new DataItem(3873, 18, 180, DateTime.Parse("2018-03-26 13:00:10"), 3)};

            //act
            DataSet actual = p.GetDataForSensor(s, startDate, endDate);

            //assert
            for(int i = 0; i < expected.Data.Length; i++)
            {
                Assert.AreEqual(expected.Data[i].Reading, actual.Data[i].Reading);
                Assert.AreEqual(expected.Data[i].Timestamp, actual.Data[i].Timestamp);
            }
            Assert.AreEqual(expected.SensorID, actual.SensorID);
        }

        [TestMethod]
        public void GetDataForSensor_EmptySet_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            Sensor s = new Sensor(3, "192.168.0.13", 502, 1, 0, 0, 0, 1, "Temperature Sensor");
            DateTime startDate = DateTime.Parse("2018-04-26 13:00:00");
            DateTime endDate = DateTime.Parse("2018-04-26 13:00:10");
            DataSet expected = new DataSet();
            expected.SensorID = s.ID;

            //act
            DataSet actual = p.GetDataForSensor(s, startDate, endDate);

            //assert
            Assert.AreEqual(expected.SensorID, actual.SensorID);
            CollectionAssert.AreEqual(expected.Data, actual.Data);
        }

        [TestMethod]
        public void GetChamberByID_Found_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            p.GetEnvironment();
            Chamber expected = new Chamber(1, "Test chamber", "For testing Sensorcom", null);

            //act
            Chamber actual = p.GetChamberByID(expected.ID);

            //assert
            Assert.AreEqual(actual.ID, expected.ID);
            Assert.AreEqual(actual.Name, expected.Name);
            Assert.AreEqual(actual.Description, expected.Description);
        }

        [TestMethod]
        public void GetChamberByID_Null_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            p.GetEnvironment();
            Chamber expected = null;

            //act
            Chamber actual = p.GetChamberByID(0);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void GetSensorByID_Found_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            p.GetEnvironment();
            Sensor expected = new Sensor(3, "192.168.0.13", 502, 1, 0, 0, 0, 1, "Temperature");

            //act
            Sensor actual = p.GetSensorByID(expected.ID);

            //assert
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.ChamberID, actual.ChamberID);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.Offset, actual.Offset);
            Assert.AreEqual(expected.Port, actual.Port);
            Assert.AreEqual(expected.Register, actual.Register);
            Assert.AreEqual(expected.Scale, actual.Scale);
            Assert.AreEqual(expected.SensorType, actual.SensorType);
        }

        [TestMethod]
        public void GetSensorByID_Null_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            p.GetEnvironment();
            Sensor expected = null;

            //act
            Sensor actual = p.GetSensorByID(0);

            //assert
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void ProduceMeanValues_Test()
        {
            //arrange
            Program p = new Program();
            DataSet input = new DataSet();
            input.Data = new DataItem[] {
                new DataItem(3843, 20, 180, DateTime.Parse("2018-03-26 13:00:00"), 3),
                new DataItem(3846, 18, 180, DateTime.Parse("2018-03-26 13:00:01"), 3),
                new DataItem(3849, 18, 180, DateTime.Parse("2018-03-26 13:00:02"), 3),
                new DataItem(3852, 19, 180, DateTime.Parse("2018-03-26 13:00:03"), 3),
                new DataItem(3856, 21, 180, DateTime.Parse("2018-03-26 13:00:04"), 3),
                new DataItem(3858, 18, 180, DateTime.Parse("2018-03-26 13:00:05"), 3),
                new DataItem(3861, 20, 180, DateTime.Parse("2018-03-26 13:00:06"), 3),
                new DataItem(3864, 18, 180, DateTime.Parse("2018-03-26 13:00:07"), 3),
                new DataItem(3867, 19, 180, DateTime.Parse("2018-03-26 13:00:08"), 3),
                new DataItem(3870, 18, 180, DateTime.Parse("2018-03-26 13:00:09"), 3),
                new DataItem(3873, 16, 180, DateTime.Parse("2018-03-26 13:00:10"), 3)};
            DataSet expected = new DataSet();
            expected.AddDataItem(new DataItem(18.63, DateTime.Parse("2018-03-26 13:00:00")));

            //act
            DataSet actual = p.ProduceMeanValues(input);

            //assert
            Assert.AreEqual(actual.SensorID, expected.SensorID);
            Assert.AreEqual(expected.Data.Length, actual.Data.Length);
            Assert.AreEqual(actual.Data[0].Reading, expected.Data[0].Reading, 0.05);
            Assert.AreEqual(actual.Data[0].Timestamp, expected.Data[0].Timestamp);
        }

        [TestMethod]
        public void SortDates_Test()
        {
            //arrange
            Program p = new Program();
            DataSet[] input = new DataSet[1];
            input[0] = new DataSet();
            input[0].SensorID = 0;
            input[0].AddDataItem(new DataItem(3843, 20, 180, DateTime.Parse("2018-03-26 13:00:00"), 3));
            input[0].AddDataItem(new DataItem(3846, 18, 180, DateTime.Parse("2018-03-26 13:00:00"), 3));
            input[0].AddDataItem(new DataItem(3849, 18, 180, DateTime.Parse("2018-03-26 13:00:01"), 3));
            input[0].AddDataItem(new DataItem(3852, 19, 180, DateTime.Parse("2018-03-26 13:00:01"), 3));
            input[0].AddDataItem(new DataItem(3856, 21, 180, DateTime.Parse("2018-03-26 13:00:02"), 3));
            input[0].AddDataItem(new DataItem(3858, 18, 180, DateTime.Parse("2018-03-26 13:00:04"), 3));
            input[0].AddDataItem(new DataItem(3861, 20, 180, DateTime.Parse("2018-03-26 13:00:02"), 3));
            input[0].AddDataItem(new DataItem(3864, 18, 180, DateTime.Parse("2018-03-26 13:00:02"), 3));
            input[0].AddDataItem(new DataItem(3867, 19, 180, DateTime.Parse("2018-03-26 13:00:04"), 3));
            input[0].AddDataItem(new DataItem(3870, 18, 180, DateTime.Parse("2018-03-26 13:00:03"), 3));
            input[0].AddDataItem(new DataItem(3873, 16, 180, DateTime.Parse("2018-03-26 13:00:03"), 3));
            Console.WriteLine(input[0].Data.Length);
            DateTime[] expected = new DateTime[] {
                DateTime.Parse("2018-03-26 13:00:00"),
                DateTime.Parse("2018-03-26 13:00:01"),
                DateTime.Parse("2018-03-26 13:00:02"),
                DateTime.Parse("2018-03-26 13:00:03"),
                DateTime.Parse("2018-03-26 13:00:04")};

            //act
            DateTime[] actual = p.SortDates(input);

            //assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AddChamber_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            p.GetEnvironment();
            Chamber expected = new Chamber(0, "UTest", "Unit test add", null);

            //act
            p.AddChamber("UTest", "Unit test add");
            p.GetEnvironment();
            Chamber actual = null;
            for (int i = 0; i < p.GetChambers().Length; i++)
            {
                if (p.GetChambers()[i].Name == "UTest")
                {
                    actual = p.GetChambers()[i];
                }
            }

            //assert
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);

        }

        [TestMethod]
        public void AddModbusSensor_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            p.GetEnvironment();
            Sensor expected = new Sensor(0, "0.0.0.0", 502, 1, 1, 1, 1, 2, "UTest add");

            //act
            p.AddModbusSensor("0.0.0.0", 502, 1, 2, 1, 1, 1, "UTest add", false);
            p.GetEnvironment();
            Sensor actual = null;
            Sensor[] temp = p.GetSensorsForChamber(p.GetChamberByID(2));
            for(int i = 0; i < temp.Length; i++)
            { 
                if(temp[i].Address == "0.0.0.0")
                {
                    actual = temp[i];
                }
            }

            //assert
            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.ChamberID, actual.ChamberID);
            Assert.AreEqual(expected.Offset, actual.Offset);
            Assert.AreEqual(expected.Port, actual.Port);
            Assert.AreEqual(expected.Register, actual.Register);
            Assert.AreEqual(expected.Scale, actual.Scale);
            Assert.AreEqual(expected.SensorType, actual.SensorType);
        }

        [TestMethod]
        public void RemoveModbusSensor_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            p.GetEnvironment();
            Sensor expected = null;

            //act
            p.RemoveModbusSensor(28);
            p.GetEnvironment();

            //assert
            Assert.AreEqual(expected, p.GetSensorByID(28));
        }

        [TestMethod]
        public void RemoveChamber_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            p.GetEnvironment();
            Chamber expected = null;

            //act
            p.RemoveChamber(10);
            p.GetEnvironment();

            //assert
            Assert.AreEqual(expected, p.GetChamberByID(6));
        }

        [TestMethod]
        public void EditModbusSensor_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            Sensor expected = new Sensor(31, "169.254.172.48", 502, 2, 2, 0.224, 0, 1, "Humidity edited");

            //act
            p.EditModbusSensor(31, "Humidity edited", true, 2, 1, "169.254.172.48", 502, 2, 0.224, 0);
            p.GetEnvironment();
            Sensor actual = p.GetSensorByID(31);

            //assert
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.ChamberID, actual.ChamberID);
            Assert.AreEqual(expected.Description, actual.Description);
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Offset, actual.Offset);
            Assert.AreEqual(expected.Port, actual.Port);
            Assert.AreEqual(expected.Register, actual.Register);
            Assert.AreEqual(expected.Scale, actual.Scale);
            Assert.AreEqual(expected.SensorType, actual.SensorType);
        }

        [TestMethod]
        public void DeleteDataForSensor_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            p.GetEnvironment();
            Sensor s = new Sensor(31, "0.0.0.0", 502, 1, 1, 1, 1, 1, "");
            DataSet expected = new DataSet();
            expected.Data = new DataItem[] { };
            expected.SensorID = s.ID;

            //act
            p.DeleteDataForSensor(31);
            DataSet actual = p.GetDataForSensor(s, DateTime.Parse("1753-01-01 00:00:00"), DateTime.Parse("9999-12-31 23:59:59")); //earliest and latest possible dates in SQL server

            //assert
            Assert.AreEqual(expected.SensorID, actual.SensorID);
            Assert.AreEqual(expected.Data.Length, actual.Data.Length);
        }

        [TestMethod]
        public void EditChamber_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            Chamber expected = new Chamber(5, "name edited", "description edited", null);

            //act
            p.EditChamber(5, "name edited","description edited");
            p.GetEnvironment();
            Chamber actual = p.GetChamberByID(5);

            //assert
            Assert.AreEqual(expected.ID, actual.ID);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Description, actual.Description);
        }

        [TestMethod]
        public void ConvertToFareheit_Test()
        {
            //arrange
            Program p = new Program();
            p.SetConfigInUnitTests();
            p.GetEnvironment();
            DataSet expected = new DataSet();
            expected.SensorID = 3;
            expected.Data = new DataItem[] {
                new DataItem(3843, 64.4, 180, DateTime.Parse("2018-03-26 13:00:00"), 3),
                new DataItem(3846, 64.4, 180, DateTime.Parse("2018-03-26 13:00:01"), 3),
                new DataItem(3849, 64.4, 180, DateTime.Parse("2018-03-26 13:00:02"), 3),
                new DataItem(3852, 64.4, 180, DateTime.Parse("2018-03-26 13:00:03"), 3),
                new DataItem(3856, 64.4, 180, DateTime.Parse("2018-03-26 13:00:04"), 3),
                new DataItem(3858, 64.4, 180, DateTime.Parse("2018-03-26 13:00:05"), 3),
                new DataItem(3861, 64.4, 180, DateTime.Parse("2018-03-26 13:00:06"), 3),
                new DataItem(3864, 64.4, 180, DateTime.Parse("2018-03-26 13:00:07"), 3),
                new DataItem(3867, 64.4, 180, DateTime.Parse("2018-03-26 13:00:08"), 3),
                new DataItem(3870, 64.4, 180, DateTime.Parse("2018-03-26 13:00:09"), 3),
                new DataItem(3873, 64.4, 180, DateTime.Parse("2018-03-26 13:00:10"), 3)};

            //act
            DataSet actual = p.ConvertToFarenheit(p.GetDataForSensor(new Sensor(3, "", 1, 1, 1, 1, 1, 1, ""), DateTime.Parse("2018-03-26 13:00:00"), DateTime.Parse("2018-03-26 13:00:10")));

            //assert
            for(int i = 0; i < expected.Data.Length; i++)
            {
                Assert.AreEqual(expected.Data[i].Reading, actual.Data[i].Reading);
            }
        }
    }
}
