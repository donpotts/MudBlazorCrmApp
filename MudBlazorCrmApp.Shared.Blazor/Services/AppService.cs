using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Web;
using MudBlazorCrmApp.Shared.Blazor.Authorization;
using MudBlazorCrmApp.Shared.Blazor.Models;
using MudBlazorCrmApp.Shared.Models;

namespace MudBlazorCrmApp.Shared.Blazor.Services;

public class AppService(
    HttpClient httpClient,
    AuthenticationStateProvider authenticationStateProvider)
{
    private readonly IdentityAuthenticationStateProvider authenticationStateProvider
            = authenticationStateProvider as IdentityAuthenticationStateProvider
                ?? throw new InvalidOperationException();

    private static async Task HandleResponseErrorsAsync(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode
            && response.StatusCode != HttpStatusCode.Unauthorized
            && response.StatusCode != HttpStatusCode.NotFound)
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception(message);
        }

        response.EnsureSuccessStatusCode();
    }

    public class ODataResult<T>
    {
        [JsonPropertyName("@odata.count")]
        public int? Count { get; set; }

        public IEnumerable<T>? Value { get; set; }
    }

    public async Task<ODataResult<T>?> GetODataAsync<T>(
            string entity,
            int? top = null,
            int? skip = null,
            string? orderby = null,
            string? filter = null,
            bool count = false,
            string? expand = null)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        var queryString = HttpUtility.ParseQueryString(string.Empty);

        if (top.HasValue)
        {
            queryString.Add("$top", top.ToString());
        }

        if (skip.HasValue)
        {
            queryString.Add("$skip", skip.ToString());
        }

        if (!string.IsNullOrEmpty(orderby))
        {
            queryString.Add("$orderby", orderby);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            queryString.Add("$filter", filter);
        }

        if (count)
        {
            queryString.Add("$count", "true");
        }

        if (!string.IsNullOrEmpty(expand))
        {
            queryString.Add("$expand", expand);
        }

        var uri = $"/odata/{entity}?{queryString}";

        HttpRequestMessage request = new(HttpMethod.Get, uri);
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ODataResult<T>>();
    }


    public async Task<Dictionary<string, List<string>>> RegisterUserAsync(RegisterModel registerModel)
    {
        var response = await httpClient.PostAsJsonAsync(
            "/identity/register",
            new { registerModel.Email, registerModel.Password });

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var json = await response.Content.ReadAsStringAsync();

            var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            return problemDetails?.Errors != null
                ? problemDetails.Errors
                : throw new Exception("Bad Request");
        }

        response.EnsureSuccessStatusCode();

        response = await httpClient.PostAsJsonAsync(
            "/identity/login",
            new { registerModel.Email, registerModel.Password });

        response.EnsureSuccessStatusCode();

        var accessTokenResponse = await response.Content.ReadFromJsonAsync<AccessTokenResponse>()
            ?? throw new Exception("Failed to authenticate");

        HttpRequestMessage request = new(HttpMethod.Put, "/api/user/@me");
        request.Headers.Authorization = new("Bearer", accessTokenResponse.AccessToken);
        request.Content = JsonContent.Create(new UpdateApplicationUserDto
        {
            FirstName = registerModel.FirstName,
            LastName = registerModel.LastName,
            Title = registerModel.Title,
            CompanyName = registerModel.CompanyName,
            Photo = registerModel.Photo,
        });

        response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        return [];
    }

    public async Task<ApplicationUserDto[]?> ListUserAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/user");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ApplicationUserDto[]>();
    }

    public Task<ODataResult<ApplicationUserDto>?> ListUserODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<ApplicationUserDto>("User", top, skip, orderby, filter, count, expand);
    }

    public async Task<ApplicationUserWithRolesDto?> GetUserByIdAsync(string id)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/user/{id}");
        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ApplicationUserWithRolesDto>();
    }

    public async Task UpdateUserAsync(string id, UpdateApplicationUserDto data)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/user/{id}");
        request.Headers.Add("Authorization", $"Bearer {token}");
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task DeleteUserAsync(string id)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/user/{id}");
        request.Headers.Add("Authorization", $"Bearer {token}");

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Customer[]?> ListCustomerAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/customer");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Customer[]>();
    }

    public Task<ODataResult<Customer>?> ListCustomerODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<Customer>("Customer", top, skip, orderby, filter, count, expand);
    }

    public async Task<Customer?> GetCustomerByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/customer/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Customer>();
    }

    public async Task UpdateCustomerAsync(long key, Customer data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/customer/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Customer?> InsertCustomerAsync(Customer data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/customer");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Customer>();
    }

    public async Task DeleteCustomerAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/customer/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Address[]?> ListAddressAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/address");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Address[]>();
    }

    public Task<ODataResult<Address>?> ListAddressODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<Address>("Address", top, skip, orderby, filter, count, expand);
    }

    public async Task<Address?> GetAddressByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/address/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Address>();
    }

    public async Task UpdateAddressAsync(long key, Address data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/address/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Address?> InsertAddressAsync(Address data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/address");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Address>();
    }

    public async Task DeleteAddressAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/address/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<ProductCategory[]?> ListProductCategoryAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/productcategory");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ProductCategory[]>();
    }

    public Task<ODataResult<ProductCategory>?> ListProductCategoryODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<ProductCategory>("ProductCategory", top, skip, orderby, filter, count, expand);
    }

    public async Task<ProductCategory?> GetProductCategoryByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/productcategory/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ProductCategory>();
    }

    public async Task UpdateProductCategoryAsync(long key, ProductCategory data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/productcategory/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<ProductCategory?> InsertProductCategoryAsync(ProductCategory data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/productcategory");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ProductCategory>();
    }

    public async Task DeleteProductCategoryAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/productcategory/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<ServiceCategory[]?> ListServiceCategoryAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/servicecategory");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ServiceCategory[]>();
    }

    public Task<ODataResult<ServiceCategory>?> ListServiceCategoryODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<ServiceCategory>("ServiceCategory", top, skip, orderby, filter, count, expand);
    }

    public async Task<ServiceCategory?> GetServiceCategoryByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/servicecategory/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ServiceCategory>();
    }

    public async Task UpdateServiceCategoryAsync(long key, ServiceCategory data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/servicecategory/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<ServiceCategory?> InsertServiceCategoryAsync(ServiceCategory data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/servicecategory");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ServiceCategory>();
    }

    public async Task DeleteServiceCategoryAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/servicecategory/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Contact[]?> ListContactAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/contact");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Contact[]>();
    }

    public Task<ODataResult<Contact>?> ListContactODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<Contact>("Contact", top, skip, orderby, filter, count, expand);
    }

    public async Task<Contact?> GetContactByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/contact/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Contact>();
    }

    public async Task UpdateContactAsync(long key, Contact data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/contact/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Contact?> InsertContactAsync(Contact data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/contact");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Contact>();
    }

    public async Task DeleteContactAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/contact/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Opportunity[]?> ListOpportunityAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/opportunity");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Opportunity[]>();
    }

    public Task<ODataResult<Opportunity>?> ListOpportunityODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<Opportunity>("Opportunity", top, skip, orderby, filter, count, expand);
    }

    public async Task<Opportunity?> GetOpportunityByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/opportunity/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Opportunity>();
    }

    public async Task UpdateOpportunityAsync(long key, Opportunity data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/opportunity/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Opportunity?> InsertOpportunityAsync(Opportunity data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/opportunity");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Opportunity>();
    }

    public async Task DeleteOpportunityAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/opportunity/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Lead[]?> ListLeadAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/lead");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Lead[]>();
    }

    public Task<ODataResult<Lead>?> ListLeadODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<Lead>("Lead", top, skip, orderby, filter, count, expand);
    }

    public async Task<Lead?> GetLeadByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/lead/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Lead>();
    }

    public async Task UpdateLeadAsync(long key, Lead data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/lead/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Lead?> InsertLeadAsync(Lead data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/lead");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Lead>();
    }

    public async Task DeleteLeadAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/lead/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Product[]?> ListProductAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/product");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Product[]>();
    }

    public Task<ODataResult<Product>?> ListProductODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<Product>("Product", top, skip, orderby, filter, count, expand);
    }

    public async Task<Product?> GetProductByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/product/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Product>();
    }

    public async Task UpdateProductAsync(long key, Product data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/product/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Product?> InsertProductAsync(Product data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/product");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Product>();
    }

    public async Task DeleteProductAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/product/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Service[]?> ListServiceAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/service");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Service[]>();
    }

    public Task<ODataResult<Service>?> ListServiceODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<Service>("Service", top, skip, orderby, filter, count, expand);
    }

    public async Task<Service?> GetServiceByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/service/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Service>();
    }

    public async Task UpdateServiceAsync(long key, Service data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/service/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Service?> InsertServiceAsync(Service data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/service");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Service>();
    }

    public async Task DeleteServiceAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/service/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Sale[]?> ListSaleAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/sale");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Sale[]>();
    }

    public Task<ODataResult<Sale>?> ListSaleODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<Sale>("Sale", top, skip, orderby, filter, count, expand);
    }

    public async Task<Sale?> GetSaleByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/sale/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Sale>();
    }

    public async Task UpdateSaleAsync(long key, Sale data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/sale/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Sale?> InsertSaleAsync(Sale data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/sale");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Sale>();
    }

    public async Task DeleteSaleAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/sale/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Vendor[]?> ListVendorAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/vendor");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Vendor[]>();
    }

    public Task<ODataResult<Vendor>?> ListVendorODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<Vendor>("Vendor", top, skip, orderby, filter, count, expand);
    }

    public async Task<Vendor?> GetVendorByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/vendor/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Vendor>();
    }

    public async Task UpdateVendorAsync(long key, Vendor data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/vendor/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Vendor?> InsertVendorAsync(Vendor data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/vendor");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Vendor>();
    }

    public async Task DeleteVendorAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/vendor/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<SupportCase[]?> ListSupportCaseAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/supportcase");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<SupportCase[]>();
    }

    public Task<ODataResult<SupportCase>?> ListSupportCaseODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<SupportCase>("SupportCase", top, skip, orderby, filter, count, expand);
    }

    public async Task<SupportCase?> GetSupportCaseByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/supportcase/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<SupportCase>();
    }

    public async Task UpdateSupportCaseAsync(long key, SupportCase data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/supportcase/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<SupportCase?> InsertSupportCaseAsync(SupportCase data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/supportcase");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<SupportCase>();
    }

    public async Task DeleteSupportCaseAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/supportcase/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<TodoTask[]?> ListTodoTaskAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/todotask");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<TodoTask[]>();
    }

    public Task<ODataResult<TodoTask>?> ListTodoTaskODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<TodoTask>("TodoTask", top, skip, orderby, filter, count, expand);
    }

    public async Task<TodoTask?> GetTodoTaskByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/todotask/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<TodoTask>();
    }

    public async Task UpdateTodoTaskAsync(long key, TodoTask data)
    {
        if (data.CreatedDateTime == null)
            data.CreatedDateTime = DateTime.UtcNow;
        data.ModifiedDateTime = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/todotask/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<TodoTask?> InsertTodoTaskAsync(TodoTask data)
    {
        if (data.CreatedDateTime == null)
            data.CreatedDateTime = DateTime.UtcNow;
        data.ModifiedDateTime = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/todotask");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<TodoTask>();
    }

    public async Task DeleteTodoTaskAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/todotask/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Reward[]?> ListRewardAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/reward");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Reward[]>();
    }

    public Task<ODataResult<Reward>?> ListRewardODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<Reward>("Reward", top, skip, orderby, filter, count, expand);
    }

    public async Task<Reward?> GetRewardByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/reward/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Reward>();
    }

    public async Task UpdateRewardAsync(long key, Reward data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/reward/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<Reward?> InsertRewardAsync(Reward data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;
        data.ModifiedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/reward");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Reward>();
    }

    public async Task DeleteRewardAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/reward/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task<string?> UploadImageAsync(Stream stream, int bufferSize, string contentType)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        MultipartFormDataContent content = [];
        StreamContent fileContent = new(stream, bufferSize);
        fileContent.Headers.ContentType = new(contentType);
        content.Add(fileContent, "image", "image");

        HttpRequestMessage request = new(HttpMethod.Post, $"/api/image");
        request.Headers.Add("Authorization", $"Bearer {token}");
        request.Content = content;

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<string>();
    }

    public async Task<string?> UploadImageAsync(IBrowserFile image)
    {
        using var stream = image.OpenReadStream(image.Size);

        return await UploadImageAsync(stream, Convert.ToInt32(image.Size), image.ContentType);
    }

    public async Task ChangePasswordAsync(string oldPassword, string newPassword)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, $"/identity/manage/info");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(new { oldPassword, newPassword });

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    public async Task ModifyRolesAsync(string key, IEnumerable<string> roles)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/user/{key}/roles");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(roles);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);
    }

    // AuditLog methods
    public async Task<AuditLog[]?> ListAuditLogAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/auditlog");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<AuditLog[]>();
    }

    public Task<ODataResult<AuditLog>?> ListAuditLogODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<AuditLog>("AuditLog", top, skip, orderby, filter, count, expand);
    }

    public async Task<AuditLog?> GetAuditLogByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/auditlog/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<AuditLog>();
    }

    public async Task<List<AuditLog>?> GetAuditLogByEntityAsync(string entityType, string entityId)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/auditlog/entity/{entityType}/{entityId}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<AuditLog>>();
    }

    public async Task<object?> GetAuditLogSummaryAsync(int days = 30)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/auditlog/summary?days={days}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<object>();
    }

    // ActivityLog methods
    public async Task<ActivityLog[]?> ListActivityLogAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/activitylog");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ActivityLog[]>();
    }

    public Task<ODataResult<ActivityLog>?> ListActivityLogODataAsync(
        int? top = null,
        int? skip = null,
        string? orderby = null,
        string? filter = null,
        bool count = false,
        string? expand = null)
    {
        return GetODataAsync<ActivityLog>("ActivityLog", top, skip, orderby, filter, count, expand);
    }

    public async Task<ActivityLog?> GetActivityLogByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/activitylog/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ActivityLog>();
    }

    // Authentication Activity methods
    public async Task<List<ActivityLog>?> GetMyAuthActivityAsync(int count = 10)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/authactivity/my-activity?count={count}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<ActivityLog>>();
    }

    public async Task<List<ActivityLog>?> GetAllAuthActivityAsync(int count = 50)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/authactivity/all-activity?count={count}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);

        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<ActivityLog>>();
    }

    // Activity CRUD
    public async Task<Activity[]?> ListActivityAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/activity");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Activity[]>();
    }

    public Task<ODataResult<Activity>?> ListActivityODataAsync(
        int? top = null, int? skip = null, string? orderby = null,
        string? filter = null, bool count = false, string? expand = null)
    {
        return GetODataAsync<Activity>("Activity", top, skip, orderby, filter, count, expand);
    }

    public async Task<Activity?> GetActivityByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/activity/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Activity>();
    }

    public async Task UpdateActivityAsync(long key, Activity data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/activity/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);
    }

    public async Task<Activity?> InsertActivityAsync(Activity data)
    {
        if (data.CreatedDate == null)
            data.CreatedDate = DateTime.UtcNow;

        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/activity");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Activity>();
    }

    public async Task DeleteActivityAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/activity/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);
    }

    // Tag CRUD
    public async Task<Tag[]?> ListTagAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/tag");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Tag[]>();
    }

    public Task<ODataResult<Tag>?> ListTagODataAsync(
        int? top = null, int? skip = null, string? orderby = null,
        string? filter = null, bool count = false, string? expand = null)
    {
        return GetODataAsync<Tag>("Tag", top, skip, orderby, filter, count, expand);
    }

    public async Task<Tag?> GetTagByIdAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/tag/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Tag>();
    }

    public async Task UpdateTagAsync(long key, Tag data)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Put, $"/api/tag/{key}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);
    }

    public async Task<Tag?> InsertTagAsync(Tag data)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/tag");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<Tag>();
    }

    public async Task DeleteTagAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/tag/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);
    }

    // EntityTag
    public async Task<EntityTag[]?> ListEntityTagAsync(string entityType, long entityId)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        var filter = $"EntityType eq '{entityType}' and EntityId eq {entityId}";
        var result = await GetODataAsync<EntityTag>("EntityTag", filter: filter, expand: "Tag");
        return result?.Value?.ToArray();
    }

    public async Task<EntityTag?> InsertEntityTagAsync(EntityTag data)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Post, "/api/entitytag");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = JsonContent.Create(data);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<EntityTag>();
    }

    public async Task DeleteEntityTagAsync(long key)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Delete, $"/api/entitytag/{key}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);
    }

    // Dashboard
    public async Task<KpiSummaryDto?> GetDashboardKpisAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/dashboard/kpis");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<KpiSummaryDto>();
    }

    public async Task<List<SalesOverTimeDto>?> GetSalesOverTimeAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/dashboard/sales-over-time");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<SalesOverTimeDto>>();
    }

    public async Task<List<LeadSourceDto>?> GetLeadSourcesAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/dashboard/lead-sources");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<LeadSourceDto>>();
    }

    public async Task<List<PipelineStageDto>?> GetPipelineByStageAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/dashboard/pipeline-by-stage");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<PipelineStageDto>>();
    }

    public async Task<List<RecentActivityDto>?> GetRecentActivityAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/dashboard/recent-activity");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<RecentActivityDto>>();
    }

    public async Task<List<TopOpportunityDto>?> GetTopOpportunitiesAsync()
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, "/api/dashboard/top-opportunities");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<TopOpportunityDto>>();
    }

    public async Task<List<TimelineEventDto>?> GetTimelineAsync(string entityType, long entityId)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/dashboard/timeline/{entityType}/{entityId}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<TimelineEventDto>>();
    }

    // Search
    public async Task<SearchResultDto?> SearchAsync(string query)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/search?q={Uri.EscapeDataString(query)}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<SearchResultDto>();
    }

    // Import
    public async Task<ImportResult?> ImportCsvAsync(string entityType, Stream fileStream, string fileName)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);

        HttpRequestMessage request = new(HttpMethod.Post, $"/api/import/{entityType}");
        request.Headers.Authorization = new("Bearer", token);
        request.Content = content;

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<ImportResult>();
    }

    // Reports
    public async Task<List<SalesOverTimeDto>?> GetSalesReportAsync(DateTime? from, DateTime? to, string groupBy = "month")
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        var queryParams = new List<string>();
        if (from.HasValue) queryParams.Add($"from={from.Value:yyyy-MM-dd}");
        if (to.HasValue) queryParams.Add($"to={to.Value:yyyy-MM-dd}");
        queryParams.Add($"groupBy={groupBy}");

        HttpRequestMessage request = new(HttpMethod.Get, $"/api/report/sales?{string.Join("&", queryParams)}");
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<SalesOverTimeDto>>();
    }

    public async Task<List<PipelineStageDto>?> GetPipelineReportAsync(string? stage = null)
    {
        var token = await authenticationStateProvider.GetBearerTokenAsync()
            ?? throw new Exception("Not authorized");

        var url = "/api/report/pipeline";
        if (!string.IsNullOrEmpty(stage)) url += $"?stage={Uri.EscapeDataString(stage)}";

        HttpRequestMessage request = new(HttpMethod.Get, url);
        request.Headers.Authorization = new("Bearer", token);

        var response = await httpClient.SendAsync(request);
        await HandleResponseErrorsAsync(response);

        return await response.Content.ReadFromJsonAsync<List<PipelineStageDto>>();
    }
}

public class ImportResult
{
    public int Imported { get; set; }
    public int Total { get; set; }
    public List<string>? Errors { get; set; }
}
