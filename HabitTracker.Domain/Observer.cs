using System;

namespace HabitTracker.Domain
{
    public interface IObserver<T>
    {
        void Handle(T e);
    }

    public interface IObservable<T>
    {
        void Attach(IObserver<T> obs);
        void Broadcast(T e);
    }
}