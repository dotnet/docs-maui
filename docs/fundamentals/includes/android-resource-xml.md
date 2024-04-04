---
ms.topic: include
ms.date: 11/22/2023
---

> [!NOTE]
> Rather than set individual files to the **AndroidResource** build action, the contents of a specific folder can be set to this build action by adding the following XML to your app's project (.csproj) file:
>
>```xml
><ItemGroup Condition="$(TargetFramework.Contains('-android'))">
>   <AndroidResource Include="Platforms\Android\Resources\**" TargetPath="%(RecursiveDir)%(Filename)%(Extension)" />
></ItemGroup>
>```
>
>This example sets any content in the *Platforms\Android\Resources* folder, including content in sub-folders, to the **AndroidResource** build action. It also sets the output path for each file with this build action.
