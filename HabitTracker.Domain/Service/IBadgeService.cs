using System;
using HabitTracker.Domain.UserAggregate;

namespace HabitTracker.Domain.Service
{
    public interface IBadgeService
    {
        void InsertBadge(String name, Guid userID);
        User GetUser(Guid userID);
    }
}