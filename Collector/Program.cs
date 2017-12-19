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
            program.CalculateVoltage(program.CalculateRegisterValue(rawData));
            Console.Read();
        }

        ushort CalculateRegisterValue(byte[] rawData){ //calculates register value based off of bits in packet (uses little endian)
            ushort regValue;
            byte[] toAdd = new byte[2]; //always 2 bytes of data returned (modmux modules use 12 bits to send value)
            toAdd = new byte[2]{rawData[10], rawData[9]}; //always 9th and 10th bit for modmux modules
            regValue = BitConverter.ToUInt16(toAdd, 0); //concatenate bits to form 16 bit word
            Console.WriteLine("Register value is: " + regValue.ToString());
            return regValue;
        }

        double CalculateVoltage(ushort regValue) { //calculates voltage based off register value received
            double voltage = regValue/409.5;
            Console.WriteLine("Voltage is: " + voltage.ToString());
            return voltage;
        }
        
        void CalculateSensorReading(double voltage, int sensorType){
            //needs equation from Kris
            switch (sensorType) {
                case 0:
                    //temp sensor
                    break;
                case 1:
                    //pressure sensor
                    break;
                case 2:
                    //humidity sensor
                    break;
                default:
                    //unknown - throw error
                    break;
            }
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

        void ReadConfig() { //reads configuration from a file
            XmlSerializer configReader = new XmlSerializer(typeof(List<Sensor>));
            FileStream loadStream = new FileStream("C:\\Users\\Raife\\source\\repos\\Final-Year-Project\\Collector\\sensors.xml", FileMode.Open, FileAccess.Read);
            sensors = (List<Sensor>)configReader.Deserialize(loadStream);
            loadStream.Close();
        } 
    }          
}
