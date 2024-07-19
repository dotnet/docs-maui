---
title: "System special folders on Mac Catalyst"
description: "Learn about how the .NET Environment.GetFolderPath API behavior differs between macOS and Mac Catalyst."
ms.date: 07/19/2024
---

# System special folders on Mac Catalyst

The <xref:System.Environment.GetFolderPath%2A> API returns the path to the system special folder that's identified by the <xref:System.Environment.SpecialFolder> enumeration. However, its behavior is different between macOS and Mac Catalyst. The following table provides a comparison of its behavior between macOS and Mac Catalyst for all supported <xref:System.Environment.SpecialFolder> enumeration values:

| Environment.SpecialFolder Enum value | Returned path on macOS            | Returned path on Mac Catalyst |
|--------------------------------------|-----------------------------------|-------------------------------|
| `AdminTools`                         |                                   |                               |
| `ApplicationData`                    | $HOME/Library/Application Support | $HOME/Documents/.config       |
| `CDBurning`                          |                                   |                               |
| `CommonAdminTools`                   |                                   |                               |
| `CommonApplicationData`              | /usr/share                        | /usr/share                    |
| `CommonDesktopDirectory`             |                                   |                               |
| `CommonDocuments`                    |                                   |                               |
| `CommonMusic`                        |                                   |                               |
| `CommonOemLinks`                     |                                   |                               |
| `CommonPictures`                     |                                   |                               |
| `CommonProgramFiles`                 |                                   |                               |
| `CommonProgramFilesX86`              |                                   |                               |
| `CommonPrograms`                     |                                   |                               |
| `CommonStartMenu`                    |                                   |                               |
| `CommonStartup`                      |                                   |                               |
| `CommonTemplates`                    | /usr/share/templates              | /usr/share/templates          |
| `CommonVideos`                       |                                   |                               |
| `Cookies`                            |                                   |                               |
| `Desktop`                            | $HOME/Desktop                     | $HOME/Documents/Desktop       |
| `DesktopDirectory`                   | $HOME/Desktop                     | $HOME/Documents/Desktop       |
| `Favorites`                          | $HOME/Library/Favorites           | $HOME/Library/Favorites       |
| `Fonts`                              | $HOME/Library/Fonts               | $HOME/Documents/.fonts        |
| `History`                            |                                   |                               |
| `InternetCache`                      | $HOME/Library/Caches              | $HOME/Library/Caches          |
| `LocalApplicationData`               | $HOME/Library/Application Support | $HOME/Documents               |
| `LocalizedResources`                 |                                   |                               |
| `MyComputer`                         |                                   |                               |
| `MyDocuments`                        | $HOME/Documents                   | $HOME/Documents               |
| `MyMusic`                            | $HOME/Music                       | $HOME/Documents/Music         |
| `MyPictures`                         | $HOME/Pictures                    | $HOME/Documents/Pictures      |
| `MyVideos`                           | $HOME/Movies                      | $HOME/Documents/Videos        |
| `NetworkShortcuts`                   |                                   |                               |
| `Personal`                           | $HOME/Documents                   | $HOME/Documents               |
| `PrinterShortcuts`                   |                                   |                               |
| `ProgramFiles`                       | /Applications                     | $HOME/Applications            |
| `ProgramFilesX86`                    |                                   |                               |
| `Programs`                           |                                   |                               |
| `Recent`                             |                                   |                               |
| `Resources`                          |                                   | $HOME/Library                 |
| `SendTo`                             |                                   |                               |
| `StartMenu`                          |                                   |                               |
| `Startup`                            |                                   |                               |
| `System`                             | /System                           |                               |
| `SystemX86`                          |                                   |                               |
| `Templates`                          | $HOME/Templates                   | $HOME/Documents/Templates     |
| `UserProfile`                        | $HOME                             | $HOME                         |
| `Windows`                            |                                   |                               |

The returned system paths for `ApplicationData`, `Desktop`, `DesktopDirectory`, `Fonts`, `LocalApplicationData`, `MyMusic`, `MyPictures`, `MyVideos`, `ProgramFiles`, `System`, `Templates` differ beween macOS and Mac Catalyst. This is because by design Mac Catalyst matches the behavior of iOS.

> [!IMPORTANT]
> The `$HOME` path differs between apps which have the App Sandbox capability enabled or disabled. In a sandboxed environment `$HOME` resolves to `/Users/<username>/Library/Containers/<bundle-id>/Data`, while in a non-sandboxed environment it resolves to `/Users/<username>`. For more information about capabilities, see [Mac Catalyst capabilities](~/mac-catalyst/capabilities.md).

## Match macOS behavior on Mac Catalyst

If you need to match macOS app behavior and use the same system paths on Mac Catalyst, the recommended way of obtaining such paths is shown below.

### `Environment.SpecialFolder.ApplicationData`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, Environment.SpecialFolderOption.None)`, use `new NSFileManager().GetUrls(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.User)[0].Path`.

### `Environment.SpecialFolder.Desktop`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.Desktop, Environment.SpecialFolderOption.None)`, use `new NSFileManager().GetUrls(NSSearchPathDirectory.DesktopDirectory, NSSearchPathDomain.User)[0].Path`.

### `Environment.SpecialFolder.DesktopDirectory`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory, Environment.SpecialFolderOption.None)`, use `new NSFileManager().GetUrls(NSSearchPathDirectory.DesktopDirectory, NSSearchPathDomain.User)[0].Path`.

### `Environment.SpecialFolder.Fonts`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.Fonts, Environment.SpecialFolderOption.None)`, use `Path.Combine(new NSFileManager().GetUrls(NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User)[0].Path, "Fonts")`.

### `Environment.SpecialFolder.LocalApplicationData`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.None)`, use `new NSFileManager().GetUrls(NSSearchPathDirectory.ApplicationSupportDirectory, NSSearchPathDomain.User)[0].Path`.

### `Environment.SpecialFolder.MyMusic`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.MyMusic, Environment.SpecialFolderOption.None)`, use `new NSFileManager().GetUrls(NSSearchPathDirectory.MusicDirectory, NSSearchPathDomain.User)[0].Path`.

### `Environment.SpecialFolder.MyPictures`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.MyPictures, Environment.SpecialFolderOption.None)`, use `new NSFileManager().GetUrls(NSSearchPathDirectory.PicturesDirectory, NSSearchPathDomain.User)[0].Path`.

### `Environment.SpecialFolder.MyVideos`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.MyVideos, Environment.SpecialFolderOption.None)`, use `new NSFileManager().GetUrls(NSSearchPathDirectory.MoviesDirectory, NSSearchPathDomain.User)[0].Path`.

### `Environment.SpecialFolder.ProgramFiles`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles, Environment.SpecialFolderOption.None)`, use `"/Applications"`.

### `Environment.SpecialFolder.System`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.System, Environment.SpecialFolderOption.None)`, use `"/System"`.

### `Environment.SpecialFolder.Templates`

Instead of `Environment.GetFolderPath(Environment.SpecialFolder.Templates, Environment.SpecialFolderOption.None)`, use `Path.Combine(NSFileManager.HomeDirectory, "Templates")`.
