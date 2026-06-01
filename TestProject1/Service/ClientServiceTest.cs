using athelticFitness.Controllers;
using athletic_fitness.Data.Entities;
using athletic_fitness.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject1.Helpers;

namespace TestProject1.Service
{
    public class ClientServiceTest
    {
        [Test]
        public async Task GetAllClientsTest()
        {
            var context = TestDBFactory.CreateContext();

            User user = new User
            {
                Id = 20,
                Username="Ivayla08",
                Password="123"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            Client client = new Client
            {
                Id = 21,
                FirstName = "Ivayla",
                LastName = "Ivanova",
                Phone = "089",
                Email = "ivayla@mail",
                UserId=20
            };
            context.Clients.Add(client);       
            
            await context.SaveChangesAsync();
            ClientController controller = new ClientController(context);
            await controller.CreateClient(client);
            Assert.IsNotNull(client);
            Assert.AreEqual("Ivayla",client.FirstName);
            Assert.AreEqual(1,controller.GetAllClients().Result.Count());
        }
    }
}
