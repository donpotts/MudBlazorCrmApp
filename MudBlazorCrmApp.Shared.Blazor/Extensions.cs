using MudBlazorCrmApp.Shared.Blazor.Authorization;
using MudBlazorCrmApp.Shared.Blazor.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace MudBlazorCrmApp.Shared.Blazor;

public static class Extensions
{
    public static void AddBlazorServices(this IServiceCollection services, string baseAddress)
    {
        services.AddScoped<AppService>();

        services.AddScoped(sp
            => new HttpClient { BaseAddress = new Uri(baseAddress) });

        services.AddAuthorizationCore();
        services.AddScoped<AuthenticationStateProvider, IdentityAuthenticationStateProvider>();
        services.AddScoped<ThemeService>();
        services.AddMudServices();
    }

    public static void AddBrowserStorageService(this IServiceCollection services)
    {
        services.AddBlazoredLocalStorage();
        services.AddScoped<IStorageService, BrowserStorageService>();
    }
}
