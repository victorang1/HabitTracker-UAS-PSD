using System;
using System.Collections.Generic;
using System.Linq;

using HabitTracker.Infrastructure.Repository;
using UserEntity = HabitTracker.Domain.UserAggregate;

using HabitTracker.Api;

namespace HabitTracker.API.AntiCorruption
{
    public class BadgeACL
    {
        private IAppBadgeService _appBadgeService;

        public BadgeACL(IAppBadgeService appBadgeService)
        {
            _appBadgeService = appBadgeService;
        }

        public List<Badge> GetUserBadge(Guid userID)
        {
            UserEntity.User user = _appBadgeService.GetUserBadge(userID);
            if(user.Badges != null && user.Badges.Any())
            {
                List<Badge> badges = new List<Badge>();
                foreach(UserEntity.Badge item in user.Badges)
                {
                    badges.Add(bindBadgeObject(user, item));
                }
                return badges;
            }
            return null;
        }

        public Badge bindBadgeObject(UserEntity.User user, UserEntity.Badge model)
        {
            Badge badge = new Badge();
            badge.ID = model.BadgeID;
            badge.Name = model.Name;
            badge.Description = model.Description;
            badge.UserID = user.UserID;
            badge.CreatedAt = model.CreatedAt;
            return badge;
        }
    }
}