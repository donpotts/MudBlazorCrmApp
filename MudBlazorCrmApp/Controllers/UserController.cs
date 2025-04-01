using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using MudBlazorCrmApp.Models;
using MudBlazorCrmApp.Shared.Models;

namespace MudBlazorCrmApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("Fixed")]
public class UserController(UserManager<ApplicationUser> userManager, ILogger<UserController> logger) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<UserController> _logger = logger;

    [HttpGet("")]
    [EnableQuery]
    [Authorize(Roles = "Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public ActionResult<IQueryable<ApplicationUserDto>> Get()
    {
        var users = userManager.Users.Select(x => new ApplicationUserDto
        {
            Id = x.Id,
            Email = x.Email,
            EmailConfirmed = x.EmailConfirmed,
            PhoneNumber = x.PhoneNumber,
            PhoneNumberConfirmed = x.PhoneNumberConfirmed,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Title = x.Title,
            CompanyName = x.CompanyName,
            Photo = x.Photo,
        });

        foreach (var user in users)
        {
            _logger.LogInformation("User Info: Id={Id}, Email={Email}, FirstName={FirstName}, LastName={LastName}", user.Id, user.Email, user.FirstName, user.LastName);
        }
        
        return Ok(users);
    }

    [HttpGet("{key}")]
    [EnableQuery]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationUserWithRolesDto>> GetAsync(string key)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (key != userId && !User.IsInRole("Administrator"))
        {
            return Forbid();
        }

        var user = await userManager.FindByIdAsync(key);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(new ApplicationUserWithRolesDto
        {
            Id = user.Id,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Title = user.Title,
            CompanyName = user.CompanyName,
            Photo = user.Photo,
            Roles = [.. (await userManager.GetRolesAsync(user))],
        });
    }

    [HttpGet("@me")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationUserWithRolesDto>> GetMeAsync()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return NotFound();
        }

        return await GetAsync(userId);
    }

    [HttpPut("{key}")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationUserDto>> PutAsync(string key, UpdateApplicationUserDto update)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (key != userId && !User.IsInRole("Administrator"))
        {
            return Forbid();
        }

        var user = await userManager.FindByIdAsync(key);

        if (user == null)
        {
            return NotFound();
        }

        user.FirstName = update.FirstName;
        user.LastName = update.LastName;
        user.Title = update.Title;
        user.CompanyName = update.CompanyName;
        user.Photo = update.Photo;

        await userManager.UpdateAsync(user);

        return Ok(new ApplicationUserDto
        {
            Id = user.Id,
            Email = user.Email,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumber = user.PhoneNumber,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Title = user.Title,
            CompanyName = user.CompanyName,
            Photo = user.Photo,
        });
    }

    [HttpPut("@me")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationUserDto>> PutMeAsync(UpdateApplicationUserDto update)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return NotFound();
        }

        return await PutAsync(userId, update);
    }

    [HttpDelete("{key}")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(string key)
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (key != userId && !User.IsInRole("Administrator"))
        {
            return Forbid();
        }

        var user = await userManager.FindByIdAsync(key);

        if (user == null)
        {
            return NotFound();
        }

        if ((await userManager.GetUsersInRoleAsync("Administrator")).Count == 1
            && await userManager.IsInRoleAsync(user, "Administrator"))
        {
            ModelStateDictionary errors = new();
            errors.AddModelError("LastAdministrator", "The last administrator cannot be deleted.");

            return BadRequest(errors);
        }

        await userManager.DeleteAsync(user);

        return NoContent();
    }

    [HttpDelete("@me")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteMeAsync()
    {
        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return NotFound();
        }

        return await DeleteAsync(userId);
    }

    [HttpPut("{key}/roles")]
    [Authorize(Roles = "Administrator")]
    [ODataIgnored]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutRolesAsync(string key, IEnumerable<string> roles)
    {
        var user = await userManager.FindByIdAsync(key);

        if (user == null)
        {
            return NotFound();
        }

        var adminUsers = await userManager.GetUsersInRoleAsync("Administrator") ?? [];

        var userRoles = await userManager.GetRolesAsync(user) ?? [];

        var removeRoles = userRoles.Where(x => !roles.Contains(x));
        var addRoles = roles.Where(x => !userRoles.Contains(x));

        if (removeRoles.Contains("Administrator") && !adminUsers.Any(x => x.Id != key))
        {
            ModelStateDictionary errors = new();
            errors.AddModelError("LastAdministrator", "The last administrator cannot be removed from the Administrator role.");

            return BadRequest(errors);
        }

        await userManager.RemoveFromRolesAsync(user, removeRoles);
        await userManager.AddToRolesAsync(user, addRoles);

        return NoContent();
    }
}
