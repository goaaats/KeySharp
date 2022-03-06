using System;
using System.Runtime.InteropServices;

namespace KeySharp;

internal static unsafe class Glue
{
    [DllImport("skeychain", EntryPoint = "setPassword")]
    public static extern bool SetPassword(byte* package, byte* service, byte* user, byte* password);

    [DllImport("skeychain", EntryPoint = "getPassword")]
    public static extern byte* GetPassword(byte* package, byte* service, byte* user);

    [DllImport("skeychain", EntryPoint = "deletePassword")]
    public static extern bool DeletePassword(byte* package, byte* service, byte* user);

    [DllImport("skeychain", EntryPoint = "getLastErrorMessage")]
    public static extern byte* GetLastErrorMessage();

    [DllImport("skeychain", EntryPoint = "getLastError")]
    public static extern ErrorType GetLastError();
}