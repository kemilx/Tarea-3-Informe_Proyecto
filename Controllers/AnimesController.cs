using AnimeCatalog.Api.Data;
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

}
