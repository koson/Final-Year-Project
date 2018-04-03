Use Sensorcom;
CREATE TABLE Calibration_Record (
	Cal_Record_ID int IDENTITY(1,1) PRIMARY KEY,
	Sensor_ID int FOREIGN KEY REFERENCES Sensor(Sensor_ID),
	Cal_Sensor_ID int FOREIGN KEY REFERENCES Sensor(Sensor_ID),
	Time_From DATETIME NOT NULL,
	Time_To DATETIME NOT NULL
	)