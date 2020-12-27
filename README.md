# IPInfoProvider.Api
## Table of Contents
1. [General Info](#general-info)
2. [Installation](#installation)
3. [Notes](#notes)
### General Info
***
This project is a simple REST Web Api which consumes the exposedendpoint from an external Api called **IPStack**.<br/>
The project is developed in Visual Studio 2019 with .NET Core 3.1 Framework and EF Core for database queries.<br/>
SQL Server is used as a database provider.<br/>
Repository pattern is used.<br/>
Caching is used to add performance control.<br/>

## Installation
***
Download the project <br/>
Before you begin change the connection string in appSettings.json file.<br/>
Run the following command in Package Manager Console
```
PM> update-database
```
This will create the database **IPStack_dev** and apply all migrations.<br/>

Build the project.
## Notes
***

You can use Postman collection and environment to test the endpoints. <br/>
Open fileExplorer and navigate to the project. Open folder **..\Docs\Postman**



