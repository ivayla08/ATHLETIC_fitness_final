using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace athletic_fitness.Data.Entities
{
    public class Gym
    {
        public int Id { get; set; }
        public string City {  get; set; }
        public string Address {  get; set; }

        public ICollection<Coach> Coaches { get; set; }=new List<Coach>();
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();

        public string FullAddress=>City+" "+Address;

    }
}
