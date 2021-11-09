# MVC Private Notes
A fast and reliable platform to store your notes on. Supports multiple user sessions and stores all data locally in a SQL server.

**Dependencies**
```
ASP.NET Core
SQL Server
EF Core
Identity
```

**Notes**

Use ```dotnet restore``` to restore the project and install all the necessary dependencies.

Create a migration using ```dotnet ef migrations add``` and update the database to this migration using ```dotnet ef database update```.

Then use ```dotnet build``` and ```dotnet run``` to compile and run the web application locally.
