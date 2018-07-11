using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    [Table("Exercise")]
    public class Exercise : BaseClass
    {
        public int ExerciseTemplateId { get; set; }
        [Ignore]
        public ExerciseTemplate Template { get; set; }
        [Ignore]
        public List<Set> Sets { get; set; }
        public int StageId { get; set; }

        public Exercise()
        {
            
        }
        public Exercise(ExerciseTemplate tempalate, int stageId)
        {
            StageId = stageId;
            Sets = new List<Set>();
            Template = tempalate;
            ExerciseTemplateId = tempalate.Id;
        }
    }
}
