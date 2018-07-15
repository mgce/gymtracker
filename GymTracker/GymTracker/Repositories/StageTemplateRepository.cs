using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Services;

namespace GymTracker.Repositories
{
    public interface IStageTemplateRepository : IDatabase<StageTemplate>
    {
        Task<List<StageTemplate>> GetStagesByTrainingTemplateId(int trainingId);
    }

    public class StageTemplateRepository : Database<StageTemplate>, IStageTemplateRepository
    {
        public StageTemplateRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }

        public async Task<List<StageTemplate>> GetStagesByTrainingTemplateId(int trainingId)
        {
            return await _database.Table<StageTemplate>().Where(x => x.TrainingTemplateId == trainingId).ToListAsync();
        }
    }
}
