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
    }
}
