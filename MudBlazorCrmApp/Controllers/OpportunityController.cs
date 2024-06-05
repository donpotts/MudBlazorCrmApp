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
public class OpportunityController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Opportunity>> Get()
    {
        return Ok(ctx.Opportunity);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Opportunity>> GetAsync(long key)
    {
        var opportunity = await ctx.Opportunity.FirstOrDefaultAsync(x => x.Id == key);

        if (opportunity == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(opportunity);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Opportunity>> PostAsync(Opportunity opportunity)
    {
        var record = await ctx.Opportunity.FindAsync(opportunity.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        await ctx.Opportunity.AddAsync(opportunity);

        await ctx.SaveChangesAsync();

        return Created($"/opportunity/{opportunity.Id}", opportunity);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Opportunity>> PutAsync(long key, Opportunity update)
    {
        var opportunity = await ctx.Opportunity.FirstOrDefaultAsync(x => x.Id == key);

        if (opportunity == null)
        {
            return NotFound();
        }

        ctx.Entry(opportunity).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(opportunity);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Opportunity>> PatchAsync(long key, Delta<Opportunity> delta)
    {
        var opportunity = await ctx.Opportunity.FirstOrDefaultAsync(x => x.Id == key);

        if (opportunity == null)
        {
            return NotFound();
        }

        delta.Patch(opportunity);

        await ctx.SaveChangesAsync();

        return Ok(opportunity);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var opportunity = await ctx.Opportunity.FindAsync(key);

        if (opportunity != null)
        {
            ctx.Opportunity.Remove(opportunity);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
