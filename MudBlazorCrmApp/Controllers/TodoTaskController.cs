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
public class TodoTaskController(ApplicationDbContext _ctx, ILogger<TodoTaskController> _logger) : ControllerBase
{
    private readonly ILogger<TodoTaskController> logger = _logger;
    private readonly ApplicationDbContext ctx = _ctx;

    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<TodoTask>> Get()
    {
        return Ok(ctx.TodoTask);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoTask>> GetAsync(long key)
    {
        var todoTask = await ctx.TodoTask.FirstOrDefaultAsync(x => x.Id == key);

        if (todoTask == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(todoTask);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<TodoTask>> PostAsync(TodoTask todoTask)
    {
        var record = await ctx.TodoTask.FindAsync(todoTask.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        await ctx.TodoTask.AddAsync(todoTask);

        await ctx.SaveChangesAsync();

        return Created($"/todotask/{todoTask.Id}", todoTask);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoTask>> PutAsync(long key, TodoTask update)
    {
        var todoTask = await ctx.TodoTask.FirstOrDefaultAsync(x => x.Id == key);

        if (todoTask == null)
        {
            return NotFound();
        }

        ctx.Entry(todoTask).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(todoTask);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoTask>> PatchAsync(long key, Delta<TodoTask> delta)
    {
        var todoTask = await ctx.TodoTask.FirstOrDefaultAsync(x => x.Id == key);

        if (todoTask == null)
        {
            return NotFound();
        }

        delta.Patch(todoTask);

        await ctx.SaveChangesAsync();

        return Ok(todoTask);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var todoTask = await ctx.TodoTask.FindAsync(key);

        if (todoTask != null)
        {
            ctx.TodoTask.Remove(todoTask);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
