# KeySharp [![Nuget](https://img.shields.io/nuget/v/MinSharp)](https://www.nuget.org/packages/MinSharp/)

Cross-platform OS keyring access for C#/.NET based on [keychain by hrantzsch](https://github.com/hrantzsch/keychain).

### Example

```csharp
private delegate int MessageBoxWDelegate(
  IntPtr hWnd,
  [MarshalAs(UnmanagedType.LPWStr)] string text,
  [MarshalAs(UnmanagedType.LPWStr)] string caption,
  NativeFunctions.MessageBoxType type);

IntPtr pTarget = [...]; // Find address of target function

var hook = new Hook<MessageBoxWDelegate>(pTarget, MessageBoxWDetour);
hook.Enable();

private int MessageBoxWDetour(IntPtr hwnd, string text, string caption, NativeFunctions.MessageBoxType type)
{
    Console.WriteLine($"Hook triggered: {hwnd:X} {text} {caption} {type}");
    return this.messageBoxMinHook.Original(hwnd, text, caption, type);
}
```

### Native libraries
The precompiled shared libraries in this repository are based on the code in the `native` folder, wrapping the keychain library by hrantzsch.
| Platform | Compiler |
|----------|----------|
| Windows  | Visual C++ 2022 (Keychain library doesn't support mingw-w64 at the moment, TBD) |
|          |          |
|          |          |
