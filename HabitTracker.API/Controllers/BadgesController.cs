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
  public class BadgesController : ControllerBase
  {
    private readonly ILogger<BadgesController> _logger;

    public BadgesController(ILogger<BadgesController> logger)
    {
      _logger = logger;
    }

    [HttpGet("api/v1/users/{userID}/badges")]
    public ActionResult<IEnumerable<Badge>> All(Guid userID)
    {
      using(var db = new PostgresUnitOfWork())
      {
        IEnumerable<BadgeModel> items = db.BadgeRepository.GetUserBadge(userID);
        List<Badge> badges = new List<Badge>();
        if(items != null && items.Any()) {
          foreach(BadgeModel item in items)
          {
            Badge badge = new Badge();
            badge.ID = item.BadgeID;
            badge.Name = item.Name;
            badge.Description = item.Description;
            badge.UserID = item.UserID;
            badge.CreatedAt = item.CreatedAt;
            badges.Add(badge);
          }
          return badges;
        }
        return NotFound("This user does not have any badge");
      }
    }
  }
}
