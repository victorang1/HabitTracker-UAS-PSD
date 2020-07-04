using System;
using System.Collections.Generic;
using System.Linq;
using HabitTracker.Domain.HabitAggregate;
using HabitTracker.Domain.UserAggregate;
using HabitTracker.Domain.Event;

namespace HabitTracker.Domain.Service
{
    public class StreakCalculationService : IStreakCalculationService, IObservable<LogCreated>
    {
        private IHabitRepository _habitRepository;
        private IUserRepository _userRepository;

        private IHabitService _habitService;
        private IBadgeService _badgeService;
        protected List<IObserver<LogCreated>> _observers = new List<IObserver<LogCreated>>();

        public StreakCalculationService(IHabitRepository habitRepository, IUserRepository userRepository)
        {
            _habitRepository = habitRepository;
            _userRepository = userRepository;
            _habitService = new HabitService(_habitRepository);
            _badgeService = new BadgeService(_userRepository);
        }

        public Habit InsertHabitLog(Guid userID, Guid habitID, DateTime currentDate)
        {
            Attach(new LogCreatedHandler(_habitService, _badgeService));
            try 
            {
                Habit habit = InsertLogForThisHabit(userID, habitID, currentDate);
                Broadcast(new LogCreated(userID, habitID));
                return habit;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public Habit InsertLogForThisHabit(Guid userID, Guid habitID, DateTime currentDate)
        {
            Habit habitData = _habitRepository.GetHabit(userID, habitID);
            String[] daysOff = habitData.DaysOff.daysOff;
            String strLastLog = getLastLogs(habitData.Logs);
            if(strLastLog != null && !strLastLog.Equals(""))
            {
                DateTime lastLog = DateTime.Parse(strLastLog);
                if(checkValid(currentDate, lastLog, daysOff) && !isSameDay(currentDate, lastLog))
                {
                    String strLastHabitSnapshotDT = _habitRepository.GetLastHabitSnapshot(userID, habitID);
                    Int16 currentStreak = _habitRepository.GetHabitCurrentStreak(userID, habitID);
                    if(!strLastHabitSnapshotDT.Equals("") 
                        && checkValid(currentDate, DateTime.Parse(strLastHabitSnapshotDT), daysOff))
                    {
                        Int16 streakToInput = (Int16) (currentStreak + 1);
                        _habitRepository.InsertHabitLogSnapshot(userID, habitID, streakToInput, currentDate);
                    }
                    else {
                        _habitRepository.InsertHabitLogSnapshot(userID, habitID, 2, currentDate);
                    }
                }
            }
            Boolean isHoliday = inHolidays(currentDate, daysOff);
            _habitRepository.InsertHabitLog(userID, habitID, currentDate, isHoliday);
            Habit latestHabitData = _habitRepository.GetHabit(userID, habitID);
            return latestHabitData;
        }

        public void Attach(IObserver<LogCreated> obs)
        {
            _observers.Add(obs);
        }

        public void Broadcast(LogCreated evnt)
        {
            foreach (var obs in _observers)
            {
                obs.Handle(evnt);
            }
        }

        private Boolean isStreakStillValid(DateTime currentDate, String strLastLog, String[] daysOff)
        {
            if(strLastLog == null || strLastLog.Equals("")) return false;
            else
            {
                DateTime lastLog = DateTime.Parse(strLastLog);
                if(lastLog > currentDate) {
                    throw new Exception("Last habit log is greater than currentDate");
                }
                if((currentDate - lastLog).TotalDays > 7) {
                    return false;
                }
                for(var i = currentDate.Date.AddDays(-1); i > lastLog.Date;  i = i.AddDays(-1))
                {
                    if(!inHolidays(i, daysOff))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private Boolean checkValid(DateTime currentDate, DateTime lastLog, String[] daysOff)
        {
            return (isDifferenceDayIsOne(lastLog, currentDate)
                    || isStreakStillValid(currentDate, lastLog.ToString(), daysOff));
        }

        private Boolean inHolidays(DateTime currentDate, String[] daysOff)
        {
            foreach(String str in daysOff)
            {
                if(currentDate.ToString("ddd").Equals(str))
                {
                    return true;
                }
            }
            return false;
        }

        private String getLastLogs(IEnumerable<DateTime> logs)
        {
            if(logs == null || !logs.Any()) return "";
            return logs.ElementAt(logs.Count()-1).ToString();
        }

        private static Boolean isDifferenceDayIsOne(DateTime yesterdayDate, DateTime currentDate)
        {
            return currentDate.Date - yesterdayDate.Date == TimeSpan.FromDays(1);
        }

        private static Boolean isSameDay(DateTime dateOne, DateTime dateTwo)
        {
            return dateOne.Date == dateTwo.Date;
        }
    }
}