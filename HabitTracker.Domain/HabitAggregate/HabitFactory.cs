using System;

namespace HabitTracker.Domain.HabitAggregate
{
    public class HabitFactory
    {
        public static Habit CreateHabit(Guid habitId, String habitName)
        {
            
        }

        public static Name CreateName(String name)
        {
            return new Name(name);
        }

        public static DaysOff CreateDaysOff(String[] daysOff)
        {
            return new DaysOff(daysOff);
        }

        public static Streak CreateStreak()
        {
            
        }
    }
}