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
public class SearchController(ApplicationDbContext _ctx) : ControllerBase
{
    private readonly ApplicationDbContext ctx = _ctx;

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SearchResultDto>> Search([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
        {
            return Ok(new SearchResultDto());
        }

        var query = q.ToLower();

        var customers = await ctx.Customer
            .Where(c => c.Name != null && c.Name.ToLower().Contains(query))
            .Take(5)
            .Select(c => new SearchResultItem
            {
                Id = c.Id,
                Name = c.Name,
                EntityType = "Customer",
                Detail = c.Industry
            })
            .ToListAsync();

        var contacts = await ctx.Contact
            .Where(c => (c.Name != null && c.Name.ToLower().Contains(query)) ||
                        (c.Email != null && c.Email.ToLower().Contains(query)))
            .Take(5)
            .Select(c => new SearchResultItem
            {
                Id = c.Id,
                Name = c.Name,
                EntityType = "Contact",
                Detail = c.Email
            })
            .ToListAsync();

        var opportunities = await ctx.Opportunity
            .Where(o => o.Name != null && o.Name.ToLower().Contains(query))
            .Take(5)
            .Select(o => new SearchResultItem
            {
                Id = o.Id,
                Name = o.Name,
                EntityType = "Opportunity",
                Detail = o.Stage
            })
            .ToListAsync();

        var leads = await ctx.Lead
            .Include(l => l.Contact)
            .Where(l => l.Contact != null && l.Contact.Name != null && l.Contact.Name.ToLower().Contains(query))
            .Take(5)
            .Select(l => new SearchResultItem
            {
                Id = l.Id,
                Name = l.Contact != null ? l.Contact.Name : "Unknown",
                EntityType = "Lead",
                Detail = l.Status
            })
            .ToListAsync();

        return Ok(new SearchResultDto
        {
            Customers = customers,
            Contacts = contacts,
            Opportunities = opportunities,
            Leads = leads
        });
    }
}
