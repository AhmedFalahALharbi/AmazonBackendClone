# Mini Amazon Clone

## Project Overview
This project is a mini e-commerce system built using .NET Core. It provides user authentication, product listings, order management, and secure access control.

## Features
- User authentication with JWT tokens
- Admin role for managing products
- Customers can view products and place orders
- Role-based and policy-based authorization
- Optimized queries using Dapper
- Unit testing with Moq & xUnit/NUnit
- API documentation with Swagger

## Technologies Used
- .NET Core
- Entity Framework Core
- Dapper
- JWT Authentication
- Moq & xUnit/NUnit for unit testing
- Swashbuckle (Swagger) for API documentation

## Setup Instructions

### Prerequisites
- Install [.NET Core SDK](https://dotnet.microsoft.com/)
- Install [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Install [Postman](https://www.postman.com/) for API testing

### Steps to Run the Project
1. Clone the repository:
   ```sh
   git clone <repository-url>
   cd mini-amazon-clone
   ```
2. Set up the database:
   ```sh
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```
3. Run the application:
   ```sh
   dotnet run
   ```
4. Access API endpoints via Postman or a frontend client.

### Swagger API Documentation
Once the application is running, open your browser and go to:
- [Swagger UI](http://localhost:5000/swagger) (for local development)
- [Swagger UI](https://localhost:5001/swagger) (for HTTPS)

## API Endpoints

### Authentication
- `POST /api/users/register` - Register a new user
- `POST /api/users/login` - Authenticate and receive a JWT token

### Products
- `GET api/products` - List all products
- `POST api/products` - Admin only: Add a new product

### Orders
- `POST api/orders/create` - Customers place an order
- `GET api/orders/{id}` - Retrieve order details
- `GET api/orders/all` - Admin only: View all orders

## Testing
Run unit tests with:
```sh
dotnet test
```

## Contribution
Feel free to fork the repository and submit pull requests!

## License
MIT License

