namespace UrlShortener.Dtos.Request;
public class GetAllUrl
{ 
    public string? SearchFor { get; set; }
    public bool IsActive { get; set; } = true;
    public int PageNumber { get; set; } = 0;
    public int PageSize { get; set; } = 10;
}