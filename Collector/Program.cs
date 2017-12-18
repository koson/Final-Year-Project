using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml.Serialization;

namespace Collector
{
    class Program
    {
        [XmlArray("Sensors")]
        [XmlArrayItem("Sensors")]
        private List<Sensor> sensors = new List<Sensor>();
        static void Main(string[] args)
        {
            Program program = new Program();
            program.ReadConfig();
            byte[] rawData = program.RequestData(program.sensors[0]);
            program.PrintData(rawData);
            Console.Read();
        }

        void CalculateData(byte[] rawData, int sensorType) { } //calculates data based off voltage received

        void PrintData(byte[] rawData)
        {
            for(int i = 0; i < 12; i++)
            {
                Console.WriteLine(rawData[i]);
            }
        }

        byte[] RequestData(Sensor sensor) { //reads data from a sensor
            byte upperTransIdentifier = 0b0;
            byte transIdentifier = (byte)0;
            byte protocolIdentifier = 0b0;
            byte upperHeaderLength = 0b0;
            byte lowerHeaderLength = 0b110;
            byte unitIdentifier = 0b1;
            byte functionCode = 0b11;
            byte register = (byte) sensor.Register;
            Console.WriteLine("register number is : " + register);
            TcpClient client = new TcpClient(sensor.Address, sensor.Port);
            NetworkStream nwStream = client.GetStream();
            byte[] request = new byte[] { upperTransIdentifier, transIdentifier, protocolIdentifier, protocolIdentifier, upperHeaderLength, lowerHeaderLength, unitIdentifier, functionCode, 0b0, register, 0b0, 0b1 };
            nwStream.Write(request, 0, request.Length);
            byte[] received = new byte[client.ReceiveBufferSize];
            Console.WriteLine(received.Length);
            int bytesRead = nwStream.Read(received, 0, client.ReceiveBufferSize);
            client.Close();
            return received;
        }
        void ReadConfig() { //reads configuration from a file
            XmlSerializer configReader = new XmlSerializer(typeof(List<Sensor>));
            FileStream loadStream = new FileStream("C:\\Users\\Raife\\source\\repos\\Final-Year-Project\\Collector\\sensors.xml", FileMode.Open, FileAccess.Read);
            sensors = (List<Sensor>)configReader.Deserialize(loadStream);
            loadStream.Close();
        } 

    }          
}
