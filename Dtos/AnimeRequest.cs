using AnimeCatalog.Api.Models;
using AnimeCatalog.Api.Validation;
using System.ComponentModel.DataAnnotations;

namespace AnimeCatalog.Api.Dtos;

public class AnimeRequest
{
    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(120, MinimumLength = 2,
        ErrorMessage = "El título debe tener entre 2 y 120 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "El género es obligatorio.")]
    [StringLength(80, MinimumLength = 2,
        ErrorMessage = "El género debe tener entre 2 y 80 caracteres.")]
    public string Genre { get; set; } = string.Empty;

    [StringLength(120, ErrorMessage = "El estudio no puede superar 120 caracteres.")]
    public string? Studio { get; set; }

    [StringLength(1000, ErrorMessage = "La sinopsis no puede superar 1000 caracteres.")]
    public string? Synopsis { get; set; }

    [ReleaseYear]
    public int ReleaseYear { get; set; }

    [Range(1, 5000, ErrorMessage = "La cantidad de episodios debe estar entre 1 y 5000.")]
    public int Episodes { get; set; }

    [Range(typeof(decimal), "0", "10",
        ErrorMessage = "La calificación debe estar entre 0 y 10.")]
    public decimal Rating { get; set; }

    [EnumDataType(typeof(AnimeStatus), ErrorMessage = "El estado no es válido.")]
    public AnimeStatus Status { get; set; }
}
