# Restaurant Management System

A full-stack restaurant management system built with ASP.NET Core Web API, Entity Framework Core, SQL Server, and vanilla JavaScript.

The application provides features for managing restaurant products, categories, customers, employees, orders, and billing through a layered backend architecture and a separate frontend.

## Features

### Authentication and Authorization
- JWT-based authentication
- Refresh token support
- Role-based authorization

### Restaurant Management
- Product and category management
- Customer management
- Employee management
- Order and order item management
- Billing management
- Dashboard statistics

### Backend Features
- RESTful API
- Entity Framework Core
- Pagination and filtering
- FluentValidation
- Global exception handling middleware
- Logging
- Result Pattern

## Architecture

The solution is organized into separate projects to improve separation of concerns and maintainability.

- **RestaurantFrontend** — Customer-facing pages and management interfaces built with HTML, CSS, and JavaScript.
- **RestruantApi** — API controllers, application configuration, and middleware.
- **RestaurantBusiness** — Business services, service interfaces, and validation rules.
- **RestaurantDataAccess** — Entity Framework Core, repositories, and Unit of Work.
- **RestaurantShared** — Entities, DTOs, enums, parameters, and shared result models.

## Design Patterns

- Repository Pattern
- Generic Repository Pattern
- Unit of Work Pattern
- Result Pattern

## Technologies

### Backend
- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- FluentValidation
- Swagger / OpenAPI

### Frontend
- HTML5
- CSS3
- JavaScript
- IndexedDB for client-side cart storage

## Project Structure

```text
RestaurantManagementSystem/
├── RestruantApi/
├── RestaurantBusiness/
├── RestaurantDataAccess/
├── RestaurantShared/
├── ResturantFrontend/
├── RestaurantManagementSystem.slnx
├── .gitignore
└── README.md
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- A code editor such as Visual Studio
- A modern web browser

### 1. Clone the Repository

```bash
git clone https://github.com/ayham32/Restaurant-Management-System.git
cd Restaurant-Management-System
```

### 2. Configure the Database

Configure the database connection string in the API configuration.

Do not commit real database passwords, JWT signing keys, or other secrets to source control.

Make sure the database schema is available before running the application. Database creation or migration instructions should be added here according to the project's actual database setup.

### 3. Run the Backend

Open the solution in Visual Studio, set `RestruantApi` as the startup project, and run the application.

Once the API is running, open Swagger using the local HTTPS address displayed by the application, followed by `/swagger`.

For example:

```text
https://localhost:<port>/swagger
```

### 4. Run the Frontend

Open the frontend pages using a local development server.

Ensure the API base URL configured in the frontend matches the address where the backend is running.

## Future Improvements

- Unit and integration testing
- TypeScript
- Frontend framework integration
- Docker support
- Caching
- Email notifications

## Project Status

This is a portfolio and learning project.

The backend and frontend have been implemented, with further testing, documentation, and improvements planned.
