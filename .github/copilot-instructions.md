# Working notes for .NET MAUI .NET 10 docs updates

These notes capture our lightweight process and guardrails while submitting focused pull requests against `main` for .NET 10 changes.

Last updated: 2025-08-18

## Branching and PRs

- Create small, topic-focused branches directly off `main` (for example: `pr01-pop-ups-async-from-main`, `pr02-mediapicker-multiselect-from-main`, `pr03-gestures-tap-click-deprecation`).
- Open PRs directly to `main` with clear scope and migration context. Avoid staging branches.
- Keep changes minimal: only the pages/includes required for the topic.

## Monikers and versioning

- Preserve existing content for <= .NET 9 and add >= .NET 10 content using DocFX monikers:

  ```md
  ::: moniker range="<=net-maui-9.0"
  ... existing <= 9 content ...
  ::: moniker-end

  ::: moniker range=">=net-maui-10.0"
  ... new .NET 10 content ...
  ::: moniker-end
  ```

- Use includes when appropriate to keep duplication low, but don’t over-abstract if a single page change is small and clear.

## Preview drift and verification

Changes must be verified against the .NET MAUI source for .NET 10 to avoid preview drift:

1. Check APIs on the `dotnet/maui` `net10.0` branch.
   - Repository: https://github.com/dotnet/maui
   - Browse `src/Controls/src/Core` and handlers/platform folders as needed.
2. Cross-check against API docs/xrefs where available.
3. Confirm platform notes (Android/iOS/Mac Catalyst/Windows) reflect real behavior.

Examples already verified:

- Pop-ups (.NET 10): `DisplayAlertAsync`, `DisplayActionSheetAsync` replace non-`Async` APIs.
- Media picker (.NET 10): `PickPhotosAsync` / `PickVideosAsync` returning `List<FileResult>`; options like `SelectionLimit`, `MaximumWidth/Height`, `CompressionQuality`, etc.

## Quality gates before submitting a PR

- Lint the markdown visually in the diff for:
  - Valid xrefs and relative links.
  - Correct admonitions and moniker blocks are balanced.
  - Code fences have a language hint and compile logically.
- Keep `ms.date` current on pages you materially change.
- Make sure headings form a sensible outline and anchors aren’t unintentionally renamed.

## PR description checklist

- Summarize the .NET 10 change and motivation.
- Call out migration guidance and any breaking changes.
- List files changed and a quick test of samples where relevant.
- Note platform-specific behaviors/limitations.
- Link to upstream MAUI PRs or source lines used for verification when helpful.

## Known .NET 10 changes we’ve documented

- Pop-ups: `DisplayAlertAsync` / `DisplayActionSheetAsync` (PR01).
- Media picker: multi-select `PickPhotosAsync` / `PickVideosAsync` (PR02).
- Gestures: deprecate `ClickGestureRecognizer`; promote `TapGestureRecognizer` and `PointerGestureRecognizer` (PR03).

## Scope and note policy for conceptual docs

To keep conceptual docs focused and avoid churn from low-impact API surface tweaks:

- Don’t add callouts/notes in conceptual topics for minor API shape changes such as:
  - Method/property visibility changes (for example, private → public, internal → public).
  - Binding mode default changes (for example, TwoWay → OneWay) where usage doesn’t materially change.
  - Handler default value changes that don’t alter how you use the API.
- Instead, update any code samples, snippets, or embedded guidance to reflect .NET 10 behavior and build cleanly.
- Leave full surface/shape details to the API reference. Only add migration notes when developer behavior or recommended usage changes in a meaningful way.
- When in doubt, prefer: “update samples quietly” over “add a prominent breaking note.”
