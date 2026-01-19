using System.Net.Http.Json;
using System.Net.Http.Headers;
using StudioAlAmalWeb.Models;

namespace StudioAlAmalWeb.Services;

public class CommunicationService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationService _authService;

    // Constructor - gets called when the service is created
    public CommunicationService(HttpClient httpClient, AuthenticationService authService)
    {
        _httpClient = httpClient;
        _authService = authService;
    }

    /// <summary>
    /// Submit a contact form (PUBLIC - no authentication needed)
    /// Anyone can submit a contact form from the website
    /// </summary>
    public async Task<(bool Success, string Message)> SubmitContactFormAsync(ContactSubmissionCreateDto contact)
    {
        try
        {
            // Send POST request to backend
            var response = await _httpClient.PostAsJsonAsync(
                "http://localhost:5003/api/Contact",
                contact);

            if (response.IsSuccessStatusCode)
            {
                return (true, "Thank you! Your message has been sent successfully. We'll get back to you soon.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
            {
                return (false, "You've submitted too many messages. Please try again later.");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (false, $"Failed to send message: {error}");
            }
        }
        catch (HttpRequestException ex)
        {
            return (false, $"Network error: {ex.Message}. Please make sure you're connected to the internet.");
        }
        catch (Exception ex)
        {
            return (false, $"Unexpected error: {ex.Message}");
        }
    }

    /// <summary>
    /// Get all contact submissions (ADMIN ONLY - requires authentication)
    /// </summary>
    public async Task<List<ContactSubmission>> GetContactSubmissionsAsync(bool unreadOnly = false)
    {
        try
        {
            // Set authorization header with JWT token
            await SetAuthorizationHeaderAsync();

            // Build URL with optional query parameter
            var url = unreadOnly
                ? "http://localhost:5003/api/Contact?unreadOnly=true"
                : "http://localhost:5003/api/Contact";

            // Send GET request
            var submissions = await _httpClient.GetFromJsonAsync<List<ContactSubmission>>(url);

            return submissions ?? new List<ContactSubmission>();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error getting contact submissions: {ex.Message}");
            return new List<ContactSubmission>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return new List<ContactSubmission>();
        }
    }

    /// <summary>
    /// Get a specific contact submission by ID (ADMIN ONLY)
    /// </summary>
    public async Task<ContactSubmission?> GetContactSubmissionAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            return await _httpClient.GetFromJsonAsync<ContactSubmission>(
                $"http://localhost:5003/api/Contact/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting contact submission: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Mark a message as read (ADMIN ONLY)
    /// </summary>
    public async Task<bool> MarkAsReadAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.PutAsync(
                $"http://localhost:5003/api/Contact/{id}/mark-read",
                null); // No body needed for this endpoint

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error marking message as read: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Delete a contact submission (ADMIN ONLY)
    /// </summary>
    public async Task<bool> DeleteSubmissionAsync(int id)
    {
        try
        {
            await SetAuthorizationHeaderAsync();

            var response = await _httpClient.DeleteAsync(
                $"http://localhost:5003/api/Contact/{id}");

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting submission: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Private helper method to set JWT token in request header
    /// This is needed for all admin-only endpoints
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
}