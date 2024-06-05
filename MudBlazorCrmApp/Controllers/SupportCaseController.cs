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
public class SupportCaseController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<SupportCase>> Get()
    {
        return Ok(ctx.SupportCase);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SupportCase>> GetAsync(long key)
    {
        var supportCase = await ctx.SupportCase.FirstOrDefaultAsync(x => x.Id == key);

        if (supportCase == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(supportCase);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<SupportCase>> PostAsync(SupportCase supportCase)
    {
        var record = await ctx.SupportCase.FindAsync(supportCase.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        await ctx.SupportCase.AddAsync(supportCase);

        await ctx.SaveChangesAsync();

        return Created($"/supportcase/{supportCase.Id}", supportCase);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SupportCase>> PutAsync(long key, SupportCase update)
    {
        var supportCase = await ctx.SupportCase.FirstOrDefaultAsync(x => x.Id == key);

        if (supportCase == null)
        {
            return NotFound();
        }

        ctx.Entry(supportCase).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(supportCase);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SupportCase>> PatchAsync(long key, Delta<SupportCase> delta)
    {
        var supportCase = await ctx.SupportCase.FirstOrDefaultAsync(x => x.Id == key);

        if (supportCase == null)
        {
            return NotFound();
        }

        delta.Patch(supportCase);

        await ctx.SaveChangesAsync();

        return Ok(supportCase);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var supportCase = await ctx.SupportCase.FindAsync(key);

        if (supportCase != null)
        {
            ctx.SupportCase.Remove(supportCase);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
