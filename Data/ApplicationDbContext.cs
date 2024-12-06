using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WorkoutTracker.Models;

namespace WorkoutTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // You can also add custom configurations if needed.
            // Example: Configure IdentityUserLogin if necessary
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(login => new { login.LoginProvider, login.ProviderKey });


            modelBuilder.Entity<Exercise>()
            .HasKey(e => e.ExerciseId);

            // If needed, configure relationships or ignore unsupported fields
            modelBuilder.Entity<Exercise>().Ignore(e => e.TargetMuscles);
            modelBuilder.Entity<Exercise>().Ignore(e => e.BodyParts);
            modelBuilder.Entity<Exercise>().Ignore(e => e.Equipments);
            modelBuilder.Entity<Exercise>().Ignore(e => e.SecondaryMuscles);
            modelBuilder.Entity<Exercise>().Ignore(e => e.Instructions);


            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId);

            modelBuilder.Entity<Workout>()
                .HasMany(w => w.WorkoutExercises)
                .WithOne(we => we.Workout)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete

        }
        public DbSet<WorkoutTracker.Models.Workout> Workout { get; set; }
        public DbSet<WorkoutTracker.Models.WorkoutExercise> WorkoutExercise { get; set; }
        public DbSet<Exercise> Exercises { get; set; }

    }
}
