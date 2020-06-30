using System;
using System.Collections.Generic;
using HabitTracker.Infrastructure.Model;

namespace HabitTracker.Infrastructure
{
    public interface IBadgeRepository
    {
        IEnumerable<BadgeModel> GetUserBadge(Guid userID);
    }
}