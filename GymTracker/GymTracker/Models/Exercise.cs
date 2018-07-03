using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    [Table("Exercise")]
    public class Exercise : BaseClass
    {
        public string Name { get; set; }
        public int? Repetition { get; set; }
        public int? MinTime { get; set; }
        public int? MaxTime { get; set; }
        public int StageId { get; set; }

        public Exercise()
        {
            
        }
    }
}
