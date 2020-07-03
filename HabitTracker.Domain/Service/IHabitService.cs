using System;
using HabitTracker.Domain.HabitAggregate;

namespace HabitTracker.Domain.Service
{
    public interface IHabitService
    {
        Habit Create(Guid userID, String habitName, String[] daysOff);
        Habit Update(Guid userID, Guid habitID, String name, String[] daysOff);
    }
}