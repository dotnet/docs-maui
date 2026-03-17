# .NET MAUI Docs

This repository contains the conceptual documentation for .NET Multi-platform App UI (MAUI). It's published at the [.NET MAUI documentation site](https://learn.microsoft.com/dotnet/maui).

## LLMS Files

This repo includes curated `llms.txt` files for AI-friendly documentation discovery.

- Use [docs/llms.txt](docs/llms.txt) for top-level routing across the MAUI docs set.
- Use subtopic `llms.txt` files when the question is scoped to one area such as fundamentals, user interface, platform integration, data, or deployment.
- Use nested `llms.txt` files for higher-signal retrieval in busy areas such as data binding, Shell, controls, handlers, and CollectionView.
- Use [docs/llms-full.txt](docs/llms-full.txt) only when a tool needs one broader context file and cannot follow subtopic links.
- Validate changes with `./docs/tools/verify-llms.ps1`.

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community. For more information, see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).
