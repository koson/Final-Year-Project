Use Sensorcom;
CREATE TABLE Sensor (
	Sensor_ID int IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(45) NOT NULL,
	Sensor_Enabled bit NOT NULL,
	Sensor_Type int NOT NULL,
	Chamber_ID int FOREIGN KEY REFERENCES Chamber(Chamber_ID) NOT NULL,
	Modbus_Info_ID INT FOREIGN KEY REFERENCES Modbus_Info(Modbus_Info_ID)
	)
	
	