using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WorkoutTracker.Models
{
    public class WorkoutExercise
    {

        
        public int Id { get; set; }
        public int WorkoutId { get; set; }

        [JsonIgnore]
        public Workout Workout { get; set; }
        public string ExerciseName { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Count must be a positive number.")]
        public int Count { get; set; }
    }
}
