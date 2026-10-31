# Restaurant Management System — Frontend

The frontend of the Restaurant Management System, built using **HTML5, CSS3, and JavaScript**.

This project was developed to apply frontend development concepts in a practical application and integrate the user interface with a backend RESTful API built using ASP.NET Core Web API.

## Features

- Customer menu and shopping cart.
- Product and category management interfaces.
- Order and billing management interfaces.
- Customer and employee management interfaces.
- Authentication and role-based access to management features.
- Reusable UI components.
- API integration for retrieving and managing application data.
- Client-side cart storage using IndexedDB.

## Technologies

- HTML5
- CSS3
- JavaScript
- Fetch API
- IndexedDB
- RESTful API integration

## Project Structure

```text
ResturantFrontend/
├── assets/
│   └── images/
├── components/
│   ├── footer.html
│   ├── header.html
│   ├── sidebar.html
│   └── switchMode.html
├── css/
├── js/
└── pages/
    ├── admin/
    ├── auth/
    ├── bills/
    ├── categories/
    ├── customers/
    ├── employee/
    ├── employees/
    ├── orders/
    ├── people/
    ├── products/
    ├── public/
    ├── settings/
    └── users/
```

*This structure highlights the main frontend directories. Individual files may differ.*

## Architecture and Maintainability

The frontend separates page markup, styling, JavaScript functionality, and reusable components.

- **Pages:** Organize the user interfaces by functionality.
- **CSS:** Keeps styling separate from page markup and JavaScript.
- **JavaScript:** Handles UI behavior and API communication.
- **Components:** Provide reusable interface elements such as headers, footers, and sidebars.
- **API Integration:** Connects the frontend to the ASP.NET Core backend.

This structure makes the code easier to understand, maintain, and extend.

## Getting Started

### 1. Open the Project

Open the `ResturantFrontend` directory in your preferred code editor.

### 2. Start the Backend

Run the ASP.NET Core Web API by following the instructions in the main repository README.

### 3. Configure the API URL

Make sure the frontend's API configuration points to the correct backend address.

### 4. Start the Frontend

Serve the frontend using a local development server.

If the frontend and backend run on different origins, ensure that the backend's CORS configuration allows the frontend origin.

## Related Project

The backend and complete application documentation are available in the [main repository README](../README.md).
