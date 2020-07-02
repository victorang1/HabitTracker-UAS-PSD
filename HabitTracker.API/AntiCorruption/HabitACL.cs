using System;
using System.Collections.Generic;
using System.Linq;

using HabitTracker.Infrastructure.Model;

using HabitTracker.Api;

namespace HabitTracker.API.AntiCorruption
{
    public class HabitACL
    {
        private IHabitService _habitService;
        public HabitACL(IHabitService habitService)
        {
            _habitService = habitService;
        }
        public List<Habit> GetAllUserHabits(Guid userID)
        {
            IEnumerable<HabitModel> habitModels = _habitService.GetAllUserHabits(userID);
            if(habitModels != null && habitModels.Any())
            {
                List<Habit> habits = new List<Habit>();
                foreach(HabitModel item in habitModels)
                {
                    habits.Add(bindHabitObject(item));
                }
                return habits;
            }
            return null;
        }

        public Habit GetHabitByID(Guid userID, Guid habitID)
        {
            HabitModel item = _habitService.GetHabitByID(userID, habitID);
            if(item != null)
            {
                return bindHabitObject(item);
            }
            return null;
        }

        public Habit AddHabit(Guid userID, String name, String[] daysOff)
        {
            HabitModel item = _habitService.AddHabit(userID, name, daysOff);
            if(item != null)
            {
                return bindHabitObject(item);
            }
            return null;
        }

        public Habit UpdateHabit(Guid userID, Guid habitID, String name, String[] daysOff)
        {
            HabitModel item = _habitService.UpdateHabit(userID, habitID, name, daysOff);
            if(item != null)
            {
                return bindHabitObject(item);
            }
            return null;
        }

        public Habit DeleteHabit(Guid userID, Guid habitID)
        {
            HabitModel item = _habitService.DeleteHabit(userID, habitID);
            if(item != null)
            {
                return bindHabitObject(item);
            }
            return null;
        }

        public Habit InsertLog(Guid userID, Guid habitID)
        {
            HabitModel item = _habitService.InsertLog(userID, habitID);
            if(item != null)
            {
                return bindHabitObject(item);
            }
            return null;
        }

        private Habit bindHabitObject(HabitModel model)
        {
            Habit habit = new Habit();
            habit.ID = model.HabitID;
            habit.Name = model.HabitName;
            habit.DaysOff = model.DaysOff;
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