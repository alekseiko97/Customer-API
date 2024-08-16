using Customer_API.Controllers;
using Customer_API.Models;
using Customer_API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Customer_API_Test
{
    [TestFixture]
    public class UserControllerTest
    {
        public Mock<IUserService> _mockUserService;
        public UserController _controller;
        
        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Test]
        public async Task GetUser_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var customerId = 1;
            var user = new User { ID = customerId, Name = "John", Surname = "Doe" };
            _mockUserService.Setup(service => service.GetUserInfoAsync(customerId)).ReturnsAsync(user);

            // Act
            var result = await _controller.GetUser(customerId) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            var returnedUser = result.Value as User;
            Assert.That(returnedUser, Is.Not.Null);
            Assert.That(returnedUser.ID, Is.EqualTo(customerId));
        }

        [Test]
        public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var customerId = 1;
            // Set up the mock to return null for the GetUserInfoAsync call
            _mockUserService.Setup(service => service.GetUserInfoAsync(customerId)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.GetUser(customerId) as NotFoundObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public async Task CreateUser_ReturnsCreatedAtActionResult_WhenSuccessful()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var newUser = new User { ID = 1, Name = firstName, Surname = lastName };

            // Set up the mock to return the newly created user
            _mockUserService.Setup(service => service.CreateUserAsync(firstName, lastName))
                            .ReturnsAsync(newUser);

            // Act
            var result = await _controller.CreateUser(firstName, lastName) as CreatedAtActionResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(201)); // 201 Created

            var createdUser = result.Value as User;
            Assert.That(createdUser, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(createdUser.ID, Is.EqualTo(newUser.ID));
                Assert.That(createdUser.Name, Is.EqualTo(firstName));
                Assert.That(createdUser.Surname, Is.EqualTo(lastName));
            });

            // Verify that CreateUserAsync was called with correct parameters
            _mockUserService.Verify(service => service.CreateUserAsync(firstName, lastName), Times.Once);
        }

        [Test]
        public async Task CreateUser_ReturnsBadRequest_WhenInvalidData()
        {
            // Arrange
            var firstName = string.Empty;
            var lastName = string.Empty;

            // Act
            var result = await _controller.CreateUser(firstName, lastName) as BadRequestObjectResult; // 400

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400)); // bad request
        }

        [Test]
        public async Task CreateUser_ReturnsBadRequest_WhenUserWithGivenNameExists()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var existingUser = new User { ID = 1, Name = firstName, Surname = lastName };

            // Set up the mock to return the newly created user
            _mockUserService.Setup(service => service.GetAllUsersAsync())
                            .ReturnsAsync(new List<User> { existingUser });

            // Create user for the first time
            await _controller.CreateUser(firstName, lastName);

            // Act
            var result = await _controller.CreateUser(firstName, lastName) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400)); // bad request
        }
    }
}