using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
      //mock only. replace with your solution
      return new[] {
        new Badge {
          ID = Guid.NewGuid(),
          Name = "Dominating",
          Description = "4+ streak",
          CreatedAt = DateTime.Now.AddDays(-13),
        },
        new Badge {
          ID = Guid.NewGuid(),
          Name = "Epic Comeback",
          Description = "10 streak after 10 days without logging",
          CreatedAt = DateTime.Now.AddDays(-7),
        }
      };
    }
  }
}
