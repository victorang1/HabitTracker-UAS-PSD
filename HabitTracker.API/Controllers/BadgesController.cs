using System;
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
  public class BadgesController : ControllerBase
  {
    private readonly ILogger<BadgesController> _logger;
    private BadgeACL dataSource;

    public BadgesController(ILogger<BadgesController> logger)
    {
      _logger = logger;
      dataSource = new BadgeACL(new AppBadgeService());
    }

    [HttpGet("api/v1/users/{userID}/badges")]
    public ActionResult<IEnumerable<Badge>> All(Guid userID)
    {
      List<Badge> badges = dataSource.GetUserBadge(userID);
      if(badges != null && badges.Any())
      {
        return badges;
      }
      return NotFound("This user does not have any badge");
    }
  }
}
