namespace AnimeCatalog.Api.Models;

public class Anime
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Genre { get; set; }
    public string? Studio { get; set; }
    public string? Synopsis { get; set; }
    public int ReleaseYear { get; set; }
    public int Episodes { get; set; }
    public decimal Rating { get; set; }
    public AnimeStatus Status { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
