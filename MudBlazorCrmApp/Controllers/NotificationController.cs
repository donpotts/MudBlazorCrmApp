using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MudBlazorCrmApp.Data;
using MudBlazorCrmApp.Shared.Models;
using System.Security.Claims;

namespace MudBlazorCrmApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Fixed")]
public class NotificationController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IQueryable<Notification>> Get()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(ctx.Notification.Where(n => n.UserId == userId).OrderByDescending(n => n.CreatedDate));
    }

    [HttpGet("unread-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetUnreadCount()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var count = await ctx.Notification.CountAsync(n => n.UserId == userId && !n.IsRead);
        return Ok(count);
    }

    [HttpGet("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Notification>> GetAsync(long key)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var record = await ctx.Notification.FirstOrDefaultAsync(n => n.Id == key && n.UserId == userId);
        if (record == null) return NotFound();
        return Ok(record);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Notification>> PostAsync([FromBody] Notification record)
    {
        record.CreatedDate = DateTime.UtcNow;
        record.IsRead = false;
        await ctx.Notification.AddAsync(record);
        await ctx.SaveChangesAsync();
        return Created($"/api/notification/{record.Id}", record);
    }

    [HttpPut("{key}/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> MarkAsRead(long key)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var record = await ctx.Notification.FirstOrDefaultAsync(n => n.Id == key && n.UserId == userId);
        if (record == null) return NotFound();
        record.IsRead = true;
        record.ReadDate = DateTime.UtcNow;
        await ctx.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("read-all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> MarkAllAsRead()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var unread = await ctx.Notification.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
        foreach (var n in unread)
        {
            n.IsRead = true;
            n.ReadDate = DateTime.UtcNow;
        }
        await ctx.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync(long key)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var record = await ctx.Notification.FirstOrDefaultAsync(n => n.Id == key && n.UserId == userId);
        if (record == null) return NotFound();
        ctx.Notification.Remove(record);
        await ctx.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("generate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GenerateNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var notifications = new List<Notification>();
        var now = DateTime.UtcNow;

        // Overdue support cases
        var overdueCases = await ctx.SupportCase
            .Where(s => !s.IsDeleted && s.DueDate < now && s.Status != "Closed" && s.Status != "Resolved")
            .ToListAsync();

        foreach (var sc in overdueCases)
        {
            var exists = await ctx.Notification.AnyAsync(n =>
                n.UserId == userId && n.EntityType == "SupportCase" && n.EntityId == sc.Id && n.Type == "Alert" && !n.IsRead);
            if (!exists)
            {
                notifications.Add(new Notification
                {
                    Title = $"Overdue: {sc.Title}",
                    Message = $"Support case #{sc.Id} was due {sc.DueDate:MMM dd}",
                    Type = "Alert",
                    Priority = sc.Priority == "Critical" ? "Urgent" : "High",
                    UserId = userId,
                    EntityType = "SupportCase",
                    EntityId = sc.Id,
                    CreatedDate = now,
                });
            }
        }

        // Follow-up reminders for support cases
        var followups = await ctx.SupportCase
            .Where(s => !s.IsDeleted && s.FollowupDate != null && s.FollowupDate <= now.AddDays(1) && s.Status != "Closed" && s.Status != "Resolved")
            .ToListAsync();

        foreach (var sc in followups)
        {
            var exists = await ctx.Notification.AnyAsync(n =>
                n.UserId == userId && n.EntityType == "SupportCase" && n.EntityId == sc.Id && n.Type == "Reminder" && !n.IsRead);
            if (!exists)
            {
                notifications.Add(new Notification
                {
                    Title = $"Follow-up: {sc.Title}",
                    Message = $"Follow-up scheduled for {sc.FollowupDate:MMM dd}",
                    Type = "Reminder",
                    Priority = "Normal",
                    UserId = userId,
                    EntityType = "SupportCase",
                    EntityId = sc.Id,
                    CreatedDate = now,
                });
            }
        }

        // Opportunities closing soon
        var closingSoon = await ctx.Opportunity
            .Where(o => !o.IsDeleted && o.EstimatedCloseDate != null && o.EstimatedCloseDate <= now.AddDays(7) && o.EstimatedCloseDate >= now && o.Stage != "Closed Won" && o.Stage != "Closed Lost")
            .ToListAsync();

        foreach (var opp in closingSoon)
        {
            var exists = await ctx.Notification.AnyAsync(n =>
                n.UserId == userId && n.EntityType == "Opportunity" && n.EntityId == opp.Id && n.Type == "Warning" && !n.IsRead);
            if (!exists)
            {
                notifications.Add(new Notification
                {
                    Title = $"Closing Soon: {opp.Name}",
                    Message = $"Opportunity closes {opp.EstimatedCloseDate:MMM dd} - Value: {opp.Value:C}",
                    Type = "Warning",
                    Priority = "High",
                    UserId = userId,
                    EntityType = "Opportunity",
                    EntityId = opp.Id,
                    CreatedDate = now,
                });
            }
        }

        if (notifications.Count > 0)
        {
            await ctx.Notification.AddRangeAsync(notifications);
            await ctx.SaveChangesAsync();
        }

        return Ok(notifications.Count);
    }
}
