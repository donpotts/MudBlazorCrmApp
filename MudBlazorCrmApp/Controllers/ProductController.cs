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
public class ProductController(ApplicationDbContext _ctx, ILogger<ProductController> _logger) : ControllerBase
{
    private readonly ILogger<ProductController> logger = _logger;
    private readonly ApplicationDbContext ctx = _ctx;

    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<IQueryable<Product>> Get()
    {
        return Ok(ctx.Product.Include(x => x.ProductCategory));
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetAsync(long key)
    {
        var product = await ctx.Product.Include(x => x.ProductCategory).FirstOrDefaultAsync(x => x.Id == key);

        if (product == null)
        {
            return NotFound();
        }
        else
        {
            return Ok(product);
        }
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Product>> PostAsync(Product product)
    {
        var record = await ctx.Product.FindAsync(product.Id);
        if (record != null)
        {
            return Conflict();
        }
    
        var productCategory = product.ProductCategory;
        product.ProductCategory = null;

        await ctx.Product.AddAsync(product);

        if (productCategory != null)
        {
            var newValues = await ctx.ProductCategory.Where(x => productCategory.Select(y => y.Id).Contains(x.Id)).ToListAsync();
            product.ProductCategory = [..newValues];
        }

        await ctx.SaveChangesAsync();

        return Created($"/product/{product.Id}", product);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> PutAsync(long key, Product update)
    {
        var product = await ctx.Product.Include(x => x.ProductCategory).FirstOrDefaultAsync(x => x.Id == key);

        if (product == null)
        {
            return NotFound();
        }

        ctx.Entry(product).CurrentValues.SetValues(update);

        if (update.ProductCategory != null)
        {
            var updateValues = update.ProductCategory.Select(x => x.Id);
            product.ProductCategory ??= [];
            product.ProductCategory.RemoveAll(x => !updateValues.Contains(x.Id));
            var addValues = updateValues.Where(x => !product.ProductCategory.Select(y => y.Id).Contains(x));
            var newValues = await ctx.ProductCategory.Where(x => addValues.Contains(x.Id)).ToListAsync();
            product.ProductCategory.AddRange(newValues);
        }

        await ctx.SaveChangesAsync();

        return Ok(product);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> PatchAsync(long key, Delta<Product> delta)
    {
        var product = await ctx.Product.Include(x => x.ProductCategory).FirstOrDefaultAsync(x => x.Id == key);

        if (product == null)
        {
            return NotFound();
        }

        delta.Patch(product);

        await ctx.SaveChangesAsync();

        return Ok(product);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(long key)
    {
        var product = await ctx.Product.FindAsync(key);

        if (product != null)
        {
            ctx.Product.Remove(product);
            await ctx.SaveChangesAsync();
        }

        return NoContent();
    }
}
