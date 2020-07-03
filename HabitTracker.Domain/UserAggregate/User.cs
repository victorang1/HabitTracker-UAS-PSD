using System;
using System.Collections.Generic;

namespace HabitTracker.Domain.UserAggregate
{
    public class User
    {
        public Guid UserID { get; private set; }
        public String Username { get; private set; }
        public IEnumerable<Badge> Badges { get; private set; }

        public User(Guid userID, String username)
        {
            this.UserID = userID;
            this.Username = username;
        }

        public User(Guid userID, String username, IEnumerable<Badge> badges)
        {
            this.UserID = userID;
            this.Username = username;
            this.Badges = badges;
        }

        public User AddBadges(IEnumerable<Badge> badges)
        {
            return new User(this.UserID, this.Username, badges);
        }

        public override bool Equals(object obj)
        {
            User user = obj as User;
            if(user == null) return false;
            return this.UserID == user.UserID;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + UserID.GetHashCode();
                return hash;
            }
        }
    }
}