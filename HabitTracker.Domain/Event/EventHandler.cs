using System;
using HabitTracker.Domain.Service;
using HabitTracker.Domain.UserAggregate;

namespace HabitTracker.Domain.Event
{
    public class LogCreatedHandler : IObserver<LogCreated>
    {
        private IHabitService _habitService;
        private IBadgeService _badgeService;

        public LogCreatedHandler(IHabitService habitService, IBadgeService badgeService)
        {
            _habitService = habitService;
            _badgeService = badgeService;
        }

        public void Handle(LogCreated evnt)
        {
            User user = _badgeService.GetUser(evnt.UserID);
            if(checkDominatingBadge(user, evnt.HabitID))
            {
                _badgeService.InsertBadge("Dominating", evnt.UserID);
            }
            if(checkWorkaholicBadge(user, evnt.HabitID))
            {
                _badgeService.InsertBadge("Workaholic", evnt.UserID);
            }
            if(checkEpicComebackBadge(user, evnt.HabitID))
            {
                _badgeService.InsertBadge("Epic Comeback", evnt.UserID);
            }
        }

        private Boolean checkDominatingBadge(User user, Guid habitID)
        {
            if(!IsUserValid(user, "Dominating")) return false;
            if(!_habitService.CheckDominating(user.UserID, habitID)) return false;
            return true;
        }

        private Boolean checkWorkaholicBadge(User user, Guid habitID)
        {
            if(user != null)
            {
                if(!IsUserValid(user, "Workaholic")) return false;
                if(!_habitService.CheckWorkaholic(user.UserID)) return false;
                return true;
            }
            return false;
        }

        private Boolean checkEpicComebackBadge(User user, Guid habitID)
        {
            if(!IsUserValid(user, "Epic Comeback")) return false;
            if(!_habitService.CheckEpicComeback(user.UserID, habitID)) return false;
            return true;
        }

        private Boolean IsUserValid(User user, String name)
        {
            if(user == null) return false;
            if(user.IsBadgeExists(name)) return false;
            return true;
        }
    }
}