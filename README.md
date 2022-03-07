# KeySharp [![Nuget](https://img.shields.io/nuget/v/KeySharp)](https://www.nuget.org/packages/KeySharp/)

Cross-platform OS keyring access for C#/.NET based on [keychain by hrantzsch](https://github.com/hrantzsch/keychain).
All calls are potentially blocking, as the OS may ask the user to grant access or unlock the keychain.

### Example

```csharp
Keyring.SetPassword("com.example.test", "TestService", "user", "password");

try {
  var password = Keyring.GetPassword("com.example.test", "TestService", "user");
} catch (KeyringException ex) // Thrown if password was not saved
  // handle
}

Keyring.DeletePassword("com.example.test", "TestService", "user");
```

### Native libraries
The precompiled shared libraries in this repository are based on the code in the `native` folder, wrapping the keychain library by hrantzsch.
| Platform | Compiler |
|----------|----------|
| win-x64  | Visual C++ 2022 (Keychain library doesn't support mingw-w64 at the moment, TBD) |
| osx-x64   | AppleClang 13.0.0.13000029 (universal), macOS 10.11+ |
| osx-arm64 | AppleClang 13.0.0.13000029 (universal), macOS 10.11+, take care: you need to sign your binaries to run on m1! |