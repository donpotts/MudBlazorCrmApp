using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using MudBlazorCrmApp.Shared.Models;
using MudBlazorCrmApp.Shared.Blazor.Models;
using MudBlazorCrmApp.Shared.Blazor.Services;

namespace MudBlazorCrmApp.Shared.Blazor.Authorization;

public class IdentityAuthenticationStateProvider(HttpClient httpClient, IStorageService storageService) : AuthenticationStateProvider
{
    private AccessTokenResponse? accessTokenResponse;
    private DateTimeOffset? expiresAt;
    private static readonly ClaimsPrincipal anonymousUser = new(new ClaimsIdentity());
    private ClaimsPrincipal currentUser = anonymousUser;
    private bool hasLoaded = false;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (hasLoaded)
        {
            return new AuthenticationState(currentUser);
        }

        accessTokenResponse = await storageService.GetAsync<AccessTokenResponse>("IdentityAuthenticationStateProvider_accessTokenResponse");
        expiresAt = await storageService.GetAsync<DateTimeOffset>("IdentityAuthenticationStateProvider_expiresAt");
        var claims = await storageService.GetAsync<IDictionary<string, string>>("IdentityAuthenticationStateProvider_claims");
        
        if (accessTokenResponse == null || expiresAt == null || claims == null)
        {
            accessTokenResponse = null;
            expiresAt = null;
            claims = null;

            currentUser = anonymousUser;
        }
        else
        {
            ClaimsIdentity loggedInUserIdentity = new(claims.Select(x => new Claim(x.Key, x.Value)), "Identity");
            currentUser = new(loggedInUserIdentity);
        }

        hasLoaded = true;
        return new AuthenticationState(currentUser);
    }

    public async Task LoginAsync(LoginModel loginModel)
    {
        accessTokenResponse = null;
        expiresAt = null;
        currentUser = anonymousUser;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));

        var response = await httpClient.PostAsJsonAsync("/identity/login?useCookies=false", loginModel);

        var date = response.Headers.Date ?? DateTimeOffset.UtcNow;

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("The login attempt failed.");
            }
            else if (response.StatusCode != HttpStatusCode.NotFound)
            {
                string? message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }

            response.EnsureSuccessStatusCode();
        }

        accessTokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>()
            ?? throw new Exception("The login attempt failed.");

        expiresAt = date.AddSeconds(accessTokenResponse.ExpiresIn);

        HttpRequestMessage meRequest = new(HttpMethod.Get, "/api/user/@me");
        meRequest.Headers.Authorization = new("Bearer", accessTokenResponse.AccessToken);

        response = await httpClient.SendAsync(meRequest);

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("The login attempt failed.");
            }
            else if (response.StatusCode != HttpStatusCode.NotFound)
            {
                string? message = await response.Content.ReadAsStringAsync();
                throw new Exception(message);
            }

            response.EnsureSuccessStatusCode();
        }

        var me = await response.Content.ReadFromJsonAsync<ApplicationUserWithRolesDto>()
            ?? throw new Exception("The login attempt failed.");

        List<IDictionary<string, string>> claims = new List<IDictionary<string, string>>();
        Dictionary<string, string> claim = new Dictionary<string, string>();

        string name = string.Empty;

        if (!string.IsNullOrWhiteSpace(me.FirstName))
        {
            claim.Add(ClaimTypes.GivenName, me.FirstName);

            name = me.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(me.LastName))
        {
            claim.Add(ClaimTypes.Surname, me.LastName);

            if (!string.IsNullOrEmpty(name))
            {
                name += " ";
            }

            name += me.LastName;
        }

        if (string.IsNullOrEmpty(name))
        {
            name = "User";
        }

        claim.Add(ClaimTypes.Name, name);

        if (!string.IsNullOrEmpty(me.Email))
        {
            claim.Add(ClaimTypes.Email, me.Email);
        }

        foreach (var role in me.Roles ?? Enumerable.Empty<string>())
        {
            claim.Add(ClaimTypes.Role, role);
        }

        ClaimsIdentity loggedInUserIdentity = new(claim.Select(x => new Claim(x.Key, x.Value)), "Identity");
        currentUser = new ClaimsPrincipal(loggedInUserIdentity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));

        await storageService.SetAsync("IdentityAuthenticationStateProvider_accessTokenResponse", accessTokenResponse);
        await storageService.SetAsync("IdentityAuthenticationStateProvider_expiresAt", expiresAt);
        await storageService.SetAsync("IdentityAuthenticationStateProvider_claims", claim);
    }

    public async Task<string?> GetBearerTokenAsync()
    {
        if (accessTokenResponse == null || expiresAt == null)
        {
            return null;
        }

        if (expiresAt <= DateTimeOffset.UtcNow)
        {
            var response = await httpClient.PostAsJsonAsync(
                "/identity/refresh",
                new { accessTokenResponse.RefreshToken });

            var date = response.Headers.Date ?? DateTimeOffset.UtcNow;

            if (!response.IsSuccessStatusCode)
            {
                accessTokenResponse = null;
                expiresAt = null;
                currentUser = anonymousUser;
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new Exception("The token could not be refreshed.");
                }
                else if (response.StatusCode != HttpStatusCode.NotFound)
                {
                    string? message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }

                response.EnsureSuccessStatusCode();
            }

            accessTokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();

            if (accessTokenResponse == null)
            {
                expiresAt = null;
                currentUser = anonymousUser;
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));

                throw new Exception("The login attempt failed.");
            }

            expiresAt = date.AddSeconds(accessTokenResponse.ExpiresIn);
        }

        return accessTokenResponse.AccessToken;
    }

    public async Task LogoutAsync()
    {
        await storageService.RemoveAsync("IdentityAuthenticationStateProvider_accessTokenResponse");
        await storageService.RemoveAsync("IdentityAuthenticationStateProvider_expiresAt");
        await storageService.RemoveAsync("IdentityAuthenticationStateProvider_claims");

        accessTokenResponse = null;
        expiresAt = null;
        currentUser = anonymousUser;

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));
    }
}
