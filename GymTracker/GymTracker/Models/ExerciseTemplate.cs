using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    [Table("ExerciseTemplate")]
    public class ExerciseTemplate : BaseClass
    {
        public string Name { get; set; }
        public int? Repetition { get; set; }
        public bool Timer { get; set; }
        public int Sets { get; set; }
        public int? MinTime { get; set; }
        public int? MaxTime { get; set; }
        public int StageId { get; set; }

        public ExerciseTemplate()
        {
            
        }

        public ExerciseTemplate(string name, int? repetition,int sets, bool timer, int? minTime, int? maxTime, int stageId)
        {
            Name = name;
            Repetition = repetition;
            Sets = sets;
            Timer = timer;
            MinTime = minTime;
            MaxTime = maxTime;
            StageId = stageId;
        }
    }
}
