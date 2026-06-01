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
using Microsoft.EntityFrameworkCore;

namespace TestProject1.Service
{
    public class ReservationServiceTest
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
                Address = "Mladost 1"
            };
            await context.Gyms.AddAsync(gym);
            await context.SaveChangesAsync();
        }

        private async Task SeedCoachAsync(GymContext context, int coachId, int userId, int gymId)
        {
            var coach = new Coach
            {
                Id = coachId,
                UserId = userId,
                GymId = gymId,
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "ivan.ivanov@gym.com",
                Phone = "1234567890"
            };
            await context.Coaches.AddAsync(coach);
            await context.SaveChangesAsync();
        }

        private async Task SeedClientAsync(GymContext context, int clientId, int userId)
        {
            var client = new Client
            {
                Id = clientId,
                UserId = userId,
                FirstName = "Ivayla",
                LastName = "Ivanova",
                Email = "ivayla.ivanova@gym.com",
                Phone = "1234567890",
                MembershipId = 1
            };
            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();
        }

        private async Task SeedWorkoutAsync(GymContext context, int workoutId, int coachId, int gymId)
        {
            var workout = new Workout
            {
                Id = workoutId,
                Name = "Yoga",
                DateTime = DateTime.Now.AddDays(1),
                Duration = 60,
                Level = LevelType.Begginer,
                Capacity = 15,
                CoachId = coachId,
                GymId = gymId
            };
            await context.Workouts.AddAsync(workout);
            await context.SaveChangesAsync();
        }

        [Test]
        public async Task CreateReservation_ValidData_ReturnsReservationAndSavesToDb()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);
            await SeedClientAsync(context, 5, 10);
            await SeedGymAsync(context, 1);
            await SeedUserAsync(context, 11);
            await SeedCoachAsync(context, 2, 11, 1);
            await SeedWorkoutAsync(context, 20, 2, 1);

            var controller = new ReservationController(context);
            var newReservation = new Reservation
            {
                ClientId = 5,
                WorkoutId = 20
            };

            var result = await controller.CreateReservation(newReservation);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.ClientId);
            Assert.AreEqual(20, result.WorkoutId);

            var dbReservation = await context.Reservations.FirstOrDefaultAsync(r => r.ClientId == 5 && r.WorkoutId == 20);
            Assert.IsNotNull(dbReservation);
        }

        [Test]
        public async Task CreateReservation_ClientDoesNotExist_ThrowsArgumentExceptionAsync()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedGymAsync(context, 1);
            await SeedUserAsync(context, 11);
            await SeedCoachAsync(context, 2, 11, 1);
            await SeedWorkoutAsync(context, 20, 2, 1);

            var controller = new ReservationController(context);
            var invalidReservation = new Reservation
            {
                ClientId = 999,
                WorkoutId = 20
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await controller.CreateReservation(invalidReservation)
            );
            Assert.AreEqual("ClientId does not exist.", ex.Message);
        }

        [Test]
        public async Task CreateReservation_WorkoutDoesNotExist_ThrowsArgumentExceptionAsync()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);
            await SeedClientAsync(context, 5, 10);

            var controller = new ReservationController(context);
            var invalidReservation = new Reservation
            {
                ClientId = 5,
                WorkoutId = 999
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await controller.CreateReservation(invalidReservation)
            );
            Assert.AreEqual("WorkoutId does not exist.", ex.Message);
        }

        [Test]
        public async Task CreateReservation_ClientAlreadyHasReservation_ThrowsArgumentException()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);
            await SeedClientAsync(context, 5, 10);
            await SeedGymAsync(context, 1);
            await SeedUserAsync(context, 11);
            await SeedCoachAsync(context, 2, 11, 1);
            await SeedWorkoutAsync(context, 20, 2, 1);

            var existingReservation = new Reservation { ClientId = 5, WorkoutId = 20 };
            await context.Reservations.AddAsync(existingReservation);
            await context.SaveChangesAsync();

            var controller = new ReservationController(context);
            var duplicateReservation = new Reservation { ClientId = 5, WorkoutId = 20 };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await controller.CreateReservation(duplicateReservation)
            );
            Assert.AreEqual("You have already booked this workout", ex.Message);
        }

        [Test]
        public async Task GetAllReservations_WhenCalled_ReturnsReservationsForSpecificUser()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedGymAsync(context, 1);
            await SeedUserAsync(context, 11);
            await SeedCoachAsync(context, 2, 11, 1);
            await SeedWorkoutAsync(context, 20, 2, 1);

            await SeedUserAsync(context, 10);
            await SeedClientAsync(context, 5, 10);

            await SeedUserAsync(context, 12);
            await SeedClientAsync(context, 6, 12);

            await context.Reservations.AddRangeAsync(new List<Reservation>
        {
            new Reservation { ClientId = 5, WorkoutId = 20 },
            new Reservation { ClientId = 6, WorkoutId = 20 }
        });
            await context.SaveChangesAsync();

            var controller = new ReservationController(context);

            var result = await controller.GetAllReservations(10);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(5, result[0].ClientId);
        }

        [Test]
        public async Task GetReservationByKey_ReservationExists_ReturnsReservation()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);
            await SeedClientAsync(context, 5, 10);
            await SeedGymAsync(context, 1);
            await SeedUserAsync(context, 11);
            await SeedCoachAsync(context, 2, 11, 1);
            await SeedWorkoutAsync(context, 20, 2, 1);

            var reservation = new Reservation { ClientId = 5, WorkoutId = 20 };
            await context.Reservations.AddAsync(reservation);
            await context.SaveChangesAsync();

            var controller = new ReservationController(context);

            var result = await controller.GetReservationByKey(5, 20);

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.ClientId);
            Assert.AreEqual(20, result.WorkoutId);
        }

        [Test]
        public async Task GetReservationByKey_ReservationDoesNotExist_ReturnsNull()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new ReservationController(context);

            var result = await controller.GetReservationByKey(99, 99);

            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteReservation_ReservationExists_RemovesFromDbAndReturnsTrue()
        {
            using var context = TestDBFactory.CreateContext();
            await SeedUserAsync(context, 10);
            await SeedClientAsync(context, 5, 10);
            await SeedGymAsync(context, 1);
            await SeedUserAsync(context, 11);
            await SeedCoachAsync(context, 2, 11, 1);
            await SeedWorkoutAsync(context, 20, 2, 1);

            var reservation = new Reservation { ClientId = 5, WorkoutId = 20 };
            await context.Reservations.AddAsync(reservation);
            await context.SaveChangesAsync();

            var controller = new ReservationController(context);

            var result = await controller.DeleteReservation(5, 20);

            Assert.IsTrue(result);

            var dbReservation = await context.Reservations.FirstOrDefaultAsync(r => r.ClientId == 5 && r.WorkoutId == 20);
            Assert.IsNull(dbReservation);
        }

        [Test]
        public async Task DeleteReservation_ReservationDoesNotExist_ReturnsFalse()
        {
            using var context = TestDBFactory.CreateContext();
            var controller = new ReservationController(context);

            var result = await controller.DeleteReservation(99, 99);

            Assert.IsFalse(result);
        }
    }
}
