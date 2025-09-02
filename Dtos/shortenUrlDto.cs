namespace UrlShortener.Dtos
{
    public record ShortenUrlDto
    {
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public int ClickCount { get; set; }

        public string CreatedAt { get; set; }
    }
}
