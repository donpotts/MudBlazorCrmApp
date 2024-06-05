using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MudBlazorCrmApp.Data;
using MudBlazorCrmApp.Shared.Models;

namespace MudBlazorCrmApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Fixed")]
public class LeadController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Lead>> Get()
    {
        return Ok(ctx.Lead.Include(x => x.Address).Include(x => x.Opportunity).Include(x => x.Contact));
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Lead>> GetAsync(long key)
    {
        var lead = await ctx.Lead.Include(x => x.Address).Include(x => x.Opportunity).Include(x => x.Contact).FirstOrDefaultAsync(x => x.Id == key);

        if (lead == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(lead);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Lead>> PostAsync(Lead lead)
    {
        var record = await ctx.Lead.FindAsync(lead.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        await ctx.Lead.AddAsync(lead);

        await ctx.SaveChangesAsync();

        return Created($"/lead/{lead.Id}", lead);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Lead>> PutAsync(long key, Lead update)
    {
        var lead = await ctx.Lead.FirstOrDefaultAsync(x => x.Id == key);

        if (lead == null)
        {
            return NotFound();
        }

        ctx.Entry(lead).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(lead);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Lead>> PatchAsync(long key, Delta<Lead> delta)
    {
        var lead = await ctx.Lead.FirstOrDefaultAsync(x => x.Id == key);

        if (lead == null)
        {
            return NotFound();
        }

        delta.Patch(lead);

        await ctx.SaveChangesAsync();

        return Ok(lead);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var lead = await ctx.Lead.FindAsync(key);

        if (lead != null)
        {
            ctx.Lead.Remove(lead);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
