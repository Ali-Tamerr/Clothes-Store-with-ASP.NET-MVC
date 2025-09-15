# Clothes Store with ASP.NET MVC

A full-featured online clothes store built with ASP.NET MVC. This project demonstrates a modern e-commerce web application, showcasing best practices in ASP.NET MVC, Entity Framework, and front-end technologies. Easily browse, search, and order clothing items with an intuitive UI and robust backend.

## Features

- **User Authentication & Authorization:** Registration, login, and secure account management.
- **Product Catalog:** Browse clothing by categories, filter and search products.
- **Shopping Cart:** Add, update, or remove items and view cart summaries.
- **Order Management:** Checkout, order history, and order tracking for users.
- **Admin Dashboard:** Manage products, categories, orders, and users.
- **Data Persistence:** Uses Entity Framework with a SQL database.

## Technologies Used

- **Backend:** ASP.NET MVC (C#), Entity Framework
- **Frontend:** HTML5, CSS3, JavaScript, Bootstrap
- **Database:** SQL Server (LocalDB or SQL Express)
- **Authentication:** ASP.NET Identity

## Getting Started

### Prerequisites

- Visual Studio (2019 or later recommended)
- .NET Framework 4.7.2 or later
- SQL Server Express or LocalDB

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/Ali-Tamerr/Clothes-Store-with-ASP.NET-MVC.git
   ```
2. **Open the solution** in Visual Studio.

3. **Restore NuGet Packages** (Visual Studio should do this automatically).

4. **Update the Database:**
   - Open the Package Manager Console.
   - Run:
     ```
     Update-Database
     ```
   - This will create and seed the database.

5. **Run the Application**
   - Press `F5` or click on **Start Debugging**.

## Usage

- Register as a new user or log in.
- Browse products, add them to your cart, and place orders.
- If you log in as an admin, manage products, categories, and orders from the admin dashboard.

## Folder Structure

- `/Controllers` - MVC controllers for application logic
- `/Models` - Entity Framework models and view models
- `/Views` - Razor views (UI)
- `/Content` - CSS, images, and static files
- `/Scripts` - JavaScript files
- `/Migrations` - Entity Framework migrations

## Screenshots

<!-- Add screenshots/gifs of your app here -->
![Home Page](docs/screenshots/home.png)
![Product Page](docs/screenshots/product.png)
![Admin Dashboard](docs/screenshots/admin.png)

## Contributing

Contributions are welcome! Please open issues or submit pull requests for improvements and fixes.

## License

This project is licensed under the [MIT License](LICENSE).

## Author

- [Ali Tamerr](https://github.com/Ali-Tamerr)

---

> **Note:** This is a sample project for educational purposes. For production use, ensure to follow security best practices and further harden authentication and data validation.
