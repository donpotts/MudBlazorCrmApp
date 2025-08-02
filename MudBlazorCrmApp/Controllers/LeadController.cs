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
    
        var address = lead.Address;
        lead.Address = null;

        var opportunity = lead.Opportunity;
        lead.Opportunity = null;

        var contact = lead.Contact;
        lead.Contact = null;

        await ctx.Lead.AddAsync(lead);

        if (address != null)
        {
            var newValues = await ctx.Address.Where(x => address.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            lead.Address = [..newValues];
        }

        if (opportunity != null)
        {
            var newValues = await ctx.Opportunity.Where(x => opportunity.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            lead.Opportunity = [..newValues];
        }

        if (contact != null)
        {
            var newValues = await ctx.Contact.Where(x => contact.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            lead.Contact = [..newValues];
        }

        await ctx.SaveChangesAsync();

        return Created($"/lead/{lead.Id}", lead);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Lead>> PutAsync(long key, Lead update)
    {
        var lead = await ctx.Lead.Include(x => x.Address).Include(x => x.Opportunity).Include(x => x.Contact).FirstOrDefaultAsync(x => x.Id == key);

        if (lead == null)
        {
            return NotFound();
        }

        ctx.Entry(lead).CurrentValues.SetValues(update);

        if (update.Address != null)
        {
            var updateValues = update.Address.Select(x => x.Id);
            lead.Address ??= [];
            lead.Address.RemoveAll(x => !updateValues.Contains(x.Id));
            var addValues = updateValues.Where(x => !lead.Address.Select(y => y.Id).Contains(x));
            var newValues = await ctx.Address.Where(x => addValues.Contains(x.Id)).ToListAsync();
            lead.Address.AddRange(newValues);
        }

        if (update.Opportunity != null)
        {
            var updateValues = update.Opportunity.Select(x => x.Id);
            lead.Opportunity ??= [];
            lead.Opportunity.RemoveAll(x => !updateValues.Contains(x.Id));
            var addValues = updateValues.Where(x => !lead.Opportunity.Select(y => y.Id).Contains(x));
            var newValues = await ctx.Opportunity.Where(x => addValues.Contains(x.Id)).ToListAsync();
            lead.Opportunity.AddRange(newValues);
        }

        if (update.Contact != null)
        {
            var updateValues = update.Contact.Select(x => x.Id);
            lead.Contact ??= [];
            lead.Contact.RemoveAll(x => !updateValues.Contains(x.Id));
            var addValues = updateValues.Where(x => !lead.Contact.Select(y => y.Id).Contains(x));
            var newValues = await ctx.Contact.Where(x => addValues.Contains(x.Id)).ToListAsync();
            lead.Contact.AddRange(newValues);
        }

        await ctx.SaveChangesAsync();

        return Ok(lead);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Lead>> PatchAsync(long key, Delta<Lead> delta)
    {
        var lead = await ctx.Lead.Include(x => x.Address).Include(x => x.Opportunity).Include(x => x.Contact).FirstOrDefaultAsync(x => x.Id == key);

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
