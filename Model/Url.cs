using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Model;

[Index(nameof(No), IsUnique = true)]
public class Url
{
    public Guid Id { get; set; }
    public int No { get; set; }
    public required string OriginalUrl { get; set; }
    public required string ShortenedUrl { get; set; }
    public int ClickCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastAccessedAt { get; set; }
    public bool IsActive { get; set; }
}