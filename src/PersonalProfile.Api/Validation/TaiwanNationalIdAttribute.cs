using System.ComponentModel.DataAnnotations;

namespace PersonalProfile.Api.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class TaiwanNationalIdAttribute : ValidationAttribute
{
    private static readonly Dictionary<char, int> AreaCodes = new()
    {
        ['A'] = 10, ['B'] = 11, ['C'] = 12, ['D'] = 13, ['E'] = 14,
        ['F'] = 15, ['G'] = 16, ['H'] = 17, ['I'] = 34, ['J'] = 18,
        ['K'] = 19, ['L'] = 20, ['M'] = 21, ['N'] = 22, ['O'] = 35,
        ['P'] = 23, ['Q'] = 24, ['R'] = 25, ['S'] = 26, ['T'] = 27,
        ['U'] = 28, ['V'] = 29, ['W'] = 32, ['X'] = 30, ['Y'] = 31,
        ['Z'] = 33
    };

    public TaiwanNationalIdAttribute()
    {
        ErrorMessage = "身分證字號不符合台灣規則。";
    }

    public override bool IsValid(object? value)
    {
        if (value is not string input)
        {
            return false;
        }

        var id = input.Trim().ToUpperInvariant();
        if (id.Length != 10 || !AreaCodes.TryGetValue(id[0], out var areaCode))
        {
            return false;
        }

        if (id[1] is not ('1' or '2'))
        {
            return false;
        }

        for (var i = 1; i < id.Length; i++)
        {
            if (!char.IsDigit(id[i]))
            {
                return false;
            }
        }

        var sum = areaCode / 10 + (areaCode % 10) * 9;
        for (var i = 1; i <= 8; i++)
        {
            sum += (id[i] - '0') * (9 - i);
        }

        sum += id[9] - '0';
        return sum % 10 == 0;
    }
}
