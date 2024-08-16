# Customer API

## Overview

The Customer API provides endpoints for managing users, their accounts, and transactions. Key functionalities include creating users, managing accounts, recording transactions, and retrieving user details with account balances and transaction histories.

## Features

- **User Management**: Create and retrieve user/customer details.
- **Account Management**: Create accounts and manage transactions.
- **Transaction Management**: Record transactions and calculate account balances.

## Technologies

- ASP.NET Core
- Entity Framework Core
- In-Memory Database for Testing
- NUnit and Moq for Unit Testing

## Setup

### Prerequisites

- .NET 6.0 SDK or later

### Installation

1. **Clone the Repository**
   ```bash
   git clone https://github.com/alekseiko97/customer-api.git

2. **Navigate to the Project Directory**
    ```bash
    cd customer-api
    ```
3. **Restore Dependencies**
    ```bash
    dotnet restore
    ```
4. **Run the Application**
    ```bash
    dotnet run
    ```
5. **Run the Tests**
    ```bash
    dotnet test
    ```

## Configuration

### Exception Handling 

To log unhandled exceptions and provide a generic error message, use the following middleware configuration in Program.cs:

```csharp
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception != null)
        {
            logger.LogError(exception, "Unhandled exception occurred");
            var result = new { error = "An unexpected error occurred. Please try again later." };
            await context.Response.WriteAsJsonAsync(result);
        }
    });
});
```

### Data Seeding
To seed the database with initial data, SeedData method is used:

```csharp
static async Task SeedData(ApplicationDbContext context, IUserService userService, IAccountService accountService, ITransactionService transactionService)
{
    // Create Users
    var user1 = await userService.CreateUserAsync("John", "Doe");
    var user2 = await userService.CreateUserAsync("Jane", "Smith");

    // Create Accounts
    var account1 = await accountService.CreateAccountAsync(user1.ID, 1000);
    var account2 = await accountService.CreateAccountAsync(user2.ID, 500);
    var account3 = await accountService.CreateAccountAsync(user1.ID, 1500);

    // Create Transactions
    if (account1 != null)
    {
        await transactionService.CreateTransactionAsync(account1, 500);
        await transactionService.CreateTransactionAsync(account1, 200);
    }
    if (account2 != null)
    {
        await transactionService.CreateTransactionAsync(account2, 300);
    }
    if (account3 != null)
    {
        await transactionService.CreateTransactionAsync(account3, 100);
    }
}
```

Note that transactions can only be created by utilizing TransactionService.

## API Endpoints

### Users
### Create User
- Endpoint: POST /api/v1/users
- Query Parameters: firstName, lastName
- Description: Creates a new user.

### Get User
- Endpoint: GET /api/v1/users/{id}
- Description: Retrieves a user by ID.

### Accounts
### Create Account
- Endpoint: POST /api/v1/accounts
- Query Parameters: customerId, initialBalance
- Description: Creates a new account with an optional initial balance.

### Get Account
- Endpoint: GET /api/v1/accounts/{id}
- Description: Retrieves an account by ID.

### Get transactions per account
- Endpoint: GET /api/v1/accounts/{accountId}/transactions
- Description: Retrieves all transactions for account ID.

## Testing

*** Unit tests cover: ***
- User creation and validation
- Account creation and transaction recording
- Error handling and response validation

### Running Tests
Execute the following command to run all unit tests:

```bash
dotnet test
```
## Logging
Unhandled exceptions are logged using ILogger configured in Program.cs.