---
ms.topic: include
ms.date: 03/23/2023
---

This is accomplished by adding the `ITSAppUsesNonExemptEncryption` key to your app's *Info.plist* with a `boolean` value that indicates whether your app uses encryption:

```xml
<key>ITSAppUsesNonExemptEncryption</key>
<false/>
```

For more information, see [Complying with Encryption Export Regulations](https://developer.apple.com/documentation/security/complying_with_encryption_export_regulations) on developer.apple.com.
