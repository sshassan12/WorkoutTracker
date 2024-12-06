using System.Collections.Generic;

namespace WorkoutTracker.Models
{
    public class ExerciseApiResponse
    {
        public bool Success { get; set; }
        public ExerciseData Data { get; set; }
    }

    public class ExerciseData
    {
        public string PreviousPage { get; set; }
        public string NextPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalExercises { get; set; }
        public int CurrentPage { get; set; }
        public List<Exercise> Exercises { get; set; }
    }

    public class Exercise
    {
        public string ExerciseId { get; set; }
        public string Name { get; set; }
        public string GifUrl { get; set; }
        public List<string> TargetMuscles { get; set; }
        public List<string> BodyParts { get; set; }
        public List<string> Equipments { get; set; }
        public List<string> SecondaryMuscles { get; set; }
        public List<string> Instructions { get; set; }
    }
}

