# TaskMgmntApp
Backend logic(server) of TaskManagementApp, exposed crucial apis to serve for TaskMgmntFrontEnd.
Added the active azure connection details in appsettings.json, which will be removed after a week.

![image](https://github.com/user-attachments/assets/2e8004dd-2329-4e1c-afc3-03331217de03)


# Configuration
.NET 8,
EFCore 9,
Azure Sqldb,
Azure Blob,
NUnit,
Serilog,
API.

# Design Pattern
UOW,
Respository,
Iterator.

# Controllers
TaskController used for all Task related CRUD operations.
AuthenticationController used for creating and validating user.

# Service
For connecting Blob

# EFCore
Used code first approach.
Add-Migration InitialCreate
Update-Database



