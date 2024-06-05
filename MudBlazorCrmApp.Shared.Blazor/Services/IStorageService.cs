namespace MudBlazorCrmApp.Shared.Blazor.Services;

public interface IStorageService
{
    public Task<T?> GetAsync<T>(string key);

    public Task SetAsync<T>(string key, T value);

    public Task RemoveAsync(string key);

    public Task ClearAsync();
}
