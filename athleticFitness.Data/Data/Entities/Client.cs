using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace athletic_fitness.Data.Entities
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }

        public int MembershipId { get; set; }
        public Membership Membership { get; set; }

        public int UserId {  get; set; }
        public User User {  get; set; }
        public ICollection<Reservation> Reservations { get; set; }= new List<Reservation>();

        public string FullName => FirstName + " " + LastName;
    }
}
