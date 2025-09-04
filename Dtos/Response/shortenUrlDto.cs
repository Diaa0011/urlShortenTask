namespace UrlShortener.Dtos.Response;

    public record ShortenUrlDto
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortenedUrl { get; set; }
        public int ClickCount { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastAccessedAt { get; set; }
        public bool isActive { get; set; }
    }

