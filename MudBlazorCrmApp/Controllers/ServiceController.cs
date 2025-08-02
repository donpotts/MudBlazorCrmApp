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
public class ServiceController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Service>> Get()
    {
        return Ok(ctx.Service.Include(x => x.ServiceCategory));
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Service>> GetAsync(long key)
    {
        var service = await ctx.Service.Include(x => x.ServiceCategory).FirstOrDefaultAsync(x => x.Id == key);

        if (service == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(service);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Service>> PostAsync(Service service)
    {
        var record = await ctx.Service.FindAsync(service.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        var serviceCategory = service.ServiceCategory;
        service.ServiceCategory = null;

        await ctx.Service.AddAsync(service);

        if (serviceCategory != null)
        {
            var newValues = await ctx.ServiceCategory.Where(x => serviceCategory.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            service.ServiceCategory = [..newValues];
        }

        await ctx.SaveChangesAsync();

        return Created($"/service/{service.Id}", service);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Service>> PutAsync(long key, Service update)
    {
        var service = await ctx.Service.Include(x => x.ServiceCategory).FirstOrDefaultAsync(x => x.Id == key);

        if (service == null)
        {
            return NotFound();
        }

        ctx.Entry(service).CurrentValues.SetValues(update);

        if (update.ServiceCategory != null)
        {
            var updateValues = update.ServiceCategory.Select(x => x.Id);
            service.ServiceCategory ??= [];
            service.ServiceCategory.RemoveAll(x => !updateValues.Contains(x.Id));
            var addValues = updateValues.Where(x => !service.ServiceCategory.Select(y => y.Id).Contains(x));
            var newValues = await ctx.ServiceCategory.Where(x => addValues.Contains(x.Id)).ToListAsync();
            service.ServiceCategory.AddRange(newValues);
        }

        await ctx.SaveChangesAsync();

        return Ok(service);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Service>> PatchAsync(long key, Delta<Service> delta)
    {
        var service = await ctx.Service.Include(x => x.ServiceCategory).FirstOrDefaultAsync(x => x.Id == key);

        if (service == null)
        {
            return NotFound();
        }

        delta.Patch(service);

        await ctx.SaveChangesAsync();

        return Ok(service);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var service = await ctx.Service.FindAsync(key);

        if (service != null)
        {
            ctx.Service.Remove(service);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
