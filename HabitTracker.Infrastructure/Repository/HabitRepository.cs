using System;
using System.Collections.Generic;
using HabitTracker.Infrastructure.Model;
using HabitTracker.Infrastructure.Util;
using System.Linq;

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

        public IEnumerable<HabitModel> GetAllHabit(Guid userID)
        {
            List<HabitModel> listHabit = new List<HabitModel>();
            string rawQuery = @"
            SELECT 
                h.habit_id, 
                habit_name, 
                days_off,
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
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listHabit.Add(bindHabitData(reader));
                    }
                }
            }
            foreach(HabitModel model in listHabit)
            {
                model.CurrentStreak = GetCurrentStreak(model.HabitID);
                model.LongestStreak = getLongestStreak(model.HabitID);
            }
            return listHabit;
        }
        public HabitModel GetHabit(Guid userID, Guid habitID)
        {
            HabitModel habit = null;
            string rawQuery = @"
            SELECT 
                h.habit_id, 
                habit_name, 
                days_off,
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
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        habit = bindHabitData(reader);
                    }
                }
            }
            if(habit != null) {
                habit.CurrentStreak = GetCurrentStreak(habit.HabitID);
                habit.LongestStreak = getLongestStreak(habit.HabitID);
            }
            return habit;
        }
        public HabitModel AddHabit(Guid userID, String habitName, String[] daysOff) {
            string rawQuery = 
                @"INSERT INTO habit VALUES (@habitId, @habitName, @daysOff, @userId, @createdAt)";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                try {
                    // String[] arrDaysOff = daysOff != null 
                    //     ? arrDaysOff = daysOff.Select(i => i).ToArray() : new String[]{};
                    Guid habitID = Guid.NewGuid();
                    cmd.Parameters.AddWithValue("habitId", habitID);
                    cmd.Parameters.AddWithValue("habitName", habitName);
                    cmd.Parameters.AddWithValue("daysOff", daysOff);
                    cmd.Parameters.AddWithValue("userId", userID);
                    cmd.Parameters.AddWithValue("createdAt", DateTime.Parse(DateUtil.GetServerDateTimeFormat()));
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
        public HabitModel UpdateHabit(Guid userID, Guid habitID, String habitName, String[] daysOff) {
            string rawQuery = 
                @"UPDATE habit SET habit_name = @habitName, days_off = @daysOff
                    WHERE habit_id = @habitId AND user_id = @userId";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                try {
                    // String[] arrDaysOff = daysOff != null 
                    //     ? arrDaysOff = daysOff.Select(i => i).ToArray() : new String[]{};
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

        public HabitModel DeleteHabit(Guid userID, Guid habitID) {
            HabitModel deletedHabit = null;
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

        public HabitModel InsertHabitLog(Guid userID, Guid habitID)
        {
            string rawQuery = 
                @"INSERT INTO habit_logs VALUES (@logsId, @habitId, @userId)";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                try {
                    cmd.Parameters.AddWithValue("logsId", Guid.NewGuid());
                    cmd.Parameters.AddWithValue("habitId", habitID);
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

        private HabitModel bindHabitData(NpgsqlDataReader reader)
        {
            HabitModel habit = new HabitModel();
            habit.HabitID = reader.GetGuid(0);
            habit.HabitName = reader.GetString(1);
            habit.DaysOff = (String[]) reader.GetValue(2);
            habit.LogCount = reader.GetInt16(3);
            habit.Logs = getLogs(reader.GetString(4));
            habit.UserID = reader.GetGuid(5);
            habit.CreatedAt = (DateTime) reader.GetValue(6);
            return habit;
        }

        public Int16 GetCurrentStreak(Guid habitID)
        {
            Int16 currentStreak = 0;
            String rawQuery = @"
                    SELECT coalesce(last_streak, 0) FROM habit_logs_snapshot
                    WHERE last_habit_id = @habitId
                    AND logs_snapshot_created::date = @currDate::date
                    ORDER BY logs_snapshot_created DESC
                    LIMIT 1";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("habitId", habitID);
                cmd.Parameters.AddWithValue("currDate", DateUtil.GetServerDateTimeFormat());
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        currentStreak = reader.GetInt16(0);
                    }
                }
            }
            return currentStreak;
        }
        private Int16 getLongestStreak(Guid habitID)
        {
            Int16 longestStreak = 0;
            String maxQuery = @"SELECT coalesce(max(last_streak), 0) FROM habit_logs_snapshot
                    WHERE last_habit_id = @habitId";
            using (var cmd = new NpgsqlCommand(maxQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("habitId", habitID);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        longestStreak = reader.GetInt16(0);
                    }
                }
            }
            return longestStreak;
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
                    WHERE last_habit_id = @habitId LIMIT 1";
            using (var cmd = new NpgsqlCommand(maxQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("habitId", habitID);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lastHabitSnapshotDateTime = (String) reader.GetValue(0);
                    }
                }
            }
            return lastHabitSnapshotDateTime;
        }
        
        public void InsertHabitLogSnapshot(Guid userID, Guid habitID, Int16 streak)
        {
            
        }
    }
}