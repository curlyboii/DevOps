using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Shop.Models;
using Shop.Repositories;
using Microsoft.AspNetCore.Routing;
using System.Numerics;

namespace Shop.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private DbContextOptions<ShopDbContext> _options;

        [SetUp]
        public void Setup()
        {
            // Налаштування контексту бази даних для тестів
            _options = new DbContextOptionsBuilder<ShopDbContext>()
                .UseInMemoryDatabase(databaseName: "Shop")
                .Options;
        }

        [Test]
        public void GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            using (var context = new ShopDbContext(_options))
            {
                // Створюємо тестових лікарів
                var users = new List<User>
                {
                    new User { Id = 1, Name = "John Doe" },
                    new User { Id = 2, Name = "Jane Smith" },
                    new User { Id = 3, Name = "Michael Johnson" }
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            using (var context = new ShopDbContext(_options))
            {
                var repository = new UserRepository(context);

                // Act
                var result = repository.GetAllUsers();

                // Assert
                Assert.AreEqual(3, result.Count()); // Переконаємося, що повернуто трьох лікарів
            }
        }

        [Test]
        public void GetUserById_ReturnsCorrectUser()
        {
            // Arrange
            using (var context = new ShopDbContext(_options))
            {
                // Створюємо тестових лікарів
                var users = new List<User>
                {
                    new User { Id = 1, Name = "John Doe" },
                    new User { Id = 2, Name = "Jane Smith" },
                    new User { Id = 3, Name = "Michael Johnson" }
                };
                context.Users.AddRange(users);
                context.SaveChanges();
            }

            using (var context = new ShopDbContext(_options))
            {
                var repository = new UserRepository(context);

                // Act
                var result = repository.GetUserById(2);

                // Assert
                Assert.IsNotNull(result); // Переконаємося, що лікар не є порожнім
                Assert.AreEqual("Jane Smith", result.Name); // Переконаємося, що правильний лікар повернуто
            }
        }

        [Test]
        public void AddUser_CreatesNewUser()
        {
            // Arrange
            using (var context = new ShopDbContext(_options))
            {
                var repository = new UserRepository(context);

                // Act
                var newUser = new User { Id = 1, Name = "John Doe" };
                repository.AddUser(newUser);

                // Assert
                Assert.AreEqual(1, context.Users.Count()); // Переконаємося, що лікаря додали
                Assert.AreEqual("John Doe", context.Users.First().Name); // Переконаємося, що правильне ім'я лікаря було додано
            }
        }
    }
}