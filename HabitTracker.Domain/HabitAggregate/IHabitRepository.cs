using System;
using System.Collections.Generic;

namespace HabitTracker.Domain.HabitAggregate {

    public interface IHabitRepository {
        IEnumerable<Habit> GetAllHabit(Guid userID);
        Habit GetHabit(Guid userID, Guid habitID);
        Habit AddHabit(Guid userID, String habitName, String[] daysOff);
        Habit UpdateHabit(Guid userID, Guid habitID, String habitName, String[] daysOff);
        Habit DeleteHabit(Guid userID, Guid habitID);
        Habit InsertHabitLog(Guid userID, Guid habitID, DateTime currentDate, Boolean isHoliday);
        String GetLastHabitSnapshot(Guid userID, Guid habitID);
        Int16 GetHabitCurrentStreak(Guid userID, Guid habitID);
        void InsertHabitLogSnapshot(Guid userID, Guid habitID, Int16 streak, DateTime currentDate);
        Int16 GetTotalLogOnHolidays(Guid userID);
        DateTime GetFirstFromTenStreakDay(Guid userID, Guid habitID);
        DateTime GetLastDayBeforeTenStreak(Guid userID, Guid habitID, DateTime firstStreakDay);
    }
    
}