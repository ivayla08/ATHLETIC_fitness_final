using athelticFitness.Controllers;
using athletic_fitness.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject1.Helpers;

namespace TestProject1.Service
{
    public class GymServiceTest
    {
        [Test]
        public async Task CreateGym_ValidGym_ReturnsCreatedGymAndSavesToDb()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new GymController(context);
            var newGym = new Gym
            {
                Id = 1,
                City = "Sofia",
                Address = "Mladost 1"
            };

            var result = await controller.CreateGym(newGym);

            Assert.IsNotNull(result);
            Assert.AreEqual("Sofia", result.City);

            var dbGym = await context.Gyms.FindAsync(1);
            Assert.IsNotNull(dbGym);
            Assert.AreEqual("Mladost 1", dbGym.Address);
        }

        [Test]
        public async Task GetAllGyms_WhenCalled_ReturnsAllGyms()
        {
            using var context = TestDBFactory.CreateContext();
            await context.Gyms.AddRangeAsync(new List<Gym>
        {
            new Gym { Id = 1, City = "Sofia", Address = "Mladost 1" },
            new Gym { Id = 2, City = "Plovdiv", Address = "Center" }
        });
            await context.SaveChangesAsync();

            var controller = new GymController(context);

            var result = await controller.GetAllGyms();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetGymById_GymExists_ReturnsGym()
        {
            using var context = TestDBFactory.CreateContext();
            var existingGym = new Gym
            {
                Id = 1,
                City = "Varna",
                Address = "Sea Garden"
            };
            await context.Gyms.AddAsync(existingGym);
            await context.SaveChangesAsync();

            var controller = new GymController(context);

            var result = await controller.GetGymById(1);

            Assert.IsNotNull(result);
            Assert.AreEqual("Varna", result.City);
            Assert.AreEqual("Sea Garden", result.Address);
        }

        [Test]
        public async Task GetGymById_GymDoesNotExist_ReturnsNull()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new GymController(context);

            var result = await controller.GetGymById(99);

            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateGym_GymExists_UpdatesFieldsAndReturnsTrue()
        {
            using var context = TestDBFactory.CreateContext();
            var gym = new Gym { Id = 1, City = "Sofia", Address = "Old Address" };
            await context.Gyms.AddAsync(gym);
            await context.SaveChangesAsync();

            var controller = new GymController(context);
            var updatedGym = new Gym { Id = 1, City = "Plovdiv", Address = "New Address" };

            var result = await controller.UpdateGym(updatedGym);

            Assert.IsTrue(result);

            var dbGym = await context.Gyms.FindAsync(1);
            Assert.AreEqual("Plovdiv", dbGym.City);
            Assert.AreEqual("New Address", dbGym.Address);
        }

        [Test]
        public async Task UpdateGym_GymDoesNotExist_ReturnsFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new GymController(context);
            var nonExistentGym = new Gym { Id = 99, City = "Burgas", Address = "Beach Alley" };

            var result = await controller.UpdateGym(nonExistentGym);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteGym_GymExists_RemovesFromDbAndReturnsTrue()
        {
            using var context = TestDBFactory.CreateContext();
            var gym = new Gym { Id = 1, City = "Sofia", Address = "To Be Deleted" };
            await context.Gyms.AddAsync(gym);
            await context.SaveChangesAsync();

            var controller = new GymController(context);

            var result = await controller.DeleteGym(1);

            Assert.IsTrue(result);

            var dbGym = await context.Gyms.FindAsync(1);
            Assert.IsNull(dbGym);
        }

        [Test]
        public async Task DeleteGym_GymDoesNotExist_ReturnsFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new GymController(context);

            var result = await controller.DeleteGym(99);

            Assert.IsFalse(result);
        }
    }
}
