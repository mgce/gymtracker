using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Services;

namespace GymTracker.Repositories
{
    public interface ISetRepository : IDatabase<Set>
    {
        Task<List<Set>> GetSetsByExerciseId(int exerciseId);
    }

    public class SetRepository : Database<Set>, ISetRepository
    {
        public SetRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }

        public Task<List<Set>> GetSetsByExerciseId(int exerciseId)
        {
            return _database.Table<Set>().Where(s => s.ExerciseId == exerciseId).ToListAsync();
        }
    }
}
