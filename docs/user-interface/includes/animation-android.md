---
ms.topic: include
ms.date: 11/07/2023
---

::: moniker range=">=net-maui-8.0"

On Android, animations respect the system animation settings:

- If the system's animations are disabled (either by accessibility features or developer features), new animations will jump immediately to their finished state.
- If the device's power saving mode is activated while animations are in progress, the animations will immediately jump to their finished state.
- If the device's animation durations is set to zero (disabled) while animations are in progress and the API version is 33 or greater, the animations will immediately jump to their finished state.

::: moniker-end
