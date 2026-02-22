using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MudBlazorCrmApp.Data;
using MudBlazorCrmApp.Shared.Models;

namespace MudBlazorCrmApp.Controllers;

/// <summary>
/// Provides dashboard data and KPIs for the CRM system
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Fixed")]
public class DashboardController(ApplicationDbContext _ctx) : ControllerBase
{
    [HttpGet("kpis")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    {


        {
            NewLeads = newLeads,
            DealsClosed = dealsClosed,
            OpenSupportCases = openSupportCases
        });
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    {

            {
                Total = g.Sum(s => s.TotalAmount ?? 0),
                Count = g.Count()
            })

    }

    /// <summary>
    /// Get lead sources distribution
    /// </summary>
    [HttpGet("lead-sources")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    {
            .GroupBy(l => l.Source ?? "Unknown")
            {
                Source = g.Key,
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync();

    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    {
            .GroupBy(o => o.Stage ?? "Unknown")
            {
                Stage = g.Key,
                Count = g.Count(),
            })
            .OrderBy(x => x.Stage)
            .ToListAsync();

    }

    /// <summary>
    /// Get recent activity feed
    /// </summary>
    [HttpGet("recent-activity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    {
            .ToListAsync();

        return Ok(activities);
    }

    /// <summary>
    /// Get top opportunities
    /// </summary>
    [HttpGet("top-opportunities")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    {
        var opportunities = await ctx.Opportunity
            .Include(o => o.Customer)
            .Where(o => o.Stage != "Closed Won" && o.Stage != "Closed Lost")
            .OrderByDescending(o => o.Value)
            .ToListAsync();

        return Ok(opportunities);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
            {

    {

        {
}
        }));

{
{
}

{
{
}

}

public class SupportStats
{
    public int OpenCases { get; set; }
    public int HighPriorityCases { get; set; }
    public double AvgResolutionHours { get; set; }
    public Dictionary<string, int> CasesByStatus { get; set; } = new();
}
