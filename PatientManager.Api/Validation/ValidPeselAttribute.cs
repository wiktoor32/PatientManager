using System.ComponentModel.DataAnnotations;
using PatientManager.Api.Helpers;

namespace PatientManager.Api.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class ValidPeselAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not string pesel || !PeselHelper.IsValid(pesel))
        {
            return new ValidationResult(
                "Invalid PESEL number.",
                new[] { validationContext.MemberName! });
        }

        return ValidationResult.Success;
    }
}