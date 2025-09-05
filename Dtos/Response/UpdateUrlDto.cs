namespace UrlShortener.Dtos.Response;

public record UpdateUrlDto
{
        
    public Guid Id { get; set; }
    public int No { get; set; }
    public string ShortenedUrl { get; set; }
    public int ClickCount { get; set; }
    public DateTime LastAccessedAt { get; set; }
}