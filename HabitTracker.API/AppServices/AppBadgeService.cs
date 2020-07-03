using System;
using System.Collections.Generic;

using HabitTracker.Infrastructure.Repository;
using UserEntity = HabitTracker.Domain.UserAggregate;

namespace HabitTracker.API.Services
{
    public class AppBadgeService : IAppBadgeService
    {
        public UserEntity.User GetUserBadge(Guid userID)
        {
            using(var db = new PostgresUnitOfWork())
            {
                return db.UserRepository.GetUserBadge(userID);
            }
        }
    }
}