using System;

namespace HabitTracker.Domain
{
    public class LogCreated
    {
        public Guid UserID { get; private set; }
        public Guid HabitID { get; private set; }

        public LogCreated(Guid userID, Guid habitID)
        {
            this.UserID = userID;
            this.HabitID = habitID;
        }
    }
}