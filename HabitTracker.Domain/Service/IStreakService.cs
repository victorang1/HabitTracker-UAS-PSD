using System;
using HabitTracker.Domain.HabitAggregate;

namespace HabitTracker.Domain.Service
{
    public interface IStreakCalculationService
    {
        Habit InsertHabitLog(Guid userID, Guid habitID, DateTime currentDate);
    }
}