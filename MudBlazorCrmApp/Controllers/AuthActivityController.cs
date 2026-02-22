using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MudBlazorCrmApp.Data;
using MudBlazorCrmApp.Shared.Models;
using System.Security.Claims;

namespace MudBlazorCrmApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("Fixed")]
public class AuthActivityController(ApplicationDbContext ctx, ILogger<AuthActivityController> logger) : ControllerBase
{
    /// <summary>
    /// Logs a login activity with IP address and user agent
    /// </summary>
    [HttpPost("login")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> LogLoginAsync([FromBody] AuthActivityRequest? request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue(ClaimTypes.Email);
        var email = User.FindFirstValue(ClaimTypes.Email);

        var ipAddress = GetClientIpAddress();
        var userAgent = Request.Headers.UserAgent.ToString();

        var activityLog = new ActivityLog
        {
            UserId = userId,
            UserName = userName ?? email ?? "Unknown",
            EntityType = EntityTypes.User,
            EntityId = null,
            EntityName = email,
            Action = ActivityActions.Login,
            Description = $"User '{userName ?? email}' logged in",
            IpAddress = ipAddress,
            UserAgent = userAgent?.Length > 500 ? userAgent[..500] : userAgent,
            Timestamp = DateTime.UtcNow
        };

        await ctx.ActivityLog.AddAsync(activityLog);

        // Also log to AuditLog for comprehensive tracking
        var auditLog = new AuditLog
        {
            UserId = userId,
            UserName = userName ?? email ?? "Unknown",
            EntityType = EntityTypes.User,
            EntityId = userId ?? "Unknown",
            ChangeType = "Login",
            TableName = "User",
            NewValues = System.Text.Json.JsonSerializer.Serialize(new
            {
                Email = email,
                LoginTime = DateTime.UtcNow,
                IpAddress = ipAddress,
                UserAgent = request?.UserAgent ?? userAgent
            }),
            IpAddress = ipAddress,
            UserAgent = userAgent?.Length > 500 ? userAgent[..500] : userAgent,
            Timestamp = DateTime.UtcNow,
            AdditionalInfo = request?.AdditionalInfo
        };

        await ctx.AuditLog.AddAsync(auditLog);
        await ctx.SaveChangesAsync();

        logger.LogInformation("User {Email} logged in from IP {IpAddress}", email, ipAddress);

        return Ok(new { Message = "Login activity logged", IpAddress = ipAddress });
    }

    /// <summary>
    /// Logs a logout activity with IP address and user agent
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> LogLogoutAsync([FromBody] AuthActivityRequest? request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue(ClaimTypes.Email);
        var email = User.FindFirstValue(ClaimTypes.Email);

        var ipAddress = GetClientIpAddress();
        var userAgent = Request.Headers.UserAgent.ToString();

        var activityLog = new ActivityLog
        {
            UserId = userId,
            UserName = userName ?? email ?? "Unknown",
            EntityType = EntityTypes.User,
            EntityId = null,
            EntityName = email,
            Action = ActivityActions.Logout,
            Description = $"User '{userName ?? email}' logged out",
            IpAddress = ipAddress,
            UserAgent = userAgent?.Length > 500 ? userAgent[..500] : userAgent,
            Timestamp = DateTime.UtcNow
        };

        await ctx.ActivityLog.AddAsync(activityLog);

        // Also log to AuditLog
        var auditLog = new AuditLog
        {
            UserId = userId,
            UserName = userName ?? email ?? "Unknown",
            EntityType = EntityTypes.User,
            EntityId = userId ?? "Unknown",
            ChangeType = "Logout",
            TableName = "User",
            NewValues = System.Text.Json.JsonSerializer.Serialize(new
            {
                Email = email,
                LogoutTime = DateTime.UtcNow,
                IpAddress = ipAddress
            }),
            IpAddress = ipAddress,
            UserAgent = userAgent?.Length > 500 ? userAgent[..500] : userAgent,
            Timestamp = DateTime.UtcNow,
            AdditionalInfo = request?.AdditionalInfo
        };

        await ctx.AuditLog.AddAsync(auditLog);
        await ctx.SaveChangesAsync();

        logger.LogInformation("User {Email} logged out from IP {IpAddress}", email, ipAddress);

        return Ok(new { Message = "Logout activity logged", IpAddress = ipAddress });
    }

    /// <summary>
    /// Logs a failed login attempt (can be called without authentication)
    /// </summary>
    [HttpPost("login-failed")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> LogLoginFailedAsync([FromBody] LoginFailedRequest request)
    {
        var ipAddress = GetClientIpAddress();
        var userAgent = Request.Headers.UserAgent.ToString();

        var activityLog = new ActivityLog
        {
            UserId = null,
            UserName = request.Email ?? "Unknown",
            EntityType = EntityTypes.User,
            EntityId = null,
            EntityName = request.Email,
            Action = "LoginFailed",
            Description = $"Failed login attempt for '{request.Email}'",
            IpAddress = ipAddress,
            UserAgent = userAgent?.Length > 500 ? userAgent[..500] : userAgent,
            Timestamp = DateTime.UtcNow
        };

        await ctx.ActivityLog.AddAsync(activityLog);

        // Also log to AuditLog for security tracking
        var auditLog = new AuditLog
        {
            UserId = null,
            UserName = request.Email ?? "Unknown",
            EntityType = EntityTypes.User,
            EntityId = request.Email ?? "Unknown",
            ChangeType = "LoginFailed",
            TableName = "User",
            NewValues = System.Text.Json.JsonSerializer.Serialize(new
            {
                Email = request.Email,
                AttemptTime = DateTime.UtcNow,
                IpAddress = ipAddress,
                Reason = request.Reason
            }),
            IpAddress = ipAddress,
            UserAgent = userAgent?.Length > 500 ? userAgent[..500] : userAgent,
            Timestamp = DateTime.UtcNow,
            AdditionalInfo = $"Failed login: {request.Reason}"
        };

        await ctx.AuditLog.AddAsync(auditLog);
        await ctx.SaveChangesAsync();

        logger.LogWarning("Failed login attempt for {Email} from IP {IpAddress}: {Reason}",
            request.Email, ipAddress, request.Reason);

        return Ok(new { Message = "Failed login logged" });
    }

    /// <summary>
    /// Gets recent authentication activity for the current user
    /// </summary>
    [HttpGet("my-activity")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ActivityLog>>> GetMyActivityAsync([FromQuery] int count = 10)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var activities = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
            ctx.ActivityLog
                .Where(a => a.UserId == userId && 
                    (a.Action == ActivityActions.Login || a.Action == ActivityActions.Logout || a.Action == "LoginFailed"))
                .OrderByDescending(a => a.Timestamp)
                .Take(count));

        return Ok(activities);
    }

    /// <summary>
    /// Gets all authentication activity (admin only)
    /// </summary>
    [HttpGet("all-activity")]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ActivityLog>>> GetAllAuthActivityAsync([FromQuery] int count = 50)
    {
        var activities = await Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync(
            ctx.ActivityLog
                .Where(a => a.Action == ActivityActions.Login || a.Action == ActivityActions.Logout || a.Action == "LoginFailed")
                .OrderByDescending(a => a.Timestamp)
                .Take(count));

        return Ok(activities);
    }

    private string GetClientIpAddress()
    {
        // Check for forwarded headers (when behind a proxy/load balancer)
        var forwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            // X-Forwarded-For can contain multiple IPs, first one is the client
            var ips = forwardedFor.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (ips.Length > 0)
            {
                return ips[0].Trim();
            }
        }

        // Check X-Real-IP header
        var realIp = Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        // Fall back to connection remote IP
        var remoteIp = HttpContext.Connection.RemoteIpAddress;
        if (remoteIp != null)
        {
            // Handle IPv6 loopback mapped to IPv4
            if (remoteIp.IsIPv4MappedToIPv6)
            {
                return remoteIp.MapToIPv4().ToString();
            }
            return remoteIp.ToString();
        }

        return "Unknown";
    }
}

/// <summary>
/// Request model for authentication activity logging
/// </summary>
public class AuthActivityRequest
{
    public string? UserAgent { get; set; }
    public string? AdditionalInfo { get; set; }
}

/// <summary>
/// Request model for logging failed login attempts
/// </summary>
public class LoginFailedRequest
{
    public string? Email { get; set; }
    public string? Reason { get; set; }
}
