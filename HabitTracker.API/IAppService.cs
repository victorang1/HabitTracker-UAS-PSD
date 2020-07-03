using System;
using System.Collections.Generic;

using HabitEntity = HabitTracker.Domain.HabitAggregate;
using UserEntity = HabitTracker.Domain.UserAggregate;

namespace HabitTracker.API
{
    public interface IAppHabitService
    {
        IEnumerable<HabitEntity.Habit> GetAllUserHabits(Guid userID);
        HabitEntity.Habit GetHabitByID(Guid userID, Guid habitID);
        HabitEntity.Habit AddHabit(Guid userID, String name, String[] daysOff);
        HabitEntity.Habit UpdateHabit(Guid userID, Guid habitID, String name, String[] daysOff);
        HabitEntity.Habit DeleteHabit(Guid userID, Guid habitID);
        HabitEntity.Habit InsertLog(Guid userID, Guid habitID);
    }

    public interface IAppBadgeService
    {
        UserEntity.User GetUserBadge(Guid userID);
    }
}