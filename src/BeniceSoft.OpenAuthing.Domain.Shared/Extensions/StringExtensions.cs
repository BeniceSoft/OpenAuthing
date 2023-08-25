namespace BeniceSoft.OpenAuthing.Extensions;

public static class StringExtensions
{
    public static string Mask(this string source, int start, int maskLength)
    {
        return source.Mask(start, maskLength, '*');
    }

    public static string Mask(this string value, int start, int maskLength, char maskCharacter)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        var result = value;

        if (value.Length < start) return result;

        var mask = new string(maskCharacter, maskLength);
        result = value.Insert(start, mask);

        result = result.Length >= (start + (maskLength * 2)) ? result.Remove(start + maskLength, maskLength) : result.Remove(start + maskLength, result.Length - (start + maskLength));

        return result;
    }
}