using athelticFitness.Controllers;
using athletic_fitness.Data.Entities;
using athletic_fitness.Data;
using athletic_fitness.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject1.Helpers;

namespace TestProject1.Service
{
    public class CoachServiceTest
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

        private async Task SeedGymAsync(GymContext context, int gymId)
        {
            var gym = new Gym
            {
                Id = gymId,
                City = "Sofia",
                Address = "Main Street " + gymId
            };
            await context.Gyms.AddAsync(gym);
            await context.SaveChangesAsync();
        }

        [Test]
        public async Task CreateCoach_ValidUserAndGymExist_ReturnsCreatedCoachAndSavesToDb()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);
            await SeedGymAsync(context, 1);

            var controller = new CoachController(context);
            var newCoach = new Coach
            {
                Id = 1,
                UserId = 10,
                GymId = 1,
                FirstName = "Ivayla",
                LastName = "Ivanova",
                Email = "ivayla.ivanova@gym.com",
                Phone = "1234567890"
            };

            var result = await controller.CreateCoach(newCoach);

            Assert.IsNotNull(result);
            Assert.AreEqual("Ivayla", result.FirstName);

            var dbCoach = await context.Coaches.FindAsync(1);
            Assert.IsNotNull(dbCoach);
            Assert.AreEqual("Ivanova", dbCoach.LastName);
        }

        [Test]
        public async Task CreateCoach_UserDoesNotExist_ThrowsArgumentException()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedGymAsync(context, 1);

            var controller = new CoachController(context);
            var invalidCoach = new Coach
            {
                Id = 1,
                UserId = 999,
                GymId = 1,
                FirstName = "Ivayla",
                LastName = "Ivanova",
                Email = "ivayla.ivanova@gym.com",
                Phone = "1234567890"
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await controller.CreateCoach(invalidCoach)
            );
            Assert.AreEqual("UserId does not exist.", ex.Message);
        }

        [Test]
        public async Task CreateCoach_GymDoesNotExist_ThrowsArgumentException()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);

            var controller = new CoachController(context);
            var invalidCoach = new Coach
            {
                Id = 1,
                UserId = 10,
                GymId = 999,
                FirstName = "Ivayla",
                LastName = "Ivanova",
                Email = "ivayla.ivanova@gym.com",
                Phone = "1234567890"
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await controller.CreateCoach(invalidCoach)
            );
            Assert.AreEqual("GymId does not exist.", ex.Message);
        }

        [Test]
        public async Task GetAllCoaches_WhenCalled_ReturnsAllCoaches()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 1);
            await SeedUserAsync(context, 2);
            await SeedGymAsync(context, 1);

            await context.Coaches.AddRangeAsync(new List<Coach>
        {
            new Coach { Id = 1, UserId = 1, GymId = 1, FirstName = "Ivayla", LastName = "Ivanova", Email = "ivayla@gym.com", Phone = "1234567890" },
            new Coach { Id = 2, UserId = 2, GymId = 1, FirstName = "Ivan", LastName = "Ivanov", Email = "ivan@gym.com", Phone = "0987654321" }
        });
            await context.SaveChangesAsync();

            var controller = new CoachController(context);

            var result = await controller.GetAllCoaches();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetAllCoachesByGym_WhenCalled_ReturnsOnlyCoachesInThatGym()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 1);
            await SeedUserAsync(context, 2);
            await SeedGymAsync(context, 1);
            await SeedGymAsync(context, 2);

            await context.Coaches.AddRangeAsync(new List<Coach>
        {
            new Coach { Id = 1, UserId = 1, GymId = 1, FirstName = "Ivayla", LastName = "Ivanova", Email = "ivayla@gym.com", Phone = "1234567890" },
            new Coach { Id = 2, UserId = 2, GymId = 2, FirstName = "Ivan", LastName = "Ivanov", Email = "ivan@gym.com", Phone = "0987654321" }
        });
            await context.SaveChangesAsync();

            var controller = new CoachController(context);

            var result = await controller.GetAllCoachesByGym(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Ivayla", result[0].FirstName);
        }

        [Test]
        public async Task GetCoachById_CoachExists_ReturnsCoach()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 20);
            await SeedGymAsync(context, 1);

            var existingCoach = new Coach
            {
                Id = 5,
                UserId = 20,
                GymId = 1,
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "ivan.ivanov@gym.com",
                Phone = "1234567890"
            };
            await context.Coaches.AddAsync(existingCoach);
            await context.SaveChangesAsync();

            var controller = new CoachController(context);

            var result = await controller.GetCoachById(5);

            Assert.IsNotNull(result);
            Assert.AreEqual("Ivan", result.FirstName);
        }

        [Test]
        public async Task GetCoachById_CoachDoesNotExist_ReturnsNull()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new CoachController(context);

            var result = await controller.GetCoachById(99);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetCoachByUserId_CoachExists_ReturnsCoach()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 20);
            await SeedGymAsync(context, 1);

            var existingCoach = new Coach
            {
                Id = 5,
                UserId = 20,
                GymId = 1,
                FirstName = "Petya",
                LastName = "Petrova",
                Email = "petya@gym.com",
                Phone = "1234567890"
            };
            await context.Coaches.AddAsync(existingCoach);
            await context.SaveChangesAsync();

            var controller = new CoachController(context);

            var result = await controller.GetCoachByUserId(20);

            Assert.IsNotNull(result);
            Assert.AreEqual("Petya", result.FirstName);
        }

        [Test]
        public async Task GetCoachByUserId_CoachDoesNotExist_ReturnsNull()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new CoachController(context);

            var result = await controller.GetCoachByUserId(99);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateCoach_CoachExists_UpdatesFieldsAndReturnsTrue()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 1);
            await SeedGymAsync(context, 1);
            await SeedGymAsync(context, 2);

            var coach = new Coach { Id = 1, UserId = 1, GymId = 1, FirstName = "Ivan", LastName = "Ivanov", Email = "ivan@gym.com", Phone = "1111111111" };
            await context.Coaches.AddAsync(coach);
            await context.SaveChangesAsync();

            var controller = new CoachController(context);
            var updatedCoach = new Coach { Id = 1, UserId = 1, GymId = 2, FirstName = "Petya", LastName = "Petrova", Email = "petya@gym.com", Phone = "2222222222" };

            var result = await controller.UpdateCoach(updatedCoach);

            Assert.IsTrue(result);

            var dbCoach = await context.Coaches.FindAsync(1);
            Assert.AreEqual("Petya", dbCoach.FirstName);
            Assert.AreEqual("petya@gym.com", dbCoach.Email);
            Assert.AreEqual(2, dbCoach.GymId);
        }

        [Test]
        public async Task UpdateCoach_CoachDoesNotExist_ReturnsFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new CoachController(context);
            var nonExistentCoach = new Coach { Id = 99, UserId = 1, GymId = 1, FirstName = "Petya", LastName = "Petrova", Email = "petya@gym.com", Phone = "1234567890" };

            var result = await controller.UpdateCoach(nonExistentCoach);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteCoach_CoachExists_RemovesFromDbAndReturnsTrue()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);
            await SeedGymAsync(context, 1);

            var coach = new Coach { Id = 10, UserId = 10, GymId = 1, FirstName = "Petya", LastName = "Petrova", Email = "petya@gym.com", Phone = "1234567890" };
            await context.Coaches.AddAsync(coach);
            await context.SaveChangesAsync();

            var controller = new CoachController(context);

            var result = await controller.DeleteCoach(10);

            Assert.IsTrue(result);

            var dbCoach = await context.Coaches.FindAsync(10);
            Assert.IsNull(dbCoach);
        }

        [Test]
        public async Task DeleteCoach_CoachDoesNotExist_ReturnsFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new CoachController(context);

            var result = await controller.DeleteCoach(99);

            Assert.IsFalse(result);
        }
    }
}
