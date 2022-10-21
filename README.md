1. The applications are created in Visual Studio Code
2. To run them the current machine needs:
- SQL Server 2019 Express
- .NET Framework 6.0

3. The packet includes 3 directories:
- The first directory "BuiltApplications" is with the built applications, ready to be tested:
. In the backend directory is the backend API. Before running it from the ShipmentAPI.exe
the connection string aswell as the Kestrel Endpoints must be changed in file appsettings.json:

Server=SQL Server;
Database=SQL database

"Http": {
        "Url": "http://localhost:5400"
      },

"Https": {
        "Url": "https://localhost:5401"
      }

After setting the appsettings file the application can be started.

. In the frontend diretory is the ASP.NET MVC website. Before running it the appsettings.json must be changed:

"AppSettings": {
    "Port": 5400 - Port number of the running backend API.
  },

"Http": {
    "Url": "http://localhost:5402" -- Ports on which the website is run
},

"Https": {
    "Url": "https://localhost:5403"
}

After setting up the appsettings.json the Shipment.exe can be started. After starting it the website can be accesed at localhost:<the set port>.

- The directory "Shipment" is the directory of the source code of the website.
- The directory "ShipmentAPI" is the directory of the source code of the backend API.
- The directory "ShipmentCommonClasses" is the source code of the built NuGet package, containing common classes for both applications.
- The directory "ShipmentCommonClassesPackages" is the NuGet package.
- The script TablesCreateAndFill.sql is the sql script file which creates the tables with their indexes and primary/foreign keys aswell as an insert batch to fill the tables with meaningful data.
