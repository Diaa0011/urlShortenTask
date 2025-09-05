namespace UrlShortener.Dtos.Response;

public record GetAllUrlResponse
{
    public List<ShortenUrlDto> Urls { get; set; }
    public int TotalCount { get; set; }
}
public record ShortenUrlDto
{
    public Guid Id { get; set; }
    public int No { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortenedUrl { get; set; }
    public string DeCodedUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool isActive { get; set; }
    public ClickCountDto? ClickCount { get; set; }
}
public record ClickCountDto
{
    public Guid Id { get; set; }
    public int ClickCount { get; set; }
    public DateTime? LastAccessedAt { get; set; }
}
