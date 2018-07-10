using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Models
{
    public class Set : BaseClass
    {
        public int ExerciseId { get; set; }
        public int TimeDuration { get; set; }
        public int Reps { get; set; }

        public Set()
        {
            
        }

        public Set(int exerciseId)
        {
            ExerciseId = exerciseId;
        }
    }
}
