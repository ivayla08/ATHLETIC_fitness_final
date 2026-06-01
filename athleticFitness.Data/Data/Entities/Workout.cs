using athletic_fitness.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace athletic_fitness.Data.Entities
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateTime { get; set; }
        public int Duration {  get; set; }          
        public LevelType Level { get; set; }
        public int Capacity { get; set; }
        public int CoachId {  get; set; }
        public Coach Coach { get; set; }
        public int GymId {  get; set; }
        public Gym Gym { get; set; }      
        public ICollection<Reservation> Reservations { get; set; }
      = new List<Reservation>();

        public string WorkoutInfo => Name + " " + DateTime;
    }
}
