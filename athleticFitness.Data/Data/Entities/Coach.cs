using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace athletic_fitness.Data.Entities
{
    public class Coach
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email {  get; set; }
        public string Phone {  get; set; }
        public int GymId {  get; set; }
        public Gym Gym { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Workout> Workouts { get; set; }=new List<Workout>();

        public string FullName => FirstName + " " + LastName;
    }
}
