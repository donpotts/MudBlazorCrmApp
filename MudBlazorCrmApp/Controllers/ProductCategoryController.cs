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
public class ProductCategoryController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<ProductCategory>> Get()
    {
        return Ok(ctx.ProductCategory);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductCategory>> GetAsync(long key)
    {
        var productCategory = await ctx.ProductCategory.FirstOrDefaultAsync(x => x.Id == key);

        if (productCategory == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(productCategory);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ProductCategory>> PostAsync(ProductCategory productCategory)
    {
        var record = await ctx.ProductCategory.FindAsync(productCategory.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        await ctx.ProductCategory.AddAsync(productCategory);

        await ctx.SaveChangesAsync();

        return Created($"/productcategory/{productCategory.Id}", productCategory);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductCategory>> PutAsync(long key, ProductCategory update)
    {
        var productCategory = await ctx.ProductCategory.FirstOrDefaultAsync(x => x.Id == key);

        if (productCategory == null)
        {
            return NotFound();
        }

        ctx.Entry(productCategory).CurrentValues.SetValues(update);

        await ctx.SaveChangesAsync();

        return Ok(productCategory);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductCategory>> PatchAsync(long key, Delta<ProductCategory> delta)
    {
        var productCategory = await ctx.ProductCategory.FirstOrDefaultAsync(x => x.Id == key);

        if (productCategory == null)
        {
            return NotFound();
        }

        delta.Patch(productCategory);

        await ctx.SaveChangesAsync();

        return Ok(productCategory);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var productCategory = await ctx.ProductCategory.FindAsync(key);

        if (productCategory != null)
        {
            ctx.ProductCategory.Remove(productCategory);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
