using System;

namespace KeySharp;

/// <summary>
/// Exception thrown when an error during keyring operations occurs.
/// </summary>
public class KeyringException : Exception
{
    internal KeyringException(ErrorType type, string message) : base($"{message} ({type})")
    {
    }
}