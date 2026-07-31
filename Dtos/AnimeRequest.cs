using AnimeCatalog.Api.Models;

namespace AnimeCatalog.Api.Dtos;

public class AnimeRequest
{
    public string Title { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public string? Studio { get; set; }

    public string? Synopsis { get; set; }

    public int ReleaseYear { get; set; }

    public int Episodes { get; set; }

    public decimal Rating { get; set; }

    public AnimeStatus Status { get; set; }
}
