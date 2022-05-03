using System;

namespace KeySharp;

/// <summary>
/// Exception thrown when an error during keyring operations occurs.
/// </summary>
public class KeyringException : Exception
{
    /// <summary>
    /// The type of error that occurred.
    /// </summary>
    public ErrorType Type { get; private set; }
    
    /// <summary>
    /// The message returned by the keyring backend.
    /// </summary>
    public string BackendMessage { get; private set; }
    
    internal KeyringException(ErrorType type, string message) : base($"{message} ({type})")
    {
        Type = type;
        BackendMessage = message;
    }
}