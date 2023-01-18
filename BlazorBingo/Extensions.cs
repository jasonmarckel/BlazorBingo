namespace BlazorBingo;

public static class Extensions
{
    public static string CssName(this string value)
    {
        return value.Replace(" ", string.Empty).ToLower();
    }
}