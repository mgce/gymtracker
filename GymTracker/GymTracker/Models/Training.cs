using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    [Table("Training")]
    public class Training : BaseClass
    {
        [Ignore]
        public TrainingTemplate Template { get; set; }
        public int TrainingTemplateId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long DurationInTicks { get; set; }
        [Ignore]
        public List<Stage> Stages { get; set; }

        public Training()
        {
           
        }

        public Training(TrainingTemplate template)
        {
            Template = template;
            TrainingTemplateId = template.Id;
            DateCreated = DateTime.Now;
        }
    }
}
