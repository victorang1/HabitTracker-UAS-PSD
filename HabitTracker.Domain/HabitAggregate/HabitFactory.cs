using System;
using System.Linq;
using System.Collections.Generic;

namespace HabitTracker.Domain.HabitAggregate
{
    public class HabitFactory
    {
        public static Habit CreateHabit(Guid habitID, String habitName, String[] daysOff
            , Int16 currentStreak, Int16 longestStreak, Int16 logCount
            , String logs, Guid userID, DateTime createdAt)
        {
            Name name = new Name(habitName);
            DaysOff holidays = new DaysOff(daysOff);
            Streak streak = new Streak(currentStreak, longestStreak);
            List<DateTime> habitLogs = getLogs(logs);
            return new Habit(habitID, name, holidays, streak, logCount, habitLogs, userID, createdAt);
        }

        private static List<DateTime> getLogs(String tempLogs)
        {
            List<String> result = tempLogs.Split(',').ToList();
            List<DateTime> logs = new List<DateTime>();
            foreach(String item in result) {
                if(item == null || item.Equals("")) continue;
                logs.Add(DateTime.Parse(item));
            }
            return logs;
        }
    }
}