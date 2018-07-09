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
                _database.DropTableAsync<TrainingTemplate>();
                _database.CreateTableAsync<TrainingTemplate>();
                _database.CreateTableAsync<StageTemplate>();
                _database.CreateTableAsync<ExerciseTemplate>();
            })).Wait();

        }

        public async Task<List<TrainingTemplate>> GetTrainingsAsync()
        {
            var trainings = await _database.Table<TrainingTemplate>().ToListAsync();
            foreach (var training in trainings)
            {
                training.Stages = JsonConvert.DeserializeObject<List<StageTemplate>>(training.StagesAsJson);
            }

            return trainings;
        }

        public async Task<List<StageTemplate>> GetStagesAsync()
        {
            var stages = await _database.Table<StageTemplate>().ToListAsync();
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
