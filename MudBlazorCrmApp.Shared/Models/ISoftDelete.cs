namespace MudBlazorCrmApp.Shared.Models;

/// <summary>
/// Interface for entities that support soft delete functionality
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Indicates whether the entity has been soft deleted
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Date and time when the entity was deleted
    /// </summary>
    DateTime? DeletedDate { get; set; }

    /// <summary>
    /// User ID who deleted the entity
    /// </summary>
    string? DeletedBy { get; set; }
}
