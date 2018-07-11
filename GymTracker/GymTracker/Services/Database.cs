using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using Newtonsoft.Json;
using SQLite;

namespace GymTracker.Services
{
    public interface IDatabase<T>
    {
        Task<List<T>> GetItemsAsync();
        Task<List<T>> GetItemsNotDoneAsync();
        Task<T> GetItemAsync(int id);
        Task<int> SaveItemAsync(T item);
        Task<int> DeleteItemAsync(T item);
    }

    public class Database<T> : IDatabase<T> where T : BaseClass, new() 
    {
        protected SQLiteAsyncConnection _database;

        public Database(IFileHelper fileHelper)
        {
            var dbPath = fileHelper.GetLocalFilePath("GymTracker2.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            Task.Run((() =>
            {
                //_database.DropTableAsync<Training>();
                _database.CreateTableAsync<TrainingTemplate>();
                _database.CreateTableAsync<StageTemplate>();
                _database.CreateTableAsync<ExerciseTemplate>();
                _database.CreateTableAsync<Training>();
                _database.CreateTableAsync<Stage>();
                _database.CreateTableAsync<Exercise>();
                _database.CreateTableAsync<Set>();
            })).Wait();

        }

        public async Task<List<T>> GetItemsAsync()
        {
            var items = await _database.Table<T>().ToListAsync();
            items = await LoadChildren(items);
            return items;
        }

        private async Task<List<T>> LoadChildren(List<T> objectList)
        {
            var type = typeof(T);
            if (type == typeof(TrainingTemplate))
            {
                foreach (var item in objectList)
                {
                    try
                    {
                        var properties = item.GetType().GetProperties();
                        var asJsonProperty = properties.Single(x => x.Name == nameof(TrainingTemplate.StagesAsJson));
                        var stagesAsJson = asJsonProperty.GetValue(item, null);
                        if(stagesAsJson == null)
                            continue;
                        var stagesProperty = properties.Single(x => x.Name == nameof(TrainingTemplate.Stages));
                        var stageList = stagesProperty.GetValue(item, null) as List<StageTemplate>;
                        if (stageList != null)
                            stageList = JsonConvert.DeserializeObject<List<StageTemplate>>(stagesAsJson as string);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    
                }
                //return await Task.FromResult(await LoadTrainingChildrent(objectList));
            }

            return await Task.FromResult(objectList);
        }

        private async Task<List<T>> LoadTrainingChildrent(List<T> objectList)
        {
            var trainings = objectList as List<TrainingTemplate>;

            if (trainings == null || trainings.Count == 0)
                return await Task.FromResult(objectList);

            foreach (var training in trainings)
            {
                if(training.StagesAsJson != null)
                    training.Stages = JsonConvert.DeserializeObject<List<StageTemplate>>(training.StagesAsJson);
            }

            var list = trainings as List<T>;

            return await Task.FromResult(list);
        }

        public Task<List<T>> GetItemsNotDoneAsync()
        {
            return _database.QueryAsync<T>($"SELECT * FROM [{nameof(T)}] WHERE [Done] = 0");
        }

        public Task<T> GetItemAsync(int id)
        {
            return _database.Table<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(T item)
        {
            if (item.Id != 0)
            {
                return _database.UpdateAsync(item);
            }
            else
            {
                return _database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(T item)
        {
            return _database.DeleteAsync(item);
        }
    }
}
