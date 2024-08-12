using Customer_API.Controllers;
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
    [TestFixture]
    public class AccountControllerTest
    {
        private AccountController _controller;
        private Mock<IAccountService> _mockAccountService;
        
        [SetUp]
        public void Setup()
        {
            _mockAccountService = new Mock<IAccountService>();
            _controller = new AccountController(_mockAccountService.Object);
        }

        [Test]
        public async Task CreateAccount_ReturnsCreatedAtActionResult_WhenSuccessful()
        {
            // Arrange
            var customerId = 1;
            var initialBalance = 100m;
            var accountId = 1;
            var account = new Account { Id = accountId, Balance = initialBalance };

            _mockAccountService.Setup(service => service.CreateAccountAsync(customerId, initialBalance))
                               .ReturnsAsync(account);

            // Act
            var result = await _controller.CreateAccount(customerId, initialBalance) as CreatedAtActionResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("GetAccount"));
                Assert.That(((Account)result.Value).Id, Is.EqualTo(accountId));
                Assert.That(((Account)result.Value).Balance, Is.EqualTo(initialBalance));
            });
        }

        [Test]
        public async Task CreateAccount_CreatesTransaction_WhenInitialBalanceIsNonZero()
        {
            // Arrange
            var customerId = 1;
            var initialBalance = 100m;
            var accountId = 1;
            var account = new Account { Id = accountId, Balance = initialBalance };

            // setup the service to return the created account
            _mockAccountService.Setup(service => service.CreateAccountAsync(customerId, initialBalance))
                               .ReturnsAsync(account);

            // Act
            var result = await _controller.CreateAccount(customerId, initialBalance) as CreatedAtActionResult;
            
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.Not.Null);
            
            var accountFromResult = (Account)result.Value;
            Assert.That(accountFromResult, Is.Not.Null);
            
            Assert.Multiple(() =>
            {
                Assert.That(result.ActionName, Is.EqualTo("GetAccount"));
                Assert.That(accountFromResult.Id, Is.EqualTo(accountId));
                Assert.That(accountFromResult.Balance, Is.EqualTo(initialBalance));
            });

            // verify that CreateAccountAsync was called with the expected parameters
            _mockAccountService.Verify(service => service.CreateAccountAsync(customerId, initialBalance), Times.Once);

            // check if a transaction was created
            Assert.That(accountFromResult.Transactions, Has.Count.EqualTo(1));
        }

        [Test]
        public async Task CreateAccount_DoesNotCreateTransaction_WhenInitialBalanceIsZero()
        {
            // Arrange
            var customerId = 1;
            var initialBalance = 0m;
            var accountId = 1;
            var account = new Account { Id = accountId, Balance = initialBalance};

            // setup the service to return the created account
            _mockAccountService.Setup(service => service.CreateAccountAsync(customerId, initialBalance))
                               .ReturnsAsync(account);

            // Act
            var result = await _controller.CreateAccount(customerId, initialBalance) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result, "Expected CreatedAtActionResult result.");
            var createdAccount = result.Value as Account;
            Assert.IsNotNull(createdAccount, "Expected account in the result.");

            // Verify that CreateAccountAsync was called with the expected parameters
            _mockAccountService.Verify(service => service.CreateAccountAsync(customerId, initialBalance), Times.Once);

            // Additional checks would be required here if your service method interacts with transactions
            // You might need to mock and verify the transaction creation logic if it’s part of a different service
        }

        [Test]
        public async Task GetAccount_ReturnsOkResult_WhenAccountExists()
        {
            // Arrange
            var accountId = 1;
            var account = new Account { Id = accountId, Balance = 100m };
            _mockAccountService.Setup(service => service.GetAccountAsync(accountId))
                               .ReturnsAsync(account);

            // Act
            var result = await _controller.GetAccount(accountId) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(account));
        }

        [Test]
        public async Task GetAccount_ReturnsNotFound_WhenAccountDoesNotExist()
        {
            // Arrange
            var accountId = 1;
            _mockAccountService.Setup(service => service.GetAccountAsync(accountId))
                               .ReturnsAsync((Account)null);

            // Act
            var result = await _controller.GetAccount(accountId) as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo($"Account with id {accountId} doesn't exist"));
        }
    }
}
