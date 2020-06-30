using System;
using System.Collections.Generic;
using HabitTracker.Infrastructure.Util;

namespace HabitTracker.Infrastructure.Model
{
    public class HabitModel
    {
        public HabitModel()
        {

        }
        public HabitModel(String habitName, String[] daysOff, Guid userID)
        {
            this.HabitID = Guid.NewGuid();
            this.HabitName = habitName;
            this.DaysOff = daysOff;
            this.UserID = userID;
            this.CreatedAt = DateTime.Parse(DateUtil.GetServerDateTimeFormat());
        }
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