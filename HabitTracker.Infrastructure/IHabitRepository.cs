using System;
using System.Collections.Generic;
using HabitTracker.Infrastructure.Model;

namespace HabitTracker {
    public interface IHabitRepository {
        IEnumerable<HabitModel> GetAllHabit(Guid userID);
        HabitModel GetHabit(Guid userID, Guid habitID);
        HabitModel AddHabit(Guid userID, String habitName, String[] daysOff);
        HabitModel UpdateHabit(Guid userID, Guid habitID, String habitName, String[] daysOff);
        HabitModel DeleteHabit(Guid userID, Guid habitID);
        HabitModel InsertHabitLog(Guid userID, Guid habitID);
        Int16 GetCurrentStreak(Guid habitID);
        String GetLastHabitSnapshot(Guid userID, Guid habitID);
        void InsertHabitLogSnapshot(Guid userID, Guid habitID, Int16 streak);
    }
}