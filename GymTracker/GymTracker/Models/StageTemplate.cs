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
        public int TrainingTemplateId { get; set; }
        public string ExercisesAsJson { get; set; }
        [Ignore]
        public List<ExerciseTemplate> Exercises { get; set; }

        public StageTemplate()
        {
            
        }

        public StageTemplate(string name, int trainingTemplateId)
        {
            Name = name;
            TrainingTemplateId = trainingTemplateId;
        }
    }
}
