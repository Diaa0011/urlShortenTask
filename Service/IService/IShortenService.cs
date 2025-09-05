using UrlShortener.Dtos.Request;
using UrlShortener.Dtos.Response;

namespace UrlShortener.Service.IService;

public interface IShortenService
{
    Task<Response<ShortenUrlDto>> ShortenUrlAsync(string originalUrl);
    Task<Response<List<ShortenUrlDto>>> GetAllUrlsAsync(GetAllUrl request);
    Task<Response<ShortenUrlDto>> GetUrlByIdAsync(GetUrlRequest request);
    Task<Response<UpdateUrlDto>> UpdateCountAsync(UpdateUrlRequest request);
    Task<Response<ShortenUrlDto>> ToggleUrlAsync(ToggleUrlRequest request);
}
