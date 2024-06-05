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
public class ServiceCategoryController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<ServiceCategory>> Get()
    {
        return Ok(ctx.ServiceCategory);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceCategory>> GetAsync(long key)
    {
        var serviceCategory = await ctx.ServiceCategory.FirstOrDefaultAsync(x => x.Id == key);

        if (serviceCategory == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(serviceCategory);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ServiceCategory>> PostAsync(ServiceCategory serviceCategory)
    {
        var record = await ctx.ServiceCategory.FindAsync(serviceCategory.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        await ctx.ServiceCategory.AddAsync(serviceCategory);

        await ctx.SaveChangesAsync();

        return Created($"/servicecategory/{serviceCategory.Id}", serviceCategory);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceCategory>> PutAsync(long key, ServiceCategory update)
    {
        var serviceCategory = await ctx.ServiceCategory.FirstOrDefaultAsync(x => x.Id == key);

        if (serviceCategory == null)
        {
            return NotFound();
        }

        ctx.Entry(serviceCategory).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(serviceCategory);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ServiceCategory>> PatchAsync(long key, Delta<ServiceCategory> delta)
    {
        var serviceCategory = await ctx.ServiceCategory.FirstOrDefaultAsync(x => x.Id == key);

        if (serviceCategory == null)
        {
            return NotFound();
        }

        delta.Patch(serviceCategory);

        await ctx.SaveChangesAsync();

        return Ok(serviceCategory);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var serviceCategory = await ctx.ServiceCategory.FindAsync(key);

        if (serviceCategory != null)
        {
            ctx.ServiceCategory.Remove(serviceCategory);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
