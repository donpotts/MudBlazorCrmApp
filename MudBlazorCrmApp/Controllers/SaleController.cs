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
public class SaleController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Sale>> Get()
    {
        return Ok(ctx.Sale);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Sale>> GetAsync(long key)
    {
        var sale = await ctx.Sale.FirstOrDefaultAsync(x => x.Id == key);

        if (sale == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(sale);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Sale>> PostAsync(Sale sale)
    {
        var record = await ctx.Sale.FindAsync(sale.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        await ctx.Sale.AddAsync(sale);

        await ctx.SaveChangesAsync();

        return Created($"/sale/{sale.Id}", sale);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Sale>> PutAsync(long key, Sale update)
    {
        var sale = await ctx.Sale.FirstOrDefaultAsync(x => x.Id == key);

        if (sale == null)
        {
            return NotFound();
        }

        ctx.Entry(sale).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(sale);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Sale>> PatchAsync(long key, Delta<Sale> delta)
    {
        var sale = await ctx.Sale.FirstOrDefaultAsync(x => x.Id == key);

        if (sale == null)
        {
            return NotFound();
        }

        delta.Patch(sale);

        await ctx.SaveChangesAsync();

        return Ok(sale);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var sale = await ctx.Sale.FindAsync(key);

        if (sale != null)
        {
            ctx.Sale.Remove(sale);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
