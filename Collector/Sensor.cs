using System;
using System.Xml.Serialization;

public class Sensor
{
    [XmlAttribute("Address")]
    public string Address { get; set; }
    [XmlAttribute("port")]
    public int Port { get; set; }
    [XmlAttribute("ID")]
    public int ID { get; set; }
    [XmlAttribute("SensorType")]
    public int SensorType { get; set; }
    [XmlAttribute("ChamberID")]
    public int ChamberID { get; set; }
    [XmlAttribute("Register")]
    public int Register { get; set; }

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
	}
    public Sensor(String newAddress, int newID, int newType, int newChamberID, int newRegister)
    {
        Address = newAddress;
        Port = 502;
        ID = newID;
        SensorType = newType;
        ChamberID = newChamberID;
        Register = newRegister;
    }
}
