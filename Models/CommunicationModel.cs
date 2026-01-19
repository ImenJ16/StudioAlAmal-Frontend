using System.ComponentModel.DataAnnotations;

namespace StudioAlAmalWeb.Models;

// This is what we send to the backend when submitting a contact form
public class ContactSubmissionCreateDto
{
    [Required(ErrorMessage = "Please enter your full name")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your email address")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Please enter a valid phone number")]
    public string? Phone { get; set; }

    [Required(ErrorMessage = "Please enter a subject")]
    [StringLength(200, ErrorMessage = "Subject cannot exceed 200 characters")]
    public string Subject { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your message")]
    [MinLength(10, ErrorMessage = "Message must be at least 10 characters")]
    public string Message { get; set; } = string.Empty;
}

// This is what we receive back from the backend (for admin to view submissions)
public class ContactSubmission
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime SubmittedAt { get; set; }
    public DateTime? ReadAt { get; set; }
}