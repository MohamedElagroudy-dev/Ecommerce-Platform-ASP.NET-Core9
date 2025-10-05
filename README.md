# 🛍️ E-Commerce API

This project is the backend implementation of an **E-Commerce application** built with **ASP.NET Core Web API** following the **Clean Architecture** approach.
Originally developed as part of a full-stack .NET + Angular course, this repository contains **only the backend logic**.

---

## 🚀 Features

* **ASP.NET Core 9 Web API**
* **Entity Framework Core** with Code-First Migrations
* **Repository Pattern & Unit of Work**
* **Clean Architecture** design for scalability and maintainability
* **Authentication & Authorization** using **JWT** and **ASP.NET Identity**
* **Role-based Authorization (Admin, Customer)**
* **Shopping Cart** stored in **Redis**
* **Order Management**
* **Payment Integration** with **Stripe**
* **Rating System** for products
* **Error Handling Middleware** & Validation Responses
* **Pagination, Filtering, Sorting, Searching**
* **Scalar** integrated for API testing and documentation

---

## 🛠️ Tech Stack

* **.NET Core 9 (Web API)**
* **Entity Framework Core**
* **SQL Server** (Database)
* **Redis** (Shopping Cart)
* **Stripe API** (Payments)
* **Scalar** (API documentation & testing)

---

## 📂 Project Structure

```
src/
 ├── API/               → Presentation Layer (Controllers, Middleware, etc.)
 ├── Application/       → Application Layer (Services, DTOs, Business Logic)
 ├── Core/              → Domain Layer (Entities, Interfaces)
 ├── Infrastructure/    → Data Access Layer (EF Core, Repositories)
```

---

## ⚡ Getting Started

1. **Clone the repo**

   ```bash
   git clone https://github.com/your-username/ecommerce-backend.git
   ```

2. **Navigate into the project**

   ```bash
   cd ecommerce-backend/src/API
   ```

3. **Update connection strings**
   Edit `appsettings.Development.json` with your **SQL Server** and **Redis** configurations.

4. **Run migrations**

   ```bash
   dotnet ef database update
   ```

5. **Run the API**

   ```bash
   dotnet run
   ```

6. **Explore the API using Scalar**
   Once the app is running, open:

   ```
   https://localhost:<port>/scalar/v1
   ```

---

## 📜 License

This project is for **learning purposes**.
Feel free to **use, modify, and extend** it.
