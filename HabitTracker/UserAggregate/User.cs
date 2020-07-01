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
    }
}