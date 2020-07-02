using System;
using System.Collections.Generic;
using System.Linq;

namespace HabitTracker.Domain.UserAggregate
{
    public class Badge
    {
        public String _name;
        public String _description;

        private List<String> listBadges = new List<String>(new []{ 
            "Dominating",
            "Workaholic",
            "Epic Comeback"});

        public String Name
        {
            get
            {
                return _name;
            }
        }

        public String Description
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
            foreach(String item in listBadges)
            {
                if(item.Equals(name)) return true;
            }
            return false;
        }
    }
}