using System;
using System.Collections.Generic;
using System.Linq;

using HabitTracker.Infrastructure.Repository;
using HabitEntity = HabitTracker.Domain.HabitAggregate;
using HabitTracker.Api;

namespace HabitTracker.API.AntiCorruption
{
    public class HabitACL
    {
        private IAppHabitService _appHabitService;
        public HabitACL(IAppHabitService appHabitService)
        {
            this._appHabitService = appHabitService;
        }
        public List<Habit> GetAllUserHabits(Guid userID)
        {
            IEnumerable<HabitEntity.Habit> habitModels = _appHabitService.GetAllUserHabits(userID);
            if(habitModels != null && habitModels.Any())
            {
                List<Habit> habits = new List<Habit>();
                foreach(HabitEntity.Habit item in habitModels)
                {
                    habits.Add(bindHabitObject(item));
                }
                return habits;
            }
            return null;
        }

        public Habit GetHabitByID(Guid userID, Guid habitID)
        {
            HabitEntity.Habit item = _appHabitService.GetHabitByID(userID, habitID);
            if(item != null)
            {
                return bindHabitObject(item);
            }
            return null;
        }

        public Habit AddHabit(Guid userID, RequestData data)
        {
            HabitEntity.Habit item = _appHabitService.AddHabit(userID, data.Name, data.DaysOff);
            if(item != null)
            {
                return bindHabitObject(item);
            }
            return null;
        }

        public Habit UpdateHabit(Guid userID, Guid habitID, String name, String[] daysOff)
        {
            HabitEntity.Habit item = _appHabitService.UpdateHabit(userID, habitID, name, daysOff);
            if(item != null)
            {
                return bindHabitObject(item);
            }
            return null;
        }

        public Habit DeleteHabit(Guid userID, Guid habitID)
        {
            HabitEntity.Habit item = _appHabitService.DeleteHabit(userID, habitID);
            if(item != null)
            {
                return bindHabitObject(item);
            }
            return null;
        }

        public Habit InsertLog(Guid userID, Guid habitID)
        {
            HabitEntity.Habit item = _appHabitService.InsertLog(userID, habitID);
            if(item != null)
            {
                return bindHabitObject(item);
            }
            return null;
        }

        private Habit bindHabitObject(HabitEntity.Habit model)
        {
            Habit habit = new Habit();
            habit.ID = model.HabitID;
            habit.Name = model.Name.HabitName;
            habit.DaysOff = model.Holidays;
            habit.CurrentStreak = model.CurrentStreak;
            habit.LongestStreak = model.LongestStreak;
            habit.LogCount = model.LogCount;
            habit.Logs = model.Logs;
            habit.UserID = model.UserID;
            habit.CreatedAt = model.CreatedAt;
            return habit;
        }
    }
}