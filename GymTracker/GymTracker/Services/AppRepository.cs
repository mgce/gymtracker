using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;

namespace GymTracker.Services
{
    public class AppRepository
    {
        private SQLiteAsyncConnection _database;

        public AppRepository(IFileHelper fileHelper)
        {
            var dbPath = fileHelper.GetLocalFilePath("GymTracker.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            Task.Run((() =>
            {
                _database.DropTableAsync<Training>();
                _database.CreateTableAsync<Training>();
                _database.CreateTableAsync<Stage>();
                _database.CreateTableAsync<ExerciseTemplate>();
            })).Wait();

        }

        public async Task<List<Training>> GetTrainingsAsync()
        {
            var trainings = await _database.Table<Training>().ToListAsync();
            foreach (var training in trainings)
            {
                training.Stages = JsonConvert.DeserializeObject<List<Stage>>(training.StagesAsJson);
            }

            return trainings;
        }

        public async Task<List<Stage>> GetStagesAsync()
        {
            var stages = await _database.Table<Stage>().ToListAsync();
            foreach (var stage in stages)
            {
                stage.Exercises = JsonConvert.DeserializeObject<List<ExerciseTemplate>>(stage.ExercisesAsJson);
            }

            return stages;
        }

        public Task<List<ExerciseTemplate>> GetExercisesAsync()
        {
            return _database.Table<ExerciseTemplate>().ToListAsync();
        }

        //public Task<Training> GetTrainingAsync(int id)
        //{
        //    var training = 
        //}

    }
}
