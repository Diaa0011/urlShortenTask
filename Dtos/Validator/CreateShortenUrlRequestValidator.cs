using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Dtos.Request;
namespace UrlShortener.Dtos.Validator;

public class CreateShortenUrlRequestValidator : AbstractValidator<CreateShortenUrlRequest>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateShortenUrlRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.OriginalUrl).NotNull()
            .NotEmpty().WithMessage("Original URL is required.")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("Original URL must be a valid URL.");

        RuleFor(x=>x.OriginalUrl).MustAsync(async (originalUrl, cancellation) =>
        {
            var exists = await _dbContext.Urls.FirstOrDefaultAsync(x => x.OriginalUrl == originalUrl);
            return exists == null;
        }).WithMessage(originalUrl => $"The URL '{originalUrl.OriginalUrl}' has already been shortened.");
    }
}

