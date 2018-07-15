using System;
using System.Collections.Generic;
using System.Text;
using GymTracker.Models;
using GymTracker.Services;

namespace GymTracker.Repositories
{
    public interface ITrainingTemplateRepository : IDatabase<TrainingTemplate>
    {

    }
    public class TrainingTemplateRepository : Database<TrainingTemplate>, ITrainingTemplateRepository
    {
        public TrainingTemplateRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }
    }
}
