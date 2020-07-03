using System;
using System.Collections.Generic;

namespace HabitTracker.Infrastructure.Model
{
    public class BadgeModel
    {
        public Guid BadgeID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Guid UserID { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}