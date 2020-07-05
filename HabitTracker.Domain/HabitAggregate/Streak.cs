using System;

namespace HabitTracker.Domain.HabitAggregate
{
    public class Streak
    {
        public Int16 CurrentStreak { get; private set; }
        public Int16 LongestStreak { get; private set; }

        public Streak(Int16 currentStreak, Int16 longest)
        {
            
        }
    }
}