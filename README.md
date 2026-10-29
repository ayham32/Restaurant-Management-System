# Restaurant Management System API

## Overview

A RESTful API for managing restaurant operations, including products, orders, customers, employees, billing, and delivery workflows.

Built with ASP.NET Core 8 and SQL Server using a layered architecture and several software design patterns.

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
* Frontend Application
* Docker Support
* Caching
* Email Notifications

---

## Project Status

Backend implementation completed.

Frontend application will be developed in the future.
