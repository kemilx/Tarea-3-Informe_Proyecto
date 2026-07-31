using AnimeCatalog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeCatalog.Api.Data;

public class AnimeDbContext(DbContextOptions<AnimeDbContext> options)
    : DbContext(options)
{
    public DbSet<Anime> Animes => Set<Anime>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var anime = modelBuilder.Entity<Anime>();

        anime.HasIndex(item => item.Title).IsUnique();
        anime.Property(item => item.Title).HasMaxLength(120);
        anime.Property(item => item.Genre).HasMaxLength(80);
        anime.Property(item => item.Studio).HasMaxLength(120);
        anime.Property(item => item.Synopsis).HasMaxLength(1000);
        anime.Property(item => item.Rating).HasPrecision(3, 1);
        anime.Property(item => item.Status).HasConversion<string>().HasMaxLength(20);

        anime.HasData(
            new Anime
            {
                Id = 1,
                Title = "Fullmetal Alchemist: Brotherhood",
                Genre = "Acción, aventura y fantasía",
                Studio = "Bones",
                Synopsis = "Dos hermanos alquimistas buscan recuperar sus cuerpos.",
                ReleaseYear = 2009,
                Episodes = 64,
                Rating = 9.1m,
                Status = AnimeStatus.Finished,
                CreatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Anime
            {
                Id = 2,
                Title = "Frieren: Beyond Journey's End",
                Genre = "Aventura, drama y fantasía",
                Studio = "Madhouse",
                Synopsis = "Una elfa emprende un viaje para comprender mejor a sus amigos.",
                ReleaseYear = 2023,
                Episodes = 28,
                Rating = 9.0m,
                Status = AnimeStatus.Finished,
                CreatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            });
    }
}
