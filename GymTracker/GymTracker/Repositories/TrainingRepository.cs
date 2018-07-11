using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Services;

namespace GymTracker.Repositories
{
    public interface ITrainingRepository : IDatabase<Training>
    {
        Task<Training> GetActiveTrainingByTemplateId(int templateId);
    }

    public class TrainingRepository : Database<Training>, ITrainingRepository
    {
        public TrainingRepository(IFileHelper fileHelper) : base(fileHelper)
        {
        }

        public Task<Training> GetActiveTrainingByTemplateId(int templateId)
        {
            return _database.Table<Training>().Where(t => t.TrainingTemplateId == templateId && t.Active)
                .FirstOrDefaultAsync();
        }
    }
}
