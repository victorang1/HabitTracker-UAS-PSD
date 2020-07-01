using System;
using System.Collections.Generic;

using HabitTracker.Infrastructure.Repository;
using HabitTracker.Infrastructure.Model;

namespace HabitTracker.API.Services
{
    public class HabitService : IHabitService
    {
        public IEnumerable<HabitModel> GetAllUserHabits(Guid userID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.GetAllUserHabit(userID);
            }
        }

        public HabitModel GetHabitByID(Guid userID, Guid habitID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.GetUserHabit(userID, habitID);
            }
        }
        public HabitModel AddHabit(Guid userID, String name, IEnumerable<String> daysOff)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.AddHabit(userID, name, daysOff);
            }
        }
        public HabitModel UpdateHabit(Guid userID, Guid habitID, String name, IEnumerable<String> daysOff)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.UpdateHabit(userID, habitID, name, daysOff);
            }
        }

        public HabitModel DeleteHabit(Guid userID, Guid habitID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.DeleteHabit(userID, habitID);
            }
        }
        public HabitModel InsertLog(Guid userID, Guid habitID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.InsertHabitLog(userID, habitID);
            }
        }
    }
}