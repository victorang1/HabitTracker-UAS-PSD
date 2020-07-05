using System;
using System.Linq;

namespace HabitTracker.Domain.HabitAggregate
{
    public class DaysOff
    {
        public String[] Holidays { get; private set; }

        public DaysOff(String[] daysOff)
        {
            if(!isValid(daysOff)) throw new Exception("Invalid Days Input");
            this.Holidays = daysOff;
        }
        
        private bool isValid(String[] daysOff)
        {
            if(!daysOff.Any()) return true;
            if(daysOff.Length >= 7) return false;
            if(isDuplicateExists(daysOff)) return false;
            string[] days = new string[] {
                "Mon", "Tue", "Wed", "Thu",
                "Fri", "Sat", "Sun"
            };

            foreach(String str in daysOff)
            {
                if(!days.Any(value => value.Contains(str))) return false;
            }
            return true;
        }

        private Boolean isDuplicateExists(String[] daysOff)
        {
            return daysOff.GroupBy(x => x)
                .Any(c => c.Count() > 1);
        }

        public Boolean InHolidays(DateTime currentDate)
        {
            foreach(String str in Holidays)
            {
                if(currentDate.ToString("ddd").Equals(str))
                {
                    return true;
                }
            }
            return false;
        }

    }
}