using System.Net.Http.Json;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using StudioAlAmalWeb.Models;

namespace StudioAlAmalWeb.Services;

public class AuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private const string TokenKey = "authToken";
    private const string UsernameKey = "username";
    private const string RoleKey = "role";

    public event Action? OnAuthStateChanged;

    public AuthenticationService(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
    }

    public async Task<(bool Success, string Message)> LoginAsync(LoginRequest request)
    {
        try
        {
            // Call backend API
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:5001/api/Auth/login",
                request);

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (authResponse != null)
                {
                    // Store token and user info in browser storage
                    await _localStorage.SetItemAsStringAsync(TokenKey, authResponse.Token);
                    await _localStorage.SetItemAsStringAsync(UsernameKey, authResponse.Username);
                    await _localStorage.SetItemAsStringAsync(RoleKey, authResponse.Role);

                    // Set authorization header for future requests
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authResponse.Token);

                    // Notify components that auth state changed
                    OnAuthStateChanged?.Invoke();

                    return (true, "Login successful!");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return (false, "Invalid username or password");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Login failed: {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            return (false, $"Network error: {ex.Message}. Make sure the backend is running.");
        }
        catch (Exception ex)
        {
            return (false, $"Unexpected error: {ex.Message}");
        }

        return (false, "Login failed");
    }

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:5001/api/Auth/register",
                request);

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (authResponse != null)
                {
                    // Store token and user info
                    await _localStorage.SetItemAsStringAsync(TokenKey, authResponse.Token);
                    await _localStorage.SetItemAsStringAsync(UsernameKey, authResponse.Username);
                    await _localStorage.SetItemAsStringAsync(RoleKey, authResponse.Role);

                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authResponse.Token);

                    OnAuthStateChanged?.Invoke();

                    return (true, "Registration successful!");
                }
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Registration failed: {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            return (false, $"Network error: {ex.Message}. Make sure the backend is running.");
        }
        catch (Exception ex)
        {
            return (false, $"Unexpected error: {ex.Message}");
        }

        return (false, "Registration failed");
    }

    public async Task LogoutAsync()
    {
        // Remove all stored data
        await _localStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(UsernameKey);
        await _localStorage.RemoveItemAsync(RoleKey);

        // Clear authorization header
        _httpClient.DefaultRequestHeaders.Authorization = null;

        // Notify components
        OnAuthStateChanged?.Invoke();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsStringAsync(TokenKey);
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsStringAsync(TokenKey);
    }

    public async Task<string?> GetUsernameAsync()
    {
        return await _localStorage.GetItemAsStringAsync(UsernameKey);
    }

    public async Task<string?> GetRoleAsync()
    {
        return await _localStorage.GetItemAsStringAsync(RoleKey);
    }

    public async Task InitializeAsync()
    {
        // On app startup, restore auth header if token exists
        var token = await _localStorage.GetItemAsStringAsync(TokenKey);
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }
}