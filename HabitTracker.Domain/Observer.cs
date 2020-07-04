using System;

namespace HabitTracker.Domain
{
    public interface IObserver<T>
    {
        void Handle(T evnt);
    }

    public interface IObservable<T>
    {
        void Attach(IObserver<T> obs);
        void Broadcast(T evnt);
    }
}