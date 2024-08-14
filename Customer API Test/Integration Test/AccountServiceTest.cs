using Customer_API;
using Customer_API.Models;
using Customer_API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer_API_Test
{
    public class AccountServiceTest
    {
        private Mock<ApplicationDbContext> _mockContext;
        private Mock<ITransactionService> _mockTransactionService;
        private AccountService _accountService;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _mockTransactionService = new Mock<ITransactionService>();
            _accountService = new AccountService(_mockContext.Object, _mockTransactionService.Object);
        }
/*        [Test]
        public async Task CreateAccount_CreatesTransaction_WhenInitialBalanceIsNonZero()
        {
            // Arrange
            var customerId = 1;
            var initialBalance = 100m;
            var accountId = 1;
            var user = new User { ID = customerId, Accounts = new List<Account>() };
            var account = new Account { Id = accountId, Balance = initialBalance };

            // Mock DbContext
            _mockContext.Setup(c => c.Users)
                        .Returns(new List<User> { user });
            _mockContext.Setup(c => c.Accounts)
                        .Returns(new List<Account> { account });

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                        .ReturnsAsync(1);

            // Mock CreateTransactionAsync
            _mockTransactionService.Setup(service => service.CreateTransactionAsync(It.IsAny<Account>(), initialBalance))
                                    .Returns(Task.CompletedTask);
            if (initialBalance != 0)
            {
                _mockTransactionService.Verify(service => service.CreateTransactionAsync(It.IsAny<Account>(), It.IsAny<decimal>()));
            }

            Assert.That(createdAccount.Transactions, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task CreateAccount_DoesNotCreateTransaction_WhenInitialBalanceIsZero()
        {
            // Arrange
            var customerId = 1;
            var initialBalance = 0m;
            var accountId = 1;
            var account = new Account { Id = accountId, Balance = initialBalance };

            // setup the service to return the created account
            _mockAccountService.Setup(service => service.CreateAccountAsync(customerId, initialBalance))
                               .ReturnsAsync(account);

            // Act
            var result = await _controller.CreateAccount(customerId, initialBalance) as CreatedAtActionResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            var createdAccount = result.Value as Account;
            Assert.That(createdAccount, Is.Not.Null);

            // Verify that CreateAccountAsync was called 
            _mockAccountService.Verify(service => service.CreateAccountAsync(customerId, initialBalance), Times.Once);

            Assert.That(createdAccount.Transactions, Has.Count.EqualTo(0));
        }*/
    }
}
