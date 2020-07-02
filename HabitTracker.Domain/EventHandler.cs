using System;

namespace HabitTracker.Domain
{
    public class LogCreatedHandler : IObserver<LogCreated>
    {
        public void Handle(LogCreated e)
        {

        }
    }

    public class DominatingBadgeAttainedHandler : LogCreatedHandler
    {

    }

    public class WorkaholicBadgeAttainedHandler : LogCreatedHandler
    {

    }

    public class EpicComebackBadgeAttainedHandler : LogCreatedHandler
    {

    }
}