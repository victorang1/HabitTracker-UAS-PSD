using System;
using System.Collections.Generic;
using System.Linq;

using HabitTracker.Domain.HabitAggregate;

namespace HabitTracker.Domain.Service
{
    public class HabitService : IHabitService
    {
        private IHabitRepository _habitRepository;

        public HabitService(IHabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
        }

        public Habit Create(Guid userID, String habitName, String[] daysOff)
        {
            Habit habit = Habit.NewHabit(userID, habitName, daysOff);
            return _habitRepository.AddHabit(habit.UserID, habit.Name.HabitName, habit.DaysOff.daysOff);
        }

        public Habit Update(Guid userID, Guid habitID, String name, String[] daysOff)
        {
            Habit habit = Habit.NewHabit(userID, habitID, name, daysOff);
            return _habitRepository.UpdateHabit(habit.UserID, habit.HabitID, habit.Name.HabitName, habit.DaysOff.daysOff);
        }
    }
}