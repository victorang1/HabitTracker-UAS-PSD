using System;
using System.Linq;

namespace HabitTracker.HabitAggregate
{
    public class DaysOff
    {
        public String[] daysOff { get; private set; }

        public DaysOff(String[] daysOff)
        {
            if(!IsValid(daysOff)) throw new Exception("Invalid Days Input");
            this.daysOff = daysOff;
        }
        private bool IsValid(String[] daysOff)
        {
            if(daysOff.Length > 7) return false;
            string[] days = new string[] {
                "Mon", "Tue", "Wed", "Thu",
                "Fri", "Sat", "Sun"
            };

            foreach(string str in daysOff)
            {
                if(!days.Any(value => value.Contains(str))) return false;
            }
            return true;
        }
    }
}