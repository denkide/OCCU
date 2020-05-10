This is the application that OCCU has requested as a sample.
I wrote the application on a MacBook Pro using Parallels and Windows 10.

The details are as follows:
	.NET Core 3.1
	Microsoft Entity Framework Core 3.1
	
The database is designed to be SQL Server.
For the purposes of development, I simply used the "(localdb)\MSSQLLocalDB" database that is installed with Visual Studio.
	
The code uses a database connection to connect the EF Db Context to the database.
The connection string can be edited in the appsettings.json file.
The appsettings.json file can be found in the root of the solution.

The application will need the two database tables that are found in the Data directory.
To add the tables, please create a new database (I called mine "MVCApp").
Then run the two sql create files against that database (Product.sql and Status.sql).
There is also another file (LoadData.sql) to seed the Status table.

The connection to the db from the application will need to have proper permissions set.
If run from within Visual Studio, the "Trusted_Connection" should work, provided the user has permissions on the db.
Otherwise, a database user will need to be defined.

Since this application is a .NET Core application, the .NET Core SDK for Core 3.1 will need to be installed.
The .NET Core SDK can be downloaded here:
https://dotnet.microsoft.com/download/dotnet-core/3.1

If running from outside Visual Studio, there are other considerations that will need to be take into account.
These considerations would involve configuring a server and IIS to support .CORE. 
If choosing to run from outside Visual Studio, the application will also need to be deployed to IIS.
You can read about that here:
https://docs.microsoft.com/en-us/aspnet/core/tutorials/publish-to-iis?view=aspnetcore-3.1&tabs=visual-studio

Microsoft has a good documentation page regarding how to configure a machine for .NET Core here:
https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/iis/?view=aspnetcore-3.1

The Product Status Index page will need to have access to the web to work.
It references the JQuery and Cloudflare cdns to enable the charting functionality.
Since jquery is already installed with the default MVC application, in the real world downloading from "code.jquery.com" would need to be changed.
It is included in this version for proof of concept purposes.

Much of the application makes use of the default scaffolding from the MVC template in Visual Studio for .NET Core 3.1.
Therefore, not a large amount of emphasis was put into restyling the UI.
The default Bootstrap css has been used.


