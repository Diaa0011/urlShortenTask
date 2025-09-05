using System.Text;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Dtos.Request;
using UrlShortener.Dtos.Response;
using UrlShortener.Model;
using UrlShortener.Service.IService;

namespace UrlShortener.Service.Service;

public class ShortenService : IShortenService
{
    private readonly ApplicationDbContext _dbContext;
    ILogger<ShortenService> _logger;
    public ShortenService(ApplicationDbContext dbContext, ILogger<ShortenService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Response<ShortenUrlDto>> ShortenUrlAsync(string originalUrl)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        int modifiedRows = 0;
        try
        {
            var existingUrl = await _dbContext.Urls.AnyAsync(u => u.OriginalUrl == originalUrl);
            var shortUrls = await _dbContext.Urls.Select(u => u.ShortenedUrl).ToListAsync();

            _logger.LogInformation("Start Proccess of Shortening URL");
            _logger.LogInformation("{OriginalUrl}", originalUrl);

            int nextNo = await _dbContext.Urls.CountAsync() + 1;
            Guid Id = Guid.NewGuid();
            var url = new Url
            {
                Id = Id,
                No = nextNo,
                OriginalUrl = originalUrl,
                ShortenedUrl = GenerateShortenedUrl(shortUrls),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            var clickCount = new UrlClick
            {
                Id = Id,
                ClickCount = 0,
                LastAccessedAt = null
            };
            await _dbContext.Urls.AddAsync(url);
            await _dbContext.UrlClicks.AddAsync(clickCount);
            modifiedRows += 2;

            var result = await _dbContext.CheckSavedChangesAsync(modifiedRows);

            var response = new ShortenUrlDto
            {
                Id = url.Id,
                No = url.No,
                OriginalUrl = url.OriginalUrl,
                ShortenedUrl = url.ShortenedUrl,
                isActive = url.IsActive,
                CreatedAt = url.CreatedAt,
            };

            if (result)
            {
                await transaction.CommitAsync();
                return new Response<ShortenUrlDto>
                {
                    IsSuccess = true,
                    Message = "URL shortened successfully",
                    Data = response
                };
            }
            else
            {
                await transaction.RollbackAsync();
                _logger.LogError("Error occurred while saving URL to DB");
                return new Response<ShortenUrlDto>
                {
                    IsSuccess = false,
                    Message = "Failed to shorten URL",
                    Data = null
                };
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while shortening URL");
            return new Response<ShortenUrlDto>
            {
                IsSuccess = false,
                Message = $"An error occurred: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<Response<List<ShortenUrlDto>>> GetAllUrlsAsync(GetAllUrl request)
    {
        try
        {
            var query = _dbContext.Urls.Where(x => x.IsActive == request.IsActive);

            _logger.LogInformation("Fetching all Urls");

            if (!string.IsNullOrEmpty(request.SearchFor))
            {
                query = query.Where(x => EF.Functions.Like(x.OriginalUrl, $"%{request.SearchFor}%") ||
                                         EF.Functions.Like(x.ShortenedUrl, $"%{request.SearchFor}%"));
            }

            if (request.PageNumber <= 0)
            {
                request.PageNumber = 0;
            }
            else
            {
                request.PageNumber -= 1;
            }

            var urls = await query
                .Skip(request.PageNumber * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();
            var urlsIds = urls.Select(x => x.Id).ToList();
            var urlCounts = await _dbContext.UrlClicks.Where(x => urlsIds.Contains(x.Id)).ToListAsync();

            var urlResponse = UrlMapper(urls, urlCounts);

            return new Response<List<ShortenUrlDto>>
            {
                IsSuccess = true,
                Message = "URLs retrieved successfully",
                Data = urlResponse
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching URLs");
            return new Response<List<ShortenUrlDto>>
            {
                IsSuccess = false,
                Message = $"An error occurred: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<Response<ShortenUrlDto>> GetUrlByIdAsync(GetUrlRequest request)
    {
        try
        {
            var url = await _dbContext.Urls.FirstOrDefaultAsync(x => x.Id == request.Id);
            var urlClick = await _dbContext.UrlClicks.FirstOrDefaultAsync(x => x.Id == request.Id);

            var urlDto = new ShortenUrlDto
            {
                Id = url.Id,
                No = url.No,
                OriginalUrl = url.OriginalUrl,
                ShortenedUrl = url.ShortenedUrl,
                CreatedAt = url.CreatedAt,
                isActive = url.IsActive,
                ClickCount = urlClick != null ? new ClickCountDto
                {
                    Id = urlClick.Id,
                    ClickCount = urlClick.ClickCount,
                    LastAccessedAt = urlClick.LastAccessedAt
                } : null
            };

            return new Response<ShortenUrlDto>
            {
                IsSuccess = true,
                Message = "URL retrieved successfully",
                Data = urlDto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching URL by ID");
            return new Response<ShortenUrlDto>
            {
                IsSuccess = false,
                Message = $"An error occurred: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<Response<UpdateUrlDto>> UpdateCountAsync(UpdateUrlRequest request)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        int modifiedRows = 0;
        try
        {

            var url = await _dbContext.Urls.FirstOrDefaultAsync(x => x.ShortenedUrl == request.link && x.IsActive);

            var urlClick = await _dbContext.UrlClicks.FirstOrDefaultAsync(x => x.Id == url.Id);

            urlClick.ClickCount++;
            urlClick.LastAccessedAt = DateTime.UtcNow;
            modifiedRows++;


            var result = await _dbContext.CheckSavedChangesAsync(modifiedRows);
            if (result)
            {
                var response = new UpdateUrlDto
                {
                    Id = urlClick.Id,
                    No = url.No,
                    ShortenedUrl = url.ShortenedUrl,
                    ClickCount = urlClick.ClickCount,
                    LastAccessedAt = urlClick.LastAccessedAt ?? DateTime.MinValue
                };
                await transaction.CommitAsync();
                return new Response<UpdateUrlDto>
                {
                    IsSuccess = true,
                    Message = "URL shortened successfully",
                    Data = response
                };
            }
            else
            {
                await transaction.RollbackAsync();
                _logger.LogError("Error occurred while updating click count");
                return new Response<UpdateUrlDto>
                {
                    IsSuccess = false,
                    Message = "Failed to update click count",
                    Data = null
                };
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while reading URL");
            return new Response<UpdateUrlDto>
            {
                IsSuccess = false,
                Message = $"An error occurred: {ex.Message}",
                Data = null
            };
        }
    }

    public async Task<Response<ShortenUrlDto>> ToggleUrlAsync(ToggleUrlRequest request)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        int modifiedRows = 0;
        try
        {
            var url = await _dbContext.Urls.FirstOrDefaultAsync(x => x.Id == request.Id);

            url.IsActive = !url.IsActive;
            modifiedRows++;

            var result = await _dbContext.CheckSavedChangesAsync(modifiedRows);
            if (result)
            {
                var response = new ShortenUrlDto
                {
                    Id = url.Id,
                    No = url.No,
                    OriginalUrl = url.OriginalUrl,
                    ShortenedUrl = url.ShortenedUrl,
                    isActive = url.IsActive,
                    CreatedAt = url.CreatedAt,
                };
                await transaction.CommitAsync();
                return new Response<ShortenUrlDto>
                {
                    IsSuccess = true,
                    Message = "URL status toggled successfully",
                    Data = response
                };
            }
            else
            {
                await transaction.RollbackAsync();
                _logger.LogError("Error occurred while toggling URL status");
                return new Response<ShortenUrlDto>
                {
                    IsSuccess = false,
                    Message = "Failed to toggle URL status",
                    Data = null
                };
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while toggling URL status");
            return new Response<ShortenUrlDto>
            {
                IsSuccess = false,
                Message = $"An error occurred: {ex.Message}",
                Data = null
            };
        }
    }

    #region helpers
    private List<ShortenUrlDto> UrlMapper(List<Url> urls, List<UrlClick> urlClicks)

    {
        var urlDtos = new List<ShortenUrlDto>();
        foreach (var url in urls)
        {
            var urlClick = urlClicks.FirstOrDefault(x => x.Id == url.Id);
            urlDtos.Add(new ShortenUrlDto
            {
                Id = url.Id,
                No = url.No,
                OriginalUrl = url.OriginalUrl,
                ShortenedUrl = url.ShortenedUrl,
                CreatedAt = url.CreatedAt,
                isActive = url.IsActive,
                ClickCount = urlClick != null ? new ClickCountDto
                {
                    Id = urlClick.Id,
                    ClickCount = urlClick.ClickCount,
                    LastAccessedAt = urlClick.LastAccessedAt
                } : null
            });
        }
        return urlDtos;
    }

    private string GenerateShortenedUrl(List<string> existingShortUrls)
    {

        int uniqueNumber = new Random().Next(1, int.MaxValue);
        string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
        string shortenedUrl;
        do
        {
            shortenedUrl = "https://smi.to/" + Encode(alphabet, uniqueNumber);
        } while (existingShortUrls.Contains(shortenedUrl));

        return shortenedUrl;
    }

    private string Encode(string alphabet, int num)
    {
        if (num == 0) return alphabet[0].ToString();

        int baseLength = alphabet.Length;
        StringBuilder sb = new StringBuilder();
        while (num > 0)
        {
            sb.Append(alphabet[num % baseLength]);
            num /= baseLength;
        }
        char[] chars = sb.ToString().ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    #endregion
}
