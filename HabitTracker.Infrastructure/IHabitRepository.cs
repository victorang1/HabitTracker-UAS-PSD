using System;
using System.Collections.Generic;
using HabitTracker.Infrastructure.Model;

namespace HabitTracker.Infrastructure {
    public interface IHabitRepository {
        IEnumerable<HabitModel> GetAllUserHabit(Guid userID);
        HabitModel GetUserHabit(Guid userID, Guid habitID);
        Habit AddHabit(String habitName, IEnumerable<String> daysOff);
        // Habit UpdateHabit(String habitName, IEnumerable<String> daysOff);
        // Habit DeleteHabit();
    }
}