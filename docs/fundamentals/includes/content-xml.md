---
ms.topic: include
ms.date: 11/22/2023
---

Rather than set individual files to the **Content** build action, the contents of a specific folder can be set to this build action by adding the following XML to your project (.csproj) file:

```xml
<ItemGroup Condition="$(TargetFramework.Contains('-windows'))">
  <Content Include="Platforms\Windows\Assets\Images\**" TargetPath="%(RecursiveDir)%(Filename)%(Extension)" />
</ItemGroup>
```

This example sets any content in the *Platforms\Windows\Assets\Images* folder, including content in sub-folders, to the **Content** build action.
