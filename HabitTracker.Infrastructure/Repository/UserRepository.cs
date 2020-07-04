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
            User user = new User();
            List<Badge> userBadges = new List<Badge>();

            string userQuery = "SELECT * FROM \"user\" WHERE user_id = @userId";

            using (var cmd = new NpgsqlCommand(userQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("userId", userID);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User(reader.GetGuid(0), reader.GetString(1));
                    }
                }
            }
            
            string rawQuery = @"
                SELECT b.badge_id, badge_name, badge_description, bu.created_at
                FROM badge_user bu
                JOIN badge b ON b.badge_id = bu.badge_id
                WHERE user_id = @userId";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("userId", userID);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    Guid userId = Guid.Empty;
                    while (reader.Read())
                    {
                        Badge badge = new Badge(reader.GetGuid(0), reader.GetString(1), reader.GetString(2), (DateTime) reader.GetValue(3));
                        userBadges.Add(badge);
                    }
                    if(userBadges != null && userBadges.Count != 0) user = user.AddBadges(userBadges);
                }
            }
            return user;
        }

        public List<Badge> GetBadges(String badgeName)
        {
            List<Badge> listBadges = new List<Badge>();
            string rawQuery = "SELECT * FROM Badge WHERE badge_name = @badgeName";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("badgeName", badgeName);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Badge badge = new Badge(reader.GetGuid(2), reader.GetString(3), reader.GetString(4), (DateTime) reader.GetValue(5));
                        listBadges.Add(badge);
                    }
                }
            }
            return listBadges;
        }

        public Guid GetBadgeID(String name)
        {
            Guid badgeId = Guid.Empty;
            String rawQuery = @"SELECT badge_id FROM badge WHERE badge_name = @badgeName";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("badgeName", name);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        badgeId = reader.GetGuid(0);
                    }
                }
            }
            return badgeId;
        }

        public void InsertBadge(Guid badgeID, Guid userID)
        {
            Console.WriteLine("Test");
            String rawQuery = @"INSERT INTO badge_user VALUES (@badgeId, @userId)";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                try {
                    cmd.Parameters.AddWithValue("badgeId", badgeID);
                    cmd.Parameters.AddWithValue("userId", userID);
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                } catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}