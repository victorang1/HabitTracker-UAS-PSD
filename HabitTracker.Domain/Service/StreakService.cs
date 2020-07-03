using System;
using System.Collections.Generic;
using System.Linq;
using HabitTracker.Domain.HabitAggregate;

namespace HabitTracker.Domain.Service
{
    public class StreakCalculationService : IStreakCalculationService
    {
        private IHabitRepository _habitRepository;

        public StreakCalculationService(IHabitRepository habitRepository)
        {
            _habitRepository = habitRepository;
        }

        public void InsertLogForThisHabit(Guid userID, Guid habitID, DateTime currentDate)
        {
            Habit habitData = _habitRepository.GetHabit(userID, habitID);
            if(IsDifferenceDayIsOne(currentDate, GetLastLogs(habitData.Logs)))
            {
                String strLastHabitSnapshotDT = _habitRepository.GetLastHabitSnapshot(userID, habitID);
                if(!strLastHabitSnapshotDT.Equals("") && IsDifferenceDayIsOne(currentDate, DateTime.Parse(strLastHabitSnapshotDT)))
                {
                    Int16 streakToInput = (Int16) (habitData.CurrentStreak + 1);
                    _habitRepository.InsertHabitLogSnapshot(userID, habitID,  streakToInput);
                }
                else _habitRepository.InsertHabitLogSnapshot(userID, habitID, 2);
            }
            _habitRepository.InsertHabitLog(userID, habitID);
        }

        private DateTime GetLastLogs(IEnumerable<DateTime> logs)
        {
            return logs.ElementAt(logs.Count()-1);
        }

        private static Boolean IsDifferenceDayIsOne(DateTime yesterdayDate, DateTime currentDate)
        {
            return currentDate - yesterdayDate == TimeSpan.FromDays(1);
        }

        private static Boolean IsSameDay(DateTime dateOne, DateTime dateTwo)
        {
            return dateOne.Date == dateTwo.Date;
        }
    }
}