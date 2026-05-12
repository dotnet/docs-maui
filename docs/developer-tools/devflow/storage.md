---
title: "File storage access"
description: "Learn how to inspect and manage sandboxed app files on device or simulator using DevFlow CLI storage commands."
ms.date: 05/08/2026
---

# File storage access

DevFlow provides commands to inspect and manage sandboxed app files on device or simulator directly from the CLI. The `maui devflow storage` command group supports app preferences, secure storage, and file storage access.

> [!IMPORTANT]
> DevFlow is experimental and will change between releases.

## Storage roots

Apps can advertise one or more file storage roots. The default root id is `appData`, which maps to `FileSystem.AppDataDirectory`.

Use `maui devflow storage roots` to list the file storage roots advertised by the connected app:

```bash
maui devflow storage roots
maui devflow storage roots --json
```

Use the `id` value from this output when passing `--root` to `files` subcommands. Call this command before specifying a non-default root.

## File storage commands

The `maui devflow storage files` subcommand group lets you list, download, upload, and delete files within an advertised app storage root. All file subcommands accept an optional `--root <id>` option (defaults to `appData`).

### List files

List files and directories under a storage root:

```bash
# List root of the default appData storage root
maui devflow storage files list

# List a subdirectory
maui devflow storage files list logs/

# Use a non-default root
maui devflow storage files list --root cache
```

### Download a file

Download a file from the app's storage. Without `--output`, prints the JSON response containing base64-encoded content. With `--output`, writes the decoded file locally:

```bash
# Print base64 JSON response
maui devflow storage files download logs/app.log

# Download to a directory (preserves the device file name)
maui devflow storage files download logs/app.log --output ./downloads/

# Download to an explicit local file path
maui devflow storage files download logs/app.log --output ./downloads/app-copy.log
```

| Option | Description |
|--------|-------------|
| `--output`, `-o` | Local file or directory path to write the downloaded file to. Relative paths are resolved from the current directory. |
| `--root` | Storage root id (default: `appData`) |

### Upload a file

Upload a file to the app's storage. Parent directories are created automatically:

```bash
# Upload a local file
maui devflow storage files upload config/settings.json --file ./settings.json

# Upload base64 content directly
maui devflow storage files upload config/settings.json <base64-string>
```

| Option/Argument | Description |
|----------------|-------------|
| `path` | Relative file path under the selected storage root |
| `contentBase64` | Base64-encoded file content (omit when using `--file`) |
| `--file`, `-f` | Local file to upload. Relative paths are resolved from the current directory. |
| `--root` | Storage root id (default: `appData`) |

### Delete a file

Delete a file from the app's storage:

```bash
maui devflow storage files delete logs/old.log

# Delete from a non-default root
maui devflow storage files delete cache/data.db --root cache
```

## See also

- [MCP server for AI agents](mcp-server.md)
- [DevFlow overview](index.md)
