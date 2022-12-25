namespace TinyUrl.WebApi.Validators;

public static class UrlValidator
{
    public static bool ValidateFullUrl(string? fullUrl, out string errorMessage)
    {
        errorMessage = string.Empty;
        if (string.IsNullOrEmpty(fullUrl))
        {
            errorMessage = "Адрес не может быть пустым.";

            return false;
        }

        if (Uri.IsWellFormedUriString(fullUrl, UriKind.Absolute))
            return true;

        errorMessage = "Некорректный адрес";

        return false;
    }
}