# Copilot working instructions: .NET 10 docs staging (docs-maui)

This document captures our working agreements and checklists for staging .NET 10 updates in this repo.

## Strategy overview
- We develop each docs update on a short-lived topic branch based off `main`.
- Open individual PRs targeting `main` (no long-lived net10 tracking PR).
- Keep changes small and focused by topic.

## Branching model
- Base: `main`
- Topic branches: `pr<nn>-<slug>` or `<area>-<slug>`
  - Examples:
    - `pr01-pop-ups-async-from-main`
    - `pr02-mediapicker-multiselect`
    - `gestures-tap-click-deprecation`
- Keep branches up-to-date by rebasing on `main` when needed.

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

## Accuracy and preview drift
- Treat the upstream `dotnet/maui` `net10.0` branch as the source of truth. Preview notes may evolve or be reverted in later previews.
- Before merging any .NET 10 docs change, verify the target API/behavior against the current `net10.0` branch:
  - Confirm APIs exist (and names/overloads match) using xrefs targeting `view=net-maui-10.0` and/or MAUI source.
  - Check whether a previously announced change was modified or reverted; adjust guidance and monikers accordingly.
  - Include links to the upstream MAUI PR(s) or commit(s) that introduced/changed the behavior in the docs PR description.
- If behavior differs across previews, keep guidance in `<= net-maui-9.0` vs `>= net-maui-10.0` monikers accurate for GA; avoid documenting transient preview-only behavior unless explicitly called out.

## Authoring conventions
- Use xref with wildcards for API overloads (for example, `xref:Microsoft.Maui.Controls.Page.DisplayAlertAsync%2A`).
- Prefer small, focused includes per version (e.g., `includes/pop-ups-dotnet9.md`, `includes/pop-ups-dotnet10.md`).
- Keep headings, note/warning blocks, and image alt texts aligned across versions.
- Commit messages: “Area (.NET 10): short summary; specifics”.

## Known .NET 10 changes already staged / proposed
- Pop-ups (PR01):
  - API naming in .NET 10:
    - `DisplayAlertAsync` (Task / Task<bool>)
    - `DisplayActionSheetAsync` (Task<string>)
    - `DisplayPromptAsync` (Task<string>) unchanged
  - Docs updated in `docs/user-interface/includes/pop-ups-dotnet10.md` to use Async-suffixed APIs and `await` consistently.
- MediaPicker (PR02):
  - Multi-select: `PickPhotosAsync` / `PickVideosAsync` return `List<FileResult>`.
  - `MediaPickerOptions` includes `SelectionLimit`, `MaximumWidth`, `MaximumHeight`, `CompressionQuality`, `RotateImage`, `PreserveMetaData`, `Title`.
  - Platform notes: Android may not enforce `SelectionLimit`; Windows doesn’t support `SelectionLimit`.

## Opening PRs
- Open PRs directly against `main`.
- PR template contents:
  - Title: concise change summary and version (e.g., “Update pop-ups docs for .NET 10: async APIs and monikers”).
  - Summary bullets of user-impacting changes.
  - Docs touched (paths).
  - Checklist: monikers, samples, links, screenshots.
  - Notes on API changes (source links to MAUI PRs or release notes when relevant).

## Maintenance
- Keep PRs small and focused; rebase/merge frequently to avoid conflicts.
- When conflicts arise in monikered pages, prioritize keeping both versioned includes accurate before deduplicating.

---

Last updated: 2025-08-18