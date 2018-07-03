using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    [Table("Stage")]
    public class Stage : BaseClass
    {
        public string Name { get; set; }
        public int TrainingId { get; set; }
        public string ExercisesAsJson { get; set; }
        [Ignore]
        public List<Exercise> Exercises { get; set; }

        public Stage()
        {
            
        }

        public Stage(string name, int trainingId)
        {
            Name = name;
            TrainingId = trainingId;
        }
    }
}
