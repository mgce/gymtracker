using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    [Table("TrainingTemplate")]
    public class TrainingTemplate : BaseClass
    {
        public string Name { get; set; }
        public string StagesAsJson { get; set; }

        [Ignore]
        public List<StageTemplate> Stages { get; set; }

        public TrainingTemplate()
        {
            
        }
        public TrainingTemplate(string name)
        {
            Name = name;
        }
    }
}
