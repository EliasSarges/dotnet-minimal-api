using Flunt.Notifications;

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
}
