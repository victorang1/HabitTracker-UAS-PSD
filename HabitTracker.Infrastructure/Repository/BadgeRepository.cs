using System;
using System.Collections.Generic;
using HabitTracker.Infrastructure.Model;
using HabitTracker.Infrastructure.Util;

using Npgsql;
using NpgsqlTypes;

namespace HabitTracker.Infrastructure.Repository
{
    public class BadgeRepository : IBadgeRepository
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        public BadgeRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public IEnumerable<BadgeModel> GetUserBadge(Guid userID)
        {
            List<BadgeModel> userBadges = new List<BadgeModel>();
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
                    while (reader.Read())
                    {
                        BadgeModel badge = new BadgeModel();
                        badge.BadgeID = reader.GetGuid(0);
                        badge.Name = reader.GetString(1);
                        badge.Description = reader.GetString(2);
                        badge.UserID = reader.GetGuid(3);
                        badge.CreatedAt = (DateTime) reader.GetValue(4);
                        userBadges.Add(badge);
                    }
                }
            }
            return userBadges;
        }
    }
}