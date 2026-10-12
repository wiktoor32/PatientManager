using System.ComponentModel.DataAnnotations;

namespace PatientManager.Api.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class ValidBirthDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not DateOnly date || date == default)
        {
            return new ValidationResult(
                "Date of birth is required.",
                new[] { validationContext.MemberName! });
        }

        DateOnly today = DateOnly.FromDateTime(DateTime.Today);

        if (date > today)
        {
            return new ValidationResult(
                "Date of birth cannot be in the future.",
                new[] { validationContext.MemberName! });
        }

        return ValidationResult.Success;
    }
}