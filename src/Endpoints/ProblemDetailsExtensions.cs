using Flunt.Notifications;
using Microsoft.AspNetCore.Identity;

namespace IWantApp.Endpoints;

public static class ProblemDetailsExtensions
{
    public static Dictionary<string, string[]> ConvertToProblemDetails(
        this IReadOnlyCollection<Notification> notifications)
    {
        return notifications
            .GroupBy(group => group.Key)
            .ToDictionary(group => group.Key,
                group => group.Select(item => item.Message).ToArray());
    }

    public static Dictionary<string, string[]> ConvertToProblemDetails(
        this IEnumerable<IdentityError> errors)
    {
        var dictionary = new Dictionary<string, string[]>
        {
            {
                "Error", errors
                    .Select(e => e.Description).ToArray()
            }
        };

        return dictionary;
    }
}
