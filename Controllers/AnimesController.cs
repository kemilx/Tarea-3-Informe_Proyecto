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

    private Task<bool> TitleExists(string title)
    {
        return context.Animes.AnyAsync(anime => anime.Title == title);
    }

    private static string? CleanOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

}
