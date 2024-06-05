namespace MudBlazorCrmApp.Shared.Blazor.Authorization;

public sealed class InfoResponse
{
    public required string Email { get; init; }

    public required bool IsEmailConfirmed { get; init; }

    public required IDictionary<string, string> Claims { get; init; }
}
