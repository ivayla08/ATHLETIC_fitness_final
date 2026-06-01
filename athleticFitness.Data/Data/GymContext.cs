using athletic_fitness.Data.Entities;
using athletic_fitness.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace athletic_fitness.Data
{
    public class GymContext : DbContext
    {
        public GymContext()
        {

        }
        public GymContext(DbContextOptions<GymContext> options) : base(options)
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();

                builder.AddJsonFile("appsettings.json");

                var config = builder.Build();

                string connectionString = config.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectionString);
            }
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasKey(x => x.Id);

            modelBuilder.Entity<Client>()
                .Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Client>()
               .Property(x => x.LastName)
               .HasMaxLength(50)
               .IsRequired();

            modelBuilder.Entity<Client>()
               .Property(x => x.Email)
               .HasMaxLength(50)
               .IsRequired();

            modelBuilder.Entity<Client>()
               .Property(x => x.Phone)
               .HasMaxLength(10)
               .IsRequired();

            modelBuilder.Entity<Client>()
                .HasOne(c => c.Membership)
                .WithOne(m => m.Client)
                .HasForeignKey<Membership>(m => m.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
    .HasOne(c => c.User)
    .WithOne()
    .HasForeignKey<Client>(c => c.UserId)
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Coach>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Coach>()
                .Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Coach>()
               .Property(x => x.LastName)
               .HasMaxLength(50)
               .IsRequired();

            modelBuilder.Entity<Coach>()
               .Property(x => x.Email)
               .HasMaxLength(50)
               .IsRequired();

            modelBuilder.Entity<Coach>()
               .Property(x => x.Phone)
               .HasMaxLength(50)
               .IsRequired();

            modelBuilder.Entity<Coach>()
                .HasOne(x => x.Gym)
                .WithMany(x => x.Coaches)
                .HasForeignKey(x => x.GymId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Coach>()
                .HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<Coach>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Gym>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Gym>()
                .Property(x => x.City)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Gym>()
                .Property(x => x.Address)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<Membership>().HasKey(x => x.Id);

            modelBuilder.Entity<Membership>()
                .Property(x => x.MembershipType)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<Membership>()
                .Property(x => x.Price)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Membership>()
                .HasOne(x => x.Client)
                .WithOne(x => x.Membership)
                .HasForeignKey<Membership>(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Membership>()
                .Property(x => x.StartDate)
                .IsRequired();

            modelBuilder.Entity<Membership>()
                .Property(x => x.EndDate)
                .IsRequired();

            modelBuilder.Entity<Reservation>()
                .HasKey(x => new { x.ClientId, x.WorkoutId });

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.Client)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(x => x.Workout)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasKey(x => x.Id);

            modelBuilder.Entity<User>()
                .Property(x => x.Username)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<User>()
               .Property(x => x.Password)
               .HasMaxLength(50)
               .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.Role)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<Workout>().HasKey(x => x.Id);

            modelBuilder.Entity<Workout>()
                .Property(x => x.Name).HasMaxLength(50).IsRequired();

            modelBuilder.Entity<Workout>()
                .Property(x => x.Duration).IsRequired();

            modelBuilder.Entity<Workout>()
                .Property(x => x.DateTime).IsRequired();

            modelBuilder.Entity<Workout>()
                .Property(x => x.Level)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<Workout>()
                .Property(x => x.Capacity).IsRequired();

            modelBuilder.Entity<Workout>()
                .HasOne(x => x.Coach)
                .WithMany(x => x.Workouts)
                .HasForeignKey(x => x.CoachId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Workout>()
                .HasOne(x => x.Gym)
                .WithMany(x => x.Workouts)
                .HasForeignKey(x => x.GymId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().HasData(
       new User
       {
           Id = 5,
           Username = "admin3",
           Password = "123",
           Role = RoleType.Admin
       }
   );
        }
    }
}
