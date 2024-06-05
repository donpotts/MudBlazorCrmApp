using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Blazor.Services;

public class ProblemDetails
{
    [JsonPropertyName("detail")]
    public string? Detail { get; set; }

    [JsonPropertyName("instance")]
    public string? Instance { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object?>? Extensions { get; set; }
}
