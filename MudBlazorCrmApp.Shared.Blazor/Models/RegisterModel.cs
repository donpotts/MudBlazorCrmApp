using System.ComponentModel.DataAnnotations;

namespace MudBlazorCrmApp.Shared.Blazor.Models;

public class RegisterModel
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
                              
    [Required]
    public string? Password { get; set; }
                              
    [Required]
    [Display(Name = "Confirm Password")]
    public string? ConfirmPassword { get; set; }
                              
    [Required]
    [Display(Name = "First Name")]
    public string? FirstName { get; set; }
                              
    [Required]
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }
                              
    public string? Title { get; set; }
                              
    [Display(Name = "Company Name")]
    public string? CompanyName { get; set; }
                              
    public string? Photo { get; set; }
}
