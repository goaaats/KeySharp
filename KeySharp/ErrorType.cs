namespace KeySharp;

/// <summary>
/// Kind of error returned by the native library.
/// </summary>
public enum ErrorType
{
    /// <summary>
    /// No error occurred.
    /// </summary>
    NoError = 0,

    /// <summary>
    /// A generic error occurred. Please check the message property.
    /// </summary>
    GenericError,

    /// <summary>
    /// Password was not found.
    /// </summary>
    NotFound,

    /// <summary>
    /// Password was too long.
    /// Can only happen on Windows.
    /// </summary>
    PasswordTooLong = 10,

    /// <summary>
    /// Access was denied.
    /// Can only happen on Mac OS X.
    /// </summary>
    AccessDenied,
}