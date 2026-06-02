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
    public class MembershipServiceTest
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

        private async Task SeedClientAsync(GymContext context, int clientId, int userId, string firstName, string lastName, int membershipId = 0)
        {
            var client = new Client
            {
                Id = clientId,
                UserId = userId,
                FirstName = firstName,
                LastName = lastName,
                Email = $"{firstName.ToLower()}.{lastName.ToLower()}@gym.com",
                Phone = "1234567890",
                MembershipId = membershipId
            };
            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();
        }

        [Test]
        public async Task CreateMembership_ValidClient_ReturnsCreatedMembershipAndSavesToDb()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);
            await SeedClientAsync(context, 5, 10, "Ivayla", "Ivanova");

            var controller = new MembershipController(context);
            var newMembership = new Membership
            {
                Id = 1,
                ClientId = 5,
                MembershipType = MembershipType.Month,
                Price = 50.00m,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            var result = await controller.CreateMembership(newMembership);

            Assert.IsNotNull(result);
            Assert.AreEqual(MembershipType.Month, result.MembershipType);

            var dbMembership = await context.Memberships.FindAsync(1);
            Assert.IsNotNull(dbMembership);
            Assert.AreEqual(50.00m, dbMembership.Price);
        }

        [Test]
        public void CreateMembership_ClientDoesNotExist_ThrowsArgumentException()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new MembershipController(context);
            var invalidMembership = new Membership
            {
                Id = 1,
                ClientId = 999,
                MembershipType = MembershipType.Month,
                Price = 50.00m,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1)
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await controller.CreateMembership(invalidMembership)
            );
            Assert.AreEqual("ClientId does not exist.", ex.Message);
        }

       

        [Test]
        public async Task GetAllMemberships_WhenCalled_ReturnsAllMemberships()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 1);
            await SeedUserAsync(context, 2);
            await SeedClientAsync(context, 10, 1, "Ivayla", "Ivanova");
            await SeedClientAsync(context, 20, 2, "Ivan", "Ivanov");

            await context.Memberships.AddRangeAsync(new List<Membership>
        {
            new Membership { Id = 1, ClientId = 10, MembershipType = MembershipType.Month, Price = 45.00m, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(1) },
            new Membership { Id = 2, ClientId = 20, MembershipType = MembershipType.Year, Price = 400.00m, StartDate = DateTime.Now, EndDate = DateTime.Now.AddYears(1) }
        });
            await context.SaveChangesAsync();

            var controller = new MembershipController(context);

            var result = await controller.GetAllMemberships();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetMembershipByClientId_MembershipExists_ReturnsMembership()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 3);
            await SeedClientAsync(context, 30, 3, "Petya", "Petrova");

            var existingMembership = new Membership
            {
                Id = 5,
                ClientId = 30,
                MembershipType = MembershipType.Day,
                Price = 15.00m,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1)
            };
            await context.Memberships.AddAsync(existingMembership);
            await context.SaveChangesAsync();

            var controller = new MembershipController(context);

            var result = await controller.GetMembershipByCleintId(30);

            Assert.IsNotNull(result);
            Assert.AreEqual(MembershipType.Day, result.MembershipType);
            Assert.AreEqual(30, result.ClientId);
        }

        [Test]
        public async Task GetMembershipByClientId_MembershipDoesNotExist_ReturnsNull()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new MembershipController(context);

            var result = await controller.GetMembershipByCleintId(999);

            Assert.IsNull(result);
        }
    }
}
