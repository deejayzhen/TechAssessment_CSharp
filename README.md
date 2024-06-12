## Technical Assessment on .Net

### Project Overview
The whole project is made in C# .Net Core 6 ASP.Net MVC, on the Products tab you can Create, Read, Update, and Delete and item.

### Setting up the database
1. Change the `Server` in `appsettings.json` according to your local server (`SqlServer Management Studio 19`).

![image](https://github.com/deejayzhen/TechAssessment_CSharp/assets/35642849/a219e395-de1d-429b-9fd8-88e0c726fed7)

2. Go to `Tools > Nuget Package Manager > Package Manager Console`
3. Enter this command `Update-database` but if `Migrations` folder is empty then enter this command first `Add-migration`

![image](https://github.com/deejayzhen/TechAssessment_CSharp/assets/35642849/871a0e7f-4787-4a80-9bab-c94e8a5d15a4)
 
