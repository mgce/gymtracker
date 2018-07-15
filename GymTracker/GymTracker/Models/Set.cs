using System;
using System.Collections.Generic;
using System.Text;

namespace GymTracker.Models
{
    public class Set : BaseClass
    {
        public int ExerciseId { get; set; }
        public int TimeDuration { get; set; }
        public int? MinTime { get; set; }
        public int? MaxTime { get; set; }
        public int? Repetitions { get; set; }
        public int Order { get; set; }
        public string Previous { get; set; }
        public bool Done { get; set; }


        public Set()
        {
            Done = false;
        }

        public Set(int exerciseId, int order, int? repetitions, int? minTime, int? maxTime)
        {
            ExerciseId = exerciseId;
            Order = order;
            Repetitions = repetitions;
            MinTime = minTime;
            MaxTime = maxTime;
        }

        public void SetPrevious(string previous)
        {
            Previous = previous;
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
