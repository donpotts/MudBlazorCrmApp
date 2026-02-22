using System.ComponentModel.DataAnnotations;

namespace MudBlazorCrmApp.Shared.Models;

public class EmailTemplate
{
    [Key]
    public long? Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Subject { get; set; }

    public string? Body { get; set; }

    public string? HtmlBody { get; set; }

    public string? Category { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedDate { get; set; }
}
