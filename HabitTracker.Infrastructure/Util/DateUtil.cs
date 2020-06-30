using System;

namespace HabitTracker.Infrastructure.Util
{
    public class DateUtil
    {
        public static String GetServerDateTimeFormat()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        } 
    }
}