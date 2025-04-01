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
public class RewardController(ApplicationDbContext _ctx, ILogger<RewardController> _logger) : ControllerBase
{
    private readonly ILogger<RewardController> logger = _logger;
    private readonly ApplicationDbContext ctx = _ctx;

    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Reward>> Get()
    {
        return Ok(ctx.Reward);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Reward>> GetAsync(long key)
    {
        var reward = await ctx.Reward.FirstOrDefaultAsync(x => x.Id == key);

        if (reward == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(reward);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Reward>> PostAsync(Reward reward)
    {
        var record = await ctx.Reward.FindAsync(reward.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        await ctx.Reward.AddAsync(reward);

        await ctx.SaveChangesAsync();

        return Created($"/reward/{reward.Id}", reward);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Reward>> PutAsync(long key, Reward update)
    {
        var reward = await ctx.Reward.FirstOrDefaultAsync(x => x.Id == key);

        if (reward == null)
        {
            return NotFound();
        }

        ctx.Entry(reward).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(reward);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Reward>> PatchAsync(long key, Delta<Reward> delta)
    {
        var reward = await ctx.Reward.FirstOrDefaultAsync(x => x.Id == key);

        if (reward == null)
        {
            return NotFound();
        }

        delta.Patch(reward);

        await ctx.SaveChangesAsync();

        return Ok(reward);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var reward = await ctx.Reward.FindAsync(key);

        if (reward != null)
        {
            ctx.Reward.Remove(reward);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
