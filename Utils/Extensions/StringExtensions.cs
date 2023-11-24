namespace Utils.Extensions;

public static class StringExtensions
{
    public static bool IsUrl(this string url) => Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                                                 (uriResult.Scheme == Uri.UriSchemeHttp ||
                                                  uriResult.Scheme == Uri.UriSchemeHttps);
}