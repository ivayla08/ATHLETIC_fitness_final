using athletic_fitness.Data.Entities;
using athletic_fitness.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace athelticFitness.Controllers
{
    public class ReservationController
    {
        private GymContext context;

        public ReservationController()
        {
            context = new GymContext();
        }
        public ReservationController(GymContext context) 
        {
            this.context = context;
        }

        public async Task<Reservation> CreateReservation(Reservation reservation)
        {
            var clientExists = await context.Clients.AnyAsync(c => c.Id == reservation.ClientId);
            var workoutExists = await context.Workouts.AnyAsync(w => w.Id == reservation.WorkoutId);

            if (!clientExists)
            {
                throw new ArgumentException("ClientId does not exist.");
            }
            if (!workoutExists)
            {
                throw new ArgumentException("WorkoutId does not exist.");
            }

            Workout workout = await context.Workouts.FirstOrDefaultAsync(x => x.Id == reservation.WorkoutId);
            if (workout.Capacity <= 0)
            {
                throw new ArgumentException("This class is fully booked");
            }
            var clientHasRes = await context.Reservations.AnyAsync(x => x.ClientId == reservation.ClientId && x.WorkoutId == reservation.WorkoutId);
            if (clientHasRes)
            {
                throw new ArgumentException("You have already booked this workout");
            }

            Client client = context.Clients.Find(reservation.ClientId);

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await context.Reservations.AddAsync(reservation);
                workout.Capacity--;

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return reservation;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<Reservation>> GetAllReservations(int userId)
        {
            return await context.Reservations
                .Where(x=>x.Client.UserId==userId)
                .Include(r => r.Client)
                .Include(r => r.Workout)
                .Include(x=>x.Workout.Gym)
                .ToListAsync();
        }

        public async Task<Reservation?> GetReservationByKey(int clientId, int workoutId)
        {
            return await context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Workout)
                .FirstOrDefaultAsync(r => r.ClientId == clientId && r.WorkoutId == workoutId);
        }

        public async Task<bool> DeleteReservation(int clientId, int workoutId)
        {
            var reservation = await context.Reservations
                .FirstOrDefaultAsync(r => r.ClientId == clientId && r.WorkoutId == workoutId);

            if (reservation == null) return false;

            Workout workout = await context.Workouts.FirstOrDefaultAsync(x => x.Id == reservation.WorkoutId);
            context.Reservations.Remove(reservation);
            if (workout != null)
            {
                workout.Capacity++;
            }
            await context.SaveChangesAsync();
            return true;
        }
    }
}
