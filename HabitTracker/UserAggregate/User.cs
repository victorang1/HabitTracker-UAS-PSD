using System;
using System.Collections.Generic;

namespace HabitTracker.UserAggregate
{
    public class User
    {
        public Guid UserID { get; private set; }
        public String Username { get; private set; }
        public IEnumerable<Badge> Badges { get; private set; }

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