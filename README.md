# TaskMgmntFrontEnd

## Overview

TaskMgmntFrontEnd is a **React 18** based frontend for the TaskMgmntApp, providing a user-friendly interface for managing tasks efficiently.

## 🛠 Technology Stack

- **React 18**
- **Material-UI (MUI) for UI Components**
- **Node.js 22.14.0**



---

## 🚀 Getting Started

### Prerequisites

Make sure you have the following installed:

- **Node.js v22.14.0** (or later)
- **npm** (comes with Node.js)
- **VS Code (Recommended)**

### 📌 Installation & Setup

1. **Backend Setup:**

   - Clone the backend repository:
     ```bash
     git clone https://github.com/rshere-25/TaskMgmntApp.git
     ```
   - Follow the backend setup instructions in its `README.md` file and launch it.

2. **Frontend Setup:**

   - Clone this repository:
     ```bash
     git clone https://github.com/your-username/TaskMgmntFrontEnd.git
     ```
   - Navigate to the project folder:
     ```bash
     cd TaskMgmntFrontEnd
     ```
   - Install dependencies:
     ```bash
     npm install
     ```
   - Start the development server:
     ```bash
     npm start
     ```

---

## 📷 Screenshots & Workflow

### 1️⃣ **Login Page**

After starting the application, you will see the **Login Page**. First, create a new user.



### 2️⃣ **User Registration**

Once a user is created, you will be redirected to the login page.



### 3️⃣ **TaskBoard**

Upon successful login, users are taken to the **Task Board**, where all tasks are displayed similar to **JIRA**.



### 4️⃣ **Adding a Task**

To create a new task:

- Click on **"Add Task"**.
- Fill in the required details.
- Click **"Submit"**.



Once submitted, the task appears in the **Task Board**, with favorite tasks marked using a filled star icon.



### 5️⃣ **Updating a Task**

- Click on the **task name** to open the update window.
- Modify any fields (e.g., status change to **In Progress**).
- The task will automatically move to the corresponding column.





### 6️⃣ **Deleting a Task**

- Click the **delete button** next to a task to remove it.
- Confirm the deletion in the popup.



---

## 🔮 Future Enhancements

- 🏗 **Drag-and-drop support for moving tasks**
- 🖼 **Image attachment & viewer integration**
- 👨‍💼 **Admin panel for managing task fields dynamically**
- 📌 **Subtasks feature**
- 📂 **Task categorization support**

---

## ❗ Troubleshooting & Guidelines

### ❌ Common Errors & Fixes

#### ⚠️ **Error: npm is not digitally signed**

If you encounter an error like:

```bash
npm : File C:\Program Files\nodejs\npm.ps1 cannot be loaded. The file is not digitally signed.
```

Run the following command in **PowerShell**:

```powershell
Set-ExecutionPolicy Unrestricted -Scope Process -Force
```

Then, try running:

```bash
npm start
```

---

## 📝 License

This project is licensed under the **MIT License**.

---

### 🚀 **Happy Coding!** 🎯

Same for this as well\
\
\# TaskMgmntApp

Backend logic(server) of TaskManagementApp, exposed crucial apis to serve for TaskMgmntFrontEnd.

Added the active azure connection details in appsettings.json, which will be removed after a week.



![image]\(https\://github.com/user-attachments/assets/2e8004dd-2329-4e1c-afc3-03331217de03)





\# Configuration

.NET 8,

EFCore 9,

Azure Sqldb,

Azure Blob,

NUnit,

Serilog,

API.



\# Design Pattern

UOW,

Respository,

Iterator.



\# Controllers

TaskController used for all Task related CRUD operations.

AuthenticationController used for creating and validating user.



\# Service

For connecting Blob



\# EFCore

Used code first approach.

Add-Migration InitialCreate

Update-Database







