namespace Tekus.Application.Common.Options
{
    /// <summary>
    /// System preferences related to notifications. Configured in appsettings rather than
    /// through a management UI, since no preferences administration is required for this project.
    /// </summary>
    public class NotificationSettings
    {
        public const string SectionName = "Notifications";

        /// <summary>Recipient notified when a supplier enables a new service.</summary>
        public string NewServiceRecipientEmail { get; set; } = string.Empty;
    }
}
