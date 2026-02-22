using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MudBlazorCrmApp.Data;
using MudBlazorCrmApp.Services;
using MudBlazorCrmApp.Shared.Models;
using System.Security.Claims;

namespace MudBlazorCrmApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Fixed")]
public class EmailController(EmailService emailService, ApplicationDbContext ctx, ILogger<EmailController> logger) : ControllerBase
{
    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> GetStatus()
    {
        return Ok(new { IsConfigured = emailService.IsConfigured });
    }

    [HttpPost("send")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SendAsync([FromBody] SendEmailRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.To))
            return BadRequest("Recipient email is required");

        if (string.IsNullOrWhiteSpace(request.Subject))
            return BadRequest("Subject is required");

        if (string.IsNullOrWhiteSpace(request.Body))
            return BadRequest("Body is required");

        try
        {
            await emailService.SendEmailAsync(request.To, request.Subject, request.Body, request.HtmlBody);

            // Log as Activity if entity context is provided
            if (!string.IsNullOrEmpty(request.EntityType) && request.EntityId.HasValue)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var activity = new Activity
                {
                    Type = "Email",
                    Subject = $"Email: {request.Subject}",
                    Description = request.Body,
                    ActivityDate = DateTime.UtcNow,
                    Status = "Completed",
                    Direction = "Outbound",
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                };

                switch (request.EntityType)
                {
                    case "Contact":
                        activity.ContactId = request.EntityId;
                        break;
                    case "Customer":
                        activity.CustomerId = request.EntityId;
                        break;
                    case "Lead":
                        activity.LeadId = request.EntityId;
                        break;
                    case "Opportunity":
                        activity.OpportunityId = request.EntityId;
                        break;
                }

                ctx.Activity.Add(activity);
                await ctx.SaveChangesAsync();
            }

            logger.LogInformation("Email sent to {To} by user {User}", request.To, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Ok(new { Message = "Email sent successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {To}", request.To);
            return StatusCode(500, $"Failed to send email: {ex.Message}");
        }
    }
}

public class SendEmailRequest
{
    public string? To { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public string? HtmlBody { get; set; }
    public string? EntityType { get; set; }
    public long? EntityId { get; set; }
}
