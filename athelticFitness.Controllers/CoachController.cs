using athletic_fitness.Data;
using athletic_fitness.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace athelticFitness.Controllers
{
    public class CoachController
    {
        private GymContext context;

        public CoachController()
        {
            context = new GymContext();
        }

        public async Task<Coach> CreateCoach(Coach coach)
        {
            var userExists = await context.Users.AnyAsync(u => u.Id == coach.UserId);
            var gymExists = await context.Gyms.AnyAsync(g => g.Id == coach.GymId);

            if (!userExists)
            {
                throw new ArgumentException("UserId does not exist.");
            }
            if (!gymExists)
            {
                throw new ArgumentException("GymId does not exist.");
            }

            await context.Coaches.AddAsync(coach);
            await context.SaveChangesAsync();
            return coach;
        }

        public async Task<List<Coach>> GetAllCoaches()
        {
            return await context.Coaches
                .Include(c => c.User)
                .Include(c => c.Gym)
                .ToListAsync();
        }

        public async Task<List<Coach>> GetAllCoachesByGym(int id)
        {
            return await context.Coaches
                .Include(c => c.User)
                .Include(c => c.Gym)
                .Where(c=>c.GymId == id)
                .ToListAsync();
        }

        public async Task<Coach?> GetCoachById(int id)
        {
            return await context.Coaches
                .Include(c => c.User)
                .Include(c => c.Gym)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Coach> GetCoachByUserId(int userId)
        {
            return await context.Coaches
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<bool> UpdateCoach(Coach updatedCoach)
        {
            var existingCoach = await context.Coaches.FindAsync(updatedCoach.Id);
            if (existingCoach == null) return false;

            existingCoach.FirstName = updatedCoach.FirstName;
            existingCoach.LastName = updatedCoach.LastName;
            existingCoach.Email = updatedCoach.Email;
            existingCoach.Phone = updatedCoach.Phone;
            existingCoach.GymId = updatedCoach.GymId;
            existingCoach.UserId = updatedCoach.UserId;

            context.Coaches.Update(existingCoach);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCoach(int id)
        {
            var coach = await context.Coaches.FindAsync(id);
            if (coach == null) return false;

            context.Coaches.Remove(coach);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
