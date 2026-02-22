using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
public class AuditLogController(ApplicationDbContext ctx, ILogger<AuditLogController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all audit logs with OData query support
    /// </summary>
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IQueryable<AuditLog>> Get()
    {
        return Ok(ctx.AuditLog.OrderByDescending(x => x.Timestamp));
    }

    /// <summary>
    /// Gets a specific audit log entry by ID
    /// </summary>
    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuditLog>> GetAsync(long key)
    {
        var auditLog = await ctx.AuditLog.FindAsync(key);
        return auditLog == null ? NotFound() : Ok(auditLog);
    }

    /// <summary>
    /// Gets audit logs for a specific entity by type and ID
    /// </summary>
    [HttpGet("entity/{entityType}/{entityId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AuditLog>>> GetByEntity(string entityType, string entityId)
    {
        var audits = await ctx.AuditLog
            .Where(a => a.EntityType == entityType && a.EntityId.Contains(entityId))
            .OrderByDescending(a => a.Timestamp)
            .Take(100)
            .ToListAsync();

        return Ok(audits);
    }

    /// <summary>
    /// Gets audit logs by user ID
    /// </summary>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AuditLog>>> GetByUser(string userId, [FromQuery] int count = 50)
    {
        var audits = await ctx.AuditLog
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.Timestamp)
            .Take(count)
            .ToListAsync();

        return Ok(audits);
    }

    /// <summary>
    /// Gets audit logs by change type
    /// </summary>
    [HttpGet("type/{changeType}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AuditLog>>> GetByChangeType(string changeType, [FromQuery] int count = 50)
    {
        var audits = await ctx.AuditLog
            .Where(a => a.ChangeType == changeType)
            .OrderByDescending(a => a.Timestamp)
            .Take(count)
            .ToListAsync();

        return Ok(audits);
    }

    /// <summary>
    /// Gets audit logs by correlation ID (grouped changes from single transaction)
    /// </summary>
    [HttpGet("correlation/{correlationId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AuditLog>>> GetByCorrelation(string correlationId)
    {
        var audits = await ctx.AuditLog
            .Where(a => a.CorrelationId == correlationId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();

        return Ok(audits);
    }

    /// <summary>
    /// Gets audit logs within a date range
    /// </summary>
    [HttpGet("daterange")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AuditLog>>> GetByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate,
        [FromQuery] int count = 100)
    {
        var audits = await ctx.AuditLog
            .Where(a => a.Timestamp >= startDate && a.Timestamp <= endDate)
            .OrderByDescending(a => a.Timestamp)
            .Take(count)
            .ToListAsync();

        return Ok(audits);
    }

    /// <summary>
    /// Gets a summary of audit activity
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetSummary([FromQuery] int days = 30)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);

        var summary = new
        {
            TotalChanges = await ctx.AuditLog.CountAsync(a => a.Timestamp >= cutoffDate),
            Inserts = await ctx.AuditLog.CountAsync(a => a.Timestamp >= cutoffDate && a.ChangeType == AuditChangeTypes.Insert),
            Updates = await ctx.AuditLog.CountAsync(a => a.Timestamp >= cutoffDate && a.ChangeType == AuditChangeTypes.Update),
            Deletes = await ctx.AuditLog.CountAsync(a => a.Timestamp >= cutoffDate && a.ChangeType == AuditChangeTypes.Delete),
            SoftDeletes = await ctx.AuditLog.CountAsync(a => a.Timestamp >= cutoffDate && a.ChangeType == AuditChangeTypes.SoftDelete),
            Restores = await ctx.AuditLog.CountAsync(a => a.Timestamp >= cutoffDate && a.ChangeType == AuditChangeTypes.Restore),
            ByEntityType = await ctx.AuditLog
                .Where(a => a.Timestamp >= cutoffDate)
                .GroupBy(a => a.EntityType)
                .Select(g => new { EntityType = g.Key, Count = g.Count() })
                .ToListAsync(),
            TopUsers = await ctx.AuditLog
                .Where(a => a.Timestamp >= cutoffDate && a.UserId != null)
                .GroupBy(a => new { a.UserId, a.UserName })
                .Select(g => new { g.Key.UserId, g.Key.UserName, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToListAsync()
        };

        return Ok(summary);
    }
}
