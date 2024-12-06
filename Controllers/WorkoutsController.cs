using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkoutTracker.Data;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    [Authorize]
    public class WorkoutsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkoutsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Workouts
        public async Task<IActionResult> Index()
        {
            // Filter workouts by the logged-in user's UserId
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workouts = await _context.Workout
            .Include(w => w.WorkoutExercises) // Ensure Exercises is loaded
            .ToListAsync();

            return View(await _context.Workout.Where(w => w.UserId == userId).ToListAsync());
        }

        // GET: Workouts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var workout = await _context.Workout
                .Include(w => w.WorkoutExercises)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId); // Ensure user owns the workout

            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // GET: Workouts/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Workouts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkoutStart,WorkoutEnd,WorkoutExercises")] Workout workout)
        {
            if (ModelState.IsValid)
            {
                // Automatically set the UserId for the logged-in user
                workout.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Save the workout to generate its Id
                _context.Workout.Add(workout);
                await _context.SaveChangesAsync();

                // Ensure WorkoutExercises are linked to the Workout
                if (workout.WorkoutExercises != null && workout.WorkoutExercises.Any())
                {
                    foreach (var exercise in workout.WorkoutExercises)
                    {
                        exercise.WorkoutId = workout.Id;
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(workout);
        }

        // GET: Workouts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workout = await _context.Workout
                .Include(w => w.WorkoutExercises)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId); // Ensure user owns the workout

            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workouts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,WorkoutStart,WorkoutEnd,WorkoutExercises")] Workout workout)
        {
            if (id != workout.Id)
            {
                return NotFound();
            }

            workout.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                try
                {
                    var existingWorkout = await _context.Workout
                        .Include(w => w.WorkoutExercises)
                        .FirstOrDefaultAsync(w => w.Id == id);

                    if (existingWorkout == null)
                    {
                        return NotFound();
                    }

                    // Update workout properties
                    existingWorkout.WorkoutStart = workout.WorkoutStart;
                    existingWorkout.WorkoutEnd = workout.WorkoutEnd;

                    var updatedExerciseIds = workout.WorkoutExercises?.Select(e => e.Id).ToList() ?? new List<int>();

                    // Identify exercises to remove
                    var removedExercises = existingWorkout.WorkoutExercises
                        .Where(e => !updatedExerciseIds.Contains(e.Id))
                        .ToList();

                    // Remove these exercises from the database
                    _context.WorkoutExercise.RemoveRange(removedExercises);

                    // Add or update exercises
                    foreach (var exercise in workout.WorkoutExercises)
                    {
                        var existingExercise = existingWorkout.WorkoutExercises
                            .FirstOrDefault(e => e.Id == exercise.Id);

                        if (existingExercise != null)
                        {
                            // Update existing exercise
                            existingExercise.ExerciseName = exercise.ExerciseName;
                            existingExercise.Count = exercise.Count;
                        }
                        else
                        {
                            // Add new exercise
                            existingWorkout.WorkoutExercises.Add(new WorkoutExercise
                            {
                                ExerciseName = exercise.ExerciseName,
                                Count = exercise.Count,
                                WorkoutId = workout.Id // Ensure proper association
                            });
                        }
                    }

                    _context.Update(existingWorkout);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutExists(workout.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(workout);
        }

        // GET: Workouts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workout = await _context.Workout
                .Include(w => w.WorkoutExercises)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId); // Ensure user owns the workout

            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        // POST: Workouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var workout = await _context.Workout
                .Include(w => w.WorkoutExercises)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId); // Ensure user owns the workout

            if (workout == null)
            {
                return NotFound();
            }

            _context.Workout.Remove(workout);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkoutExists(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _context.Workout.Any(e => e.Id == id && e.UserId == userId);
        }
    }
}
