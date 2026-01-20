using System.ComponentModel.DataAnnotations;

namespace StudioAlAmalWeb.Models;

// ==================== PROMOS ====================

// What we receive from backend when getting promos
public class Promo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

// What we send to backend when creating a promo
public class PromoCreateDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    public string ImageUrl { get; set; } = string.Empty;

    [Range(0, 100, ErrorMessage = "Display order must be between 0 and 100")]
    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}

// ==================== PHOTOS ====================

public class Photo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string? Category { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PhotoCreateDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Image URL is required")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    public string ImageUrl { get; set; } = string.Empty;

    [Url(ErrorMessage = "Please enter a valid URL")]
    public string? ThumbnailUrl { get; set; }

    public string? Category { get; set; }

    [Range(0, 100, ErrorMessage = "Display order must be between 0 and 100")]
    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}

// ==================== VIDEOS ====================

public class Video
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string VideoUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public int? Duration { get; set; }
    public string? Category { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class VideoCreateDto
{
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required(ErrorMessage = "Video URL is required")]
    [Url(ErrorMessage = "Please enter a valid URL")]
    public string VideoUrl { get; set; } = string.Empty;

    [Url(ErrorMessage = "Please enter a valid URL")]
    public string? ThumbnailUrl { get; set; }

    [Range(0, 3600, ErrorMessage = "Duration must be between 0 and 3600 seconds")]
    public int? Duration { get; set; }

    public string? Category { get; set; }

    [Range(0, 100, ErrorMessage = "Display order must be between 0 and 100")]
    public int DisplayOrder { get; set; } = 0;

    public bool IsActive { get; set; } = true;
}

// ==================== ABOUT US ====================

public class AboutUs
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class AboutUsUpdateDto
{
    [Required(ErrorMessage = "Content is required")]
    [MinLength(50, ErrorMessage = "Content must be at least 50 characters")]
    public string Content { get; set; } = string.Empty;

    [Url(ErrorMessage = "Please enter a valid URL")]
    public string? ImageUrl { get; set; }
}