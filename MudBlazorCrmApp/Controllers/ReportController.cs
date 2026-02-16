using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MudBlazorCrmApp.Data;
using MudBlazorCrmApp.Shared.Models;

namespace MudBlazorCrmApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Fixed")]
public class ReportController(ApplicationDbContext _ctx) : ControllerBase
{
    private readonly ApplicationDbContext ctx = _ctx;

    [HttpGet("sales")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<SalesOverTimeDto>>> GetSalesReport(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string groupBy = "month")
    {
        var query = ctx.Sale.AsQueryable();

        if (from.HasValue)
            query = query.Where(s => s.SaleDate >= from.Value);
        if (to.HasValue)
            query = query.Where(s => s.SaleDate <= to.Value);

        var sales = await query.ToListAsync();

        var grouped = groupBy.ToLower() switch
        {
            "week" => sales.GroupBy(s => $"{s.SaleDate?.Year}-W{GetWeekOfYear(s.SaleDate)}"),
            "quarter" => sales.GroupBy(s => $"{s.SaleDate?.Year}-Q{(s.SaleDate?.Month - 1) / 3 + 1}"),
            "year" => sales.GroupBy(s => s.SaleDate?.Year.ToString() ?? "Unknown"),
            _ => sales.GroupBy(s => s.SaleDate?.ToString("yyyy-MM") ?? "Unknown")
        };

        var result = grouped
            .Select(g => new SalesOverTimeDto
            {
                Month = g.Key,
                Total = g.Sum(s => s.TotalAmount ?? 0),
                Count = g.Count()
            })
            .OrderBy(x => x.Month)
            .ToList();

        return Ok(result);
    }

    [HttpGet("pipeline")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PipelineStageDto>>> GetPipelineReport([FromQuery] string? stage)
    {
        var query = ctx.Opportunity.AsQueryable();

        if (!string.IsNullOrEmpty(stage))
            query = query.Where(o => o.Stage == stage);

        var stages = await query
            .GroupBy(o => o.Stage ?? "Unknown")
            .Select(g => new PipelineStageDto
            {
                Stage = g.Key,
                Count = g.Count(),
                TotalValue = g.Sum(o => o.Value ?? 0)
            })
            .OrderBy(x => x.Stage)
            .ToListAsync();

        return Ok(stages);
    }

    [HttpGet("activities")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<object>> GetActivityReport(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to,
        [FromQuery] string? type)
    {
        var query = ctx.Activity.AsQueryable();

        if (from.HasValue)
            query = query.Where(a => a.ActivityDate >= from.Value);
        if (to.HasValue)
            query = query.Where(a => a.ActivityDate <= to.Value);
        if (!string.IsNullOrEmpty(type))
            query = query.Where(a => a.Type == type);

        var byType = await query
            .GroupBy(a => a.Type ?? "Unknown")
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToListAsync();

        var byStatus = await query
            .GroupBy(a => a.Status ?? "Unknown")
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        var total = await query.CountAsync();

        return Ok(new { Total = total, ByType = byType, ByStatus = byStatus });
    }

    private static int GetWeekOfYear(DateTime? date)
    {
        if (!date.HasValue) return 0;
        var cal = System.Globalization.CultureInfo.InvariantCulture.Calendar;
        return cal.GetWeekOfYear(date.Value, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }
}
