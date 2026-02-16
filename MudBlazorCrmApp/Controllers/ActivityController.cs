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
public class ActivityController(ApplicationDbContext _ctx, ILogger<ActivityController> _logger) : ControllerBase
{
    private readonly ILogger<ActivityController> logger = _logger;
    private readonly ApplicationDbContext ctx = _ctx;

    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Activity>> Get()
    {
        return Ok(ctx.Activity.Include(x => x.Contact).Include(x => x.Customer).Include(x => x.Lead).Include(x => x.Opportunity));
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Activity>> GetAsync(long key)
    {
        var activity = await ctx.Activity.Include(x => x.Contact).Include(x => x.Customer).Include(x => x.Lead).Include(x => x.Opportunity).FirstOrDefaultAsync(x => x.Id == key);

        if (activity == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(activity);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Activity>> PostAsync(Activity activity)
    {
        var record = await ctx.Activity.FindAsync(activity.Id);
        if (record != null)
        {
            return Conflict();
        }

        await ctx.Activity.AddAsync(activity);

        await ctx.SaveChangesAsync();

        return Created($"/activity/{activity.Id}", activity);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Activity>> PutAsync(long key, Activity update)
    {
        var activity = await ctx.Activity.FirstOrDefaultAsync(x => x.Id == key);

        if (activity == null)
        {
            return NotFound();
        }

        ctx.Entry(activity).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(activity);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Activity>> PatchAsync(long key, Delta<Activity> delta)
    {
        var activity = await ctx.Activity.FirstOrDefaultAsync(x => x.Id == key);

        if (activity == null)
        {
            return NotFound();
        }

        delta.Patch(activity);

        await ctx.SaveChangesAsync();

        return Ok(activity);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var activity = await ctx.Activity.FindAsync(key);

        if (activity != null)
        {
            ctx.Activity.Remove(activity);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
