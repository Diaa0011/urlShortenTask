using FluentValidation;
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

    [HttpPost("ShortenUrl")]
    public async Task<IActionResult> ShortenUrl(CreateShortenUrlRequest request, [FromServices] IValidator<CreateShortenUrlRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var result = await _shortenService.ShortenUrlAsync(request.OriginalUrl);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPost("UpdateCount")]
    public async Task<IActionResult> UpdateCount(UpdateUrlRequest request, [FromServices] IValidator<UpdateUrlRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var result = await _shortenService.UpdateCountAsync(request);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    
    [HttpPost("ToggleUrl")]
    public async Task<IActionResult> ToggleUrl([FromQuery]ToggleUrlRequest request, [FromServices] IValidator<ToggleUrlRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var result = await _shortenService.ToggleUrlAsync(request);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }


    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllUrls([FromQuery] GetAllUrl request)
    {
        var result = await _shortenService.GetAllUrlsAsync(request);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpGet("GetById")]
    public async Task<IActionResult> GetUrlById([FromQuery] GetUrlRequest request, [FromServices] IValidator<GetUrlRequest> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

        var result = await _shortenService.GetUrlByIdAsync(request);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

}