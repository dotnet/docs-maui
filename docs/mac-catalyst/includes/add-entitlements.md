---
ms.topic: include
ms.date: 03/23/2023
---

This can be accomplished by adding an *Entitlements.plist* file to the *Platforms/MacCatalyst* folder of your .NET MAUI app project:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
  <dict>
    <key>com.apple.security.app-sandbox</key>
    <true/>
  </dict>
</plist>
```

The App Sandbox entitlement is defined using the `com.apple.security.app-sandbox` key, of type `boolean`. For information about App Sandbox, see [Protecting user data with App Sandbox](https://developer.apple.com/documentation/security/app_sandbox/protecting_user_data_with_app_sandbox) on developer.apple.com. For information about the App Sandbox entitlement, see [App Sandbox Entitlement](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_security_app-sandbox).

If your app opens outgoing network connections, you'll also need to add the `com.apple.security.network.client` key, of type `boolean`, to your *Entitlements.plist* file:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
  <dict>
    <key>com.apple.security.app-sandbox</key>
    <true/>
    <key>com.apple.security.network.client</key>
    <true/>
  </dict>
</plist>
```

For information about the outgoing network connections entitlement, see [com.apple.security.network.client](https://developer.apple.com/documentation/bundleresources/entitlements/com_apple_security_network_client) on developer.apple.com.
