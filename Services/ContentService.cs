using System.Net.Http.Json;
using System.Net.Http.Headers;
using StudioAlAmalWeb.Models;

namespace StudioAlAmalWeb.Services;

public class ContentService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationService _authService;

    public ContentService(HttpClient httpClient, AuthenticationService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    // ==================== HELPER METHOD ====================

    /// <summary>
    /// Sets JWT token in request headers for authenticated requests
    /// </summary>
    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    // ==================== PROMOS ====================

    /// <summary>
    /// Get all promos (PUBLIC - no auth needed)
    /// </summary>
    public async Task<List<Promo>> GetPromosAsync(bool activeOnly = false)
    {
        try
        {
            var url = activeOnly
                ? "http://localhost:5002/api/Promos?activeOnly=true"
                : "http://localhost:5002/api/Promos";

            return await _httpClient.GetFromJsonAsync<List<Promo>>(url)
                   ?? new List<Promo>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting promos: {ex.Message}");
            return new List<Promo>();
        }
    }

    /// <summary>
    /// Get a specific promo by ID (PUBLIC)
    /// </summary>
    public async Task<Promo?> GetPromoAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Promo>(
                $"http://localhost:5002/api/Promos/{id}");
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Create a new promo (ADMIN ONLY)
    /// </summary>
    public async Task<(bool Success, string Message)> CreatePromoAsync(PromoCreateDto promo)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:5002/api/Promos",
                promo);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Promo created successfully!");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Failed to create promo: {error}");
            }
        }
        catch (Exception ex)
        {
            return (false, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Delete a promo (ADMIN ONLY)
    /// </summary>
    public async Task<bool> DeletePromoAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.DeleteAsync(
                $"http://localhost:5002/api/Promos/{id}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting promo: {ex.Message}");
            return false;
        }
    }

    // ==================== PHOTOS ====================

    /// <summary>
    /// Get all photos with optional filters (PUBLIC)
    /// </summary>
    public async Task<List<Photo>> GetPhotosAsync(bool activeOnly = false, string? category = null)
    {
        try
        {
            var url = "http://localhost:5002/api/Photos?";
            if (activeOnly) url += "activeOnly=true&";
            if (!string.IsNullOrEmpty(category)) url += $"category={category}&";

            return await _httpClient.GetFromJsonAsync<List<Photo>>(url)
                   ?? new List<Photo>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting photos: {ex.Message}");
            return new List<Photo>();
        }
    }

    /// <summary>
    /// Get a specific photo by ID (PUBLIC)
    /// </summary>
    public async Task<Photo?> GetPhotoAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Photo>(
                $"http://localhost:5002/api/Photos/{id}");
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Create a new photo (ADMIN ONLY)
    /// </summary>
    public async Task<(bool Success, string Message)> CreatePhotoAsync(PhotoCreateDto photo)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:5002/api/Photos",
                photo);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Photo added successfully!");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Failed to add photo: {error}");
            }
        }
        catch (Exception ex)
        {
            return (false, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Delete a photo (ADMIN ONLY)
    /// </summary>
    public async Task<bool> DeletePhotoAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.DeleteAsync(
                $"http://localhost:5002/api/Photos/{id}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting photo: {ex.Message}");
            return false;
        }
    }

    // ==================== VIDEOS ====================

    /// <summary>
    /// Get all videos with optional filters (PUBLIC)
    /// </summary>
    public async Task<List<Video>> GetVideosAsync(bool activeOnly = false, string? category = null)
    {
        try
        {
            var url = "http://localhost:5002/api/Videos?";
            if (activeOnly) url += "activeOnly=true&";
            if (!string.IsNullOrEmpty(category)) url += $"category={category}&";

            return await _httpClient.GetFromJsonAsync<List<Video>>(url)
                   ?? new List<Video>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting videos: {ex.Message}");
            return new List<Video>();
        }
    }

    /// <summary>
    /// Get a specific video by ID (PUBLIC)
    /// </summary>
    public async Task<Video?> GetVideoAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Video>(
                $"http://localhost:5002/api/Videos/{id}");
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Create a new video (ADMIN ONLY)
    /// </summary>
    public async Task<(bool Success, string Message)> CreateVideoAsync(VideoCreateDto video)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:5002/api/Videos",
                video);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Video added successfully!");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Failed to add video: {error}");
            }
        }
        catch (Exception ex)
        {
            return (false, $"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Delete a video (ADMIN ONLY)
    /// </summary>
    public async Task<bool> DeleteVideoAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.DeleteAsync(
                $"http://localhost:5002/api/Videos/{id}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting video: {ex.Message}");
            return false;
        }
    }

    // ==================== ABOUT US ====================

    /// <summary>
    /// Get About Us content (PUBLIC)
    /// </summary>
    public async Task<AboutUs?> GetAboutUsAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<AboutUs>(
                "http://localhost:5002/api/AboutUs");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting About Us: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Update About Us content (ADMIN ONLY)
    /// </summary>
    public async Task<(bool Success, string Message)> UpdateAboutUsAsync(AboutUsUpdateDto aboutUs)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.PutAsJsonAsync(
                "http://localhost:5002/api/AboutUs",
                aboutUs);

            if (response.IsSuccessStatusCode)
            {
                return (true, "About Us updated successfully!");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Failed to update: {error}");
            }
        }
        catch (Exception ex)
        {
            return (false, $"Error: {ex.Message}");
        }
    }
}