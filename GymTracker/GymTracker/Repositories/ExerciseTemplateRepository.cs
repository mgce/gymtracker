using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Services;

namespace GymTracker.Repositories
{
    public interface IExerciseTemplateRepository : IDatabase<ExerciseTemplate>
    {
        Task<List<ExerciseTemplate>> GetByStageId(int stageId);
    }

    public class ExerciseTemplateTemplateRepository : Database<ExerciseTemplate>, IExerciseTemplateRepository
    {
        public ExerciseTemplateTemplateRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }

        public Task<List<ExerciseTemplate>> GetByStageId(int stageId)
        {
            return _database.Table<ExerciseTemplate>().Where(x => x.StageId == stageId).ToListAsync();
        }
    }
}
