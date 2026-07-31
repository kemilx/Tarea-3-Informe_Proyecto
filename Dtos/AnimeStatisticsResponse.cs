namespace AnimeCatalog.Api.Dtos;

public record AnimeStatusCount(string Status, int Count);

public record AnimeStatisticsResponse(
    int TotalAnimes,
    decimal AverageRating,
    int TotalEpisodes,
    IReadOnlyList<AnimeStatusCount> ByStatus);
