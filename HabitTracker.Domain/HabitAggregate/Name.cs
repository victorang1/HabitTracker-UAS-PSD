using System;

namespace HabitTracker.Domain.HabitAggregate
{
    public class Name
    {
        public String HabitName { get; private set; }

        public Name(String name)
        {
            if(IsNullOrEmpty(name)) throw new Exception("Name cannot be empty");
            if(IsHabitNameInvalid(name)) throw new Exception("Habit name must be 2 to 100 characters");
            this.HabitName = name;
        }

        private Boolean IsNullOrEmpty(String name)
        {
            if(name == null) return true;
            if(name.Equals("")) return true;
            return false;
        }

        private Boolean IsHabitNameInvalid(String name)
        {
            if(name.Length < 2 || name.Length > 100) return true;
            return false;
        }
    }
}