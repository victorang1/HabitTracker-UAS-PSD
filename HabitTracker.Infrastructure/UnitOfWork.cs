using System;

namespace HabitTracker.Infrastructure
{
    public interface UnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}