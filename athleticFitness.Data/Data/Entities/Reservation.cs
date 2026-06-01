using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace athletic_fitness.Data.Entities
{
    public class Reservation
    {      
        public int ClientId {  get; set; }
        public Client Client { get; set; }
        public int WorkoutId {  get; set; }
        public Workout Workout { get; set; }
    }
}
