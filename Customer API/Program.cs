using Customer_API;
using Customer_API.Models;
using Customer_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure EF Core to use InMemory database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TestDatabase"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer API", Version = "v1" });

    // set the comments path for the Swagger JSON and UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Register other services
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IUserService, UserService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    SeedData(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void SeedData(ApplicationDbContext context)
{
    var user1 = new User { Name = "John", Surname = "Doe" };
    var user2 = new User { Name = "Jane", Surname = "Smith" };

    var account1 = new Account { Balance = 1000};
    var account2 = new Account { Balance = 500 };
    var account3 = new Account { Balance = 1500 };
    
    user1.Accounts.Append(account1);
    user2.Accounts.Append(account2);
    user1.Accounts.Append(account3);

    var transaction1 = new Transaction { Amount = 500, Timestamp = DateTime.UtcNow };
    var transaction2 = new Transaction { Amount = 200, Timestamp = DateTime.UtcNow };
    var transaction3 = new Transaction { Amount = 300, Timestamp = DateTime.UtcNow };
    var transaction4 = new Transaction { Amount = 100, Timestamp = DateTime.UtcNow };

    account1.Transactions.Add(transaction1);
    account1.Transactions.Add(transaction2);
    account2.Transactions.Add(transaction3);
    account3.Transactions.Add(transaction4);

    context.Accounts.AddRange(account1, account2, account3);
    context.Users.AddRange(user1, user2);
    context.Transactions.AddRange(transaction1, transaction2, transaction3, transaction4);
    
    context.SaveChanges(); 
}