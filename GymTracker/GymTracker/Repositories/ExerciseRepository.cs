using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GymTracker.Models;
using GymTracker.Services;

namespace GymTracker.Repositories
{
    public interface IExerciseRepository : IDatabase<Exercise>
    {
    }

    public class ExerciseRepository : Database<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(IFileHelper fileHelper) 
            : base(fileHelper)
        {
        }

    }
}
