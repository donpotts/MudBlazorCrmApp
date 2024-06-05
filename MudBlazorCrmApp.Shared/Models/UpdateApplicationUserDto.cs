using System.ComponentModel.DataAnnotations;

namespace MudBlazorCrmApp.Shared.Models;

public class UpdateApplicationUserDto
{
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    public string? Title { get; set; }

    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }

    public string? Photo { get; set; }
}
