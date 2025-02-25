using Microsoft.AspNetCore.WebUtilities;

namespace SharedKernel.Application.HelperClasses;

public class QueryStringHelper
{
    public static string BuildPageUrl(
        string basePath, Dictionary<string, string?> queryParams)
    {
        return QueryHelpers.AddQueryString(basePath, queryParams);
    }
}
