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
public class DashboardController(ApplicationDbContext ctx, ILogger<DashboardController> logger) : ControllerBase
{
    /// <summary>
    /// Get all dashboard KPIs
    /// </summary>
    [HttpGet("kpis")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardKpis>> GetKpis([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null)
    {
        var fromDate = from ?? DateTime.UtcNow.AddMonths(-1);
        var toDate = to ?? DateTime.UtcNow;

        var newLeads = await ctx.Lead
            .Where(l => l.CreatedDate >= fromDate && l.CreatedDate <= toDate)
            .CountAsync();

        var totalRevenue = await ctx.Sale
            .Where(s => s.SaleDate >= fromDate && s.SaleDate <= toDate && s.Status == "Completed")
            .SumAsync(s => s.TotalAmount ?? 0);

        var previousFromDate = fromDate.AddDays(-(toDate - fromDate).TotalDays);
        var previousRevenue = await ctx.Sale
            .Where(s => s.SaleDate >= previousFromDate && s.SaleDate < fromDate && s.Status == "Completed")
            .SumAsync(s => s.TotalAmount ?? 0);

        var totalLeads = await ctx.Lead.CountAsync(l => l.CreatedDate >= fromDate && l.CreatedDate <= toDate);
        var convertedLeads = await ctx.Lead.CountAsync(l => l.ConvertedDate >= fromDate && l.ConvertedDate <= toDate);
        var conversionRate = totalLeads > 0 ? (decimal)convertedLeads / totalLeads * 100 : 0;

        var pipelineValue = await ctx.Opportunity
            .Where(o => o.Stage != "Closed Won" && o.Stage != "Closed Lost")
            .SumAsync(o => (o.Value ?? 0) * (o.Probability ?? 0) / 100);

        var dealsClosed = await ctx.Opportunity
            .CountAsync(o => o.ActualCloseDate >= fromDate && o.ActualCloseDate <= toDate && o.Stage == "Closed Won");

        var closedDeals = await ctx.Opportunity
            .Where(o => o.ActualCloseDate >= fromDate && o.ActualCloseDate <= toDate && o.Stage == "Closed Won")
            .ToListAsync();
        var avgDealSize = closedDeals.Count > 0 ? closedDeals.Average(o => o.Value ?? 0) : 0;

        var activeCustomers = await ctx.Customer.CountAsync(c => c.Status == "Active");
        var openSupportCases = await ctx.SupportCase.CountAsync(s => s.Status != "Resolved" && s.Status != "Closed");

        return Ok(new DashboardKpis
        {
            NewLeads = newLeads,
            TotalRevenue = totalRevenue,
            RevenueChange = previousRevenue > 0 ? (totalRevenue - previousRevenue) / previousRevenue * 100 : 0,
            ConversionRate = Math.Round(conversionRate, 1),
            PipelineValue = pipelineValue,
            DealsClosed = dealsClosed,
            AvgDealSize = Math.Round(avgDealSize, 2),
            ActiveCustomers = activeCustomers,
            OpenSupportCases = openSupportCases
        });
    }

    /// <summary>
    /// Get sales trend data for charting
    /// </summary>
    [HttpGet("sales-trend")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SalesTrendData>>> GetSalesTrend([FromQuery] int months = 6)
    {
        var endDate = DateTime.UtcNow;
        var startDate = endDate.AddMonths(-months);

        var salesData = await ctx.Sale
            .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate && s.Status == "Completed")
            .GroupBy(s => new { s.SaleDate!.Value.Year, s.SaleDate.Value.Month })
            .Select(g => new SalesTrendData
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Total = g.Sum(s => s.TotalAmount ?? 0),
                Count = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

        return Ok(salesData);
    }

    /// <summary>
    /// Get lead sources distribution
    /// </summary>
    [HttpGet("lead-sources")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<LeadSourceData>>> GetLeadSources([FromQuery] int months = 3)
    {
        var startDate = DateTime.UtcNow.AddMonths(-months);

        var leadSources = await ctx.Lead
            .Where(l => l.CreatedDate >= startDate)
            .GroupBy(l => l.Source ?? "Unknown")
            .Select(g => new LeadSourceData
            {
                Source = g.Key,
                Count = g.Count(),
                TotalValue = g.Sum(l => l.PotentialValue ?? 0)
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync();

        return Ok(leadSources);
    }

    /// <summary>
    /// Get pipeline data by stage
    /// </summary>
    [HttpGet("pipeline-stages")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PipelineStageData>>> GetPipelineStages()
    {
        var stages = new[] { "Prospecting", "Qualification", "Proposal", "Negotiation", "Closed Won", "Closed Lost" };

        var pipelineData = await ctx.Opportunity
            .GroupBy(o => o.Stage ?? "Unknown")
            .Select(g => new PipelineStageData
            {
                Stage = g.Key,
                Count = g.Count(),
                TotalValue = g.Sum(o => o.Value ?? 0),
                WeightedValue = g.Sum(o => (o.Value ?? 0) * (o.Probability ?? 0) / 100)
            })
            .ToListAsync();

        // Ensure all stages are represented
        var result = stages.Select(stage =>
            pipelineData.FirstOrDefault(p => p.Stage == stage) ?? new PipelineStageData { Stage = stage, Count = 0, TotalValue = 0, WeightedValue = 0 }
        ).ToList();

        return Ok(result);
    }

    /// <summary>
    /// Get recent activity feed
    /// </summary>
    [HttpGet("recent-activity")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ActivityLog>>> GetRecentActivity([FromQuery] int count = 10)
    {
        var activities = await ctx.ActivityLog
            .OrderByDescending(a => a.Timestamp)
            .Take(count)
            .ToListAsync();

        return Ok(activities);
    }

    /// <summary>
    /// Get top opportunities
    /// </summary>
    [HttpGet("top-opportunities")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Opportunity>>> GetTopOpportunities([FromQuery] int count = 5)
    {
        var opportunities = await ctx.Opportunity
            .Where(o => o.Stage != "Closed Won" && o.Stage != "Closed Lost")
            .OrderByDescending(o => o.Value)
            .Take(count)
            .Include(o => o.Customer)
            .ToListAsync();

        return Ok(opportunities);
    }

    /// <summary>
    /// Get customer distribution by industry
    /// </summary>
    [HttpGet("customers-by-industry")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<IndustryData>>> GetCustomersByIndustry()
    {
        var data = await ctx.Customer
            .GroupBy(c => c.Industry ?? "Unknown")
            .Select(g => new IndustryData
            {
                Industry = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .ToListAsync();

        return Ok(data);
    }

    /// <summary>
    /// Get support case statistics
    /// </summary>
    [HttpGet("support-stats")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SupportStats>> GetSupportStats()
    {
        var openCases = await ctx.SupportCase.CountAsync(s => s.Status != "Resolved" && s.Status != "Closed");
        var highPriority = await ctx.SupportCase.CountAsync(s => s.Priority == "High" || s.Priority == "Critical");

        // Calculate average resolution time in memory (SQLite compatible)
        var resolvedCases = await ctx.SupportCase
            .Where(s => s.ResolvedDate != null && s.CreatedDate != null)
            .Select(s => new { s.CreatedDate, s.ResolvedDate })
            .ToListAsync();

        var avgResolutionHours = resolvedCases.Count > 0
            ? resolvedCases.Average(s => (s.ResolvedDate!.Value - s.CreatedDate!.Value).TotalHours)
            : 0;

        var byStatus = await ctx.SupportCase
            .GroupBy(s => s.Status ?? "Unknown")
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status, x => x.Count);

        return Ok(new SupportStats
        {
            OpenCases = openCases,
            HighPriorityCases = highPriority,
            AvgResolutionHours = Math.Round(avgResolutionHours, 1),
            CasesByStatus = byStatus
        });
    }
}

// DTO Classes for Dashboard
public class DashboardKpis
{
    public int NewLeads { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal RevenueChange { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal PipelineValue { get; set; }
    public int DealsClosed { get; set; }
    public decimal AvgDealSize { get; set; }
    public int ActiveCustomers { get; set; }
    public int OpenSupportCases { get; set; }
}

public class SalesTrendData
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal Total { get; set; }
    public int Count { get; set; }
    public string MonthName => new DateTime(Year, Month, 1).ToString("MMM");
}

public class LeadSourceData
{
    public string? Source { get; set; }
    public int Count { get; set; }
    public decimal TotalValue { get; set; }
}

public class PipelineStageData
{
    public string? Stage { get; set; }
    public int Count { get; set; }
    public decimal TotalValue { get; set; }
    public decimal WeightedValue { get; set; }
}

public class IndustryData
{
    public string? Industry { get; set; }
    public int Count { get; set; }
}

public class SupportStats
{
    public int OpenCases { get; set; }
    public int HighPriorityCases { get; set; }
    public double AvgResolutionHours { get; set; }
    public Dictionary<string, int> CasesByStatus { get; set; } = new();
}
