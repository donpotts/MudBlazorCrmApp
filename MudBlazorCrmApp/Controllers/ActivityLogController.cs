using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MudBlazorCrmApp.Data;
using MudBlazorCrmApp.Shared.Models;

namespace MudBlazorCrmApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Fixed")]
public class ActivityLogController(ApplicationDbContext ctx, ILogger<ActivityLogController> logger) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IQueryable<ActivityLog>> Get()
    {
        return Ok(ctx.ActivityLog.OrderByDescending(x => x.Timestamp));
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ActivityLog>> GetAsync(long key)
    {
        var activityLog = await ctx.ActivityLog.FindAsync(key);
        return activityLog == null ? NotFound() : Ok(activityLog);
    }

    [HttpGet("entity/{entityType}/{entityId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ActivityLog>>> GetByEntity(string entityType, long entityId)
    {
        var activities = await ctx.ActivityLog
            .Where(a => a.EntityType == entityType && a.EntityId == entityId)
            .OrderByDescending(a => a.Timestamp)
            .Take(50)
            .ToListAsync();

        return Ok(activities);
    }

    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ActivityLog>>> GetByUser(string userId, [FromQuery] int count = 20)
    {
        var activities = await ctx.ActivityLog
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.Timestamp)
            .Take(count)
            .ToListAsync();

        return Ok(activities);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ActivityLog>> PostAsync(ActivityLog activityLog)
    {
        activityLog.Timestamp = DateTime.UtcNow;
        await ctx.ActivityLog.AddAsync(activityLog);
        await ctx.SaveChangesAsync();

        return Created($"/activitylog/{activityLog.Id}", activityLog);
    }
}
