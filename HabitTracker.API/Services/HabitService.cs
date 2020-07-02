using System;
using System.Collections.Generic;

using HabitTracker.Infrastructure.Repository;
using HabitTracker.Infrastructure.Model;

using Domain = HabitTracker.Domain.HabitAggregate;

namespace HabitTracker.API.Services
{
    public class HabitService : IHabitService
    {
        public IEnumerable<HabitModel> GetAllUserHabits(Guid userID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.GetAllHabit(userID);
            }
        }

        public HabitModel GetHabitByID(Guid userID, Guid habitID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.HabitRepository.GetHabit(userID, habitID);
            }
        }
        public HabitModel AddHabit(Guid userID, String name, String[] daysOff)
        {
            using(var db = new PostgresUnitOfWork())
            {
                Domain.HabitAggregate.Habit domainModel = Domain.HabitAggregate.Habit.NewHabit(userID, name, daysOff);
                return db.HabitRepository.AddHabit(domainModel.UserID, domainModel.Name.HabitName, domainModel.DaysOff.daysOff);
            }
        }
        public HabitModel UpdateHabit(Guid userID, Guid habitID, String name, String[] daysOff)
        {
            using(var db = new PostgresUnitOfWork())
            {
                Domain.HabitAggregate.Habit domainModel = Domain.HabitAggregate.Habit.NewHabit(userID, habitID, name, daysOff);
                return db.HabitRepository.UpdateHabit(domainModel.UserID, domainModel.HabitID, domainModel.Name.HabitName, domainModel.DaysOff.daysOff);
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