using Microsoft.AspNetCore.Mvc;
using UrlShortener.Dtos.Request;
using UrlShortener.Service.IService;

namespace UrlShortener.Controller;

[ApiController]
[Route("api/[controller]")]
public class ShortenUrlController : ControllerBase
{
    private readonly IShortenService _shortenService;
    public ShortenUrlController(IShortenService shortenService)
    {
        _shortenService = shortenService;
    }

    [HttpPost("shorten")]
    public async Task<IActionResult> ShortenUrl(CreateShortenUrlRequest request)
    {
        var result = await _shortenService.ShortenUrlAsync(request.OriginalUrl);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost("GetAll")]
    public async Task<IActionResult> GetAllUrls(GetAllUrl request)
    {
        var result = await _shortenService.GetAllUrlsAsync(request);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

}