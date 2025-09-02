using System.Transactions;
using UrlShortener.Data;
using UrlShortener.Dtos;

namespace UrlShortener.Service;

public class ShortenService
{
    private readonly ApplicationDbContext _dbContext;
    public ShortenService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Response<ShortenUrlDto>> shortenUrlAsync(string originalUrl)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            

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
    

    


}
