using System;
using HabitTracker.Domain.UserAggregate;

namespace HabitTracker.Domain.Service
{
    public class BadgeService : IBadgeService
    {
        protected IUserRepository _userRepository;

        public BadgeService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public void InsertBadge(String name, Guid userID)
        {
            Guid badgeId = _userRepository.GetBadgeID(name);
            if(badgeId != Guid.Empty)
            {
                _userRepository.InsertBadge(badgeId, userID);
            }
        }

        public User GetUser(Guid userID)
        {
            return _userRepository.GetUserBadge(userID);
        }
    }
}