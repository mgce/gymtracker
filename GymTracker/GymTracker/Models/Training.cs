using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    [Table("Training")]
    public class Training : BaseClass
    {
        public string Name { get; set; }
        public string StagesAsJson { get; set; }

        [Ignore]
        public List<Stage> Stages { get; set; }

        public Training()
        {
            
        }
        public Training(string name)
        {
            Name = name;
        }
    }
}
