using System;

/// <summary>
/// Class used to represent sensors from the database
/// </summary>
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
    public string Description { get; set; }
    public String Interface { get; set; }

    /// <summary>
    /// Class constructor
    /// </summary>
    /// <param name="newID">ID of the sensor</param>
    /// <param name="newAddress">IP address of the sensor</param>
    /// <param name="newPort">Network port of the sensor</param>
    /// <param name="newRegister">register of the sensor within the modbus device</param>
    /// <param name="newSensorType">type of sensor</param>
    /// <param name="newScale">sensor scale</param>
    /// <param name="newOffset">sensor offset</param>
    /// <param name="newChamberID">ID of the chamber the sensor belongs to</param>
    /// <param name="newDescription">Description (name) of the sensor</param>
    /// <param name="newInterface">interface of sensor (unused as they're all modbus sensors)</param>
    public Sensor(int newID, String newAddress, int newPort, int newRegister, int newSensorType, double newScale, double newOffset, int newChamberID, string newDescription, String newInterface)
    {
        ID = newID;
        Address = newAddress;
        Port = newPort;
        Register = newRegister;
        SensorType = newSensorType;
        Scale = newScale;
        Offset = newOffset;
        ChamberID = newChamberID;
        Description = newDescription;
        Interface = newInterface;
    }
}
