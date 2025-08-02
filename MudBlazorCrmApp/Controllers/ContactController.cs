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
public class ContactController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Contact>> Get()
    {
        return Ok(ctx.Contact.Include(x => x.Address).Include(x => x.Reward));
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Contact>> GetAsync(long key)
    {
        var contact = await ctx.Contact.Include(x => x.Address).Include(x => x.Reward).FirstOrDefaultAsync(x => x.Id == key);

        if (contact == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(contact);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Contact>> PostAsync(Contact contact)
    {
        var record = await ctx.Contact.FindAsync(contact.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        var address = contact.Address;
        contact.Address = null;

        var reward = contact.Reward;
        contact.Reward = null;

        await ctx.Contact.AddAsync(contact);

        if (address != null)
        {
            var newValues = await ctx.Address.Where(x => address.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            contact.Address = [..newValues];
        }

        if (reward != null)
        {
            var newValues = await ctx.Reward.Where(x => reward.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            contact.Reward = [..newValues];
        }

        await ctx.SaveChangesAsync();

        return Created($"/contact/{contact.Id}", contact);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Contact>> PutAsync(long key, Contact update)
    {
        var contact = await ctx.Contact.Include(x => x.Address).Include(x => x.Reward).FirstOrDefaultAsync(x => x.Id == key);

        if (contact == null)
        {
            return NotFound();
        }

        ctx.Entry(contact).CurrentValues.SetValues(update);

        if (update.Address != null)
        {
            var updateValues = update.Address.Select(x => x.Id);
            contact.Address ??= [];
            contact.Address.RemoveAll(x => !updateValues.Contains(x.Id));
            var addValues = updateValues.Where(x => !contact.Address.Select(y => y.Id).Contains(x));
            var newValues = await ctx.Address.Where(x => addValues.Contains(x.Id)).ToListAsync();
            contact.Address.AddRange(newValues);
        }

        if (update.Reward != null)
        {
            var updateValues = update.Reward.Select(x => x.Id);
            contact.Reward ??= [];
            contact.Reward.RemoveAll(x => !updateValues.Contains(x.Id));
            var addValues = updateValues.Where(x => !contact.Reward.Select(y => y.Id).Contains(x));
            var newValues = await ctx.Reward.Where(x => addValues.Contains(x.Id)).ToListAsync();
            contact.Reward.AddRange(newValues);
        }

        await ctx.SaveChangesAsync();

        return Ok(contact);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Contact>> PatchAsync(long key, Delta<Contact> delta)
    {
        var contact = await ctx.Contact.Include(x => x.Address).Include(x => x.Reward).FirstOrDefaultAsync(x => x.Id == key);

        if (contact == null)
        {
            return NotFound();
        }

        delta.Patch(contact);

        await ctx.SaveChangesAsync();

        return Ok(contact);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var contact = await ctx.Contact.FindAsync(key);

        if (contact != null)
        {
            ctx.Contact.Remove(contact);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
