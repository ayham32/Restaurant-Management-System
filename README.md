# Restaurant Management System

A full-stack restaurant management application: an **ASP.NET Core 8 Web API** backend and a **vanilla HTML / CSS / JavaScript** frontend (no framework).

It covers the daily operations of a restaurant: products, categories, orders, bills, customers, and employees, plus a public menu with a shopping cart.

I built the backend first, then wrote the frontend from scratch on top of it, to apply what I learned about HTML, CSS, JavaScript, and consuming a REST API.

<img width="1366" height="681" alt="employee dashboard" src="https://github.com/user-attachments/assets/2434fa6e-5243-4ce4-a13a-d67b12905706" />
<img width="1347" height="681" alt="Categories Screen" src="https://github.com/user-attachments/assets/73bc8bcb-2bd0-4023-8ed0-21c9bc544d29" />
<img width="1366" height="683" alt="Swager" src="https://github.com/user-attachments/assets/f37fc6cf-001b-47b4-9cb8-6909381644f7" />

## Highlights

- JWT authentication with refresh tokens (rotated on every refresh, stored hashed) and role-based authorization (roles: Admin, Employee).
- Passwords hashed with BCrypt; rate limiting on the authentication endpoints.
- Layered backend: API → Business → Data Access → Shared.
- Orders are priced on the server from the database, never from client input, and stock is updated inside database transactions.
- Customer menu with a cart stored in the browser's IndexedDB.
- Modular frontend: shared components, API access separated from page logic.

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Security Notes](#security-notes)
- [Project Status](#project-status)
- [Author](#author)

## Features

**Authentication and authorization**
- Login, logout, and token refresh.
- Role-based access control on API endpoints.

**Restaurant management**
- Products and categories.
- Orders and order items, with order status tracking.
- Bills, with tax and percentage discounts.
- Customers, employees, and people records.
- Users and role assignment.

**Customer side**
- Public menu with category filtering and search.
- Shopping cart (IndexedDB).

**General**
- Admin and employee dashboards.
- Pagination and filtering.
- FluentValidation for request validation.
- Global exception handling middleware with logging.
- Result Pattern for operation outcomes.
- Swagger / OpenAPI documentation.

## Technologies

**Backend**
- C#, .NET 8, ASP.NET Core Web API
- Entity Framework Core (database-first), SQL Server
- JWT Bearer authentication, BCrypt
- FluentValidation
- Swagger / OpenAPI

**Frontend**
- HTML5, CSS3, JavaScript (ES modules)
- Fetch API for REST integration
- IndexedDB for client-side cart storage

## Architecture

### Backend: layered architecture

| Project | Responsibility |
| --- | --- |
| `RestruantApi` | HTTP endpoints, middleware, authentication and authorization setup |
| `RestaurantBusiness` | Business services and validation |
| `RestaurantDataAccess` | `DbContext`, repositories, Unit of Work |
| `RestaurantShared` | Entities, DTOs, enums, pagination models, result types |

Patterns used: Repository, Generic Repository, Unit of Work, Result Pattern.

### Frontend: modular structure

Pages, stylesheets, scripts, and reusable HTML components (header, footer, sidebar) live in separate folders. API calls are kept in `js/api/`, separate from page logic.

## Project Structure

```text
Restaurant-Management-System/
├── RestruantApi/            # Web API (controllers, Program.cs, appsettings.json)
├── RestaurantBusiness/      # Services, interfaces, validators
├── RestaurantDataAccess/    # DbContext, repositories, Unit of Work
├── RestaurantShared/        # Entities, DTOs, enums, pagination, results
├── ResturantFrontend/
│   ├── assets/
│   ├── components/
│   ├── css/
│   ├── js/
│   └── pages/
├── database/
│   └── RestaurantDB.sql     # Schema + demo data
├── docs/screenshots/
└── RestaurantManagementSystem.slnx
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or SQL Server Express, and SQL Server Management Studio (or any tool that can run a `.s[Uploading RestaurantDB.sql…]()
ql` script)
- Visual Studio 2022 (or another .NET IDE)
- A modern browser

### 1. Clone the repository

```bash
git clone https://github.com/ayham32/Restaurant-Management-System.git
cd Restaurant-Management-System
```

### 2. Create the database

This project uses a **database-first** approach: the database is created with SQL, and the EF Core model was scaffolded from it.

1. Open `database/RestaurantDB.sql` in SQL Server Management Studio.
2. Run the script. It creates the `ResturantDB` database, all tables and constraints, the roles (Admin, Employee), a few demo products, orders and bills, and two demo accounts. If the schema already exists, it does nothing.

Then set the connection string in `RestruantApi/appsettings.json` to match your SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ResturantDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**Demo logins (local use only):**

| Username | Password | Role |
| --- | --- | --- |
| `admin` | `Admin@123` | Admin |
| `employee` | `Admin@123` | Employee |


### 3. Configure the JWT secret

The signing key is **not** stored in the repository. Set it with .NET User Secrets (use any random string of at least 32 characters):

```bash
dotnet user-secrets init --project RestruantApi
dotnet user-secrets set "JWT_SECRET_KEY:Key" "<your-random-secret-at-least-32-chars>" --project RestruantApi
```

The issuer and audience values are in `appsettings.json` under `JWT_SECRET_KEY`.

### 4. Configure CORS

The API only accepts requests from the origins listed in `appsettings.json`. Add the address your frontend runs on:

```json
"Cors": {
  "AllowedOrigins": [
    "http://localhost:5500",
    "http://127.0.0.1:5500"
  ]
}
```

`localhost` and `127.0.0.1` are different origins, so list both if you use both. The API will not start if this list is empty.

### 5. Run the backend

Trust the local HTTPS certificate once:

```bash
dotnet dev-certs https --trust
```

Then run the API with the `https` profile:

```bash
dotnet run --project RestruantApi --launch-profile https
```

Or open `RestaurantManagementSystem.slnx` in Visual Studio, set `RestruantApi` as the startup project, and run it.

Swagger is available in the Development environment at:

```text
https://localhost:7115/swagger
```

### 6. Run the frontend

The frontend expects the API at `https://localhost:7115/api`. If your port is different, change `BASE_URL` in `ResturantFrontend/js/api/api.js`.

Serve the `ResturantFrontend` folder with any static server. The frontend uses ES modules, so opening the HTML files directly with `file://` will not work. For example:

- **VS Code:** open the folder and use the *Live Server* extension (default port 5500).

Make sure the origin you use is listed in `Cors:AllowedOrigins` (step 4).

Entry pages:

| Page | Path |
| --- | --- |
| Public menu | `pages/public/menue.html` |
| Staff login | `pages/auth/login.html` |

## Security Notes

- Secrets (JWT key) live in User Secrets or environment variables, never in source control.
- CORS is restricted to the configured origins.
- Passwords are hashed with BCrypt; refresh tokens are stored hashed and rotated on use.
- The authentication endpoints are rate limited.
- Use HTTPS in development and deployment.
- The frontend currently stores tokens in `localStorage`. For a production deployment, HttpOnly cookies would be safer (see below).

## Project Status

The core backend and frontend features are implemented and working together.

Known gaps and planned work:

- Customer checkout: customers can browse the menu and fill a cart, but placing an order is currently done by staff through the API.
- Unit and integration tests (starting with the order and billing services).
- Moving refresh tokens to HttpOnly cookies.
- Docker support.

## Author

**Ayham Loai Badran**
Computer Systems Engineering student, Palestine Technical University – Kadoorie

GitHub: [ayham32](https://github.com/ayham32) · LinkedIn: [ayham-badran](https://www.linkedin.com/in/ayham-badran-b33018361/))
