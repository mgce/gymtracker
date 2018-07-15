using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Services;

namespace GymTracker.Repositories
{
    public interface IExerciseRepository : IDatabase<Exercise>
    {
        Task<List<Exercise>> GetByStageTemplateId(int stageId);
        Task<List<Exercise>> GetLastExercisesByStageId(int stageId);
    }

    public class ExerciseRepository : Database<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }

        public Task<List<Exercise>> GetByStageTemplateId(int stageTempalateId)
        {
            return _database.Table<Exercise>().Where(x => x.StageTempalateId == stageTempalateId).ToListAsync();
        }

        public Task<List<Exercise>> GetLastExercisesByStageId(int stageId)
        {
            return _database.QueryAsync<Exercise>("SELECT * FROM Exercise ");
        }
    }
}
