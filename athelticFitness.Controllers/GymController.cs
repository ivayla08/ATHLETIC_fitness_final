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
    public class GymController
    {
        private GymContext context;

        public GymController()
        {
            context = new GymContext();
        }

        public async Task<Gym> CreateGym(Gym gym)
        {
            await context.Gyms.AddAsync(gym);
            await context.SaveChangesAsync();
            return gym;
        }

        public async Task<List<Gym>> GetAllGyms()
        {
            return await context.Gyms
                .Include(g => g.Coaches)
                .Include(g => g.Workouts)
                .ToListAsync();
        }

        public async Task<Gym?> GetGymById(int id)
        {
            return await context.Gyms
                .Include(g => g.Coaches)
                .Include(g => g.Workouts)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<bool> UpdateGym(Gym updatedGym)
        {
            var existingGym = await context.Gyms.FindAsync(updatedGym.Id);
            if (existingGym == null) return false;

            existingGym.City = updatedGym.City;
            existingGym.Address = updatedGym.Address;

            context.Gyms.Update(existingGym);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGym(int id)
        {
            var gym = await context.Gyms.FindAsync(id);
            if (gym == null) return false;

            context.Gyms.Remove(gym);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
