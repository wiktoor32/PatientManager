using System.ComponentModel.DataAnnotations;
using PatientManager.Api.Helpers;

namespace PatientManager.Api.Validation;

[AttributeUsage(AttributeTargets.Property)]
public class ValidPhoneNumberAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(
        object? value,
        ValidationContext validationContext)
    {
        if (value is not string phoneNumber || !PhoneNumberHelper.TryNormalize(phoneNumber, out _))
        {
            return new ValidationResult(
                "Invalid phone number.",
                new[] { validationContext.MemberName! });
        }

        return ValidationResult.Success;
    }
}