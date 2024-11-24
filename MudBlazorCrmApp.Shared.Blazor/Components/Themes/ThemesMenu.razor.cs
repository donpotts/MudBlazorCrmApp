using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazorCrmApp.Shared.Models;
using MudBlazor;

namespace MudBlazorCrmApp.Shared.Blazor.Components.Themes;

public partial class ThemesMenu
{
    //MudOverlay not working!
    
    private readonly List<string> _primaryColors = new()
    {
        "#594AE2",
        Colors.Green.Default,
        Colors.Blue.Default,
        Colors.BlueGray.Default,
        Colors.Purple.Default,
        Colors.Orange.Default,
        Colors.Red.Default,
        Colors.Brown.Default,
        Colors.Cyan.Default,
        Colors.DeepPurple.Default,
        Colors.Yellow.Default,
        Colors.Pink.Default,
        Colors.Lime.Default,
        Colors.Indigo.Default,
        Colors.Teal.Default,
        Colors.Amber.Default,
    };

    [EditorRequired] [Parameter] public bool ThemingDrawerOpen { get; set; }
    [EditorRequired] [Parameter] public EventCallback<bool> ThemingDrawerOpenChanged { get; set; }
    [EditorRequired] [Parameter] public ThemeManagerModel ThemeManager { get; set; }
    [EditorRequired] [Parameter] public EventCallback<ThemeManagerModel> ThemeManagerChanged { get; set; }

    private async Task UpdateThemePrimaryColor(string color)
    {
        ThemeManager.PrimaryColor = color;
        await ThemeManagerChanged.InvokeAsync(ThemeManager);
    }

    private async Task ToggleDarkLightMode(bool isDarkMode)
    {
        ThemeManager.IsDarkMode = isDarkMode;
        await ThemeManagerChanged.InvokeAsync(ThemeManager);
    }
}