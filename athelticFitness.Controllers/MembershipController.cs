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
    public class MembershipController
    {
        private GymContext context;

        public MembershipController()
        {
            context = new GymContext();
        }
        public MembershipController(GymContext context)
        {
            this.context = context;
        }
        public async Task<Membership> CreateMembership(Membership membership)
        {
            var clientExists = await context.Clients.AnyAsync(c => c.Id == membership.ClientId);
            if (!clientExists)
            {
                throw new ArgumentException("ClientId does not exist.");
            }
            var clientHasMembership = await context.Clients.AnyAsync(x => x.MembershipId == membership.Id && membership.EndDate<DateTime.Now);
            if (clientHasMembership) 
            {
                throw new ArgumentException("You already have a valid membership");
            }
            await context.Memberships.AddAsync(membership);
            await context.SaveChangesAsync();
            return membership;
        }

        public async Task<List<Membership>> GetAllMemberships()
        {
            return await context.Memberships
                .Include(m => m.Client)
                .ToListAsync();
        }

        public async Task<Membership?> GetMembershipByCleintId(int clientid)
        {
            return await context.Memberships
                .Include(m => m.Client)
                .FirstOrDefaultAsync(m => m.ClientId == clientid);
        }

      

    }
}
