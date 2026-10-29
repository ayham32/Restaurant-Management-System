# Restaurant Management System API

## Overview

A RESTful API for managing restaurant operations, including products, orders, customers, employees and billing workflows.

Built backend with .NET Core 8 and SQL Server using a layered architecture and several software design patterns, and built frontend with HTML,CSS,JavaScript

---

## Features

* JWT Authentication with Refresh Tokens
* Role-Based Authorization
* Employee Management
* Customer Management
* Product and Category Management
* Order Management
* Order Items Management
* Billing System
* Delivery Management
* Pagination and Filtering
* Global Exception Handling Middleware
* Logging
* FluentValidation
* Result Pattern

---

## Architecture

The project follows a layered architecture:

*RestaurantFrontend
* RestaurantApi
* RestaurantBusiness
* RestaurantDataAccess
* RestaurantShared

### Implemented Patterns

* Repository Pattern
* Generic Repository Pattern
* Unit of Work Pattern
* Result Pattern

---

## Technologies

* ASP.NET Core 8
* Entity Framework Core
* SQL Server
* JWT Authentication
* FluentValidation
* Swagger / OpenAPI

---

## Getting Started

### Clone the repository

```bash
git clone <repository-url>
```

### Configure the connection string

Update the connection string inside:

```json
appsettings.json
```

### Run the application

```bash
dotnet run
```

### Open Swagger

```text
https://localhost:{port}/swagger
```

---

## Project Structure

```text
RestaurantApi
RestaurantBusiness
RestaurantDataAccess
RestaurantShared
```

---

## Future Improvements

* Unit Testing
* Frontend Technology
* Docker Support
* Caching
* Email Notifications
*TypeScript
  
---

## Project Status

Backend implementation completed.

Frontend implementation can be impoved later.
