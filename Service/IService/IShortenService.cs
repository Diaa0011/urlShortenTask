using UrlShortener.Dtos.Request;
using UrlShortener.Dtos.Response;

namespace UrlShortener.Service.IService;

public interface IShortenService
{
    Task<Response<ShortenUrlDto>> ShortenUrlAsync(string originalUrl);
    Task<Response<List<ShortenUrlDto>>> GetAllUrlsAsync(GetAllUrl request);
}
