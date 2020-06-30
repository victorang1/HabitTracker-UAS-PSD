using System;
using System.Collections.Generic;
using HabitTracker.Infrastructure.Model;

namespace HabitTracker.Infrastructure {
    public interface IHabitRepository {
        IEnumerable<HabitModel> GetAllUserHabit(Guid userID);
        HabitModel GetUserHabit(Guid userID, Guid habitID);
        HabitModel AddHabit(Guid userID, String habitName, IEnumerable<String> daysOff);
        HabitModel UpdateHabit(Guid userID, Guid habitID, String habitName, IEnumerable<String> daysOff);
        HabitModel DeleteHabit(Guid userID, Guid habitID);
        HabitModel InsertHabitLog(Guid userID, Guid habitID);
    }
}