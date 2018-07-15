using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace GymTracker.Models
{
    public class BaseClass
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime DateCreated { get; }
        public DateTime DateModified { get; private set; }

        public BaseClass()
        {
            DateCreated = DateTime.Now;
            DateModified = DateTime.Now;
        }

        public void UpdateDateModified()
        {
            DateModified = DateTime.Now;
        }
    }
}
