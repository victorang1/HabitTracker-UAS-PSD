using System;
using System.Collections.Generic;
using System.Linq;

using HabitTracker.Domain.HabitAggregate;

using Npgsql;
using NpgsqlTypes;

namespace HabitTracker.Infrastructure.Repository
{
    public class HabitRepository : IHabitRepository
    {
        private NpgsqlConnection _connection;
        private NpgsqlTransaction _transaction;

        public HabitRepository(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public IEnumerable<Habit> GetAllHabit(Guid userID)
        {
            List<Habit> listHabit = new List<Habit>();
            string rawQuery = @"
            SELECT 
                h.habit_id, 
                habit_name, 
                days_off,
                (
                    SELECT coalesce(last_streak, 0) as current_streak FROM habit_logs_snapshot
                    WHERE last_habit_id = h.habit_id
                    AND logs_snapshot_created::date = @currDate::date
                    ORDER BY logs_snapshot_created DESC
                    LIMIT 1
                ),
                (
                    SELECT coalesce(max(last_streak), 0) longest_streak FROM habit_logs_snapshot
                    WHERE last_habit_id = h.habit_id
                ),
                coalesce(COUNT(logs_id), 0) as log_count, 
                string_agg(coalesce(logs_created::varchar, ''), ',') as logs, 
                h.user_id,
                created_at
            FROM habit h
            LEFT JOIN habit_logs hl ON h.habit_id = hl.habit_id
            WHERE h.user_id = @userId
            GROUP BY h.habit_id, habit_name, days_off";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("userId", userID);
                cmd.Parameters.AddWithValue("currDate", DateTime.Now.Date);
                // For manual testing
                // cmd.Parameters.AddWithValue("currDate", "2020-07-11T16:49:28.223996+07:00");
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Int16 currentStreak = (Int16) 0;
                        if(!reader.IsDBNull(3))
                        {
                            currentStreak = reader.GetInt16(3);
                        }
                        Habit habit = HabitFactory.CreateHabit(
                            reader.GetGuid(0),
                            reader.GetString(1),
                            (String[]) reader.GetValue(2),
                            currentStreak,
                            reader.GetInt16(4),
                            reader.GetInt16(5),
                            reader.GetString(6),
                            reader.GetGuid(7),
                            (DateTime) reader.GetValue(8)
                        );
                        listHabit.Add(habit);
                    }
                }
            }
            return listHabit;
        }
        public Habit GetHabit(Guid userID, Guid habitID)
        {
            Habit habit = null;
            string rawQuery = @"
            SELECT 
                h.habit_id, 
                habit_name, 
                days_off,
                (
                    SELECT coalesce(last_streak, 0) as current_streak FROM habit_logs_snapshot
                    WHERE last_habit_id = h.habit_id
                    AND logs_snapshot_created::date = @currDate::date
                    ORDER BY logs_snapshot_created DESC
                    LIMIT 1
                ),
                (
                    SELECT coalesce(max(last_streak), 0) longest_streak FROM habit_logs_snapshot
                    WHERE last_habit_id = h.habit_id
                ),
                coalesce(COUNT(logs_id), 0) as log_count, 
                string_agg(coalesce(logs_created::varchar, ''), ',') as logs, 
                h.user_id,
                created_at
            FROM habit h
            LEFT JOIN habit_logs hl ON h.habit_id = hl.habit_id
            WHERE h.user_id = @userId AND h.habit_id = @habitId
            GROUP BY h.habit_id, habit_name, days_off";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("userId", userID);
                cmd.Parameters.AddWithValue("habitId", habitID);
                cmd.Parameters.AddWithValue("currDate", DateTime.Now.Date);
                // For manual testing
                // cmd.Parameters.AddWithValue("currDate", "2020-07-11T16:49:28.223996+07:00");
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Int16 currentStreak = (Int16) 0;
                        if(!reader.IsDBNull(3))
                        {
                            currentStreak = reader.GetInt16(3);
                        }
                        habit = HabitFactory.CreateHabit(
                            reader.GetGuid(0),
                            reader.GetString(1),
                            (String[]) reader.GetValue(2),
                            currentStreak,
                            reader.GetInt16(4),
                            reader.GetInt16(5),
                            reader.GetString(6),
                            reader.GetGuid(7),
                            (DateTime) reader.GetValue(8)
                        );
                    }
                }
            }
            return habit;
        }
        public Habit AddHabit(Guid userID, String habitName, String[] daysOff) {
            string rawQuery = 
                @"INSERT INTO habit VALUES (@habitId, @habitName, @daysOff, @userId, @createdAt)";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                try {
                    Guid habitID = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("habitId", habitID);
                    cmd.Parameters.AddWithValue("habitName", habitName);
                    cmd.Parameters.AddWithValue("daysOff", daysOff);
                    cmd.Parameters.AddWithValue("userId", userID);
                    cmd.Parameters.AddWithValue("createdAt", DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    return GetHabit(userID, habitID);
                } catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return null;
        }
        public Habit UpdateHabit(Guid userID, Guid habitID, String habitName, String[] daysOff) {
            string rawQuery = 
                @"UPDATE habit SET habit_name = @habitName, days_off = @daysOff
                    WHERE habit_id = @habitId AND user_id = @userId";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                try {
                    cmd.Parameters.AddWithValue("habitId", habitID);
                    cmd.Parameters.AddWithValue("habitName", habitName);
                    cmd.Parameters.AddWithValue("daysOff", daysOff);
                    cmd.Parameters.AddWithValue("userId", userID);
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                } catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return GetHabit(userID, habitID);
        }

        public Habit DeleteHabit(Guid userID, Guid habitID) {
            Habit deletedHabit = null;
            string rawQuery = @"DELETE FROM habit WHERE user_id = @userId AND habit_id = @habitId";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                try {
                    cmd.Parameters.AddWithValue("habitId", habitID);
                    cmd.Parameters.AddWithValue("userId", userID);
                    deletedHabit = GetHabit(userID, habitID);
                    if(deletedHabit == null) return deletedHabit;
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    return deletedHabit;
                } catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return deletedHabit;
        }

        public Habit InsertHabitLog(Guid userID, Guid habitID, DateTime currentDate, Boolean isHoliday)
        {
            string rawQuery = 
                @"INSERT INTO habit_logs VALUES (@logsId, @habitId, @userId, @createdAt, @isHoliday)";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                try {
                    cmd.Parameters.AddWithValue("logsId", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("habitId", habitID);
                    cmd.Parameters.AddWithValue("userId", userID);
                    cmd.Parameters.AddWithValue("createdAt", currentDate);
                    cmd.Parameters.AddWithValue("isHoliday", isHoliday);
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                } catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            return GetHabit(userID, habitID);
        }
        
        private List<DateTime> getLogs(String tempLogs)
        {
            List<String> result = tempLogs.Split(',').ToList();
            List<DateTime> logs = new List<DateTime>();
            foreach(String item in result) {
                if(item == null || item.Equals("")) continue;
                logs.Add(DateTime.Parse(item));
            }
            return logs;
        }

        public String GetLastHabitSnapshot(Guid userID, Guid habitID)
        {
            String lastHabitSnapshotDateTime = "";
            String maxQuery = @"SELECT logs_snapshot_created FROM habit_logs_snapshot
                    WHERE last_habit_id = @habitId
                    ORDER BY logs_snapshot_created DESC
                    LIMIT 1";
            using (var cmd = new NpgsqlCommand(maxQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("habitId", habitID);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lastHabitSnapshotDateTime = ((DateTime) reader.GetValue(0)).ToString();
                    }
                }
            }
            return lastHabitSnapshotDateTime;
        }

        public Int16 GetHabitCurrentStreak(Guid userID, Guid habitID)
        {
            Int16 currentStreak = (Int16) 0;
            String maxQuery = @"SELECT coalesce(last_streak, 0) FROM habit_logs_snapshot
                    WHERE last_user_id = @userId AND last_habit_id = @habitId 
                    ORDER BY logs_snapshot_created DESC
                    LIMIT 1";
            using (var cmd = new NpgsqlCommand(maxQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("userId", userID);
                cmd.Parameters.AddWithValue("habitId", habitID);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if(!reader.IsDBNull(0))
                        {
                            currentStreak = (Int16) reader.GetInt16(0);
                        }
                    }
                }
            }
            return currentStreak;
        }
        
        public void InsertHabitLogSnapshot(Guid userID, Guid habitID, Int16 streak, DateTime currentDate)
        {
            string rawQuery = 
                @"INSERT INTO habit_logs_snapshot VALUES (
                    @logsSnapshotId
                    , @habitId
                    , @userId
                    , @streak
                    , @createdAt
                )";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                try {
                    cmd.Parameters.AddWithValue("logsSnapshotId", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("habitId", habitID);
                    cmd.Parameters.AddWithValue("userId", userID);
                    cmd.Parameters.AddWithValue("streak", streak);
                    cmd.Parameters.AddWithValue("createdAt", currentDate);
                    cmd.ExecuteNonQuery();
                } 
                catch(Exception e)
                {
                    Console.WriteLine("Insert snapshot error:" + e.Message);
                    throw new Exception("Insert snapshot failed");
                }
            }
        }

        public Int16 GetTotalLogOnHolidays(Guid userID)
        {
            Int16 total = (Int16) 0;
            String rawQuery = @"SELECT coalesce(COUNT(isHoliday), 0) FROM habit_logs
            WHERE isHoliday = true AND user_id = @userId";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("userId", userID);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        total = reader.GetInt16(0);
                    }
                }
            }
            return total;
        }

        public DateTime GetFirstFromTenStreakDay(Guid userID, Guid habitID)
        {
            DateTime secondStreakDay = new DateTime();

            String query = @"SELECT logs_snapshot_created FROM habit_logs_snapshot 
                WHERE last_habit_id = @habitId AND last_user_id = @userId
                ORDER BY logs_snapshot_created DESC
                LIMIT 9";
            using (var cmd = new NpgsqlCommand(query, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("userId", userID);
                cmd.Parameters.AddWithValue("habitId", habitID);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        secondStreakDay = (DateTime) reader.GetValue(0);
                    }
                }
            }

            DateTime firstStreakDay = new DateTime();
            String firstQuery = @"SELECT logs_created FROM habit_logs
                WHERE logs_created::date < @currDate::date
                AND habit_id = @habitId
                ORDER BY logs_created DESC
                LIMIT 1";
            using (var cmd = new NpgsqlCommand(firstQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("habitId", habitID);
                cmd.Parameters.AddWithValue("userId", userID);
                cmd.Parameters.AddWithValue("currDate", secondStreakDay);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        firstStreakDay = (DateTime) reader.GetValue(0);
                    }
                }
            }
            return firstStreakDay;
        }

        public String GetLastDayBeforeTenStreak(Guid userID, Guid habitID, DateTime firstStreakDay)
        {
            String lastDayBeforeStreak = String.Empty;
            String firstQuery = @"SELECT logs_created FROM habit_logs
                WHERE logs_created::date < @currDate::date
                AND habit_id = @habitId
                ORDER BY logs_created DESC
                LIMIT 1";
            using (var cmd = new NpgsqlCommand(firstQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("habitId", habitID);
                cmd.Parameters.AddWithValue("userId", userID);
                cmd.Parameters.AddWithValue("currDate", firstStreakDay);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        lastDayBeforeStreak = ((DateTime) reader.GetValue(0)).ToString();
                    }
                }
            }
            return lastDayBeforeStreak;
        }
    }
}