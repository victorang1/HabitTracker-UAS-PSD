using System;
using System.Collections.Generic;
using System.Linq;

namespace HabitTracker.UserAggregate
{
    public class Badge
    {
        public String _name;
        public String _description;

        private String[] listBadges = new string[] {
            "Dominating",
            "Workaholic",
            "Epic Comeback"
        };

        public String Name
        {
            get
            {
                return _name;
            }
        }

        public String _description
        {
            get
            {
                return _description;
            }
        }

        public Badge(String name, String description)
        {
            if(!IsValid(name)) throw new Exception("Badge Is Invalid");
            this._name = name;
            this._description = description;
        }

        private bool IsValid(string name)
        {
            if(name == null || name.Equals("")) return false;
            if(listBadges.Where(val => val.Contains(name))) return true;
            return false;
        }
    }
}