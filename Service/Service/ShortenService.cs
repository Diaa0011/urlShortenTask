using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Dtos.Request;
using UrlShortener.Dtos.Response;
using UrlShortener.Model;
using UrlShortener.Service.IService;

namespace UrlShortener.Service.Service;

public class ShortenService: IShortenService
{
    private readonly ApplicationDbContext _dbContext;
    public ShortenService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<ShortenUrlDto>> ShortenUrlAsync(string originalUrl)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        int modifiedRows = 0;
        try
        {
            var existingUrl = await _dbContext.Urls.AnyAsync(u => u.OriginalUrl == originalUrl);

            if (existingUrl)
            {
                return new Response<ShortenUrlDto>
                {
                    IsSuccess = false,
                    Message = "URL already exists",
                    Data = null
                };
            }
            int nextNo = await _dbContext.Urls.CountAsync() + 1;
            var url = new Url
            {
                Id = Guid.NewGuid(),
                No = nextNo,
                OriginalUrl = originalUrl,
                ShortenedUrl = Guid.NewGuid().ToString().Substring(0, 8),
                ClickCount = 0,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            await _dbContext.Urls.AddAsync(url);
            modifiedRows++;

            var result = await _dbContext.CheckSavedChangesAsync(modifiedRows);

            var response = new ShortenUrlDto
            {
                OriginalUrl = url.OriginalUrl,
                ShortenedUrl = url.ShortenedUrl
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

            var urlResponse = UrlMapper(urls);

            return new Response<List<ShortenUrlDto>>
            {
                IsSuccess = true,
                Message = "URLs retrieved successfully",
                Data = urlResponse
            };
        }
        catch (Exception ex)
        {
            return new Response<List<ShortenUrlDto>>
            {
                IsSuccess = false,
                Message = $"An error occurred: {ex.Message}",
                Data = null
            };
        }
    }


    #region helpers
    private List<ShortenUrlDto> UrlMapper(List<Url> urls)

    {
        var urlDtos = new List<ShortenUrlDto>();
        foreach (var url in urls)
        {
            urlDtos.Add(new ShortenUrlDto
            {
                Id = url.Id,
                OriginalUrl = url.OriginalUrl,
                ShortenedUrl = url.ShortenedUrl,
                ClickCount = url.ClickCount,
                CreatedAt = url.CreatedAt,
                LastAccessedAt = url.LastAccessedAt
            });
        }
        return urlDtos;
    }

    #endregion
}
