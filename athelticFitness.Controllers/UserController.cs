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
    public class UserController
    {
        private GymContext context;

        public UserController() 
        {
            context = new GymContext();
        }
      
        public UserController(GymContext context)
        {
            this.context = context;
        }
        public async Task<User> CreateUser(User user)
        {  
            bool exists = await context.Users.AnyAsync(u => u.Username == user.Username);
            if (exists)
            {
                throw new InvalidOperationException("Username is already taken.");
            }
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await context.Users.ToListAsync();
        }       
        public async Task<User?> GetUserById(int id)
        {
            return await context.Users.FindAsync(id);
        }       
        public async Task<bool> UpdateUser(User updatedUser)
        {
            var existingUser = await context.Users.FindAsync(updatedUser.Id);
            if (existingUser == null) return false;

            existingUser.Username = updatedUser.Username;
            existingUser.Role = updatedUser.Role;
         
            if (!string.IsNullOrEmpty(updatedUser.Password))
            {               
                existingUser.Password = updatedUser.Password;
            }

            context.Users.Update(existingUser);
            await context.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> DeleteUser(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null) return false;

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }

        public class LoginResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public User? User { get; set; }
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
        {         
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return new LoginResult { Success = false, Message = "Invalid username or passoword" };
            }
                       
            if (user.Password != password)
            {
                return new LoginResult { Success = false, Message = "Invalid username or password" };
            }
           
            return new LoginResult { Success = true, Message = "Successful log in!", User = user };
        }
    }
}
