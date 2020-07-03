using System;
using System.Collections.Generic;

using HabitTracker.Infrastructure.Repository;
using HabitTracker.Infrastructure.Model;

using HabitEntity = HabitTracker.Domain.HabitAggregate;
using HabitTracker.Domain.Service;

namespace HabitTracker.API.Services
{
    public class AppHabitService : IAppHabitService
    {
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
            using(var db = new PostgresUnitOfWork())
            {
                IHabitService service = new HabitService(db.HabitRepository);
                return service.Create(userID, name, daysOff);
            }
        }
        public HabitEntity.Habit UpdateHabit(Guid userID, Guid habitID, String name, String[] daysOff)
        {
            using(var db = new PostgresUnitOfWork())
            {
                IHabitService service = new HabitService(db.HabitRepository);
                return service.Update(userID, habitID, name, daysOff);
            }
        }

        public HabitEntity.Habit DeleteHabit(Guid userID, Guid habitID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.DeleteHabit(userID, habitID);
            }
        }
        public HabitEntity.Habit InsertLog(Guid userID, Guid habitID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                IStreakCalculationService service = new StreakCalculationService(db.HabitRepository);
                return service.InsertLogForThisHabit(userID, habitID, DateTime.Parse("2020-07-12 16:49:28.223996+07"));
            }
        }
    }
}