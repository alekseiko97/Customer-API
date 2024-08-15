using Customer_API.Services;
using Customer_API;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Customer_API.Models;

namespace Customer_API_Test
{
    [TestFixture]
    public class UserServiceTest
    {
        private ApplicationDbContext _context;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique database name
                .Options;

            _context = new ApplicationDbContext(options);

            // Create the AccountService instance with mocked transaction service
            _userService = new UserService(_context);
        }

        [Test]
        public async Task CreateUserAsync_CreatesUser_WhenValidData()
        {
            // Arrange
            var firstName = "Test";
            var lastName = "User";

            // Act
            var createdUser = await _userService.CreateUserAsync(firstName, lastName);

            // Assert
            Assert.That(createdUser, Is.Not.Null, "Expected a user to be created but got null.");
            Assert.Multiple(() =>
            {
                Assert.That(createdUser.Name, Is.EqualTo(firstName), "Expected user name to match input.");
                Assert.That(createdUser.Surname, Is.EqualTo(lastName), "Expected user surname to match input.");
            });

            // Verify that the user was added to the database
            var userInDb = await _context.Users.FindAsync(createdUser.ID);
            Assert.That(userInDb, Is.Not.Null, "Expected the user to be found in the database.");
            Assert.Multiple(() =>
            {
                Assert.That(userInDb.Name, Is.EqualTo(firstName), "Expected user name to match input.");
                Assert.That(userInDb.Surname, Is.EqualTo(lastName), "Expected user surname to match input.");
            });
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsAllUsers()
        {
            // Arrange
            var user1 = new User { Name = "John", Surname = "Doe" };
            var user2 = new User { Name = "Jane", Surname = "Smith" };

            await _context.Users.AddRangeAsync(user1, user2);
            await _context.SaveChangesAsync();

            // Act
            var users = await _userService.GetAllUsersAsync();

            // Assert
            Assert.That(users, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(users.Count(), Is.EqualTo(2));
                Assert.That(users, Is.EquivalentTo(new List<User> { user1, user2 }));
            });
        }

        [Test]
        public async Task GetUserInfoAsync_ReturnsUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Name = "John", Surname = "Doe" };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _userService.GetUserInfoAsync(user.ID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Name, Is.EqualTo(user.Name));
                Assert.That(result.Surname, Is.EqualTo(user.Surname));
            });
        }

        [Test]
        public async Task GetUserInfoAsync_ReturnsNull_WhenUserDoesNotExist()
        {
            // Arrange
            var nonExistingUserId = 999; // Assuming this ID doesn't exist

            // Act
            var result = await _userService.GetUserInfoAsync(nonExistingUserId);

            // Assert
            Assert.That(result, Is.Null);
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
