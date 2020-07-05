using System;
using System.Collections.Generic;

namespace HabitTracker.Domain.HabitAggregate
{
    public class Habit
    {
        public Guid HabitID { get; private set; }
        public Name Name { get; private set; }
        public DaysOff DaysOff { get; private set; }
        public Streak Streak { get; private set; }
        public Int16 LogCount { get; private set; }
        public IEnumerable<DateTime> Logs { get; private set; }
        public Guid UserID { get; private set; }
        public DateTime CreatedAt { get; set; }

        public Int16 CurrentStreak
        {
            get 
            {
                return Streak.CurrentStreak;
            }
        }

        public Int16 LongestStreak
        {
            get
            {
                return Streak.LongestStreak;
            }
        }

        public String HabitName
        {
            get
            {
                return Name.HabitName;
            }
        }

        public String[] Holidays
        {
            get
            {
                return DaysOff.Holidays;
            }
        }

        public Habit(Guid habitID, Name habitName, DaysOff daysOff, Streak streak, Int16 logCount, IEnumerable<DateTime> logs, Guid userID, DateTime createdAt)
        {
            this.HabitID = habitID;
            this.Name = habitName;
            this.DaysOff = daysOff;
            this.Streak = streak;
            this.LogCount = logCount;
            this.Logs = logs;
            this.UserID = userID;
            this.CreatedAt = createdAt;
        }

        public Habit(Guid userID, Guid HabitID, String habitName, String[] daysOff)
        {
            this.UserID = userID;
            this.HabitID = HabitID;
            this.Name = new Name(habitName);
            this.DaysOff = new DaysOff(daysOff);
            this.CreatedAt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        public static Habit NewHabit(Guid userID, Guid HabitID, String habitName, String[] daysOff)
        {
            return new Habit(userID, HabitID, habitName, daysOff);
        }

        public static Habit NewHabit(Guid userID, String habitName, String[] daysOff)
        {
            return Habit.NewHabit(userID, Guid.NewGuid(), habitName, daysOff);
        }
        
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