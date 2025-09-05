using System;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Dtos.Request;

namespace UrlShortener.Dtos.Validator;


public class UpdateUrlRequestValidator : AbstractValidator<UpdateUrlRequest>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateUrlRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.link)
            .NotEmpty().WithMessage("Shortened URL is required.");

        RuleFor(x => x).MustAsync(async (request, model) =>
        {
            var exists = await _dbContext.Urls.FirstOrDefaultAsync(x => x.ShortenedUrl == request.link);
            return exists != null;
        }).WithMessage(request => $"No URL found with link: {request.link}");
    }
}