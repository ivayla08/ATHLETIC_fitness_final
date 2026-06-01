using athelticFitness.Controllers;
using athletic_fitness.Data;
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
    public class ClientServiceTest
    {
        private async Task SeedUserAsync(GymContext context, int userId)
        {
            var user = new User
            {
                Id = userId,
                Username = "user_" + userId,
                Password = "password123",
                Role = RoleType.Client
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        [Test]
        public async Task CreateClient_ValidUserExists_ReturnsCreatedClientAndSavesToDb()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);

            var controller = new ClientController(context);
            var newClient = new Client
            {
                Id = 1,
                UserId = 10,
                FirstName = "Ivayla",
                LastName = "Ivanova",
                Email = "ivayla.ivanova@gym.com",
                Phone = "1234567890",
                MembershipId = 1
            };

            var result = await controller.CreateClient(newClient);

            Assert.IsNotNull(result);
            Assert.AreEqual("Ivayla", result.FirstName);

            var dbClient = await context.Clients.FindAsync(1);
            Assert.IsNotNull(dbClient);
            Assert.AreEqual("Ivanova", dbClient.LastName);
        }

        [Test]
        public void CreateClient_UserDoesNotExist_ThrowsArgumentException()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new ClientController(context);
            var invalidClient = new Client
            {
                Id = 1,
                UserId = 999,
                FirstName = "Ivayla",
                LastName = "Ivanova",
                Email = "ivayla.ivanova@gym.com",
                Phone = "1234567890",
                MembershipId = 1
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await controller.CreateClient(invalidClient)
            );
            Assert.AreEqual("UserId does not exist.", ex.Message);
        }

        [Test]
        public async Task GetClientByUserId_ClientExists_ReturnsClient()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 20);

            var existingClient = new Client
            {
                Id = 5,
                UserId = 20,
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "ivan.ivanov@gym.com",
                Phone = "1234567890",
                MembershipId = 1
            };
            await context.Clients.AddAsync(existingClient);
            await context.SaveChangesAsync();

            var controller = new ClientController(context);

            var result = await controller.GetClientByUserId(20);

            Assert.IsNotNull(result);
            Assert.AreEqual("Ivan", result.FirstName);
        }

        [Test]
        public async Task GetClientByUserId_ClientDoesNotExist_ReturnsNull()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new ClientController(context);

            var result = await controller.GetClientByUserId(99);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllClients_WhenCalled_ReturnsAllClients()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 1);
            await SeedUserAsync(context, 2);

            await context.Clients.AddRangeAsync(new List<Client>
        {
            new Client { Id = 1, UserId = 1, FirstName = "Ivayla", LastName = "Ivanova", Email = "ivayla@gym.com", Phone = "1234567890", MembershipId = 1 },
            new Client { Id = 2, UserId = 2, FirstName = "Ivan", LastName = "Ivanov", Email = "ivan@gym.com", Phone = "0987654321", MembershipId = 2 }
        });
            await context.SaveChangesAsync();

            var controller = new ClientController(context);

            var result = await controller.GetAllClients();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task UpdateClient_ClientExists_UpdatesFieldsAndReturnsTrue()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 1);

            var client = new Client { Id = 1, UserId = 1, FirstName = "Ivan", LastName = "Ivanov", Email = "ivan@gym.com", Phone = "1111111111", MembershipId = 1 };
            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();

            var controller = new ClientController(context);
            var updatedClient = new Client { Id = 1, UserId = 1, FirstName = "Petya", LastName = "Petrova", Email = "petya@gym.com", Phone = "2222222222", MembershipId = 2 };

            var result = await controller.UpdateClient(updatedClient);

            Assert.IsTrue(result);

            var dbClient = await context.Clients.FindAsync(1);
            Assert.AreEqual("Petya", dbClient.FirstName);
            Assert.AreEqual("petya@gym.com", dbClient.Email);
            Assert.AreEqual("2222222222", dbClient.Phone);
        }

        [Test]
        public async Task UpdateClient_ClientDoesNotExist_ReturnsFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new ClientController(context);
            var nonExistentClient = new Client { Id = 99, UserId = 1, FirstName = "Petya", LastName = "Petrova", Email = "petya@gym.com", Phone = "1234567890", MembershipId = 1 };

            var result = await controller.UpdateClient(nonExistentClient);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteClient_ClientExists_RemovesFromDbAndReturnsTrue()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);

            var client = new Client { Id = 10, UserId = 10, FirstName = "Petya", LastName = "Petrova", Email = "petya@gym.com", Phone = "1234567890", MembershipId = 1 };
            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();

            var controller = new ClientController(context);

            var result = await controller.DeleteClient(10);

            Assert.IsTrue(result);

            var dbClient = await context.Clients.FindAsync(10);
            Assert.IsNull(dbClient);
        }

        [Test]
        public async Task DeleteClient_ClientDoesNotExist_ReturnsFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new ClientController(context);

            var result = await controller.DeleteClient(99);

            Assert.IsFalse(result);
        }
    }
}
