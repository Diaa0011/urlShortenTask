namespace UrlShortener.Model;

public class UrlClick
{
    public Guid Id { get; set; }
    public int ClickCount { get; set; }
    public DateTime? LastAccessedAt { get; set; }

}