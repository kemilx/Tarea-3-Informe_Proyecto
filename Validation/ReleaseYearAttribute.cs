using System.ComponentModel.DataAnnotations;

namespace AnimeCatalog.Api.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ReleaseYearAttribute(int minimumYear = 1917)
    : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not int year)
        {
            return ValidationResult.Success;
        }

        var maximumYear = DateTime.UtcNow.Year + 1;
        if (year >= minimumYear && year <= maximumYear)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(
            $"El año de estreno debe estar entre {minimumYear} y {maximumYear}.");
    }
}
