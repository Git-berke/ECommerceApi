## Overview

ECommerceApi is a RESTful API built with ASP.NET Core 8.0 that provides e-commerce functionality for managing products, categories, users, shopping carts, and orders. This API serves as a backend service for e-commerce applications, allowing clients to perform CRUD operations on various resources.

## Features

- **Product Management**: Create, read, update, and delete products
- **Category Management**: Organize products with categories
- **User Management**: Handle user accounts and authentication
- **Shopping Cart**: Manage user shopping carts
- **Order Processing**: Create and track orders

## Tech Stack

- **ASP.NET Core 8.0**: Framework for building web applications and services
- **Entity Framework Core**: ORM for database operations
- **SQL Server**: Database storage
- **Swagger/OpenAPI**: API documentation and testing

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB or full instance)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Code](https://code.visualstudio.com/)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/Git-berke/ECommerceApi.git
   cd ECommerceApi
   ```

2. Update the database connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ECommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

3. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. Access the API at `https://localhost:5001` or `http://localhost:5000`

### API Documentation

When the application is running, you can access the Swagger UI at `/swagger` to explore the available endpoints and test the API.

## API Endpoints

### Products

- `GET /api/Products`: Get all products
- `GET /api/Products/{id}`: Get a specific product by ID
- `POST /api/Products`: Create a new product
- `PUT /api/Products/{id}`: Update an existing product
- `DELETE /api/Products/{id}`: Delete a product
