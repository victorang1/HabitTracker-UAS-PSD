using System;
using System.Collections.Generic;

using HabitTracker.Infrastructure.Model;

namespace HabitTracker.API
{
    public interface IHabitService
    {  
        IEnumerable<HabitModel> GetAllUserHabits(Guid userID);
        HabitModel GetHabitByID(Guid userID, Guid habitID);
        HabitModel AddHabit(Guid userID, String name, String[] daysOff);
        HabitModel UpdateHabit(Guid userID, Guid habitID, String name, String[] daysOff);
        HabitModel DeleteHabit(Guid userID, Guid habitID);
        HabitModel InsertLog(Guid userID, Guid habitID);
    }

    public interface IBadgeService
    {
        IEnumerable<BadgeModel> GetUserBadge(Guid userID);
    }
}