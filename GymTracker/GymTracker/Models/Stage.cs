using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    [Table("Stage")]
    public class Stage : BaseClass
    {
        [Ignore]
        public StageTemplate Template { get; set; }
        public int StageTemplateId { get; set; }
        [Ignore]
        public List<Exercise> Exercises { get; set; }
        public int TrainingId { get; set; }

        public Stage()
        {
            
        }

        public Stage(StageTemplate template, int trainingId)
        {
            StageTemplateId = template.Id;
            TrainingId = trainingId;
            Template = template;
        }
    }
}
