---
ms.topic: include
ms.date: 11/22/2023
---

Rather than set individual files to the **AndroidResource** build action, the contents of a specific folder can be set to this build action by adding the following XML to your project (.csproj) file:

```xml
<ItemGroup Condition="$(TargetFramework.Contains('-android'))">
    <AndroidResource Include="Platforms\Android\Resources\**" TargetPath="%(RecursiveDir)%(Filename)%(Extension)" />
</ItemGroup>
```

This example sets any content in the *Platforms\Android\Resources* folder, including content in sub-folders, to the **AndroidResource** build action.
