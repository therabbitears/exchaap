namespace Xaminals.iOS
{
    public class AppConstants
    {
        public static string[] SubscriptionTags { get; set; } = { "default" };
        public const string ListenConnectionString = "Endpoint=sb://loffershub.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=X0wizONl+TL9ke5gEo33sZVfokfC6rU+VdfygJnhLpA=";
        public const string NotificationHubName = "loffersHub";
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";

    }
}