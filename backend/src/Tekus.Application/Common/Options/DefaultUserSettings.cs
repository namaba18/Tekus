namespace Tekus.Application.Common.Options
{
    /// <summary>
    /// Credentials for the single, pre-provisioned application user.
    /// No user registration/management is required for this project, so authentication
    /// is performed against this fixed account configured in appsettings.
    /// </summary>
    public class DefaultUserSettings
    {
        public const string SectionName = "DefaultUser";

        public string Username { get; set; } = string.Empty;

        /// <summary>Hash of the default user's password (see <see cref="Auth.IPasswordHasher"/>).</summary>
        public string PasswordHash { get; set; } = string.Empty;
    }
}
