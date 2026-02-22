using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using MudBlazorCrmApp.Data;
using MudBlazorCrmApp.Shared.Models;
using System.Security.Claims;

namespace MudBlazorCrmApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Fixed")]
public class AttachmentController(ApplicationDbContext ctx, IWebHostEnvironment environment) : ControllerBase
{
    [HttpGet("")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IQueryable<Attachment>> Get()
    {
        return Ok(ctx.Attachment.OrderByDescending(a => a.UploadedDate));
    }

    [HttpGet("entity/{entityType}/{entityId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Attachment>>> GetByEntity(string entityType, long entityId)
    {
        var attachments = await ctx.Attachment
            .Where(a => a.EntityType == entityType && a.EntityId == entityId)
            .OrderByDescending(a => a.UploadedDate)
            .ToListAsync();

        return Ok(attachments);
    }

    [HttpPost("upload")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [RequestSizeLimit(20 * 1024 * 1024)] // 20MB
    public async Task<ActionResult<Attachment>> Upload(IFormFile file, [FromForm] string entityType, [FromForm] long entityId, [FromForm] string? description)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        var uploadPath = Path.Combine(environment.WebRootPath, "upload", "files");
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var ext = Path.GetExtension(file.FileName);
        var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        var storedFileName = $"{randomName}{ext}";
        var filePath = Path.Combine(uploadPath, storedFileName);

        using (var stream = new FileStream(filePath, FileMode.CreateNew))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new Attachment
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            FileSize = file.Length,
            FilePath = $"/upload/files/{storedFileName}",
            EntityType = entityType,
            EntityId = entityId,
            UploadedDate = DateTime.UtcNow,
            UploadedBy = User.FindFirstValue(ClaimTypes.NameIdentifier),
            Description = description,
        };

        await ctx.Attachment.AddAsync(attachment);
        await ctx.SaveChangesAsync();

        return Created($"/api/attachment/{attachment.Id}", attachment);
    }

    [HttpDelete("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteAsync(long key)
    {
        var record = await ctx.Attachment.FindAsync(key);
        if (record == null) return NotFound();

        // Delete physical file
        if (!string.IsNullOrEmpty(record.FilePath))
        {
            var fullPath = Path.Combine(environment.WebRootPath, record.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }

        ctx.Attachment.Remove(record);
        await ctx.SaveChangesAsync();
        return NoContent();
    }
}
