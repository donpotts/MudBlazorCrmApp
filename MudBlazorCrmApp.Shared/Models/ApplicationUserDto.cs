using System.ComponentModel.DataAnnotations;

namespace MudBlazorCrmApp.Shared.Models;

public class ApplicationUserDto
{
    public string? Id { get; set; }

    public string? Email { get; set; }

    [Display(Name = "Email Confirmed")]
    public bool EmailConfirmed { get; set; }

    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Phone Number Confirmed")]
    public bool PhoneNumberConfirmed { get; set; }

    [Display(Name = "First Name")]
    public string? FirstName { get; set; }

    [Display(Name = "Last Name")]
    public string? LastName { get; set; }
    
    public string? Title { get; set; }

    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }

    public string? Photo { get; set; }
}
