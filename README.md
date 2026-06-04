#BankingSystemAPI

A secure and scalable **RESTful Banking System API** built with **ASP.NET Core**, 
**Dapper ORM**, **JWT Authentication**, and **SQL Server (Azure Data Studio)**.

## Features

- User Registration & Login with **BCrypt** password hashing
- **JWT Bearer Token** Authentication & Authorization
- Create & Manage Bank Accounts (Savings / Checking)
- Deposit, Withdrawal & Transfer Transactions
- Full Transaction History per Account
- **Dapper ORM** for fast, lightweight SQL queries
- **Swagger UI** with JWT support for API testing
- Clean Architecture with Repository Pattern
- RFC 7807 Problem Details for structured error responses

## Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core| Web API Framework |
| Dapper | Micro ORM for SQL queries |
| SQL Server | Database (via Azure Data Studio) |
| JWT Bearer | Authentication & Authorization |
| BCrypt.Net | Password Hashing |
| Swagger / Swashbuckle | API Documentation & Testing |

## Project Structure
BankingSystemAPI/
├── Controllers/        # API Endpoints
├── Data/               # Dapper DB Context
├── DTOs/               # Data Transfer Objects
├── Models/             # Database Models
├── Repositories/       # Data Access Layer
│   └── Interfaces/     # Repository Contracts
├── Services/           # Business Logic (JWT Token)
├── SQL/                # Database creation scripts
├── appsettings.json    # Configuration
└── Program.cs          # App Entry Point

## Setup & Installation

### Prerequisites
- .NET 10 SDK
- SQL Server
- Azure Data Studio

### 1. Clone the repository
git clone https://github.com/aryann8140-ui/BankingSystemAPI.git
cd BankingSystemAPI

### 2. Set up the database
Open Azure Data Studio → create database

### 3. Update connection string in appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BankingDB;
   Trusted_Connection=True;TrustServerCertificate=True;"
}

### 4. Run the API
dotnet restore
dotnet run

### 5. Open Swagger UI
https://localhost:{port}/swagger

## API Endpoints

### Auth
| Method | Endpoint | Description |
|---|---|---|
| POST | /api/auth/register | Register a new user |
| POST | /api/auth/login | Login and get JWT token |

### Accounts
| Method | Endpoint | Description |
|---|---|---|
| GET | /api/account | Get all accounts for logged-in user |
| POST | /api/account | Create a new bank account |

### Transactions
| Method | Endpoint | Description |
|---|---|---|
| POST | /api/transaction | Deposit / Withdraw / Transfer |
| GET | /api/transaction/history/{accountNumber} | Get transaction history |

## Authentication
All Account and Transaction endpoints require a **JWT Bearer Token**.

1. Register and Login via `/api/auth/login`
2. Copy the token from the response
3. In Swagger click **Authorize** → enter `Bearer {your_token}`
4. All protected endpoints are now accessible

## Author
**Aryan**
GitHub: [@aryann8140-ui](https://github.com/aryann8140-ui)
