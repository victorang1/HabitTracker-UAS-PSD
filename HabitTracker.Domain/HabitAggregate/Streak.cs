using System;

namespace HabitTracker.Domain.HabitAggregate
{
    public class Streak
    {
        public Int16 CurrentStreak { get; private set; }
        public Int16 LongestStreak { get; private set; }

        public Streak(Int16 currentStreak, Int16 longestStreak)
        {
            if(!isValid(currentStreak, longestStreak)) throw new Exception("Streak is invalid");
            this.CurrentStreak = currentStreak;
            this.LongestStreak = longestStreak;
        }

        private Boolean isValid(Int16 currentStreak, Int16 longestStreak)
        {
            if(currentStreak > longestStreak) return false;
            return true;
        }


        public override bool Equals(object obj)
        {
            Streak streak = obj as Streak;
            if(streak == null) return false;

            if(streak.CurrentStreak != this.CurrentStreak) return false;
            if(streak.LongestStreak != this.LongestStreak) return false;
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + CurrentStreak.GetHashCode() + LongestStreak.GetHashCode();
                return hash;
            }
        }
    }
}