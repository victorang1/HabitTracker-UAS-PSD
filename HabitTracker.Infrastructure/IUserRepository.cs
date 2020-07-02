using System;
using System.Collections.Generic;
using HabitTracker.Infrastructure.Model;

namespace HabitTracker.Infrastructure
{
    public interface IUserRepository
    {
        IEnumerable<BadgeModel> GetUserBadge(Guid userID);
    }
}