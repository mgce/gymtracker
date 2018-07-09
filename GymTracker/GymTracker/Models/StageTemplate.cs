using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    [Table("StageTemplate")]
    public class StageTemplate : BaseClass
    {
        public string Name { get; set; }
        public int TrainingId { get; set; }
        public string ExercisesAsJson { get; set; }
        [Ignore]
        public List<ExerciseTemplate> Exercises { get; set; }

        public StageTemplate()
        {
            
        }

        public StageTemplate(string name, int trainingId)
        {
            Name = name;
            TrainingId = trainingId;
        }
    }
}
