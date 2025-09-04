
using Microsoft.EntityFrameworkCore;
using UrlShortener.Model;

namespace UrlShortener.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Url> Urls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelbuilder)
    {
        base.OnModelCreating(modelbuilder);
    }

    public async Task<bool> CheckSavedChangesAsync(int modifiedRows)
    {
        return await SaveChangesAsync() == modifiedRows;
    }
}