using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Data;
using UrlShortener.Dtos.Request;

namespace UrlShortener.Dtos.Validator;

public class GetUrlRequestValidator : AbstractValidator<GetUrlRequest>
{
    private readonly ApplicationDbContext _dbContext;
    public GetUrlRequestValidator(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id).NotNull()
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(x => x).MustAsync(async (request, model) =>
        {
            var exists = await _dbContext.Urls.FirstOrDefaultAsync(x => x.Id == request.Id);
            return exists != null;
        }).WithMessage(request => $"No URL found with ID: {request.Id}");
    }
}
