using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace KeySharp;

/// <summary>
/// Class used to interface with the OS keyring.
/// </summary>
public static unsafe class Keyring
{
    /// <summary>
    /// Save a password to the keyring.
    /// </summary>
    /// <param name="package">The package ID.</param>
    /// <param name="service">The service name.</param>
    /// <param name="username">The user name.</param>
    /// <param name="password">The password to save.</param>
    public static void SetPassword(string package, string service, string username, string password)
    {
        var nativePackage = AllocateNullTerminated(package);
        var nativeService = AllocateNullTerminated(service);
        var nativeUsername = AllocateNullTerminated(username);
        var nativePassword = AllocateNullTerminated(password);

        var ret = Glue.SetPassword(nativePackage, nativeService, nativeUsername, nativePassword);

        if (!ret)
        {
            ThrowLastError();
        }

        Util.Free(nativePackage);
        Util.Free(nativeService);
        Util.Free(nativeUsername);
        Util.Free(nativePassword);
    }

    /// <summary>
    /// Get a password from the keyring.
    /// </summary>
    /// <param name="package">The package ID.</param>
    /// <param name="service">The service name.</param>
    /// <param name="username">The user name.</param>
    /// <returns>The saved password.</returns>
    public static string GetPassword(string package, string service, string username)
    {
        var nativePackage = AllocateNullTerminated(package);
        var nativeService = AllocateNullTerminated(service);
        var nativeUsername = AllocateNullTerminated(username);

        var ret = Glue.GetPassword(nativePackage, nativeService, nativeUsername);

        if (ret == null)
        {
            ThrowLastError();
        }

        Util.Free(nativePackage);
        Util.Free(nativeService);
        Util.Free(nativeUsername);

        return ReadString(ret);
    }

    /// <summary>
    /// Delete a password.
    /// </summary>
    /// <param name="package">The package ID.</param>
    /// <param name="service">The service name.</param>
    /// <param name="username">The user name.</param>
    public static void DeletePassword(string package, string service, string username)
    {
        var nativePackage = AllocateNullTerminated(package);
        var nativeService = AllocateNullTerminated(service);
        var nativeUsername = AllocateNullTerminated(username);

        var ret = Glue.DeletePassword(nativePackage, nativeService, nativeUsername);

        if (!ret)
        {
            ThrowLastError();
        }

        Util.Free(nativePackage);
        Util.Free(nativeService);
        Util.Free(nativeUsername);
    }

    private static string ReadString(byte* data)
    {
        var passData = new List<byte>();
        var c = *data;
        while (c != 0)
        {
            passData.Add(c);
            c = *(data + 1);
            data++;
        }

        return Encoding.UTF8.GetString(passData.ToArray());
    }

    private static void ThrowLastError()
    {
        var errorMsg = Glue.GetLastErrorMessage();
        var msgString = "Unknown error";

        if (errorMsg != null)
            msgString = ReadString(errorMsg);

        throw new KeyringException(Glue.GetLastError(), msgString);
    }

    private static byte* AllocateNullTerminated(string text)
    {
        byte* native;
        if (text != null)
        {
            var byteCount = Encoding.UTF8.GetByteCount(text);
            native = Util.Allocate(byteCount + 1);
            var nativeLabelOffset = Util.GetUtf8(text, native, byteCount);
            native[nativeLabelOffset] = 0;
        }
        else { native = null; }

        return native;
    }
}

internal static unsafe class Util
{
    internal static byte* Allocate(int byteCount) => (byte*)Marshal.AllocHGlobal(byteCount);

    internal static void Free(byte* ptr) => Marshal.FreeHGlobal((IntPtr)ptr);

    internal static int GetUtf8(string s, byte* utf8Bytes, int utf8ByteCount)
    {
        fixed (char* utf16Ptr = s)
        {
            return Encoding.UTF8.GetBytes(utf16Ptr, s.Length, utf8Bytes, utf8ByteCount);
        }
    }
}