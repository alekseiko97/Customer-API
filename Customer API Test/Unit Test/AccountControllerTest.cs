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
