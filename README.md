# 🔐 JWT Authentication API (ASP.NET Core)

This project demonstrates a complete implementation of **JWT Authentication and Authorization** using **ASP.NET Core** and **Entity Framework Core** with **PostgreSQL**.

It covers real-world concepts like user registration, login, token generation, and protected endpoints.

---

## 🚀 Features

* User Registration & Login
* Secure Password Hashing
* JWT Token Generation
* Token Expiration Handling
* Role-Based Authorization (Admin/User)
* Protected API Endpoints
* Entity Framework Core with PostgreSQL
* Clean Architecture (Controllers, Models, DTOs, Data)

---

## 🛠️ Tech Stack

* ASP.NET Core (.NET)
* Entity Framework Core
* PostgreSQL
* JWT (JSON Web Tokens)
* Swagger (API Testing)

---

## 📁 Project Structure

```
JWTAuthDemo/
│
├── Controllers/
│   └── AuthController.cs
│
├── Data/
│   └── AppDbContext.cs
│
├── Models/
│   └── User.cs
│
├── DTOs/
│   ├── RegisterDto.cs
│   └── LoginDto.cs
│
├── Program.cs
├── appsettings.json
```

---

## ⚙️ Setup Instructions

### 1. Clone the Repository

```
git clone https://github.com/your-username/jwt-auth-demo.git
cd jwt-auth-demo
```

---

### 2. Configure Database

Update your `appsettings.json`:

```
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=jwt_db;Username=postgres;Password=yourpassword"
}
```

---

### 3. Configure JWT

```
"Jwt": {
  "Key": "THIS_IS_A_SECRET_KEY_CHANGE_IT",
  "Issuer": "yourapp",
  "Audience": "yourapp"
}
```

---

### 4. Run Migrations

```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

### 5. Run the Application

```
dotnet run
```

Swagger will open at:

```
https://localhost:<port>/swagger
```

---

## 🔑 API Endpoints

### Register User

```
POST /api/auth/register
```

**Request Body:**

```
{
  "username": "nikhil",
  "password": "123456",
  "role": "User"
}
```

---

### Login

```
POST /api/auth/login
```

**Response:**

```
{
  "token": "your_jwt_token_here"
}
```

---

## 🔒 Protected Endpoint Example

Add `[Authorize]` to secure endpoints:

```
[Authorize]
[HttpGet("secure")]
public IActionResult SecureData()
{
    return Ok("This is protected data");
}
```

---

## ⏳ Token Expiration

JWT token includes expiration time. Once expired:

* User must login again
* API will return `401 Unauthorized`

---

## 🧪 Testing

Use Swagger:

1. Login → Copy token
2. Click **Authorize**
3. Paste:

```
Bearer your_token_here
```

This project is for learning purposes.
