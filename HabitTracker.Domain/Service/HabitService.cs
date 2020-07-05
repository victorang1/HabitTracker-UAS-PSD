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
            return _habitRepository.AddHabit(habit.UserID, habit.HabitName, habit.Holidays);
        }

        public Habit Update(Guid userID, Guid habitID, String name, String[] daysOff)
        {
            Habit habitToBeUpdated = _habitRepository.GetHabit(userID, habitID);
            if(habitToBeUpdated != null)
            {
                Habit habit = Habit.NewHabit(userID, habitID, name, daysOff);
                return _habitRepository.UpdateHabit(habit.UserID, habit.HabitID, habit.HabitName, habit.Holidays);
            }
            throw new Exception("This user with habit id " + habitID + " not found");
        }

        public Habit Delete(Guid userID, Guid habitID)
        {
            Habit habitToBeDeleted = _habitRepository.GetHabit(userID, habitID);
            if(habitToBeDeleted != null)
            {
                return _habitRepository.DeleteHabit(userID, habitID);
            }
            throw new Exception("This user with habit id " + habitID + " not found");
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
                String lastDayBeforeStreakStr = _habitRepository.GetLastDayBeforeTenStreak(userID, habitID, firstStreakDay);
                if(firstStreakDay == null && lastDayBeforeStreakStr.Equals("")) return false;
                if(lastDayBeforeStreakStr.Equals("")) 
                {
                    DateTime habitCreatedAt = habit.CreatedAt;
                    if(firstStreakDay.Date - habitCreatedAt.Date >= TimeSpan.FromDays(10)) return true;
                    return false;
                }
                return firstStreakDay.Date - DateTime.Parse(lastDayBeforeStreakStr).Date >= TimeSpan.FromDays(10);
            }
            return false;
        }
    }
}