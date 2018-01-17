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
    public double LastReading { get; set; }
    public double LastVoltage { get; set; }

    public Sensor()
    {

    }

	public Sensor(String newAddress, int newPort, int newID, int newType, int newChamberID, int newRegister)
	{
        Address = newAddress;
        Port = newPort;
        ID = newID;
        SensorType = newType;
        ChamberID = newChamberID;
        Register = newRegister;
        LastReading = 0;
	}
    public Sensor(String newAddress, int newID, int newType, int newChamberID, int newRegister)
    {
        Address = newAddress;
        Port = 502;
        ID = newID;
        SensorType = newType;
        ChamberID = newChamberID;
        Register = newRegister;
        LastReading = 0;
    }
}
