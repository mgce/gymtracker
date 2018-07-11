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
        Task<List<Stage>> GetByTrainingId(int trainingId);
    }

    public class StageRepository : Database<Stage>, IStageRepository
    {
        public StageRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }

        public Task<List<Stage>> GetByTrainingId(int trainingId)
        {
            return _database.Table<Stage>().Where(s => s.TrainingId == trainingId).ToListAsync();
        }
    }
}
