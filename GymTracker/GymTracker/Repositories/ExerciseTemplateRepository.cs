using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Services;

namespace GymTracker.Repositories
{
    public interface IExerciseTemplateRepository : IDatabase<ExerciseTemplate>
    {
        Task<List<ExerciseTemplate>> GetByStageTemplateId(int stageId);
        Task<List<ExerciseTemplate>> GetByExerciseTemplateIds(int[] exerciseTemplateIds);
    }

    public class ExerciseTemplateRepository : Database<ExerciseTemplate>, IExerciseTemplateRepository
    {
        public ExerciseTemplateRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }

        public Task<List<ExerciseTemplate>> GetByStageTemplateId(int stageId)
        {
            return _database.Table<ExerciseTemplate>().Where(x => x.StageId == stageId).ToListAsync();
        }

        public Task<List<ExerciseTemplate>> GetByExerciseTemplateIds(int[] exerciseTemplateIds)
        {
            return _database.Table<ExerciseTemplate>().Where(x => exerciseTemplateIds.Contains(x.Id)).ToListAsync();
        }
    }
}
