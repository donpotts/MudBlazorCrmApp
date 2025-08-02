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
public class VendorController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Vendor>> Get()
    {
        return Ok(ctx.Vendor.Include(x => x.Address).Include(x => x.Product).Include(x => x.Service));
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Vendor>> GetAsync(long key)
    {
        var vendor = await ctx.Vendor.Include(x => x.Address).Include(x => x.Product).Include(x => x.Service).FirstOrDefaultAsync(x => x.Id == key);

        if (vendor == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(vendor);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Vendor>> PostAsync(Vendor vendor)
    {
        var record = await ctx.Vendor.FindAsync(vendor.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        var address = vendor.Address;
        vendor.Address = null;

        var product = vendor.Product;
        vendor.Product = null;

        var service = vendor.Service;
        vendor.Service = null;

        await ctx.Vendor.AddAsync(vendor);

        if (address != null)
        {
            var newValues = await ctx.Address.Where(x => address.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            vendor.Address = [..newValues];
        }

        if (product != null)
        {
            var newValues = await ctx.Product.Where(x => product.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            vendor.Product = [..newValues];
        }

        if (service != null)
        {
            var newValues = await ctx.Service.Where(x => service.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            vendor.Service = [..newValues];
        }

        await ctx.SaveChangesAsync();

        return Created($"/vendor/{vendor.Id}", vendor);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Vendor>> PutAsync(long key, Vendor update)
    {
        var vendor = await ctx.Vendor.Include(x => x.Address).Include(x => x.Product).Include(x => x.Service).FirstOrDefaultAsync(x => x.Id == key);

        if (vendor == null)
        {
            return NotFound();
        }

        ctx.Entry(vendor).CurrentValues.SetValues(update);

        if (update.Address != null)
        {
            var updateValues = update.Address.Select(x => x.Id);
            vendor.Address ??= [];
            vendor.Address.RemoveAll(x => !updateValues.Contains(x.Id));
            var addValues = updateValues.Where(x => !vendor.Address.Select(y => y.Id).Contains(x));
            var newValues = await ctx.Address.Where(x => addValues.Contains(x.Id)).ToListAsync();
            vendor.Address.AddRange(newValues);
        }

        if (update.Product != null)
        {
            var updateValues = update.Product.Select(x => x.Id);
            vendor.Product ??= [];
            vendor.Product.RemoveAll(x => !updateValues.Contains(x.Id));
            var addValues = updateValues.Where(x => !vendor.Product.Select(y => y.Id).Contains(x));
            var newValues = await ctx.Product.Where(x => addValues.Contains(x.Id)).ToListAsync();
            vendor.Product.AddRange(newValues);
        }

        if (update.Service != null)
        {
            var updateValues = update.Service.Select(x => x.Id);
            vendor.Service ??= [];
            vendor.Service.RemoveAll(x => !updateValues.Contains(x.Id));
            var addValues = updateValues.Where(x => !vendor.Service.Select(y => y.Id).Contains(x));
            var newValues = await ctx.Service.Where(x => addValues.Contains(x.Id)).ToListAsync();
            vendor.Service.AddRange(newValues);
        }

        await ctx.SaveChangesAsync();

        return Ok(vendor);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Vendor>> PatchAsync(long key, Delta<Vendor> delta)
    {
        var vendor = await ctx.Vendor.Include(x => x.Address).Include(x => x.Product).Include(x => x.Service).FirstOrDefaultAsync(x => x.Id == key);

        if (vendor == null)
        {
            return NotFound();
        }

        delta.Patch(vendor);

        await ctx.SaveChangesAsync();

        return Ok(vendor);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var vendor = await ctx.Vendor.FindAsync(key);

        if (vendor != null)
        {
            ctx.Vendor.Remove(vendor);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
