# 🗒️ NoteApp – ASP.NET Core Project

## 📘 Overview
**NoteApp** is a RESTful API that allows users to manage personal notes with authentication, authorization, and role-based access control.  
Built with **ASP.NET Core 8**, **Entity Framework Core**, **JWT**, and **Clean Architecture**, this project follows professional backend engineering standards and SOLID principles.

---

## ⚙️ Features
- 🔐 **Authentication & Authorization**
  - JWT Token-based Login and Register
  - Role-based Access Control (`Admin`, `User`)
  - Secure password hashing using `IPasswordHasher`
- ⚙️ **Clean Architecture**
  - Separation of layers: Controllers, Services, Repositories, Unit of Work, DTOs, and Validators
  - Exception handling with custom `ExceptionMiddleware`
  - Dependency Injection across all layers
- ✅ **Validation & Mapping**
  - Input validation using **FluentValidation**
  - Object mapping with **AutoMapper**
- 🧩 **Database**
  - PostgreSQL with **Entity Framework Core**
  - Relationships between `User` and `Note` (One-to-Many)

---


## 🧠 How It Works
1. A user **registers** or **logs in** and receives a JWT token.
2. Authenticated users can create, read, update, and delete only their own notes.
3. Admins can access and manage all users or notes.
4. All exceptions are handled globally through the **ExceptionMiddleware**.
5. Each layer has a **single responsibility** (SRP principle).

---

## 🧩 Technologies Used
| Category | Technology |
|-----------|-------------|
| Framework | ASP.NET Core 8 |
| ORM | Entity Framework Core |
| Database | PostgreSQL |
| Auth | JWT Authentication |
| Validation | FluentValidation |
| Object Mapping | AutoMapper |
| Design Pattern | Repository + Unit of Work |
| Architecture | Clean Architecture / SOLID |
| Others | Dependency Injection, Middleware |

---

## 🧱 Project Structure
```plaintext
NoteApp.Api/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── UserController.cs
│   ├── NoteController.cs
│   └── BaseController.cs
│
├── Services/
│   ├── AuthService.cs
│   ├── UserService.cs
│   ├── NoteService.cs
│   └── JwtService.cs
│
├── Repositories/
│   ├── UserRepository.cs
│   └── NoteRepository.cs
│
├── Middleware/
│   └── ExceptionMiddleware.cs
│
├── Domain/
│   ├── Entities/
│   ├── Dtos/
│   ├── Exceptions/
│   └── Validators/
│
├── UnitOfWork/
│   └── UnitOfWork.cs
│
└── Program.cs
```
---

## 🚀 Setup Instructions

### 1️⃣ Clone the repository
```bash
git clone https://github.com/<your-username>/NoteApp.git
cd NoteApp
```
### 2️⃣ Configure environment variables

 In your system or .env file, set:

```bash
JWT_KEY=YourSuperSecretKey
```
### 3️⃣ Set up the database

In your appsettings.json:
```bash
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=NoteApp;Username=postgres;Password=yourpassword"
}
```

### 4️⃣ Apply migrations
```bash
dotnet ef database update
```
### 5️⃣ Run the application
```bash
dotnet run
```

### 6️⃣ Open Scalar UI
```bash
https://localhost:5001/scalar
```

