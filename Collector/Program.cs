using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Collector
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 1500;
            String slaveAddress = "192.168.0.74";
            TcpClient client = new TcpClient(slaveAddress, port);
            NetworkStream nwStream = client.GetStream();
            byte upperTransIdentifier = 0b0;
            byte transIdentifier = (byte)0;
            byte protocolIdentifier = 0b0;
            byte upperHeaderLength = 0b0;
            byte lowerHeaderLength = 0b110;
            byte unitIdentifier = 0b1;
            byte functionCode = 0b11;
            for(int i=0;i<7;i++){
                transIdentifier = (byte) i;
                byte[] request = new byte[] {upperTransIdentifier, transIdentifier, protocolIdentifier, protocolIdentifier, upperHeaderLength, lowerHeaderLength, unitIdentifier, functionCode, 0b0, 0b0, 0b0, 0b1};
            Console.WriteLine(request.ToString());

            nwStream.Write(request, 0, request.Length);
            byte[] received = new byte[client.ReceiveBufferSize];
            Console.WriteLine(received.Length);
            int bytesRead = nwStream.Read(received, 0, client.ReceiveBufferSize);
            for(int ii=0; ii<11; ii++){
                Console.WriteLine(received[ii]);
}
}
            client.Close();
            Console.Read();
        }
    }
}
