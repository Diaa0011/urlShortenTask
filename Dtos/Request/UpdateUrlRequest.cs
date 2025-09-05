using System.ComponentModel;

namespace UrlShortener.Dtos.Request;

public class UpdateUrlRequest
{
    /// <summary>
    /// The Link count to be updated
    /// </summary>

    public string link { get; set; }
}