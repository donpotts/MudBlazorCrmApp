namespace MudBlazorCrmApp.Shared.Models;

/// <summary>
/// Interface for entities that track audit information (created/modified by/when)
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Date and time when the entity was created
    /// </summary>
    DateTime? CreatedDate { get; set; }

    /// <summary>
    /// User ID who created the entity
    /// </summary>
    string? CreatedBy { get; set; }

    /// <summary>
    /// Date and time when the entity was last modified
    /// </summary>
    DateTime ModifiedDate { get; set; }

    /// <summary>
    /// User ID who last modified the entity
    /// </summary>
    string? ModifiedBy { get; set; }
}
