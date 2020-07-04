using System;
using System.Collections.Generic;
using System.Linq;

namespace HabitTracker.Domain.UserAggregate
{
    public class Badge
    {
        public Guid BadgeID { get; private set; }
        public String Name { get; private set; }
        public String Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private List<String> listBadges = new List<String>(new []{ 
            "Dominating",
            "Workaholic",
            "Epic Comeback"});

        public Badge(Guid badgeID, String name, String description)
        {
            this.BadgeID = badgeID;
            this.Name = name;
            this.Description = description;
        }

        public Badge(Guid badgeID, String name, String description, DateTime createdAt)
        {
            if(!IsValid(name)) throw new Exception("Badge Is Invalid");
            this.BadgeID = badgeID;
            this.Name = name;
            this.Description = description;
            this.CreatedAt = createdAt;
        }

        private bool IsValid(string name)
        {
            if(name == null || name.Equals("")) return false;
            foreach(String item in listBadges)
            {
                if(item.Equals(name)) return true;
            }
            return false;
        }
    }
}