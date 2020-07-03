using System;
using System.Collections.Generic;

namespace HabitTracker.Domain.UserAggregate
{
    public interface IUserRepository
    {
        User GetUserBadge(Guid userID);
    }
}