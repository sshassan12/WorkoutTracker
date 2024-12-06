using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WorkoutTracker.Models;

namespace WorkoutTracker.Controllers
{
    public class ConsumeAPIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ConsumeAPIController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

   

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
           
            var response = await client.GetStringAsync("https://exercisedb-api.vercel.app/api/v1/exercises?search=push-up%20sit-up%20squat&offset=0&limit=100");

           var exerciseApiResponse = JsonConvert.DeserializeObject<ExerciseApiResponse>(response);

            if (exerciseApiResponse == null || !exerciseApiResponse.Success)
            {
                return View("Error");
            }

            var exercise1 = exerciseApiResponse.Data.Exercises.FirstOrDefault(e => e.ExerciseId == "i5cEhka");
            var exercise2 = exerciseApiResponse.Data.Exercises.FirstOrDefault(e => e.ExerciseId == "2gPfomN");
            var exercise3 = exerciseApiResponse.Data.Exercises.FirstOrDefault(e => e.ExerciseId == "LIlE5Tn");

            var exercises = new List<Exercise>();

            if (exercise1 != null) exercises.Add(exercise1);
            if (exercise2 != null) exercises.Add(exercise2);
            if (exercise3 != null) exercises.Add(exercise3);

            return View(exercises);

        }



    }
}



