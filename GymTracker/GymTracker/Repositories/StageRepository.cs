using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Services;

namespace GymTracker.Repositories
{
    public interface IStageRepository : IDatabase<Stage>
    {
        Task<List<Stage>> GetStagesByTrainingId(int trainingId);
    }

    public class StageRepository : Database<Stage>, IStageRepository
    {
        public StageRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }

        public async Task<List<Stage>> GetStagesByTrainingId(int trainingId)
        {
            return await _database.Table<Stage>().Where(x => x.TrainingId == trainingId).ToListAsync();
        }
    }
}
