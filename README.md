# Solution Description


The realization of IoT System contains 2 separated solutions:
- Solution for imitation IoT device and Signal Receiver
- Solution for displaying data

# IoT device and Signal Receiver

.NET Framework 4.7.2

The solution contains two Console Applications that communicate via WebSocket.

[Solution](https://github.com/OlgaTarasov/IoTSystem/tree/main/IoTSystem)

## WSTransmitter

WebSocket Client application that simulates every 2 mSec 2 types of signal: sine and state.
Each random 2, 3, 4, 5 seconds signal is replaced by an anomaly value. 
Each signal sends to Websocket Server (WSReceiver) in JSON format. 
Sending data contains:
- SignalType
- Value
- TimeStamp

## WSReceiver


WebSocket Server application receives all signals from WSTransmitter and deserializes JSON data to object. For each object calculates IsAnomaly parameters that shows is the current signal is normal or anomaly (out of bounds).
All signal data posted to Database. Posted data structure:
- Source of signal (Sine or State)
- Value
- TimeStamp
-  IsAnomaly

Database access:
Using ADO.NET
- At the start of the application opens a connection to the database
- Service call Stored Procedure InsertSignal and pass parameters:
    @SourceCode int, 
	@Value int, 
	@TimeStamp datetime,
	@IsAnomaly tinyint
- SP InsertSignal return ID of inserted row

Connection String (App.config):
- name="SignalDB"
- connectionString="Server=DEVELOP2-LAP\DEVSERVER;Database=SignalDB;User Id=signaladmin;Password=12345;"

# Displaying data Solution

.NET 6.0
Angular 12.2.2
The solution contains WEB API Project and Angular Project

[Solution](https://github.com/OlgaTarasov/IoTSystem/tree/main/MonitoringSystem)

## MonitoringSystem (Web API) 

Controllers:
- SignalController

Methods (only GET):
- signal/source - get SignalSource List
- signal/sine/page{page}/size{size}  - get Sine Signal List (with pagination)
- signal/state/page{page}/size{size}   - get State Signal List (with pagination)
- signal/anomaly/page{page}/size{size}   - get Anomaly Signal List (Sine + State) (with pagination)

Controller methods call SignalService methods.

SignalService methods call DAService for getting data from Database
(calling Stored Procedures )

Stored Procedures:
- GetSignals
	@SourceCode int, 
	@Page int, 
	@Size int
- GetAnimalySignals
	@Page int, 
	@Size int

ConnectionString (appsettings.json)
- connectionString="Server=DEVELOP2-LAP\\DEVSERVER;Database=SignalDB;User Id=signaladmin;Password=12345;MultipleActiveResultSets=true;"

Data Access Layer

- DAInstance - Create DB Connection instance and open connection (Singlethone). Instance creates in Programs.cs
- DAService - DB Access methods:
	- List<T> ExecuteSP<T>(string spName, SqlParameter[] parameters) - reter list of objects

## Angular Application 


The simplest Angular application. Contains one main component - SignalDataComponent.

In the component constructor with Interval, one second calls 3 HTTP get requests:
- Anomaly signals
- Sine signals
- State signals
Realized different pagination sizes for Anomaly signals and Sine/State signals.
In Sine/State signal tables records with anomaly values are highlighted with red color.

  ![GUI](https://user-images.githubusercontent.com/95657315/146676667-295675d1-b6a0-4ba2-95ce-7d94710074fd.png)
  
# DataBase

MS SQLServer

DatabaseName: SignalDB

Tables:
- [dbo].[Signals]
- [dbo].[Source]

SP:
- [dbo].[GetAnimalySignals]
- [dbo].[GetSignals]
- [dbo].[InsertSignal]
  
Please checkout SQL script [here](https://github.com/OlgaTarasov/IoTSystem/blob/main/DB%20Script/script.sql)
  
  ![SelectSignals](https://user-images.githubusercontent.com/95657315/146676683-46f52d8d-e3b4-4798-b6e6-02999e1d598f.PNG)
