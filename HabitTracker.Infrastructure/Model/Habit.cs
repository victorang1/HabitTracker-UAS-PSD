using System;
using System.Collections.Generic;

namespace HabitTracker.Infrastructure.Model
{
    public class HabitModel
    {
        public Guid HabitID { get; set; }
        public String HabitName { get; set; }
        public String[] DaysOff { get; set; }
        public Int16 CurrentStreak { get; set; }
        public Int16 LongestStreak { get; set; }
        public Int16 LogCount { get; set; }
        public IEnumerable<DateTime> Logs { get; set; }
        public Guid UserID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}