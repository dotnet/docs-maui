---
ms.topic: include
ms.date: 03/27/2023
---

| Method | Purpose |
| ------ | ------- |
| `ContinueUserActivity` | Informs the app that there's data associated with continuing a task specified as a <xref:Foundation.NSUserActivity> object, and then returns whether the app continued the activity. |
| `GetCurrentUIViewController` | Gets the current view controller. This method will return `null` if unable to detect a <xref:UIKit.UIViewController>. |
| `OpenUrl` | Opens the specified URI to start an authentication flow. |
| `PerformActionForShortcutItem` | Invokes the action that corresponds to the chosen `AppAction` by the user. |

The following example shows how to retrieve the currently visible <xref:UIKit.UIViewController>:

```csharp
var viewController = Platform.GetCurrentUIViewController();
```
