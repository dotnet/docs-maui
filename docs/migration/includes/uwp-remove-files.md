---
ms.topic: include
ms.date: 12/21/2023
---

The following files, which are present in Xamarin.Forms UWP projects, don't exist in WinUI 3 projects:

- *MainPage.xaml* and *MainPage.xaml.cs*
- *AssemblyInfo.cs*
- *Default.rd.xml*

Therefore, you should remove these files if you've copied them from your UWP project to your WinUI 3 project. Any required business logic contained in these files should be moved elsewhere.
