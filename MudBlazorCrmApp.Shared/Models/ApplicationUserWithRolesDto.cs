using System.ComponentModel.DataAnnotations;

namespace MudBlazorCrmApp.Shared.Models;

public class ApplicationUserWithRolesDto : ApplicationUserDto
{
    public List<string>? Roles { get; set; }
}
