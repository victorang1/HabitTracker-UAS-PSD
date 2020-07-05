using System;
using System.Collections.Generic;

using HabitTracker.Infrastructure.Repository;

using HabitEntity = HabitTracker.Domain.HabitAggregate;
using HabitTracker.Domain.Service;

namespace HabitTracker.API.Services
{
    public class AppHabitService : IAppHabitService
    {
        private IHabitService _habitService;
        private IStreakCalculationService _streakService;

        public AppHabitService(IHabitService habitService, IStreakCalculationService streakService)
        {
            _habitService = habitService;
            _streakService = streakService;
        }
        
        public IEnumerable<HabitEntity.Habit> GetAllUserHabits(Guid userID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.GetAllHabit(userID);
            }
        }

        public HabitEntity.Habit GetHabitByID(Guid userID, Guid habitID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.GetHabit(userID, habitID);
            }
        }
        public HabitEntity.Habit AddHabit(Guid userID, String name, String[] daysOff)
        {
            return _habitService.Create(userID, name, daysOff);
        }
        public HabitEntity.Habit UpdateHabit(Guid userID, Guid habitID, String name, String[] daysOff)
        {
            return _habitService.Update(userID, habitID, name, daysOff);
        }

        public HabitEntity.Habit DeleteHabit(Guid userID, Guid habitID)
        {
            return _habitService.Delete(userID, habitID);
        }
        public HabitEntity.Habit InsertLog(Guid userID, Guid habitID)
        {
            return _streakService.InsertHabitLog(userID, habitID, DateTime.Now);
            //For manual testing insert log
            // return _streakService.InsertHabitLog(userID, habitID, DateTime.Parse("2020-07-19 16:49:28.223996+07"));
        }
    }
}