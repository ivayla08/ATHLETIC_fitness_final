using athletic_fitness.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.Helpers
{
    public class TestDBFactory
    {
        public static GymContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<GymContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            GymContext context = new GymContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
