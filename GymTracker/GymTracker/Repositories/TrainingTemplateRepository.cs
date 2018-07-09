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
    public class TrainingTemplateTemplateRepository : Database<TrainingTemplate>, ITrainingTemplateRepository
    {
        public TrainingTemplateTemplateRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }
    }
}
