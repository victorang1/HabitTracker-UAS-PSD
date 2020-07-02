using System;
using System.Collections.Generic;
using HabitTracker.Infrastructure.Util;
using HabitTracker.Infrastructure.Model;

namespace HabitTracker.Domain.HabitAggregate
{
    public class Habit
    {
        public Guid HabitID { get; private set; }
        public Name Name { get; private set; }
        public DaysOff DaysOff { get; private set; }
        public Int16 CurrentStreak { get; private set; }
        public Int16 LongestStreak { get; private set; }
        public Int16 LogCount { get; private set; }
        public IEnumerable<DateTime> Logs { get; private set; }
        public Guid UserID { get; private set; }
        public DateTime CreatedAt { get; set; }

        public Habit(Guid userID, Guid HabitID, String habitName, String[] daysOff)
        {
            this.UserID = userID;
            this.HabitID = HabitID;
            this.Name = new Name(habitName);
            this.DaysOff = new DaysOff(daysOff);
            this.CreatedAt = DateTime.Parse(DateUtil.GetServerDateTimeFormat());
        }

        public Habit(HabitModel model)
        {
            this.HabitID = model.HabitID;
            this.Name = new Name(model.HabitName);
            this.DaysOff = new DaysOff(model.DaysOff);
            this.CurrentStreak = model.CurrentStreak;
            this.LongestStreak = model.LongestStreak;
            this.LogCount = model.LogCount;
            this.Logs = model.Logs;
            this.UserID = model.UserID;
            this.CreatedAt = model.CreatedAt;
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