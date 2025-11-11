---
ms.topic: include
ms.date: 03/19/2024
---

## Build with a specific version of Xcode

If you have multiple versions of Xcode installed on your Mac, it's possible to specify which Xcode version should be used when building your app. There are a number of approaches that can be used to accomplish this, but there are two recommended approaches:

1. Use `sudo xcode-select --switch ...` to choose the system's currently selected version of Xcode (this requires admin credentials). It's also possible to do this from inside Xcode (Settings -> Locations -> Command Line Tools).

1. Use the `DEVELOPER_DIR` environment variable for the duration of the current terminal session:

    1. Open the **Terminal** application.
    1. Type the following command, substituting in your version of Xcode, and press Enter:

        ```zsh
        export DEVELOPER_DIR=/Applications/Xcode_14.1.0.app
        ```

    If you want to set this environment variable permanently, you can add the `export` command to your shell profile, such as *.zprofile*.

