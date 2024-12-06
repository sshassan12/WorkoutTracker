using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkoutTracker.Data;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicWorkoutAPIController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public PublicWorkoutAPIController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("greet")]
        public IActionResult Greet()
        {
            return Ok(new { message = "Hello, world!" });
        }



        [HttpGet("workouts")]
        public async Task<ActionResult<IEnumerable<Workout>>> Workouts()
        {
            var workouts = await _context.Workout.Include(w => w.WorkoutExercises).ToListAsync();
            return Ok(workouts);
        }
    }
}
