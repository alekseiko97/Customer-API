using Customer_API;
using Customer_API.Models;
using Customer_API.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
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
builder.Services.AddTransient<ITransactionService, TransactionService>();
var app = builder.Build();

// Handle exceptions 
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

            var result = new
            {
                error = "An unexpected error occurred. Please try again later."
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    });
});

using (var scope = app.Services.CreateScope())
{
    //var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userService = services.GetRequiredService<IUserService>();
    var accountService = services.GetRequiredService<IAccountService>();
    var transactionService = services.GetRequiredService<ITransactionService>();

    // Seed the database
    await SeedData(context, userService, accountService, transactionService);
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

static async Task SeedData(ApplicationDbContext context,
    IUserService userService,
    IAccountService accountService,
    ITransactionService transactionService)
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

    // Save all changes
    await context.SaveChangesAsync();
}