using System.Text.RegularExpressions;
using PhoneNumbers;

namespace PatientManager.Api.Helpers;

public static class PhoneNumberHelper
{
    private static readonly PhoneNumberUtil PhoneUtil = PhoneNumberUtil.GetInstance();

    public static bool TryNormalize(
        string input,
        out string? normalizedNumber)
    {
        normalizedNumber = null;

        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        string phoneNumber = input.Trim();

        if (phoneNumber.Length > 40 || !Regex.IsMatch(phoneNumber, @"\A\+?[0-9 ()-]+\z"))
        {
            return false;
        }

        try
        {
            PhoneNumber parsedNumber = PhoneUtil.Parse(phoneNumber, "PL");

            if (!PhoneUtil.IsValidNumber(parsedNumber))
            {
                return false;
            }

            normalizedNumber = PhoneUtil.Format(
                parsedNumber,
                PhoneNumberFormat.E164);

            return true;
        }
        catch (NumberParseException)
        {
            return false;
        }
    }
}