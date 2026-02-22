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
public class CommunicationController(ApplicationDbContext ctx, ILogger<CommunicationController> logger) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IQueryable<Communication>> Get()
    {
        return Ok(ctx.Communication
            .Include(x => x.Contact)
            .Include(x => x.Customer)
            .Include(x => x.Lead)
            .Include(x => x.Opportunity)
            .OrderByDescending(x => x.CommunicationDate));
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Communication>> GetAsync(long key)
    {
        var communication = await ctx.Communication
            .Include(x => x.Contact)
            .Include(x => x.Customer)
            .Include(x => x.Lead)
            .Include(x => x.Opportunity)
            .FirstOrDefaultAsync(x => x.Id == key);

        return communication == null ? NotFound() : Ok(communication);
    }

    [HttpGet("contact/{contactId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Communication>>> GetByContact(long contactId)
    {
        var communications = await ctx.Communication
            .Where(c => c.ContactId == contactId)
            .OrderByDescending(c => c.CommunicationDate)
            .ToListAsync();

        return Ok(communications);
    }

    [HttpGet("lead/{leadId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Communication>>> GetByLead(long leadId)
    {
        var communications = await ctx.Communication
            .Where(c => c.LeadId == leadId)
            .OrderByDescending(c => c.CommunicationDate)
            .ToListAsync();

        return Ok(communications);
    }

    [HttpGet("customer/{customerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Communication>>> GetByCustomer(long customerId)
    {
        var communications = await ctx.Communication
            .Where(c => c.CustomerId == customerId)
            .OrderByDescending(c => c.CommunicationDate)
            .ToListAsync();

        return Ok(communications);
    }

    [HttpGet("opportunity/{opportunityId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Communication>>> GetByOpportunity(long opportunityId)
    {
        var communications = await ctx.Communication
            .Where(c => c.OpportunityId == opportunityId)
            .OrderByDescending(c => c.CommunicationDate)
            .ToListAsync();

        return Ok(communications);
    }

    [HttpGet("followups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Communication>>> GetPendingFollowups()
    {
        var followups = await ctx.Communication
            .Where(c => c.FollowUpRequired && c.FollowUpDate != null && c.FollowUpDate >= DateTime.UtcNow)
            .OrderBy(c => c.FollowUpDate)
            .Include(x => x.Contact)
            .Include(x => x.Customer)
            .ToListAsync();

        return Ok(followups);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<Communication>> PostAsync(Communication communication)
    {
        await ctx.Communication.AddAsync(communication);
        await ctx.SaveChangesAsync();

        return Created($"/communication/{communication.Id}", communication);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Communication>> PutAsync(long key, Communication update)
    {
        var communication = await ctx.Communication.FindAsync(key);

        if (communication == null)
        {
            return NotFound();
        }

        ctx.Entry(communication).CurrentValues.SetValues(update);
        await ctx.SaveChangesAsync();

        return Ok(communication);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var communication = await ctx.Communication.FindAsync(key);

        if (communication != null)
        {
            communication.IsDeleted = true;
            communication.DeletedDate = DateTime.UtcNow;
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
