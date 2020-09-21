# WealthShare-
Steps to run the WealthShare project in visual studio:

1. Install Visual Studio on your PC (preferably Visual Studio 2017). Select ASP.Net and Web Applications to install when prompt.
2. Install MS SQL Express. (link: https://www.microsoft.com/en-au/sql-server/sql-server-2019)
3. Install SQL Server Management Studio. (link: https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-2017)
4. Download the .zip file from the git repository.
5. Open the SSMS and **before connecting copy the contents of the server name** as it will be required in the connection string.
6. Now connect to the server. After connecting to the sql database navigate to the database folder and right click on it and click on "Import Data-tier Application".
   click next and in "import from local disk" browse to the **.bacpac file** included in the repository and select Next. In this window in the "database name" use the 
   name **"WealthShareDBV2"** as this is the name used in the code. After the database name is entered correctly press next and then finish. This will import the database.
7. Manage NuGet Packages ->pagedlist.mvc ->install
8. Manage NuGet Packages ->Bootstrap-4.4.0->update
9. Manage NuGet Packages ->jQuery-3.5.1->update
10. Manage NuGet Packages ->jQuery.Validation-1.19.2->update
11. Now open Visual Studio and from the toolbar click on open sln. Then browse to the WealthShareV_2.sln file located in WealthShareV_2 folder in the repository.
12. Inside the solution browser navigate to the web.config file. Inside this file navigate to the Connection string tag (line 60). In this line find the data source 
   tag and there you can see a SQL server connected. Replace the [pc name]\SQLEXPRESS with the server name copied in step 4.
13. Navigate to the homecontroller and run the project.
