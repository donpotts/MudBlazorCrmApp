using Blazored.LocalStorage;

namespace MudBlazorCrmApp.Shared.Blazor.Services;

public class BrowserStorageService(ILocalStorageService localStorage) : IStorageService
{
    public Task<T?> GetAsync<T>(string key)
    {
        return localStorage.GetItemAsync<T>(key).AsTask();
    }

    public Task SetAsync<T>(string key, T value)
    {
        return localStorage.SetItemAsync(key, value).AsTask();
    }

    public Task RemoveAsync(string key)
    {
        return localStorage.RemoveItemAsync(key).AsTask();
    }

    public Task ClearAsync()
    {
        return localStorage.ClearAsync().AsTask();
    }
}
