using System;

namespace HabitTracker
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

    public class DominatingBadgeAttained : LogCreated
    {
        public DominatingBadgeAttained(Guid userID, Guid habitID) : base(userID, habitID) {}
    }

    public class WorkaholicBadgeAttained : LogCreated
    {
        public WorkaholicBadgeAttained(Guid userID, Guid habitID) : base(userID, habitID) {}
    }

    public class EpicComebackBadgeAttained : LogCreated
    {
        public EpicComebackBadgeAttained(Guid userID, Guid habitID) : base(userID, habitID) {}
    }
}