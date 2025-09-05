namespace UrlShortener.Dtos.Request;
/// <summary>
///  Request model for creating a shortened URL.
/// </summary>
public class CreateShortenUrlRequest
{
    /// <summary>
    /// The original URL to be shortened.
    /// </summary>
    public string OriginalUrl { get; set; }
}
