# Copilot working instructions: .NET 10 docs staging (docs-maui)

This document captures our working agreements and checklists for staging .NET 10 updates in this repo.

## Strategy overview
- We maintain a long-lived staging branch: `net10`.
- Each discrete docs update is developed on a short-lived topic branch branched from `net10` (for example: `net10/pr01-pop-ups-async`, `net10/pr02-mediapicker-multiselect`).
- Topic PRs target `net10`. After approval, they’re merged into `net10`.
- As GA approaches, we open a single tracking PR from `net10` to `main` and merge when ready.

## Branching model
- Base: `main`
- Staging: `net10` (long-lived)
- Topic branches: `net10/<short-id>-<slug>`
  - Examples:
    - `net10/pr01-pop-ups-async`
    - `net10/pr02-mediapicker-multiselect`
    - `net10/pr03-gestures-tap-click-deprecation`
    - `net10/pr04-messagingcenter-migration`
    - `net10/pr05-ios-safe-area`
- Keep `net10` up-to-date by periodically merging `main` into `net10` (or rebasing topic branches on latest `net10`). Resolve conflicts conservatively; prefer keeping moniker structure intact.

## Moniker guidance
- For content that differs between releases, use DocFX monikers in the top-level article and split content into include partials when it improves clarity.
  - Typical pattern:
    - `::: moniker range="<=net-maui-9.0"` → include `*-dotnet9.md`
    - `::: moniker range=">=net-maui-10.0"` → include `*-dotnet10.md`
- If behavior/API names change in .NET 10 but guidance is otherwise similar, keep <=9 and >=10 content parallel and explicit.
- Keep phrasing and headings consistent across monikered sections to minimize diffs.

## Quality gates (for each topic PR)
- Monikers
  - Build must not emit moniker range warnings.
  - Verify xrefs resolve (API names must match the targeted version).
- Samples and snippets
  - Ensure code compiles in snippet harness where applicable.
  - For async APIs, consistently use `await` in examples and clarify return types.
- Links
  - Validate markdown links (no 404s) and image paths.
- Screenshots
  - Only update if UI/API presentation changed. Otherwise, reuse.
- Review metadata
  - Title/labels: “.NET 10”, “docs”, and area label.
  - Reviewers: CODEOWNERS will auto-assign; add relevant owners as needed.

## Authoring conventions
- Use xref with wildcards for API overloads (for example, `xref:Microsoft.Maui.Controls.Page.DisplayAlertAsync%2A`).
- Prefer small, focused includes per version (e.g., `includes/pop-ups-dotnet9.md`, `includes/pop-ups-dotnet10.md`).
- Keep headings, note/warning blocks, and image alt texts aligned across versions.
- Commit messages: “Area (.NET 10): short summary; specifics”

## Known .NET 10 changes already staged
- Pop-ups (PR01):
  - API naming in .NET 10:
    - `DisplayAlertAsync` (Task / Task<bool>)
    - `DisplayActionSheetAsync` (Task<string>)
    - `DisplayPromptAsync` (Task<string>) unchanged
  - Docs updated in `docs/user-interface/includes/pop-ups-dotnet10.md` to use Async-suffixed APIs and `await` consistently.
- <= .NET 9 content retains `DisplayAlert` and `DisplayActionSheet` while still showing proper `await` usage.

## In-flight / upcoming PRs
- PR02: MediaPicker multi-select and platform notes (.NET 10)
  - Target file: `docs/platform-integration/device-media/picker.md`
  - Plan: add a `>= net-maui-10.0` section documenting multi-select, examples iterating multiple results, and permissions per platform.
- PR03: Gestures tap/click deprecation
- PR04: MessagingCenter migration guidance
- PR05: iOS safe area updates for .NET 10

## Opening PRs
- Topic PRs → base: `net10`.
- Tracking PR (later) → base: `main`, compare: `net10`.
- PR template contents:
  - Title: concise change summary and version (e.g., “Update pop-ups docs for .NET 10: async APIs and monikers”).
  - Summary bullets of user-impacting changes.
  - Docs touched (paths).
  - Checklist: monikers, samples, links, screenshots.
  - Notes on API changes (source links to MAUI PRs or release notes when relevant).

## Maintenance
- Periodically merge `main` → `net10` to reduce final integration risk.
- When conflicts arise in monikered pages, prioritize keeping both versioned includes accurate before deduplicating.

---

Last updated: 2025-08-18
