using System;
using System.Xml.Serialization;

public class Sensor
{
    public string Address { get; set; }
    public int Port { get; set; }
    public int ID { get; set; }
    public int SensorType { get; set; }
    public int ChamberID { get; set; }
    public int Register { get; set; }
    public double Scale { get; set; }
    public double Offset { get; set; }

    public Sensor(int newID, String newAddress, int newPort, int newRegister, int newSensorType, double newScale, double newOffset, int newChamberID)
    {
        ID = newID;
        Address = newAddress;
        Port = newPort;
        Register = newRegister;
        SensorType = newSensorType;
        Scale = newScale;
        Offset = newOffset;
        ChamberID = newChamberID;
    }
}
