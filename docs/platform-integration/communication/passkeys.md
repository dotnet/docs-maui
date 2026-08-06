---
title: "Passkeys"
description: "Learn how to use the .NET MAUI Passkeys class to register and sign in with passkeys (WebAuthn/FIDO2 credentials) using the platform authenticator."
ms.date: 08/06/2026
no-loc: ["Microsoft.Maui", "Microsoft.Maui.Authentication", "WebAuthn", "FIDO2"]
---

# Passkeys

::: moniker range=">=net-maui-11.0"

[![Browse sample.](~/media/code-sample.png) Browse the sample](https://github.com/dotnet/maui-samples/tree/main/11.0/PlatformIntegration/Passkeys)

This article describes how you can use the .NET Multi-platform App UI (.NET MAUI) `IPasskeys` interface to register and sign in with *passkeys*. A passkey is a [WebAuthn/FIDO2](https://www.w3.org/TR/webauthn-3/) public-key credential that replaces a password with a key pair whose private key is managed by the platform authenticator and is never exposed to your app or to .NET MAUI. Signing in is completed by that authenticator, such as Face ID, Touch ID, Windows Hello, or an Android biometric prompt backed by the device's credential manager.

The default implementation of the `IPasskeys` interface is available through the `Passkeys.Default` property. Both the `IPasskeys` interface and the `Passkeys` class are contained in the `Microsoft.Maui.Authentication` namespace.

> [!IMPORTANT]
> Despite the similar name, passkeys are unrelated to <xref:Microsoft.Maui.Authentication.WebAuthenticator>. `WebAuthenticator` starts a browser-based OAuth redirect flow. `Passkeys` drives the native WebAuthn ceremony on the device. For more information about `WebAuthenticator`, see [Web authenticator](authentication.md).

## How the passkey flow works

The `Passkeys` API is deliberately thin. It brokers between your relying party (RP) server and the operating system's authenticator, and it performs no verification of its own:

1. Your app asks your RP server to begin a ceremony. The server returns standard WebAuthn options JSON.
1. Your app passes that JSON to `Passkeys.CreateAsync` or `Passkeys.AssertAsync`, which drives the native credential UI.
1. The API returns the standard WebAuthn response JSON produced by the authenticator.
1. Your app posts that response JSON back to your RP server, which verifies it and stores the credential or establishes the session.

Because the contract is WebAuthn JSON in and WebAuthn JSON out, it interoperates directly with existing server libraries, including the built-in passkey support in ASP.NET Core Identity. For more information, see [Enable Web Authentication API (WebAuthn) passkeys](/aspnet/core/security/authentication/passkeys).

### Security boundary

The client never validates a passkey. Understanding this split is essential before you ship a passkey flow:

| Responsibility | Owner |
| --- | --- |
| Generating the challenge | RP server |
| Presenting the platform authenticator UI | .NET MAUI / operating system |
| Verifying the challenge, RP ID, and origin | RP server |
| Verifying attestation at registration | RP server |
| Verifying the assertion signature at sign-in | RP server |
| Storing the credential's public key and sign counter | RP server |
| Establishing the authenticated session | RP server |

The private key is created and held by the platform authenticator, in secure hardware where available, and is never exposed to your app or to .NET MAUI. The API only relays the public attestation and assertion material.

> [!WARNING]
> Treat any response returned by `CreateAsync` or `AssertAsync` as untrusted input until your server has verified it. A client that trusts a passkey response without server-side verification provides no security benefit over no authentication at all.

### Origin binding

Browsers supply the WebAuthn `origin` from the current page. A native app has no page, so each platform derives the origin from the app's verified identity and writes it into the client data that your server validates:

| Platform | Origin used | Verified by |
| --- | --- | --- |
| Android | `android:apk-key-hash:<base64url-sha256-of-signing-certificate>` | Digital Asset Links binds the package name and signing certificate to the RP domain |
| iOS / Mac Catalyst | `https://<associated-domain>` | Associated Domains entitlement and the hosted `apple-app-site-association` file |
| Windows | `https://<rp-id>` | The RP ID; Windows doesn't use a separate app identity origin |

Your RP server must be configured to accept the app's native origin in addition to any web origin it already accepts. An unconfigured native origin is a common cause of a ceremony that completes on the device but fails verification on the server.

## Supported platforms

Call `Passkeys.IsSupported` before you present passkey UI, and fall back to your existing sign-in method when it returns `false`.

| Platform | Minimum version | Implementation |
| --- | --- | --- |
| Android | Android 14 (API 34) | Jetpack Credential Manager |
| iOS | iOS 16 | `AuthenticationServices` |
| Mac Catalyst | Mac Catalyst 16 | `AuthenticationServices` |
| Windows | Windows 10 version 1903 | Windows WebAuthn API (`webauthn.dll`) |

On Android, iOS, Mac Catalyst, and Windows versions below the minimum shown, `IsSupported` returns `false` and both `CreateAsync` and `AssertAsync` throw a <xref:Microsoft.Maui.ApplicationModel.FeatureNotSupportedException>. On targets with no passkey implementation at all, `IsSupported` returns `false` and the methods throw a not-supported exception.

`IsSupported` reflects only the platform and OS version. Other runtime prerequisites, such as a configured credential provider or an enrolled biometric, aren't checked by `IsSupported` and instead surface when a ceremony runs.

> [!NOTE]
> On Android, .NET MAUI takes a dependency on Jetpack Credential Manager only, and doesn't take a dependency on Google Play services. This keeps Google Play services out of your app's dependency graph, at the cost of limiting passkey support to Android 14 and later. The version check is enforced by .NET MAUI, so adding a Google Play services credential provider to your own app doesn't extend the `Passkeys` API to earlier API levels; supporting API 28-33 requires calling Credential Manager directly through platform-specific code.

> [!NOTE]
> Although the Apple passkey APIs exist on Mac Catalyst 16, the .NET 11 Mac Catalyst toolchain has a minimum deployment target of Mac Catalyst 17.0, so that's the effective floor for a Mac Catalyst app.

## Get started

To use passkeys, the following platform-specific setup is required. Passkeys are scoped to an RP domain. Apple and Android fetch a domain association file over HTTPS, so those platforms require a publicly reachable HTTPS domain that hosts your RP server; `localhost` isn't sufficient. Windows uses the HTTPS RP origin directly and requires no association file.

<!-- markdownlint-disable MD025 -->
# [Android](#tab/android)

Android binds the credential to both your package name and your app's signing certificate.

Host a [Digital Asset Links](https://developer.android.com/identity/sign-in/credential-manager#add-support-dal) file at `https://<rp-id>/.well-known/assetlinks.json` that delegates the `get_login_creds` relation to your app:

```json
[
  {
    "relation": ["delegate_permission/common.get_login_creds"],
    "target": {
      "namespace": "android_app",
      "package_name": "com.example.myapp",
      "sha256_cert_fingerprints": [
        "AA:BB:CC:DD:..."
      ]
    }
  }
]
```

The file must be served over HTTPS as `application/json`, without a redirect or an interstitial page.

The fingerprint must match the certificate that actually signed the installed APK. For debug builds, that's the .NET for Android debug keystore under the OS local application data directory at *Xamarin/Mono for Android/debug.keystore*, which is **not** Android Studio's *~/.android/debug.keystore*. Use `keytool` to read it:

```bash
keytool -list -v -keystore <path-to-debug.keystore> -alias androiddebugkey -storepass android -keypass android
```

Your RP server must also accept the corresponding native origin, which is the base64url-encoded SHA-256 of the same certificate, in the form `android:apk-key-hash:<base64url>`.

No intent filter is required. Digital Asset Links credential delegation is separate from Android App Links.

At runtime, the device also needs a credential provider, a signed-in account for that provider, and a secure screen lock. On an emulator, use a Google Play system image rather than an AOSP-only image, and sign in to a Google account.

# [iOS/Mac Catalyst](#tab/macios)

Apple requires three values to agree before it will create or use a passkey:

1. The app declares an Associated Domains entitlement containing `webcredentials:<rp-id>`.
1. `https://<rp-id>/.well-known/apple-app-site-association` lists `<TeamID>.<BundleID>` under `webcredentials`.
1. The app is signed by that Apple Developer team with a provisioning profile that includes the Associated Domains capability.

Add the entitlement to *Platforms/iOS/Entitlements.plist* and *Platforms/MacCatalyst/Entitlements.plist*:

```xml
<key>com.apple.developer.associated-domains</key>
<array>
  <string>webcredentials:myapp.example.com</string>
</array>
```

Host the association file at `https://<rp-id>/.well-known/apple-app-site-association`, served as JSON without a redirect:

```json
{
  "webcredentials": {
    "apps": ["ABCDE12345.com.example.myapp"]
  }
}
```

For more information, see [Supporting associated domains](https://developer.apple.com/documentation/xcode/supporting-associated-domains) on developer.apple.com.

> [!IMPORTANT]
> Associated Domains requires a paid Apple Developer account and an explicit App ID with the capability enabled. Free personal teams can't provision Associated Domains, and wildcard App IDs aren't sufficient.

The iOS Simulator needs the entitlement but doesn't need device signing. Physical iOS devices and Mac Catalyst require a provisioning profile that carries Associated Domains. Apple fetches and caches the association file, so verify it's publicly reachable and correct before you launch the app:

```bash
curl -i https://myapp.example.com/.well-known/apple-app-site-association
```

# [Windows](#tab/windows)

Windows trusts the HTTPS RP origin directly, so it doesn't use an association file. There's no Digital Asset Links or `apple-app-site-association` equivalent to host, and no entitlement to declare.

Windows requires:

- Windows 10 version 1903 or later, which provides the Windows WebAuthn API.
- Windows Hello configured, or a compatible FIDO2 security key.
- Network access from the app to your HTTPS RP server.

The Windows Security dialog is modal on the app's top-level window, which .NET MAUI resolves from the active window. Don't start a ceremony while your app has no foreground window.

-----
<!-- markdownlint-enable MD025 -->

## Register a passkey

Registration creates a new credential. Ask your server to begin the ceremony, pass the returned `PublicKeyCredentialCreationOptions` JSON to `Passkeys.CreateAsync`, and post the resulting response JSON back to your server:

```csharp
using System.Text;
using Microsoft.Maui.Authentication;

if (!Passkeys.IsSupported)
{
    // Fall back to your existing sign-in method.
    return;
}

// 1. Ask your server to begin registration. It returns PublicKeyCredentialCreationOptions JSON.
using HttpResponseMessage beginResponse =
    await httpClient.PostAsync("/passkeys/register/begin", content: null);
beginResponse.EnsureSuccessStatusCode();
string creationOptionsJson = await beginResponse.Content.ReadAsStringAsync();

// 2. Drive the native create-credential UI.
PasskeyCreationResponse created = await Passkeys.CreateAsync(creationOptionsJson);

// 3. Post the response JSON back to your server to verify attestation and store the public key.
using var body = new StringContent(created.ToString(), Encoding.UTF8, "application/json");
using HttpResponseMessage finishResponse =
    await httpClient.PostAsync("/passkeys/register/finish", body);
finishResponse.EnsureSuccessStatusCode();

// Optional: store the credential ID so you can reference this passkey later.
string credentialId = created.Id;
```

> [!IMPORTANT]
> `created.ToString()` is already WebAuthn JSON. Post it as a raw `application/json` body, as shown. Don't use `PostAsJsonAsync`, which re-encodes the string as a quoted JSON literal and causes server-side verification to fail.

## Sign in with a passkey

Authentication proves possession of an existing credential. The shape mirrors registration, using `PublicKeyCredentialRequestOptions` and `Passkeys.AssertAsync`:

```csharp
using System.Text;
using Microsoft.Maui.Authentication;

if (!Passkeys.IsSupported)
{
    // Fall back to your existing sign-in method.
    return;
}

// 1. Ask your server to begin sign-in. It returns PublicKeyCredentialRequestOptions JSON.
using HttpResponseMessage beginResponse =
    await httpClient.PostAsync("/passkeys/login/begin", content: null);
beginResponse.EnsureSuccessStatusCode();
string requestOptionsJson = await beginResponse.Content.ReadAsStringAsync();

// 2. Drive the native get-credential UI so the user selects a passkey and authenticates.
PasskeyAssertionResponse asserted = await Passkeys.AssertAsync(requestOptionsJson);

// 3. Post the response JSON back to your server to verify the signature and finish sign-in.
using var body = new StringContent(asserted.ToString(), Encoding.UTF8, "application/json");
using HttpResponseMessage finishResponse =
    await httpClient.PostAsync("/passkeys/login/finish", body);
finishResponse.EnsureSuccessStatusCode();

// Optional: fields that are commonly read on the device.
string credentialId = asserted.Id;        // Which passkey was used, base64url.
string? userHandle = asserted.UserHandle; // The RP's user.id, when the authenticator returns one.
```

> [!NOTE]
> A passkey ceremony spans two server calls, and the server correlates them with the challenge it issued. Use a single `HttpClient` whose handler preserves the session, so that the `finish` request is associated with the `begin` request.

## Configure the ceremony

Most WebAuthn configuration travels inside the options JSON that your server produces, so you set it server-side rather than through the .NET MAUI API. This includes `authenticatorSelection.userVerification`, `authenticatorSelection.authenticatorAttachment`, `authenticatorSelection.residentKey`, `timeout`, `excludeCredentials`, `allowCredentials`, `attestation`, and `extensions`. New WebAuthn fields can therefore be supplied without a .NET API change, although the resulting behavior depends on each platform's native support. Android passes the JSON through unchanged, while iOS, Mac Catalyst, and Windows map the subset the OS supports and ignore the rest.

Two behaviors can't be expressed in the options JSON, and are surfaced by the API instead.

### Prefer immediately available credentials

`PreferImmediatelyAvailable` is a presentation choice that keeps the ceremony on the current device and skips the cross-device flow, such as a QR code or "use another device":

```csharp
var options = new PasskeyRequestOptions(requestOptionsJson)
{
    PreferImmediatelyAvailable = true
};

PasskeyAssertionResponse asserted = await Passkeys.AssertAsync(options);
```

| Platform | Behavior when `true` |
| --- | --- |
| Android | Uses a locally available credential if one exists; otherwise fails fast instead of launching the hybrid or QR flow |
| iOS / Mac Catalyst | Presents only when a local platform passkey exists; otherwise fails without showing the nearby-device sheet |
| Windows | Ignored. The Windows Security dialog is always shown |

This setting is most useful for sign-in. On Android, iOS, and Mac Catalyst, sign-in with no immediately available credential fails with an `InvalidOperationException` rather than a cancellation. Windows ignores the setting entirely.

### Cancellation

Both methods accept a <xref:System.Threading.CancellationToken> so you can abort an in-flight ceremony, for example when the user navigates away or your own timeout elapses:

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(2));

try
{
    PasskeyAssertionResponse asserted =
        await Passkeys.AssertAsync(requestOptionsJson, cts.Token);
}
catch (OperationCanceledException)
{
    // The ceremony was cancelled, either by the user or by the token.
}
```

> [!NOTE]
> Catch <xref:System.OperationCanceledException> rather than <xref:System.Threading.Tasks.TaskCanceledException>. Cancellation during a ceremony surfaces as `TaskCanceledException`, but a token that's already cancelled when you call the method short-circuits with the base `OperationCanceledException`. Because `TaskCanceledException` derives from `OperationCanceledException`, catching the base type handles both.

## Handle errors

Failures surface as standard .NET exceptions rather than a passkey-specific exception type:

| Situation | Exception |
| --- | --- |
| The platform or OS version doesn't support passkeys | <xref:Microsoft.Maui.ApplicationModel.FeatureNotSupportedException> |
| The user dismissed the native UI, or the ceremony was cancelled by the `CancellationToken` | <xref:System.Threading.Tasks.TaskCanceledException> |
| The `CancellationToken` was already cancelled when the method was called | <xref:System.OperationCanceledException> |
| No matching credential was available during sign-in | <xref:System.InvalidOperationException> |
| The options JSON was malformed or missing required members | <xref:System.ArgumentException> |
| Domain association isn't configured, or any other native failure | <xref:System.InvalidOperationException> containing the native error message |

Cancellation normalizes to <xref:System.Threading.Tasks.TaskCanceledException>, which matches the behavior of <xref:Microsoft.Maui.Authentication.WebAuthenticator>, so catch the base <xref:System.OperationCanceledException> to cover both cancellation paths. A missing credential is deliberately *not* a cancellation, so you can distinguish "the user changed their mind" from "there's nothing to sign in with":

```csharp
try
{
    PasskeyAssertionResponse asserted = await Passkeys.AssertAsync(requestOptionsJson);
    // Post asserted.ToString() to your server.
}
catch (FeatureNotSupportedException)
{
    // Passkeys aren't available on this device. Offer another sign-in method.
}
catch (OperationCanceledException)
{
    // The user dismissed the prompt. Leave the sign-in screen as it was.
}
catch (InvalidOperationException ex)
{
    // No credential was available, or the ceremony failed.
    // ex.Message carries the platform error, which is useful in logs but not to end users.
    logger.LogWarning(ex, "Passkey assertion failed.");
}
```

> [!NOTE]
> On iOS, Mac Catalyst, and Windows, malformed options JSON is normalized to <xref:System.ArgumentException> because those platforms parse the JSON to build a native request. Android passes the JSON to Jetpack Credential Manager largely unchanged, so an invalid document can also surface as an <xref:System.InvalidOperationException> reported by the credential provider. Handle both when validating server output.

> [!TIP]
> Don't surface the raw platform message to users. Log it for diagnostics, and show a recovery path such as signing in with a password and then registering a passkey.

## Production considerations

- **Always verify server-side.** Generate every challenge on the server, use it once, and validate the challenge, RP ID, origin, attestation, and assertion before you establish a session.
- **Accept the native origins.** Configure your RP server to accept the Android `android:apk-key-hash:` origin and the Apple associated-domain origin for every build you ship, including each signing certificate you use. Debug and release builds have different Android fingerprints.
- **Keep a fallback.** `IsSupported` is `false` on older OS versions and on platforms without passkey support, and a user can decline the prompt. Never make a passkey the only way into an account.
- **Support multiple credentials per account.** Users sign in from more than one device, and passkeys don't always sync across ecosystems. Let a signed-in user register additional passkeys, list them, and revoke them.
- **Use a stable RP ID.** The RP ID is baked into every credential. Changing it invalidates every passkey your users have registered.
- **Don't authenticate native API calls with cookies.** The reference sample uses a cookie for brevity. A production native app should exchange a verified assertion for a token that's appropriate for your API.

## Troubleshooting

| Symptom | What to check |
| --- | --- |
| `IsSupported` is `false` on Android | `IsSupported` only checks the OS version. The device or emulator must run Android 14 (API 34) or later |
| `IsSupported` is `false` on Windows | `IsSupported` only checks that the Windows WebAuthn API is present. Use Windows 10 version 1903 or later |
| `IsSupported` is `true` but the ceremony fails immediately on Android | The device needs a credential provider with a signed-in account and a secure screen lock. On an emulator, use a Google Play system image |
| `IsSupported` is `true` but the ceremony fails immediately on Windows | Configure Windows Hello, or attach a compatible FIDO2 security key |
| Android reports that the request can't be validated | The package name, the signing certificate of the installed APK, the `sha256_cert_fingerprints` value in *assetlinks.json*, and the `android:apk-key-hash:` origin your server accepts must all agree |
| Apple reports that the domain isn't associated | Compare the signed team ID and bundle ID, the `webcredentials:` entitlement, and the `apps` entry in the association file; all three must match exactly |
| The association file returns HTML | The domain is serving an interstitial or a redirect. Both files must be served anonymously as JSON over HTTPS |
| The ceremony succeeds but the server rejects it | The server's expected origin most likely doesn't include the app's native origin. Compare the `origin` in the returned client data with the origins your server accepts |
| The `finish` call reports that no ceremony is in progress | The `begin` and `finish` requests weren't correlated. Reuse one `HttpClient` and preserve the session between the two calls |
| `PreferImmediatelyAvailable` appears to be ignored | This is expected on Windows, where the native API has no equivalent |

## See also

- [Passkeys sample](https://github.com/dotnet/maui-samples/tree/main/11.0/PlatformIntegration/Passkeys)
- [Web authenticator](authentication.md)
- [Enable Web Authentication API (WebAuthn) passkeys](/aspnet/core/security/authentication/passkeys)
- [Web Authentication: An API for accessing Public Key Credentials Level 3](https://www.w3.org/TR/webauthn-3/)
- [Passkeys](https://fidoalliance.org/passkeys/) on fidoalliance.org
- [Sign in your user with Credential Manager](https://developer.android.com/identity/sign-in/credential-manager) on developer.android.com
- [Supporting passkeys](https://developer.apple.com/documentation/authenticationservices/supporting-passkeys) on developer.apple.com
- [WebAuthn API](/windows/win32/api/webauthn/)

::: moniker-end

::: moniker range="<=net-maui-10.0"

The `Passkeys` API is available starting in .NET MAUI 11. For earlier versions, use [Web authenticator](authentication.md) for browser-based authentication flows, or call each platform's passkey API directly. For more information about invoking platform APIs, see [Invoke platform code](~/platform-integration/invoke-platform-code.md).

::: moniker-end
