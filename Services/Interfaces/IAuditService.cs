using shiftmaster.models;

namespace Services.Interfaces
{
    /// <summary>
    /// Interface for audit logging service to track authentication events and other critical operations.
    /// </summary>
    public interface IAuditService
    {
        /// <summary>
        /// Log a login attempt (successful or failed).
        /// </summary>
        /// <param name="userId">The user ID who attempted login (nullable for failed attempts where user doesn't exist).</param>
        /// <param name="isSuccess">Whether the login attempt succeeded.</param>
        /// <param name="ipAddress">Client IP address.</param>
        /// <param name="userAgent">Client user agent string.</param>
        /// <param name="authMethod">Authentication method used (e.g., "Password", "OAuth").</param>
        /// <param name="correlationId">Request correlation ID for tracing.</param>
        /// <param name="source">Source/channel of the login attempt (e.g., "Web", "Mobile", "API").</param>
        /// <param name="details">Optional details (e.g., failure reason).</param>
        /// <param name="clientAppVersion">Optional client application version.</param>
        Task LogLoginAttemptAsync(
            int? userId,
            bool isSuccess,
            string ipAddress,
            string userAgent,
            string authMethod = "Password",
            string correlationId = null,
            string source = "Web",
            string details = null,
            string clientAppVersion = null);

        /// <summary>
        /// Log a registration attempt.
        /// </summary>
        /// <param name="userId">The newly created user ID.</param>
        /// <param name="isSuccess">Whether the registration succeeded.</param>
        /// <param name="ipAddress">Client IP address.</param>
        /// <param name="userAgent">Client user agent string.</param>
        /// <param name="correlationId">Request correlation ID for tracing.</param>
        /// <param name="source">Source/channel of the registration (e.g., "Web", "Mobile").</param>
        /// <param name="details">Optional details (e.g., registration source or method).</param>
        /// <param name="clientAppVersion">Optional client application version.</param>
        Task LogRegistrationAsync(
            int? userId,
            bool isSuccess,
            string ipAddress,
            string userAgent,
            string correlationId = null,
            string source = "Web",
            string details = null,
            string clientAppVersion = null);

        /// <summary>
        /// Log a generic audit event.
        /// </summary>
        /// <param name="action">Action description (e.g., "UserLogin", "UserRegister", "PasswordChange").</param>
        /// <param name="entityType">Entity type being acted upon (e.g., "User", "Auth").</param>
        /// <param name="recordId">ID of the record (nullable for anonymous events).</param>
        /// <param name="userId">ID of the user performing the action (nullable for anonymous events).</param>
        /// <param name="ipAddress">Client IP address.</param>
        /// <param name="userAgent">Client user agent string.</param>
        /// <param name="details">Optional details about the event.</param>
        Task LogAuditEventAsync(
            string action,
            string entityType,
            int? recordId,
            int? userId,
            string ipAddress,
            string userAgent,
            string details = null);
    }
}
