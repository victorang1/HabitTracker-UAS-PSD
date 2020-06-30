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

        public IEnumerable<HabitModel> GetAllUserHabit(Guid userID)
        {
            List<HabitModel> listHabit = new List<HabitModel>();
            string rawQuery = @"
            SELECT 
                h.habit_id, 
                habit_name, 
                days_off,
                coalesce(COUNT(logs_id), 0) as log_count, 
                string_agg(coalesce(logs_created::varchar, ''), ',') as logs, 
                h.user_id
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
                model.CurrentStreak = getCurrentStreak(model.HabitID);
                model.LongestStreak = getLongestStreak(model.HabitID);
            }
            return listHabit;
        }
        public HabitModel GetUserHabit(Guid userID, Guid habitID)
        {
            HabitModel habit = new HabitModel();
            string rawQuery = @"
            SELECT 
                h.habit_id, 
                habit_name, 
                days_off,
                coalesce(COUNT(logs_id), 0) as log_count, 
                string_agg(coalesce(logs_created::varchar, ''), ',') as logs, 
                h.user_id
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
                habit.CurrentStreak = getCurrentStreak(habit.HabitID);
                habit.LongestStreak = getLongestStreak(habit.HabitID);
            }
            return habit;
        }
        public Habit AddHabit(String habitName, IEnumerable<String> daysOff) {
            string rawQuery = @"
                INSERT INTO habit VALUES (
                    @habitId,
                    @habitName,
                    @daysOff,
                    @createdDate
                )
            ";
            using (var cmd = new NpgsqlCommand(rawQuery, _connection, _transaction))
            {
                cmd.Parameters.AddWithValue("habitId", Guid.NewGuid());
                cmd.Parameters.AddWithValue("habitName", habitName);
                cmd.Parameters.AddWithValue("daysOff", daysOff);
                cmd.Parameters.AddWithValue("createdDate", DateUtil.GetServerDateTimeFormat());
                cmd.ExecuteNonQuery();
            }
        }
        // public Habit UpdateHabit(String habitName, IEnumerable<String> daysOff) {

        // }
        // public Habit DeleteHabit() {

        // }

        private HabitModel bindHabitData(NpgsqlDataReader reader)
        {
            HabitModel habit = new HabitModel();
            habit.HabitID = reader.GetGuid(0);
            habit.HabitName = reader.GetString(1);
            habit.DaysOff = (String[]) reader.GetValue(2);
            habit.LogCount = reader.GetInt16(3);
            habit.Logs = getLogs(reader.GetString(4));
            habit.UserID = reader.GetGuid(5);
            return habit;
        }

        private Int16 getCurrentStreak(Guid habitID)
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
    }
}