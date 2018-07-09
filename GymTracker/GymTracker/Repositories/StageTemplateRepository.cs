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
        Task<List<StageTemplate>> GetStagesByTrainingId(int trainingId);
    }

    public class StageTemplateTemplateRepository : Database<StageTemplate>, IStageTemplateRepository
    {
        public StageTemplateTemplateRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }

        public async Task<List<StageTemplate>> GetStagesByTrainingId(int trainingId)
        {
            return await _database.Table<StageTemplate>().Where(x => x.TrainingId == trainingId).ToListAsync();
        }
    }
}
