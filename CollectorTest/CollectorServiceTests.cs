using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollectorService;

namespace CollectorTest
{
    [TestClass]
    public class CollectorServiceTests
    {
        [TestMethod]
        public void CalculateRegisterValue_Test()
        {
            //arrange
            byte[] rawData = { 0b0, 0b0, 0b0, 0b0, 0b0, 0b101, 0b1, 0b100, 0b10, 0b0, (byte)210, 0b0, 0b0 };
            double expected = 210;
            Service1 s = new Service1(null);

            //act
            double actual = s.CalculateRegisterValue(rawData);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSensorReading_Successful_Temperature_Test()
        {
            //arrange
            Service1 s = new Service1(null);
            Sensor sen = new Sensor(1, "169.254.228.122", 502, 1, 0, 0, 0, 1, "", "");
            double regValue = 210;
            double expected = 21;

            //act
            double actual = s.GetSensorReading(regValue, sen);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSensorReading_Successful_Humidity_Test()
        {
            //arrange
            Service1 s = new Service1(null);
            Sensor sen = new Sensor(1, "169.254.228.122", 502, 1, 2, 0, 0, 1, "", "");
            double regValue = 210;
            double expected = 0;

            //act
            double actual = s.GetSensorReading(regValue, sen);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSensorReading_Successful_Pressure_Test()
        {
            //arrange
            Service1 s = new Service1(null);
            Sensor sen = new Sensor(1, "169.254.228.122", 502, 1, 2, 0, 0, 1, "", "");
            double regValue = 210;
            double expected = 0;

            //act
            double actual = s.GetSensorReading(regValue, sen);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetSensorReading_DefaultAction_Test()
        {
            //arrange
            Service1 s = new Service1(null);
            Sensor sen = new Sensor(0, "192.168.1.1", 502, 1, 7, 0, 0, 1, "modbus", "modbus");

            //act
            s.GetSensorReading(0, sen);
        }

        [TestMethod]
        public void CalculateHumidity_Test()
        {
            //arrange
            double regValue = 210;
            double scale = 0.2442;
            double offset = 0;
            Service1 s = new Service1(null);
            double expected = 51.282;

            //act
            double actual = s.CalculateHumidity(regValue, scale, offset);

            //assert
            Assert.AreEqual(expected, actual, 0.1);
        }
        [TestMethod]
        public void CalculatePressure_Test()
        {
            //arrange
            double regValue = 20;
            double scale = 4.884;
            double offset = -8;
            Service1 s = new Service1(null);
            double expected = 89.68;

            //act
            double actual = s.CalculatePressure(regValue, scale, offset);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculatePressure_ZeroScale_Test()
        {
            //arrange
            double regValue = 20;
            double scale = 0;
            double offset = -8;
            Service1 s = new Service1(null);
            double expected = -8;

            //act
            double actual = s.CalculatePressure(regValue, scale, offset);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateHumidity_ZeroScale_Test()
        {
            //arrange
            double regValue = 4095;
            double scale = 0;
            double offset = 0;
            Service1 s = new Service1(null);
            double expected = 0;

            //act
            double actual = s.CalculateHumidity(regValue, scale, offset);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CalculateTemperature_Test()
        {
            //arrange
            double regValue = 210;
            Service1 s = new Service1(null);
            double expected = 21;

            //act
            double actual = s.CalculateTemperature(regValue);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RequestData_Test() //using constant value modbus emulator
        {
            //arrange
            byte[] request = { 0b0, 0b0, 0b0, 0b0, 0b0, 0b110, 0b1, 0b100, 0b0, 0b1, 0b0, 0b1 };
            Service1 s = new Service1(null);
            Sensor sen = new Sensor(0, "169.254.228.122", 502, 0, 0, 0, 0, 1, " ", "modbus");
            byte[] expected = { 0b0, 0b0, 0b0, 0b0, 0b0, 0b101, 0b1, 0b100, 0b10, 0b0, (byte)210, 0b0, 0b0};

            //act
            byte[] actual = s.RequestData(sen);

            //assert
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ReadSensorConfig_Successful_Test()
        {
            //arrange
            Service1 s = new Service1(null);
            s.SetValidGeneralConfigForTesting();
            //act
            Sensor[] actual = s.ReadSensorConfig();

            //assert
            CollectionAssert.AllItemsAreInstancesOfType(actual, typeof(Sensor));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ReadSensorConfig_Failed_Test()
        {
            //arrange
            Service1 s = new Service1(null);
            s.SetInvalidGeneralConfigForTesting();
            //act
            Sensor[] actual = s.ReadSensorConfig();
        }

        /*[TestMethod]
        public void ReadGeneralConfig_Test()
        {
            //arrange
            String expectedHost = "169.254.121.230";
            String expectedName = "Sensorcom";
            String expectedUser = "sensorcom";
            String expectedPassword = "password";
            String expectedTimeout = "10";
            int expectedInterval = 1000;
            Service1 s = new Service1(null);

            //act
            s.ReadGeneralConfig();

            //assert
            
        }
        */

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReadGeneralConfig_Fails_Test()
        {
            //arrange
            Service1 s = new Service1(null);

            //act
            s.ReadGeneralConfig();
        }


    }
}
