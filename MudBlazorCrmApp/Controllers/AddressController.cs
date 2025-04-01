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
public class AddressController(ApplicationDbContext _ctx, ILogger<AddressController> _logger) : ControllerBase
{
    private readonly ILogger<AddressController> logger = _logger;
    private readonly ApplicationDbContext ctx = _ctx;

    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Address>> Get()
    {
        return Ok(ctx.Address);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Address>> GetAsync(long key)
    {
        var address = await ctx.Address.FirstOrDefaultAsync(x => x.Id == key);

        if (address == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(address);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Address>> PostAsync(Address address)
    {
        var record = await ctx.Address.FindAsync(address.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        await ctx.Address.AddAsync(address);

        await ctx.SaveChangesAsync();

        return Created($"/address/{address.Id}", address);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Address>> PutAsync(long key, Address update)
    {
        var address = await ctx.Address.FirstOrDefaultAsync(x => x.Id == key);

        if (address == null)
        {
            return NotFound();
        }

        ctx.Entry(address).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(address);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Address>> PatchAsync(long key, Delta<Address> delta)
    {
        var address = await ctx.Address.FirstOrDefaultAsync(x => x.Id == key);

        if (address == null)
        {
            return NotFound();
        }

        delta.Patch(address);

        await ctx.SaveChangesAsync();

        return Ok(address);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var address = await ctx.Address.FindAsync(key);

        if (address != null)
        {
            ctx.Address.Remove(address);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
