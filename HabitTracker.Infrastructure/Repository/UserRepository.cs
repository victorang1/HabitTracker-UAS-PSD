using System;
using System.Collections.Generic;

using HabitTracker.Domain.UserAggregate;

using Npgsql;
using NpgsqlTypes;

namespace HabitTracker.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        public UserRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public User GetUserBadge(Guid userID)
        {
            User user = null;
            List<Badge> userBadges = new List<Badge>();
            string rawQuery = @"
                SELECT b.badge_id, badge_name, badge_description, user_id, bu.created_at
                FROM badge_user bu
                JOIN badge b ON b.badge_id = bu.badge_id
                WHERE user_id = @userId";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("userId", userID);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        user = new User(reader.GetGuid(0), reader.GetString(1));
                    }
                    while (reader.Read())
                    {
                        Badge badge = new Badge(reader.GetGuid(2), reader.GetString(3), reader.GetString(4), (DateTime) reader.GetValue(5));
                        userBadges.Add(badge);
                    }
                    user = user.AddBadges(userBadges);
                }
            }
            return user;
        }
    }
}