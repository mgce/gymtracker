using System;
using System.Collections.Generic;
using System.Text;
using GymTracker.Models;
using GymTracker.Services;

namespace GymTracker.Repositories
{
    public interface ITrainingRepository : IDatabase<Training>
    {

    }

    public class TrainingRepository : Database<Training>, ITrainingRepository
    {
        public TrainingRepository(IFileHelper fileHelper) : base(fileHelper)
        {
        }
    }
}
