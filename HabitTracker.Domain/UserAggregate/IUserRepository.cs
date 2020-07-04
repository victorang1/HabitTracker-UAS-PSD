using System;
using System.Collections.Generic;

namespace HabitTracker.Domain.UserAggregate
{
    public interface IUserRepository
    {
        User GetUserBadge(Guid userID);
        void InsertBadge(Guid badgeID, Guid userID);
        Guid GetBadgeID(String name);
    }
}