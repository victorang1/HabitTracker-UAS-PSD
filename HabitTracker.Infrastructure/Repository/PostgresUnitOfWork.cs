using System;

using HabitTracker.Domain.HabitAggregate;
using HabitTracker.Domain.UserAggregate;

using Npgsql;
using NpgsqlTypes;

namespace HabitTracker.Infrastructure.Repository
{
    public class PostgresUnitOfWork : UnitOfWork
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        private IHabitRepository _habitRepository;
        private IUserRepository _userRepository;

        public PostgresUnitOfWork()
        {
            _connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=postgres;Database=habit_tracker_db;Port=5436");
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }
        
        public IHabitRepository HabitRepository
        {
            get {
                if(_habitRepository == null) {
                    _habitRepository = new HabitRepository(_connection, _transaction);
                }
                return _habitRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get {
                if(_userRepository == null) {
                    _userRepository = new UserRepository(_connection, _transaction);
                }
                return _userRepository;
            }
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _connection.Close();
                }
                disposed = true;
            }
        }
    }
}