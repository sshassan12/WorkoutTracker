using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WorkoutTracker.Models
{
    public class Workout
    {
        public Workout()
        {
            WorkoutExercises = new List<WorkoutExercise>();
        }
        public int Id { get; set; }

        [JsonIgnore]
        public string UserId { get; set; }
        public DateTime WorkoutStart { get; set; }
        public DateTime WorkoutEnd { get; set; }
        public List<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();

    }
}
