using System.ComponentModel;

namespace UrlShortener.Dtos.Request;

public record GetUrlRequest{

    public Guid Id { get; set; }
}