using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using HabitTracker.Infrastructure.Repository;
using HabitTracker.Infrastructure.Model;

namespace HabitTracker.Api.Controllers
{
  [ApiController]
  public class HabitsController : ControllerBase
  {
    private readonly ILogger<HabitsController> _logger;

    public HabitsController(ILogger<HabitsController> logger)
    {
      _logger = logger;
    }

    [HttpGet("api/v1/users/{userID}/habits")]
    public ActionResult<IEnumerable<Habit>> All(Guid userID)
    {
      using(var db = new PostgresUnitOfWork())
      {
        List<Habit> habits = new List<Habit>();
        IEnumerable<HabitModel> userHabits = db.HabitRepository.GetAllUserHabit(userID);
        if(userHabits != null && userHabits.Any()) 
        {
            foreach(HabitModel item in userHabits)
            {
              Habit habit = new Habit();
              habit.ID = item.HabitID;
              habit.Name = item.HabitName;
              habit.DaysOff = item.DaysOff;
              habit.CurrentStreak = item.CurrentStreak;
              habit.LongestStreak = item.LongestStreak;
              habit.LogCount = item.LogCount;
              habit.Logs = item.Logs;
              habit.UserID = item.UserID;
              habit.CreatedAt = item.CreatedAt;
              habits.Add(habit);
            }
            return habits;
        }
      }
      return NotFound("No Habits Found for this User");
    }

    [HttpGet("api/v1/users/{userID}/habits/{id}")]
    public ActionResult<Habit> Get(Guid userID, Guid id)
    {
      using(var db = new PostgresUnitOfWork())
      {
        HabitModel habitModel = db.HabitRepository.GetUserHabit(userID, id);
        if(habitModel != null)
        {
          Habit habit = new Habit();
          habit.ID = habitModel.HabitID;
          habit.Name = habitModel.HabitName;
          habit.DaysOff = habitModel.DaysOff;
          habit.CurrentStreak = habitModel.CurrentStreak;
          habit.LongestStreak = habitModel.LongestStreak;
          habit.LogCount = habitModel.LogCount;
          habit.Logs = habitModel.Logs;
          habit.UserID = habitModel.UserID;
          habit.CreatedAt = habitModel.CreatedAt;
        }
        return NotFound("Habit Id with this userId Not found");
      }
    }

    [HttpPost("api/v1/users/{userID}/habits")]
    public ActionResult<Habit> AddNewHabit(Guid userID, [FromBody] RequestData data)
    {
      using(var db = new PostgresUnitOfWork())
      {
        HabitModel habitModel = db.HabitRepository.AddHabit(userID, data.Name, data.DaysOff);
        if(habitModel != null) {
          Habit habit = new Habit();
          habit.ID = habitModel.HabitID;
          habit.Name = habitModel.HabitName;
          habit.DaysOff = habitModel.DaysOff;
          habit.UserID = habitModel.UserID;
          habit.CreatedAt = habitModel.CreatedAt;
          return habit;
        }
        return NotFound("Add habit fail");
      }
    }

    [HttpPut("api/v1/users/{userID}/habits/{id}")]
    public ActionResult<Habit> UpdateHabit(Guid userID, Guid id, [FromBody] RequestData data)
    {
      using(var db = new PostgresUnitOfWork())
      {
        HabitModel habitModel = db.HabitRepository.UpdateHabit(userID, id, data.Name, data.DaysOff);
        if(habitModel != null) {
          Habit habit = new Habit();
          habit.ID = habitModel.HabitID;
          habit.Name = habitModel.HabitName;
          habit.DaysOff = habitModel.DaysOff;
          habit.CurrentStreak = habitModel.CurrentStreak;
          habit.LongestStreak = habitModel.LongestStreak;
          habit.LogCount = habitModel.LogCount;
          habit.Logs = habitModel.Logs;
          habit.UserID = habitModel.UserID;
          habit.CreatedAt = habitModel.CreatedAt;
          return habit;
        }
        return NotFound("Update habit fail");
      }
    }

    [HttpDelete("api/v1/users/{userID}/habits/{id}")]
    public ActionResult<Habit> DeleteHabit(Guid userID, Guid id)
    {
      using(var db = new PostgresUnitOfWork())
      {
        HabitModel habitModel = db.HabitRepository.DeleteHabit(userID, id);
        if(habitModel != null) {
          Habit habit = new Habit();
          habit.ID = habitModel.HabitID;
          habit.Name = habitModel.HabitName;
          habit.DaysOff = habitModel.DaysOff;
          habit.CurrentStreak = habitModel.CurrentStreak;
          habit.LongestStreak = habitModel.LongestStreak;
          habit.LogCount = habitModel.LogCount;
          habit.Logs = habitModel.Logs;
          habit.UserID = habitModel.UserID;
          habit.CreatedAt = habitModel.CreatedAt;
          return habit;
        }
        return NotFound("habit id not found");
      }
    }

    [HttpPost("api/v1/users/{userID}/habits/{id}/logs")]
    public ActionResult<Habit> Log(Guid userID, Guid id)
    {
      using(var db = new PostgresUnitOfWork())
      {
        HabitModel habitModel = db.HabitRepository.InsertHabitLog(userID, id);
        if(habitModel != null) {
          Habit habit = new Habit();
          habit.ID = habitModel.HabitID;
          habit.Name = habitModel.HabitName;
          habit.DaysOff = habitModel.DaysOff;
          habit.CurrentStreak = habitModel.CurrentStreak;
          habit.LongestStreak = habitModel.LongestStreak;
          habit.LogCount = habitModel.LogCount;
          habit.Logs = habitModel.Logs;
          habit.UserID = habitModel.UserID;
          habit.CreatedAt = habitModel.CreatedAt;
          return habit;
        }
        return NotFound("Failed to insert to this habit");
      }
    }
  }
}
