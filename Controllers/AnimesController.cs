using AnimeCatalog.Api.Data;
using AnimeCatalog.Api.Dtos;
using AnimeCatalog.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnimeCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimesController(AnimeDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Anime>>> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? genre)
    {
        var query = context.Animes.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(anime => anime.Title.Contains(search.Trim()));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(anime => anime.Genre.Contains(genre.Trim()));
        }

        return Ok(await query.OrderBy(anime => anime.Title).ToListAsync());
    }

    [HttpGet("statistics")]
    public async Task<ActionResult<AnimeStatisticsResponse>> GetStatistics()
    {
        var totalAnimes = await context.Animes.CountAsync();
        var averageRating = totalAnimes == 0
            ? 0
            : await context.Animes.AverageAsync(anime => anime.Rating);
        var totalEpisodes = await context.Animes.SumAsync(anime => anime.Episodes);

        var groupedStatuses = await context.Animes
            .GroupBy(anime => anime.Status)
            .Select(group => new { Status = group.Key, Count = group.Count() })
            .ToListAsync();

        var byStatus = groupedStatuses
            .Select(group => new AnimeStatusCount(group.Status.ToString(), group.Count))
            .OrderBy(group => group.Status)
            .ToList();

        return Ok(new AnimeStatisticsResponse(
            totalAnimes,
            decimal.Round(averageRating, 1),
            totalEpisodes,
            byStatus));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Anime>> GetById(int id)
    {
        var anime = await context.Animes.AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == id);

        return anime is null
            ? NotFound(new { message = $"No se encontró el anime con id {id}." })
            : Ok(anime);
    }

    [HttpPost]
    public async Task<ActionResult<Anime>> Create(AnimeRequest request)
    {
        var normalizedTitle = request.Title.Trim();

        if (await TitleExists(normalizedTitle))
        {
            return Conflict(new { message = "Ya existe un anime con ese título." });
        }

        var now = DateTime.UtcNow;
        var anime = new Anime
        {
            Title = normalizedTitle,
            Genre = request.Genre.Trim(),
            Studio = CleanOptional(request.Studio),
            Synopsis = CleanOptional(request.Synopsis),
            ReleaseYear = request.ReleaseYear,
            Episodes = request.Episodes,
            Rating = request.Rating,
            Status = request.Status,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        context.Animes.Add(anime);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = anime.Id }, anime);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Anime>> Update(int id, AnimeRequest request)
    {
        var anime = await context.Animes.FindAsync(id);
        if (anime is null)
        {
            return NotFound(new { message = $"No se encontró el anime con id {id}." });
        }

        var normalizedTitle = request.Title.Trim();
        if (await TitleExists(normalizedTitle, id))
        {
            return Conflict(new { message = "Ya existe otro anime con ese título." });
        }

        anime.Title = normalizedTitle;
        anime.Genre = request.Genre.Trim();
        anime.Studio = CleanOptional(request.Studio);
        anime.Synopsis = CleanOptional(request.Synopsis);
        anime.ReleaseYear = request.ReleaseYear;
        anime.Episodes = request.Episodes;
        anime.Rating = request.Rating;
        anime.Status = request.Status;
        anime.UpdatedAtUtc = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return Ok(anime);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var anime = await context.Animes.FindAsync(id);
        if (anime is null)
        {
            return NotFound(new { message = $"No se encontró el anime con id {id}." });
        }

        context.Animes.Remove(anime);
        await context.SaveChangesAsync();
        return NoContent();
    }

    private Task<bool> TitleExists(string title, int? excludedId = null)
    {
        return context.Animes.AnyAsync(anime =>
            anime.Title == title &&
            (!excludedId.HasValue || anime.Id != excludedId.Value));
    }

    private static string? CleanOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

}
