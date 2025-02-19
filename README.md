# TaskMgmntApp

## Overview
TaskMgmntApp is the **backend server** for TaskManagementApp, providing essential APIs for the frontend (**TaskMgmntFrontEnd**). It integrates with **Azure SQL Database** and **Azure Blob Storage** for data management and file storage.

⚠ **Note:** Active Azure connection details are available in `appsettings.json` but will be removed after a week.

![TaskMgmntApp](https://github.com/user-attachments/assets/2e8004dd-2329-4e1c-afc3-03331217de03)

---

## 🛠 Technology Stack
- **.NET 8**
- **Entity Framework Core 9 (EFCore)**
- **Azure SQL Database**
- **Azure Blob Storage**
- **NUnit** (for unit testing)
- **Serilog** (for logging)
- **RESTful APIs**

---

## 🎯 Design Patterns Implemented
- **Unit of Work (UoW)**
- **Repository Pattern**
- **Iterator Pattern**

---

## 🚀 Controllers
### 1️⃣ **TaskController**
Handles **CRUD operations** related to tasks, including:
- Creating a new task
- Updating task details
- Deleting tasks
- Fetching task lists

### 2️⃣ **AuthenticationController**
Manages **user authentication**, including:
- User registration
- User login & token validation

---

## 🔗 Services
- **Azure Blob Storage Service**: Handles file uploads and retrieval.

---

## 📦 EF Core & Database Setup
This project follows the **Code-First Approach** using EF Core.

### 📌 Steps to Apply Migrations
1. Create an initial migration:
   ```bash
   Add-Migration InitialCreate
   ```
2. Apply the migration to update the database:
   ```bash
   Update-Database
   ```

---

## 📝 License
This project is licensed under the **MIT License**.

---

### 🚀 **Happy Coding!** 🎯

