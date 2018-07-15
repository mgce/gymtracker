using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    [Table("Training")]
    public class Training : BaseClass
    {
        
        public int TrainingTemplateId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long DurationInTicks { get; set; }
        public bool Active { get; set; }
        [Ignore]
        public TrainingTemplate Template { get; set; }
        [Ignore]
        public List<Stage> Stages { get; set; }

        public Training()
        {
           
        }

        public Training(TrainingTemplate template)
        {
            Template = template;
            TrainingTemplateId = template.Id;
        }

        public void StartTraining()
        {
            StartTime = DateTime.Now;
            Active = true;
        }
    }
}
