using System;
using System.Collections.Generic;
using System.Linq;

using HabitTracker.Infrastructure.Repository;
using HabitTracker.Infrastructure.Model;

using HabitTracker.Api;

namespace HabitTracker.API.AntiCorruption
{
    public class BadgeACL
    {
        private IBadgeService _badgeService;

        public BadgeACL(IBadgeService badgeService)
        {
            _badgeService = badgeService;
        }

        public List<Badge> GetUserBadge(Guid userID)
        {
            IEnumerable<BadgeModel> badgeModels = _badgeService.GetUserBadge(userID);
            if(badgeModels != null && badgeModels.Any())
            {
                List<Badge> badges = new List<Badge>();
                foreach(BadgeModel item in badgeModels)
                {
                    badges.Add(bindBadgeObject(item));
                }
                return badges;
            }
            return null;
        }

        public Badge bindBadgeObject(BadgeModel model)
        {
            Badge badge = new Badge();
            badge.ID = model.BadgeID;
            badge.Name = model.Name;
            badge.Description = model.Description;
            badge.UserID = model.UserID;
            badge.CreatedAt = model.CreatedAt;
            return badge;
        }
    }
}