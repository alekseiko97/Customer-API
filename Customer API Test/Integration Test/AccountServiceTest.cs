using Customer_API;
using Customer_API.Models;
using Customer_API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_API_Test
{
    [TestFixture]
    public class AccountServiceTests
    {
        /*        private Mock<DbSet<User>> _mockUsersSet;
                private Mock<DbSet<Account>> _mockAccountsSet;
                private Mock<DbSet<Transaction>> _mockTransactionsSet;
                private Mock<ApplicationDbContext> _mockContext;*/
        private ApplicationDbContext _context;
        private Mock<ITransactionService> _mockTransactionService;
        private AccountService _accountService;

        [SetUp]
        public void Setup()
        {
            // Initialize the mocks
            /*            _mockContext = new Mock<ApplicationDbContext>();

                        // Set up mock DbSets
                        _mockUsersSet = DbSetMockExtensions.CreateMockDbSet(new List<User>());
                        _mockAccountsSet = DbSetMockExtensions.CreateMockDbSet(new List<Account>());
                        _mockTransactionsSet = DbSetMockExtensions.CreateMockDbSet(new List<Transaction>());

                        // Configure the mock context to return mock DbSets
                        _mockContext.Setup(c => c.Users).Returns(_mockUsersSet.Object);
                        _mockContext.Setup(c => c.Accounts).Returns(_mockAccountsSet.Object);
                        _mockContext.Setup(c => c.Transactions).Returns(_mockTransactionsSet.Object);

                        // Setup SaveChangesAsync to return 1
                        _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            */
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            SeedData(_context);

            // Setup the Transaction Service
            _mockTransactionService = new Mock<ITransactionService>();

            // Create the AccountService instance with mocks
            _accountService = new AccountService(_context, _mockTransactionService.Object);
        }

        private static void SeedData(ApplicationDbContext context)
        {
            var user = new User { ID = 1, Name = "John", Surname = "Doe", Accounts = [] };
            context.Users.Add(user);

            context.SaveChanges(); // Save the changes to the in-memory database
        }

        [Test]
        public async Task CreateAccount_CreatesTransaction_WhenInitialBalanceIsNonZero()
        {
            // Arrange
            var customerId = 1;
            var initialBalance = 100m;

            // Setup the mock to expect the transaction creation
            _mockTransactionService
                .Setup(service => service.CreateTransactionAsync(It.IsAny<Account>(), initialBalance))
                .Returns(Task.CompletedTask);

            // Act
            var account = await _accountService.CreateAccountAsync(customerId, initialBalance);

            // Assert
            Assert.That(account, Is.Not.Null);
            Assert.That(account.Balance, Is.EqualTo(initialBalance));

            // Verify that CreateTransactionAsync was called
            _mockTransactionService.Verify(service => service.CreateTransactionAsync(It.IsAny<Account>(), initialBalance), Times.Once);
        }

        [Test]
        public async Task CreateAccount_DoesNotCreateTransaction_WhenInitialBalanceIsZero()
        {
            // Arrange
            var customerId = 1;
            var initialBalance = 0;

            // Setup the mock to expect the transaction creation
            _mockTransactionService
                .Setup(service => service.CreateTransactionAsync(It.IsAny<Account>(), initialBalance))
                .Returns(Task.CompletedTask);

            // Act
            var account = await _accountService.CreateAccountAsync(customerId, initialBalance);

            // Assert
            Assert.That(account, Is.Not.Null);
            Assert.That(account.Balance, Is.EqualTo(initialBalance));

            // Verify that CreateTransactionAsync was called
            _mockTransactionService.Verify(service => service.CreateTransactionAsync(It.IsAny<Account>(), initialBalance), Times.Never);
        }

        [TearDown]
        public void TearDown()
        {
            // Clear the in-memory database after each test to ensure isolation
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }

}
