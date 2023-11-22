---
ms.topic: include
ms.date: 11/22/2023
---

Rather than set individual files to the **BundleResource** build action, the contents of a specific folder can be set to this build action by adding the following XML to your project (.csproj) file:

```xml
<ItemGroup Condition="$(TargetFramework.Contains('-ios'))">
  <BundleResource Include="Platforms\iOS\Resources\**" TargetPath="%(RecursiveDir)%(Filename)%(Extension)" />
</ItemGroup>
```

This example sets any content in the *Platforms\iOS\Resources* folder, including content in sub-folders, to the **BundleResource** build action.
