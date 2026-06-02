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
    public class ClientController
    {
        private GymContext context;

        public ClientController() 
        {
            context = new GymContext();
        }
        public ClientController(GymContext context)
        {
            this.context = context;
        }

        public async Task<Client> CreateClient(Client client)
        {            
            var userExists = await context.Users.AnyAsync(u => u.Id == client.UserId);
            if (!userExists)
            {
                throw new ArgumentException("UserId does not exist.");
            }
            bool emailExists = await context.Clients.AnyAsync(x=> x.Email == client.Email);
            if (emailExists) 
            {
                throw new ArgumentException("This email is already taken. ");
            }
            await context.Clients.AddAsync(client);
            await context.SaveChangesAsync();
            return client;
        }
        public async Task<Client> GetClientByUserId(int userId)
        {
            return await context.Clients
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<List<Client>> GetAllClients()
        {
            return await context.Clients
                .Include(c => c.User)
                .Include(c => c.Membership)
                .ToListAsync();
        }
        public async Task<bool> UpdateClient(Client updatedClient)
        {
            var existingClient = await context.Clients.FindAsync(updatedClient.Id);
            if (existingClient == null) return false;

            existingClient.FirstName = updatedClient.FirstName;
            existingClient.LastName = updatedClient.LastName;
            existingClient.Email = updatedClient.Email;
            existingClient.Phone = updatedClient.Phone;
            existingClient.MembershipId = updatedClient.MembershipId;

            context.Clients.Update(existingClient);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteClient(int id)
        {
            var client = await context.Clients.FindAsync(id);
            if (client == null) return false;

            context.Clients.Remove(client);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
