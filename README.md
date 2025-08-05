# 🧪 ASP.NET Core API Demo Project

This is a learning/demo project built with **ASP.NET Core**, showcasing the implementation of modern and practical API features:

[![.NET Core](https://img.shields.io/badge/.NET%20Core-8.0-blueviolet)](https://dotnet.microsoft.com/)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Abdelrahman%20Zagloul-blue?logo=linkedin)](https://www.linkedin.com/in/abdelrahman-zagloul/)

- ✅ **JWT Authentication** with secure token handling
- 🔄 **Refresh Tokens** using HTTP-only cookies
- 🔐 **Basic Authentication**
- 🌐 **External Logins** via **Google** and **GitHub**
- 🔓 **Logout** with full token revocation and session cleanup
- 🧭 **AutoMapper** for clean and efficient DTO mapping
- 📬 **Notifications** – Send **Emails** (via MailKit or .NET SMTP) and **SMS**
- 🖼️ **Media Handling** – Upload images, videos, or files, with support for download and deletion
- 📦 **CRUD Operations** for multiple entities like **Product** and **Category**

🛡️ **Cross-Cutting Concerns Implemented**:
- ⏱️ **Custom Middleware** to measure endpoint execution time
- ❌ **Global Exception Handling** middleware
- 🚦 **Rate Limiting** to protect against abuse and overuse
- 🧩 **Custom Authorization Filter** for permission-based access control
- 🌍 **CORS** enabled to allow access from **any origin**

⚙️ **Configuration Management**:
- 🧰 Uses `IOptions`, `IOptionsSnapshot`, and `IOptionsMonitor` for structured and dynamic configuration handling

📋 **Logging**:
- 📊 Integrated **Serilog** for structured logging (file, console, and custom sinks)
- 🔍 Logs request pipeline events, errors, and performance metrics

---

This project is ideal for developers looking to master **ASP.NET Core Web API** by integrating real-world features like secure authentication, media management, notifications, advanced configuration, middleware, and structured logging with **Serilog**.

---

## 🚀 Features Implemented

- 🔐 **Authentication & Authorization**
  - User Registration & Login
  - JWT Token generation
  - Refresh Token support (with HTTP-only cookies)
  - Token revocation
  - Basic Authentication
  - Role-based authorization
  - External login with **Google** & **GitHub**
  - Custom Authorization Filter for permission-based access

- 🧭 **AutoMapper**
  - Clean mapping between DTOs and domain models
  - Simplifies response shaping and request validation

- 📁 **Media Upload & Download**
  - Upload image, video, and generic files
  - Download files via endpoint
  - Remove files by type (images, videos, all, specific)

- 📬 **Notifications**
  - Send emails via **MailKit**
  - Send email via **native .NET SMTP**
  - Send SMS Using **Twilio**

- 📦 **CRUD Operations**
  - Full Create, Read, Update, Delete for:
    - Categories
    - Products
  - RESTful structure with clear endpoints

- ⚙️ **Configuration Management**
  - Uses `IOptions`, `IOptionsSnapshot`, and `IOptionsMonitor`
  - Strongly typed settings (e.g., JWT, Email, etc.)
  - Real-time configuration changes support

- ⏱️ **Middleware**
  - Measure and log **execution time** for each request
  - Handle **global exceptions** in a unified way
  - Apply **rate limiting** to prevent abuse

- 🌐 **CORS Configuration**
  - CORS enabled for **any origin**
  - Allows integration with web apps, mobile apps, and tools like Postman

- 📋 **Logging**
  - Integrated **Serilog** for structured and extensible logging
  - Console, file, and custom sink support
  - Captures errors, request lifecycle, and performance data


---

## 🧪 API Endpoints

### 🔐 Account

| Method | Endpoint                       | Description                       |
|--------|--------------------------------|-----------------------------------|
| POST   | `/api/Account/Register`        | Register a new user               |
| POST   | `/api/Account/Login`           | Login and receive JWT             |
| POST   | `/api/Account/Logout`          | Logout user and revoke all tokens |
| POST   | `/api/Account/RefreshToken`    | Get new access token              |
| POST   | `/api/Account/RevokeToken`     | Revoke a refresh token            |
| GET    | `/api/Account/google-login`    | Redirect to Google login          |
| GET    | `/api/Account/google-response` | Handle Google login response      |
| GET    | `/api/Account/github-login`    | Redirect to GitHub login          |
| GET    | `/api/Account/github-response` | Handle GitHub login response      |
| POST   | `/api/Account/AddRole`         | Assign role to a user             |

### 📚 Category

| Method | Endpoint             | Description                  |
|--------|----------------------|------------------------------|
| GET    | `/api/Category`      | Get all categories           |
| POST   | `/api/Category`      | Create a new category        |
| GET    | `/api/Category/{id}` | Get category by ID           |
| PUT    | `/api/Category/{id}` | Update a category            |
| DELETE | `/api/Category/{id}` | Delete a category            |

### 🖼️ Media

| Method | Endpoint                        | Description                         |
|--------|----------------------------------|-------------------------------------|
| POST   | `/api/Media/UploadImage`        | Upload an image file                |
| POST   | `/api/Media/UploadVideo`        | Upload a video file                 |
| POST   | `/api/Media/UploadFile`         | Upload a generic file               |
| POST   | `/api/Media/{type}`             | Upload file based on type           |
| GET    | `/api/Media/Download`           | Download a file                     |
| DELETE | `/api/Media/Remove File`        | Remove a specific file              |
| DELETE | `/api/Media/Clear `             | Remove all media files              |
| DELETE | `/api/Media/Remove Images`      | Remove only images                  |
| DELETE | `/api/Media/Remove Videos`      | Remove only videos                  |
| DELETE | `/api/Media/Remove Files`       | Remove all non-image/video files    |

### 🔔 Notification

| Method | Endpoint                              | Description                         |
|--------|----------------------------------------|-------------------------------------|
| POST   | `/api/Notification/Send Email (MailKit)` | Send email via MailKit             |
| POST   | `/api/Notification/Send Email (.Net)` | Send email via native .NET SMTP     |
| POST   | `/api/Notification/Send SMS`          | Send SMS (mock or configured)       |

### 🛍️ Product

| Method | Endpoint              | Description               |
|--------|-----------------------|---------------------------|
| GET    | `/api/Product`        | Get all products          |
| POST   | `/api/Product`        | Create a new product      |
| GET    | `/api/Product/{id}`   | Get product by ID         |
| PUT    | `/api/Product/{id}`   | Update a product          |
| DELETE | `/api/Product/{id}`   | Delete a product          |

---

## 🛠️ Technologies Used

- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Identity
- JWT Bearer Authentication
- OAuth 2.0 (Google, GitHub)
- AutoMapper
- MailKit
- Swagger / Postman
- HTTP-only cookie-based refresh tokens
- Role-based access control
- Serilog

---

## 🛠️ Setup Instructions

1. **Clone the repository**:

   ```bash
   git clone https://github.com/Abdelrahman-Zagloul/API-Demo.git

2. Add your `appsettings.json` (not included in repo) with OAuth credentials:

   ```json
   {
   "ConnectionStrings": {
    "DefaultConnection": "YourDatabaseConnectionString"
    }
     "Authentication": {
       "Google": {
         "ClientId": "your-google-client-id",
         "ClientSecret": "your-google-secret"
       },
       "GitHub": {
         "ClientId": "your-github-client-id",
         "ClientSecret": "your-github-secret"
       },
       "Facebook": {
         "AppId": "your-facebook-app-id",
         "AppSecret": "your-facebook-secret"
       },
       "LinkedIn": {
         "ClientId": "your-linkedin-client-id",
         "ClientSecret": "your-linkedin-secret"
       },
       "Microsoft": {
         "ClientId": "your-microsoft-client-id",
         "ClientSecret": "your-microsoft-secret"
       }
     },
     "MailSetting": {
       "Email": "Your Email",
       "DisplayName": "Your Name",
       "Password": "Your Password",
       "Host": "smtp.gmail.com",
       "Port": 587
     },
   "JWT": {
      "Key": "Your Key",
      "Issuer": "https://localhost:7250/",
      "Audience": "https://localhost:4200/",
      "DurationInMinutes": 60
    },
    "Twilio": {
       "AccountSID": "Your AccountSID",
       "AuthToken": "Your AuthToken",
       "TwilioPhoneNumber": "+Your TwilioPhoneNumber"
     }
   }
   ```

3. Apply Migrations:

   ```bash
   dotnet ef database update
   ```

4. Run the project:

   ```bash
   dotnet run
   ```

---

## 👨‍💻 Author

Abdelrahman Zaglol
.NET Developer | Computer Science Student
[LinkedIn](https://www.linkedin.com/in/abdelrahman-zagloul/)

---
