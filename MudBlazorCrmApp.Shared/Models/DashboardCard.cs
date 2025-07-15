using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Models;

public class DashboardCard
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [JsonPropertyName("x")] public int X { get; set; }
    [JsonPropertyName("y")] public int Y { get; set; }
    [JsonPropertyName("w")] public int W { get; set; }
    [JsonPropertyName("h")] public int H { get; set; }

    // This property tells GridStack to create the inner .grid-stack-item-content div.
    [JsonPropertyName("content")]
    public string Content { get; set; } = "";

    [JsonIgnore] public string Title { get; set; } = "New Card";
    [JsonIgnore] public string CardType { get; set; } = "default";
}