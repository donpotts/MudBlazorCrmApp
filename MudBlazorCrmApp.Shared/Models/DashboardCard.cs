using System.Text.Json.Serialization;

namespace MudBlazorCrmApp.Shared.Blazor.Models
{
    public class DashboardCard
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string CardType { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }

        // Chart-specific properties
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ChartConfig? ChartConfig { get; set; }
    }

    public class ChartConfig
    {
        public string Type { get; set; } = "line"; // line, bar, doughnut, pie, etc.
        public List<string> Labels { get; set; } = new();
        public List<ChartDataset> Datasets { get; set; } = new();
        public ChartOptions? Options { get; set; }
    }

    public class ChartDataset
    {
        public string Label { get; set; } = string.Empty;
        public List<double> Data { get; set; } = new();
        public string? BorderColor { get; set; }
        public string? BackgroundColor { get; set; }
        public List<string>? BackgroundColors { get; set; } // For multi-color charts like doughnut
        public double? Tension { get; set; } // For line charts
        public bool? Fill { get; set; }
    }

    public class ChartOptions
    {
        public bool Responsive { get; set; } = true;
        public bool MaintainAspectRatio { get; set; } = false;
        public ChartLegend? Legend { get; set; }
        public Dictionary<string, object>? Scales { get; set; }
    }

    public class ChartLegend
    {
        public string Position { get; set; } = "top";
        public bool Display { get; set; } = true;
    }
}