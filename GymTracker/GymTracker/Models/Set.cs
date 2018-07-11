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
        public int Order { get; set; }
        public bool Done { get; set; }

        public Set()
        {
            Done = false;
        }

        public Set(int exerciseId, int order)
        {
            ExerciseId = exerciseId;
            Order = order;
        }

        public void SetDone()
        {
            if(Done == false)
            Done = true;
        }

        public void SetUndone()
        {
            if (Done)
                Done = false;
        }
    }
}
