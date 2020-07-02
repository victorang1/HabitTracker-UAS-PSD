﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using HabitTracker.API.AntiCorruption;
using HabitTracker.API.Services;

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
      HabitACL ds = new HabitACL(new HabitService());
      List<Habit> habits = ds.GetAllUserHabits(userID);
      if(habits != null && habits.Any())
      {
        return habits;
      }
      return NotFound("No Habits Found for this User");
    }

    [HttpGet("api/v1/users/{userID}/habits/{id}")]
    public ActionResult<Habit> Get(Guid userID, Guid id)
    {
      HabitACL ds = new HabitACL(new HabitService());
      Habit habit = ds.GetHabitByID(userID, id);
      if(habit != null)
      {
        return habit;
      }
      return NotFound("Habit Id with this userId Not found");
    }

    [HttpPost("api/v1/users/{userID}/habits")]
    public ActionResult<Habit> AddNewHabit(Guid userID, [FromBody] RequestData data)
    {
      try
      {
        HabitACL ds = new HabitACL(new HabitService());
        Habit habit = ds.AddHabit(userID, data.Name, data.DaysOff);
        if(habit != null)
        {
          return habit;
        }
        return NotFound("Add habit fail");
      }
      catch(Exception e)
      {
        return NotFound(e.Message);
      }
    }

    [HttpPut("api/v1/users/{userID}/habits/{id}")]
    public ActionResult<Habit> UpdateHabit(Guid userID, Guid id, [FromBody] RequestData data)
    {
      try
      {
        HabitACL ds = new HabitACL(new HabitService());
        Habit habit = ds.UpdateHabit(userID, id, data.Name, data.DaysOff);
        if(habit != null)
        {
          return habit;
        }
        return NotFound("Update habit fail");
      }
      catch (Exception e)
      {
        return NotFound(e.Message);
      }
    }

    [HttpDelete("api/v1/users/{userID}/habits/{id}")]
    public ActionResult<Habit> DeleteHabit(Guid userID, Guid id)
    {
      HabitACL ds = new HabitACL(new HabitService());
      Habit habit = ds.DeleteHabit(userID, id);
      if(habit != null)
      {
        return habit;
      }
      return NotFound("habit id not found");
    }

    [HttpPost("api/v1/users/{userID}/habits/{id}/logs")]
    public ActionResult<Habit> Log(Guid userID, Guid id)
    {
      HabitACL ds = new HabitACL(new HabitService());
      Habit habit = ds.InsertLog(userID, id);
      if(habit != null)
      {
        return habit;
      }
      return NotFound("Failed to insert log to this habit");
    }
  }
}
