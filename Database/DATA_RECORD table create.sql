Use Sensorcom;
CREATE TABLE Data_Record (
	Data_Record_ID int IDENTITY(1,1) PRIMARY KEY,
	Computed_Value float NOT NULL,
	Register_Value float NOT NULL,
	Record_Time DATETIME NOT NULL,
	Sensor_ID int FOREIGN KEY REFERENCES Sensor(Sensor_ID)
	);