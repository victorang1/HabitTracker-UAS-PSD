using System;
using HabitTracker.Domain.HabitAggregate;

namespace HabitTracker.Domain.Service
{
    public interface IHabitService
    {
        Habit Create(Guid userID, String habitName, String[] daysOff);
        Habit Update(Guid userID, Guid habitID, String name, String[] daysOff);
        Boolean CheckDominating(Guid userID, Guid habitID);
        Boolean CheckWorkaholic(Guid userID);
        Boolean CheckEpicComeback(Guid userID, Guid habitID);
    }
}