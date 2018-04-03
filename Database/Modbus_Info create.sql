USE Sensorcom;
CREATE TABLE Modbus_Info(
	Modbus_Info_ID int Identity(1,1) PRIMARY KEY,
	IP_Address VARCHAR(15) NOT NULL,
	Network_Port int NOT NULL,
	Register int NOT NULL,
	Scale float NOT NULL,
	Offset float NOT NULL
);