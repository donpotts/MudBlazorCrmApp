using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MudBlazorCrmApp.Services;

namespace MudBlazorCrmApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Fixed")]
public class ImageController(ImageService imageService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostAsync(IFormFile image)
    {
        var extension = image.ContentType switch
        {
            "image/png" => ".png",
            "image/jpeg" => ".jpg",
            _ => null
        };

        try
        {
            using var stream = image.OpenReadStream();
            return Ok($"\"{await imageService.SaveToUploadsAsync(extension, stream)}\"");
        }
        catch (ArgumentException ex)
        {
            return BadRequest($"\"{ex.Message}\"");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"\"{ex.Message}\"");
        }
    }
}
