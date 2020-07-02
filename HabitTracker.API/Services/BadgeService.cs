using System;
using System.Collections.Generic;

using HabitTracker.Infrastructure.Repository;
using HabitTracker.Infrastructure.Model;

namespace HabitTracker.API.Services
{
    public class BadgeService : IBadgeService
    {
        public IEnumerable<BadgeModel> GetUserBadge(Guid userID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.UserRepository.GetUserBadge(userID);
            }
        }
    }
}