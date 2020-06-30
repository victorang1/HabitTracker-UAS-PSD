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
        foreach(HabitModel item in userHabits) {
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

      return NotFound("user not found");
    }

    [HttpGet("api/v1/users/{userID}/habits/{id}")]
    public ActionResult<Habit> Get(Guid userID, Guid id)
    {
      using(var db = new PostgresUnitOfWork())
      {
        HabitModel habitModel = db.HabitRepository.GetUserHabit(userID, id);
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
      //mock only. replace with your solution
      return new Habit
      {
        UserID = userID,
        ID = id,
      };
    }

    [HttpPost("api/v1/users/{userID}/habits/{id}/logs")]
    public ActionResult<Habit> Log(Guid userID, Guid id)
    {
      //mock only. replace with your solution
      return new Habit
      {
        UserID = userID,
        ID = id,
        Logs = new[] { DateTime.Now },
        CurrentStreak = 1,
        LongestStreak = 1
      };
    }
    
    //mock data only. remove later
    private static readonly Guid AmirID = Guid.Parse("4fbb54f1-f340-441e-9e57-892329464d56");
    private static readonly Guid BudiID = Guid.Parse("0b54c1fe-a374-4df8-ba9a-0aa7744a4531");

    //mock data only. remove later
    private static readonly Habit habitAmir1 = new Habit
    {
      ID = Guid.Parse("fd725b05-a221-461a-973c-4a0899cee14d"),
      Name = "baca buku",
      UserID = AmirID
    };

    //mock data only. remove later
    private static readonly Habit habitAmir2 = new Habit
    {
      ID = Guid.Parse("01169031-752e-4c52-822c-a04d290438ea"),
      Name = "code one simple app prototype",
      DaysOff = new[] { "Sat", "Sun" },
      UserID = AmirID
    };

    //mock data only. remove later
    private static readonly Habit habitBudi1 = new Habit
    {
      ID = Guid.Parse("05fb5a61-aa1f-4a96-b952-378bf73ca713"),
      Name = "100 push-ups, 100 sit-ups, 100 squats",
      LongestStreak = 100,
      CurrentStreak = 10,
      LogCount = 123,
      UserID = BudiID
    };
  }
}
