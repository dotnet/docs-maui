---
title: "Performance Profiling"
description: "Learn how to profile the performance of your .NET MAUI app."
ms.date: 11/11/2024
---

# Performance Profiling

Performance profiling is the process of measuring the performance of
an application to identify areas for improvement. .NET MAUI and client
applications, in general, are interested in:

- **Startup time**: The time it takes for the application to start and
  display the first screen.
- **CPU usage**: If specific methods are consuming too much CPU time:
  either through many calls or long-running operations.
- **Memory usage**: If many allocations are made beyond reason or if
  there are memory leaks.
- **Scrolling responsiveness**: If your application scrolls smoothly
  and responds to user input without lag.

The techniques and tools for improving these metrics are different,
which we plan to demystify in this guide. The tools used to profile
.NET MAUI applications can also vary depending on the platform. This
guide will focus heavily on Android and Apple platforms, while
profiling on Windows is most easily done using the [tools in Visual
Studio][prof-overview].

[prof-overview]: /visualstudio/profiling/profiling-feature-tour?pivots=programming-language-dotnet

## Tools for Performance Profiling

