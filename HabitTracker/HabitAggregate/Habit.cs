using System;
using System.Collections.Generic;

namespace HabitTracker.HabitAggregate
{
    public class Habit
    {
        public Guid HabitID { get; private set; }
        public String HabitName { get; private set; }
        public DaysOff DaysOff { get; private set; }
        public Int16 CurrentStreak { get; private set; }
        public Int16 LongestStreak { get; private set; }
        public Int16 LogCount { get; private set; }
        public IEnumerable<DateTime> Logs { get; private set; }
        public Guid UserID { get; private set; }
        
        public override bool Equals(object obj)
        {
            Habit habit = obj as Habit;
            if(habit == null) return false;
            return this.HabitID == habit.HabitID;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + HabitID.GetHashCode();
                return hash;
            }
        }
    }
}