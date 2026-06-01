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
    public class WorkoutController
    {
        private GymContext context;

        public WorkoutController()
        {
            context = new GymContext();
        }

        public async Task<(bool IsSuccess, string Message)> AddWorkoutToScheduleAsync(Workout workout)
        {
            DateTime proposedStart = workout.DateTime;
            DateTime proposedEnd = workout.DateTime.AddMinutes(workout.Duration);

            if (proposedStart < DateTime.Now)
            {
                return (false, "Cannot schedule a workout session in the past.");
            }

            bool isCoachBusy = await context.Workouts.AnyAsync(w =>
                w.CoachId == workout.CoachId &&
                ((proposedStart >= w.DateTime && proposedStart < w.DateTime.AddMinutes(w.Duration)) ||
                 (proposedEnd > w.DateTime && proposedEnd <= w.DateTime.AddMinutes(w.Duration)) ||
                 (proposedStart <= w.DateTime && proposedEnd >= w.DateTime.AddMinutes(w.Duration)))
            );

            if (isCoachBusy)
            {
                return (false, "Coach already has another workout scheduled during this time slot.");
            }


            bool isGymBusy = await context.Workouts.AnyAsync(w =>
                w.GymId == workout.GymId &&
                ((proposedStart >= w.DateTime && proposedStart < w.DateTime.AddMinutes(w.Duration)) ||
                 (proposedEnd > w.DateTime && proposedEnd <= w.DateTime.AddMinutes(w.Duration)) ||
                 (proposedStart <= w.DateTime && proposedEnd >= w.DateTime.AddMinutes(w.Duration)))
            );

            if (isGymBusy)
            {
                return (false, "The selected gym is already reserved for another session during this time slot.");
            }


            try
            {
                context.Workouts.Add(workout);
                await context.SaveChangesAsync();
                return (true, "Workout successfully added to the schedule!");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while saving the schedule: {ex.Message}");
            }
        }


        public async Task<List<Workout>> GetWorkouts()
        {
            return await context.Workouts.Include(c=>c.Coach).ToListAsync();
        }
        public async Task<List<Workout>> GetWorkoutsForCoach(int id)
        {
            return await context.Workouts.Where(x=>x.CoachId==id).Include(c => c.Coach).ToListAsync();
        }
        public async Task<List<Workout>> GetWorkoutsForGym(int id)
        {
            return await context.Workouts.Where(x=>x.Coach.GymId==id).Include(c => c.Coach).ToListAsync();
        }

        public async Task<Workout?> GetWorkout(int id)
        {
            return await context.Workouts.FindAsync(id);
        }

        public async Task<bool> DeleteWorkout(int id)
        {
            Workout workout = await context.Workouts.FindAsync(id);
            if (workout == null) return false;

            context.Remove(workout);
            await context.SaveChangesAsync();
            return true;

        }
        public async Task<bool> UpdateWorkout(Workout updatedWorkout)
        {
            var existingWorkout = await context.Workouts.FindAsync(updatedWorkout.Id);
            if (existingWorkout == null) return false;

            existingWorkout.Name = updatedWorkout.Name;
            existingWorkout.Duration = updatedWorkout.Duration;
            existingWorkout.Level = updatedWorkout.Level;
            existingWorkout.DateTime = updatedWorkout.DateTime;
            existingWorkout.Capacity = updatedWorkout.Capacity;
            existingWorkout.CoachId = updatedWorkout.CoachId;
            existingWorkout.GymId = updatedWorkout.GymId;

            context.Workouts.Update(existingWorkout);
            await context.SaveChangesAsync();
            return true;
        }
    }

}
