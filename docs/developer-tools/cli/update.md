---
title: "Update the .NET MAUI CLI"
description: "Learn how to update the .NET MAUI CLI with maui update --self, and how to control daily update notices."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# Update the .NET MAUI CLI

The `maui update --self` command updates the .NET MAUI CLI (`Microsoft.Maui.Cli`) to the latest eligible version. The CLI can also tell you when a newer version is available.

> [!IMPORTANT]
> The .NET MAUI CLI is experimental and will change between releases.

## Update the CLI

Run the following command to update the CLI:

```bash
maui update --self
```

The `--self` option is required. It confirms that you want to update the CLI itself. The `maui update` command doesn't update .NET MAUI projects. If you omit `--self`, the command reports an error and doesn't update anything.

The command runs `dotnet tool update --global Microsoft.Maui.Cli` with the .NET SDK, so an installed .NET SDK is required. The SDK chooses the latest eligible version, and the command doesn't downgrade the CLI.

The following options are available:

| Option | Description |
|---|---|
| `--self` | Required. Update the .NET MAUI CLI itself, not a .NET MAUI project. |
| `--prerelease` | Include preview releases. This option is applied automatically when the installed CLI is a preview version. |
| `--dry-run` | Show the command that would run, and the working directory, without updating anything. |
| `--json` | Output the result in JSON format. |

You can also use the `--ci` and `--verbose` (`-v`) global options.

For example, to preview the update without running it:

```bash
maui update --self --dry-run
```

```console
ℹ Would run: dotnet tool update --global Microsoft.Maui.Cli --prerelease
ℹ Working directory: /Users/me/src
```

### Supported installations

Self-update supports only the default global .NET tool installation, which you get when you install the CLI with `dotnet tool install -g`. It doesn't support local tool manifests, custom `--tool-path` installations, or builds from source. For a local tool manifest, run `dotnet tool update Microsoft.Maui.Cli --prerelease` from the folder that contains the manifest. For a custom tool path, run `dotnet tool update Microsoft.Maui.Cli --prerelease --tool-path <path>`. For a build from source, rebuild from the latest source.

### Run from a trusted directory

The update uses the .NET SDK and NuGet configuration for the current directory, including any `global.json` and `NuGet.config` files. The command doesn't change any feeds or configuration files.

> [!WARNING]
> A repository-local `NuGet.config` file can select package feeds and influence which CLI package version is installed. Run `maui update --self` from a trusted directory. To avoid a repository's configuration, run the command from a trusted directory outside that repository.

### Platform behavior

On macOS and Linux, the command waits for the .NET SDK to finish the update and reports the result in the same terminal session.

On Windows, the running CLI can't replace its own files, so `maui update --self` starts a separate PowerShell process that waits for the CLI to exit and then runs the .NET SDK update. The command prints the path of a log file that records the SDK output and a final `SDK_EXIT` line:

- In an interactive terminal, the update process prints progress and a final completion or failure message in the same terminal. Your shell prompt might return before these messages appear. Wait for the completion message before you run the CLI again, and keep the terminal open until the update completes.
- If you miss the message, check the log file. `SDK_EXIT=0` indicates success, and any other value indicates failure.
- The update process waits up to one minute for the original CLI process to exit.
- With `--json`, `--ci`, or redirected input or output, no completion message is printed. Check the log file for the result.

### JSON output

With `--json`, the command reports a `status` value, the `arguments` passed to `dotnet`, and the `working_directory`:

```bash
maui update --self --dry-run --json
```

```json
{
  "status": "dry_run",
  "arguments": [
    "tool",
    "update",
    "--global",
    "Microsoft.Maui.Cli",
    "--prerelease"
  ],
  "working_directory": "/Users/me/src"
}
```

The `status` value is one of the following:

| Status | Meaning |
|---|---|
| `dry_run` | The command ran with `--dry-run` and didn't update anything. |
| `completed` | The update succeeded (macOS and Linux). The output also includes `exit_code`, `standard_output`, and `standard_error`. |
| `failed` | The update failed (macOS and Linux). The output also includes `exit_code`, `standard_output`, and `standard_error`. |
| `started` | The update process started (Windows). The output also includes `log_path`. |

On Windows, `started` and a `0` exit code only confirm that the update process started, not that the update succeeded. Check the log file for the result.

## Update notices

When a newer version of the CLI is available, some interactive commands show a notice after they complete successfully:

```console
A newer version of the MAUI CLI is available (0.1.0-preview.14.26555.2 → 0.1.0-preview.15.26600.1). Run 'maui update --self' to update.
```

The CLI checks for a newer version at most once per UTC day. The first eligible command each day checks the versions listed on NuGet.org, with a two-second timeout. Later commands that day use a small cache in your user profile, and failed checks are cached too. The check runs in the CLI process, without a background service. Stable CLI installations are only notified about stable versions.

Notices appear only for the default global tool installation, and only for successful interactive runs of the `doctor`, `device`, `android`, `apple`, `project`, `version`, `port`, and `profile` commands. Notices are suppressed in the following cases:

- When you use `--json`, `--ci`, or `--dry-run`.
- When input or output is redirected.
- When a CI environment variable such as `CI`, `TF_BUILD`, or `GITHUB_ACTIONS` is set.

To turn off update checks and notices, set the `MAUI_CLI_NO_UPDATE_CHECK` environment variable to `1`:

```bash
export MAUI_CLI_NO_UPDATE_CHECK=1
```

Update notices use public package information from NuGet.org. They don't affect which feeds `maui update --self` uses to install the update.

## See also

- [.NET MAUI CLI overview](index.md)
