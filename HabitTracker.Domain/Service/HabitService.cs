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

        public Boolean CheckDominating(Guid userID, Guid habitID)
        {
            return _habitRepository.GetHabit(userID, habitID).CurrentStreak == 4;
        }

        public Boolean CheckWorkaholic(Guid userID)
        {
            return _habitRepository.GetTotalLogOnHolidays(userID) == 10;
        }

        public Boolean CheckEpicComeback(Guid userID, Guid habitID)
        {
            Habit habit = _habitRepository.GetHabit(userID, habitID);
            if(habit.CurrentStreak == 10)
            {
                DateTime firstStreakDay = _habitRepository.GetFirstFromTenStreakDay(userID, habitID);
                DateTime lastDayBeforeStreak = _habitRepository.GetLastDayBeforeTenStreak(userID, habitID, firstStreakDay);
                if(firstStreakDay == null || lastDayBeforeStreak == null) return false;
                if(lastDayBeforeStreak == null) return true;
                return firstStreakDay.Date - lastDayBeforeStreak.Date >= TimeSpan.FromDays(10);
            }
            return false;
        }
    }
}