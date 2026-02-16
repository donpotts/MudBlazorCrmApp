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
public class EntityTagController(ApplicationDbContext _ctx, ILogger<EntityTagController> _logger) : ControllerBase
{
    private readonly ILogger<EntityTagController> logger = _logger;
    private readonly ApplicationDbContext ctx = _ctx;

    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<EntityTag>> Get()
    {
        return Ok(ctx.EntityTag.Include(x => x.Tag));
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EntityTag>> GetAsync(long key)
    {
        var entityTag = await ctx.EntityTag.Include(x => x.Tag).FirstOrDefaultAsync(x => x.Id == key);

        if (entityTag == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(entityTag);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<EntityTag>> PostAsync(EntityTag entityTag)
    {
        var record = await ctx.EntityTag.FindAsync(entityTag.Id);
        if (record != null)
        {
            return Conflict();
        }

        await ctx.EntityTag.AddAsync(entityTag);
        await ctx.SaveChangesAsync();

        return Created($"/entitytag/{entityTag.Id}", entityTag);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var entityTag = await ctx.EntityTag.FindAsync(key);

        if (entityTag != null)
        {
            ctx.EntityTag.Remove(entityTag);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
