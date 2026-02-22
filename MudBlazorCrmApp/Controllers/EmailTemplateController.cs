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
public class EmailTemplateController(ApplicationDbContext ctx) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IQueryable<EmailTemplate>> Get()
    {
        return Ok(ctx.EmailTemplate.AsQueryable());
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmailTemplate>> GetAsync(long key)
    {
        var record = await ctx.EmailTemplate.FindAsync(key);
        if (record == null) return NotFound();
        return Ok(record);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<EmailTemplate>> PostAsync([FromBody] EmailTemplate record)
    {
        record.CreatedDate = DateTime.UtcNow;
        ctx.EmailTemplate.Add(record);
        await ctx.SaveChangesAsync();
        return Created($"/api/emailtemplate/{record.Id}", record);
    }

    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmailTemplate>> PutAsync(long key, [FromBody] EmailTemplate update)
    {
        var record = await ctx.EmailTemplate.FindAsync(key);
        if (record == null) return NotFound();

        record.Name = update.Name;
        record.Subject = update.Subject;
        record.Body = update.Body;
        record.HtmlBody = update.HtmlBody;
        record.Category = update.Category;
        record.Description = update.Description;
        record.IsActive = update.IsActive;
        record.ModifiedDate = DateTime.UtcNow;

        await ctx.SaveChangesAsync();
        return Ok(record);
    }

    [HttpPatch("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EmailTemplate>> PatchAsync(long key, [FromBody] Delta<EmailTemplate> delta)
    {
        var record = await ctx.EmailTemplate.FindAsync(key);
        if (record == null) return NotFound();

        delta.Patch(record);
        record.ModifiedDate = DateTime.UtcNow;
        await ctx.SaveChangesAsync();
        return Ok(record);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync(long key)
    {
        var record = await ctx.EmailTemplate.FindAsync(key);
        if (record == null) return NotFound();

        ctx.EmailTemplate.Remove(record);
        await ctx.SaveChangesAsync();
        return NoContent();
    }
}
