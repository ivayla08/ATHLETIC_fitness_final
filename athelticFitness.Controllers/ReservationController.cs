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
            Client client=context.Clients.Find(reservation.ClientId);
            var clientHasRes=await context.Reservations.AnyAsync(x=>x.ClientId==reservation.ClientId && x.WorkoutId==reservation.WorkoutId);
            if (clientHasRes)
            {
                throw new ArgumentException("You have already booked this workout");
            }

            await context.Reservations.AddAsync(reservation);
            await context.SaveChangesAsync();
            return reservation;
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

            context.Reservations.Remove(reservation);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
