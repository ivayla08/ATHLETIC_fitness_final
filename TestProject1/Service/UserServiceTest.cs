using athelticFitness.Controllers;
using athletic_fitness.Data.Entities;
using athletic_fitness.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject1.Helpers;

namespace TestProject1.Service
{
    public class UserServiceTest
    {
        [Test]

        public async Task CreateUser_UsernameDoesNotExist_ReturnsUserAndSavesToDb()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new UserController(context);
            var newUser = new User { Id = 1, Username = "ivayla.ivanova", Password = "password123", Role = RoleType.Client };

            var result = await controller.CreateUser(newUser);

            Assert.IsNotNull(result);
            Assert.AreEqual("ivayla.ivanova", result.Username);

            var dbUser = await context.Users.FindAsync(1);
            Assert.IsNotNull(dbUser);
            Assert.AreEqual("password123", dbUser.Password);
        }

        [Test]
        public async Task CreateUser_UsernameAlreadyExists_ThrowsInvalidOperationException()
        {
            using var context = TestDBFactory.CreateContext();
            var existingUser = new User { Id = 1, Username = "ivan.ivanov", Password = "password123", Role = RoleType.Client };
            await context.Users.AddAsync(existingUser);
            await context.SaveChangesAsync();

            var controller = new UserController(context);
            var duplicateUser = new User { Id = 2, Username = "ivan.ivanov", Password = "differentpassword", Role = RoleType.Client };

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await controller.CreateUser(duplicateUser)
            );
            Assert.AreEqual("Username is already taken.", ex.Message);
        }

        [Test]
        public async Task GetAllUsers_WhenCalled_ReturnsAllUsers()
        {
            using var context = TestDBFactory.CreateContext();
            await context.Users.AddRangeAsync(new List<User>
        {
            new User { Id = 1, Username = "ivayla.ivanova", Password = "password123", Role = RoleType.Client },
            new User { Id = 2, Username = "ivan.ivanov", Password = "password456", Role = RoleType.Client }
        });
            await context.SaveChangesAsync();

            var controller = new UserController(context);

            var result = await controller.GetAllUsers();

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [Test]
        public async Task GetUserById_UserExists_ReturnsUser()
        {
            using var context = TestDBFactory.CreateContext();
            var user = new User { Id = 1, Username = "petya.petrova", Password = "password123", Role = RoleType.Client };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var controller = new UserController(context);

            var result = await controller.GetUserById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("petya.petrova", result.Username);
        }

        [Test]
        public async Task GetUserById_UserDoesNotExist_ReturnsNull()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new UserController(context);

            var result = await controller.GetUserById(99);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateUser_UserExistsWithNewPassword_UpdatesAllFieldsAndReturnsTrue()
        {
            using var context = TestDBFactory.CreateContext();
            var user = new User { Id = 1, Username = "ivan.ivanov", Password = "oldpassword", Role = RoleType.Client };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var controller = new UserController(context);
            var updatedUser = new User { Id = 1, Username = "ivan.ivanov.updated", Password = "newpassword", Role = RoleType.Client };

            var result = await controller.UpdateUser(updatedUser);

            Assert.IsTrue(result);

            var dbUser = await context.Users.FindAsync(1);
            Assert.AreEqual("ivan.ivanov.updated", dbUser.Username);
            Assert.AreEqual("newpassword", dbUser.Password);
        }

        [Test]
        public async Task UpdateUser_UserExistsWithEmptyPassword_DoesNotUpdatePassword()
        {
            using var context = TestDBFactory.CreateContext();
            var user = new User { Id = 1, Username = "ivayla.ivanova", Password = "keepthispassword", Role = RoleType.Client };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var controller = new UserController(context);
            var updatedUser = new User { Id = 1, Username = "ivayla.new", Password = "", Role = RoleType.Client };

            var result = await controller.UpdateUser(updatedUser);

            Assert.IsTrue(result);

            var dbUser = await context.Users.FindAsync(1);
            Assert.AreEqual("ivayla.new", dbUser.Username);
            Assert.AreEqual("keepthispassword", dbUser.Password);
        }

        [Test]
        public async Task UpdateUser_UserDoesNotExist_ReturnsFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new UserController(context);
            var nonExistentUser = new User { Id = 99, Username = "ghost", Password = "password", Role = RoleType.Client };

            var result = await controller.UpdateUser(nonExistentUser);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteUser_UserExists_RemovesFromDbAndReturnsTrue()
        {
            using var context = TestDBFactory.CreateContext();
            var user = new User { Id = 1, Username = "petya.petrova", Password = "password123", Role = RoleType.Client };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var controller = new UserController(context);

            var result = await controller.DeleteUser(1);

            Assert.IsTrue(result);

            var dbUser = await context.Users.FindAsync(1);
            Assert.IsNull(dbUser);
        }

        [Test]
        public async Task DeleteUser_UserDoesNotExist_ReturnsFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new UserController(context);

            var result = await controller.DeleteUser(99);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task LoginAsync_ValidCredentials_ReturnsSuccessTrue()
        {
            using var context = TestDBFactory.CreateContext();
            var user = new User { Id = 1, Username = "ivayla.ivanova", Password = "correctpassword", Role = RoleType.Client };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var controller = new UserController(context);

            var result = await controller.LoginAsync("ivayla.ivanova", "correctpassword");

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Successful log in!", result.Message);
            Assert.IsNotNull(result.User);
            Assert.AreEqual("ivayla.ivanova", result.User.Username);
        }

        [Test]
        public async Task LoginAsync_InvalidUsername_ReturnsSuccessFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var user = new User { Id = 1, Username = "ivan.ivanov", Password = "password123", Role = RoleType.Client };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var controller = new UserController(context);

            var result = await controller.LoginAsync("wrong.username", "password123");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid username or passoword", result.Message);
            Assert.IsNull(result.User);
        }

        [Test]
        public async Task LoginAsync_InvalidPassword_ReturnsSuccessFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var user = new User { Id = 1, Username = "petya.petrova", Password = "correctpassword", Role = RoleType.Client };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var controller = new UserController(context);

            var result = await controller.LoginAsync("petya.petrova", "wrongpassword");

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid username or password", result.Message);
            Assert.IsNull(result.User);
        }
    }
}
