---
title: "Environment diagnostics with maui doctor"
description: "Learn how to use the maui doctor command to diagnose, troubleshoot, and auto-fix .NET MAUI development environment issues."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# Environment diagnostics with `maui doctor`

The `maui doctor` command runs a comprehensive set of checks on your .NET MAUI development environment, identifies issues, and can automatically fix common problems. Use it to verify that your machine is correctly set up for .NET MAUI development.

> [!IMPORTANT]
> The .NET MAUI CLI is experimental and will change between releases.

## Run diagnostics

Run the following command to perform a full environment check:

```dotnetcli
maui doctor
```

The command checks for the following components and their configurations:

- .NET SDK version and installed workloads
- Android SDK packages and platform tools
- Java Development Kit (JDK) version and configuration
- Xcode version and command-line tools (macOS)
- Apple simulator runtimes and availability (macOS)
- Visual Studio installation and required components
- Environment variables and path configuration

Each check reports an OK, warning, error, or skipped status so you can quickly identify what needs attention.

## Check a single platform

Use the `--platform` option to run only the checks for one platform. Supported values are `dotnet`, `android`, `apple`, and `windows`:

```dotnetcli
maui doctor --platform apple
```

Apple checks run only on macOS, and Windows checks run only on Windows. On other operating systems, these checks report a skipped status.

## Auto-fix issues

Some issues include an automatic fix. To apply every available automatic fix, run:

```dotnetcli
maui doctor --fix
```

The `--fix` option doesn't prompt you to approve individual fixes. It runs the fix command for each issue that's marked as automatically fixable, and then runs the checks again so that the exit code reflects the results after the fixes. For issues without an automatic fix, the terminal output shows a short fix description. To get the detailed manual steps, run `maui doctor --json` and check the `manual_steps` values.

Automatic fixes include the following:

- Installing the Android SDK, Android SDK command-line tools, platform tools, and emulator.
- Accepting Android SDK licenses.
- Installing OpenJDK.
- Installing the .NET MAUI workload.
- Installing the Xcode Command Line Tools (macOS).
- Switching to a compatible Xcode version (macOS).

A fix that requires administrator permission, such as switching the selected Xcode, runs with `sudo`, so you might be prompted for your password.

## Xcode compatibility (macOS)

On macOS, when Xcode is installed, `maui doctor` checks whether the selected Xcode version matches the version required by the .NET Apple SDK packs installed for the .NET runtime that's running the CLI. For example, it checks the `Microsoft.iOS.Sdk` and `Microsoft.MacCatalyst.Sdk` packs. The check compares major and minor versions only, and it appears as **Xcode Compatibility** in the output. When the check fails, the warning message shows the required Xcode version and the SDK packs that require it.

When the versions don't match, the check reports a warning with issue ID `E2224`. What happens next depends on which Xcode versions are installed:

- **A compatible Xcode is installed.** The warning includes an automatic fix. Run `maui doctor --fix` to switch to the compatible Xcode with a command like the following:

  ```bash
  sudo xcode-select --switch "/Applications/Xcode-26.2.0.app"
  ```

- **No compatible Xcode is installed.** The warning describes the fix. Install the required Xcode version from the Mac App Store or Apple Developer downloads, and then select it with `sudo xcode-select --switch "/Applications/<Xcode>.app"`, replacing `<Xcode>` with the name of the installed app. To list the Xcode versions that are installed, run `maui apple xcode list`. For more information, see [Apple platform management](apple-management.md).
- **The installed SDK packs require different Xcode versions.** For example, the iOS SDK pack requires one Xcode version and the Mac Catalyst SDK pack requires another. There's no automatic fix. Run `dotnet workload update` so that the installed Apple SDK packs require the same Xcode version. If that doesn't resolve the mismatch, run `dotnet workload repair`. To inspect the installed workloads, run `dotnet workload list`.

If no Apple SDK packs are installed for the current .NET runtime, the check is skipped.

After you switch Xcode, run `maui doctor --platform apple` to confirm that the check passes. Because an Xcode mismatch is a warning, it doesn't cause `maui doctor` to return a nonzero exit code.

## Use in CI pipelines

For continuous integration environments, use the `--json` and `--ci` flags together:

```dotnetcli
maui doctor --json --ci
```

The `--ci` flag makes the command non-interactive and causes it to fail fast when a critical issue is detected, which is useful for gating CI pipelines. Combine with `--json` to produce machine-readable output that you can pipe to other tools:

```dotnetcli
maui doctor --json --ci | jq '.checks[] | select(.status == "Error")'
```

In JSON output, each check has a `status` value of `Ok`, `Warning`, `Error`, or `Skipped`. When a check has a fix, its `fix` object includes the `issue_id`, `description`, and `auto_fixable` values, plus either the `command` that `--fix` runs or a list of `manual_steps`.

`maui doctor` returns exit code `1` when at least one check reports an error. Warnings don't cause a nonzero exit code.

## Verbose output

For detailed diagnostic information, use the `-v` flag:

```dotnetcli
maui doctor -v
```

Verbose output includes additional details about each check, such as exact file paths, version strings, and environment variable values. This level of detail is helpful when reporting issues or debugging edge cases in your environment.

## See also

- [.NET MAUI CLI overview](index.md)
