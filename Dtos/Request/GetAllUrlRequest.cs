namespace UrlShortener.Dtos.Request;
/// <summary>
/// Request model for retrieving all URLs with optional search and pagination.
/// </summary>
public class GetAllUrl
{
    /// <summary>
    /// Optional search term to search for URLs.
    /// </summary>
    public string? SearchFor { get; set; }

    /// <summary>
    /// Indicates whether to include only active URLs.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// The page number for pagination default is zero.
    /// </summary>
    public int PageNumber { get; set; } = 0;
    /// <summary>
    /// The number of items per page for pagination default is ten.     
    /// </summary>
    public int PageSize { get; set; } = 10;
}