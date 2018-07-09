using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;

namespace GymTracker.Services
{
    public interface IActiveTrainingService
    {
        Task Initialize(int trainingId);
        Task<ExerciseTemplate> GetExerciseForCurrentStage();
    }



    public class ActiveTrainingService : IActiveTrainingService
    {
        public ActiveTrainingService()
        {
            
        }

        public Task Initialize(int trainingId)
        {
            throw new NotImplementedException();
        }

        public Task<ExerciseTemplate> GetExerciseForCurrentStage()
        {
            throw new NotImplementedException();
        }
    }
}
