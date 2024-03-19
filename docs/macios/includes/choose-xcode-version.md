---
ms.topic: include
ms.date: 03/19/2024
---

## Build with a specific version of Xcode

If you have multiple versions of Xcode installed on your Mac, it's possible to specify which Xcode version should be used when building your app. There are a number of approaches that can be used to accomplish this, but the recommended approach is to set the `MD_APPLE_SDK_ROOT` environment variable to the path of the Xcode version.

> [!WARNING]
> Using `xcode-select -s` to set the version of Xcode to use isn't recommended.

To set the `MD_APPLE_SDK_ROOT` environment variable for the duration of the current terminal session:

1. Open the **Terminal** application.
1. Type the following command, substituting in your version of Xcode, and press Enter:

    ```zsh
    export MD_APPLE_SDK_ROOT=/Applications/Xcode_14.1.0.app
    ```

If you want to set this environment variable permanently, you can add the `export` command to your shell profile, such as *.zprofile*.
