namespace PatientManager.Api.Helpers;

public static class PeselHelper
{
    private static readonly int[] Weights =
        [1, 3, 7, 9, 1, 3, 7, 9, 1, 3];

    public static bool IsValid(string pesel)
    {
        if (pesel.Length != 11 || pesel.Any(c => c < '0' || c > '9'))
        {
            return false;
        }

        int sum = 0;

        for (int i = 0; i < Weights.Length; i++)
        {
            int digit = pesel[i] - '0';
            sum += digit * Weights[i];
        }

        int controlDigit = (10 - (sum % 10)) % 10;

        if (controlDigit != pesel[10] - '0')
        {
            return false;
        }

        int year = int.Parse(pesel.AsSpan(0, 2));
        int encodedMonth = int.Parse(pesel.AsSpan(2, 2));
        int day = int.Parse(pesel.AsSpan(4, 2));

        int century;
        int month;

        switch (encodedMonth)
        {
            case >= 81 and <= 92:
                century = 1800;
                month = encodedMonth - 80;
                break;

            case >= 1 and <= 12:
                century = 1900;
                month = encodedMonth;
                break;

            case >= 21 and <= 32:
                century = 2000;
                month = encodedMonth - 20;
                break;

            case >= 41 and <= 52:
                century = 2100;
                month = encodedMonth - 40;
                break;

            case >= 61 and <= 72:
                century = 2200;
                month = encodedMonth - 60;
                break;

            default:
                return false;
        }

        year += century;

        if (day < 1 || day > DateTime.DaysInMonth(year, month))
        {
            return false;
        }

        DateOnly dateOfBirth = new DateOnly(year, month, day);
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);

        return dateOfBirth <= today;
    }
}