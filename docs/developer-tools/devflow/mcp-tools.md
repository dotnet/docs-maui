---
title: "MCP tools reference"
description: "Reference for all MCP tools exposed by the DevFlow MCP server for AI agent integration with running .NET MAUI apps."
ms.date: 11/27/2026
ai-usage: ai-assisted
---

# MCP tools reference

The DevFlow MCP server exposes tools that AI agents can call to inspect, interact with, and debug running .NET MAUI apps.

> [!IMPORTANT]
> DevFlow is experimental and can change between releases. Tool and platform availability depends on the CLI, agent, app, and device. Your MCP client can discover the tools and their parameters from the running server.

## Select an agent

Most tools that target an app accept an optional `agentPort`. If no explicit port or session default is set, and project discovery and the `.mauidevflow` configuration don't resolve a target, a tool call fails when multiple agents are connected. The error lists the agents and their ports instead of choosing one.

Call `maui_list_agents` to find the port of the app you want to use. Then either pass that port as `agentPort` on each call or call `maui_select_agent` with `{"agentPort": 7000}` to set the default for the current MCP session. An explicit `agentPort` on a later call overrides the session default.

Without an explicit or session port, the broker first tries to resolve an agent for the current project or select the only connected agent. Only if that fails does the server use a `port` from `.mauidevflow`. `maui_wait` also sets the session default to the first agent it matches. You can filter by app name with its `app` parameter, but select a port explicitly when more than one agent might match. For CLI commands, use `--agent-port` to select a target explicitly.

## Tools

Call `maui_capabilities` to check the features supported by a connected agent.

### Agent tools

| Tool | Description |
|------|-------------|
| `maui_list_agents` | List connected agents and their ports. |
| `maui_select_agent` | Set the default agent port for the MCP session. |
| `maui_status` | Get the status of an agent. |
| `maui_wait` | Wait for an agent to connect and select it for the session. |
| `maui_capabilities` | Get the capabilities of an agent. |
| `maui_control_status` | Inspect the holder of the app's mutation lease. |
| `maui_take_control` | Claim the app's mutation lease. |
| `maui_release_control` | Release the session's mutation lease. |

Use `force=true` with `maui_take_control` only after the user approves interrupting the current lease holder.

### Tree tools

| Tool | Description |
|------|-------------|
| `maui_tree` | Inspect the visual tree and its element IDs. |
| `maui_hittest` | Find the element at screen coordinates. |
| `maui_layout_diagnostics` | Inspect the rendered UI for layout issues. |

### Query tools

| Tool | Description |
|------|-------------|
| `maui_element` | Get details for an element by visual tree ID. |
| `maui_query` | Find visual tree elements by type, automation ID, or text. |
| `maui_query_css` | Find Blazor WebView elements with a CSS selector. |

### Assert tools

| Tool | Description |
|------|-------------|
| `maui_assert` | Compare an element property with an expected value. |

### Property tools

| Tool | Description |
|------|-------------|
| `maui_get_property` | Read an element property. |
| `maui_set_property` | Set an element property. |

### Interaction tools

| Tool | Description |
|------|-------------|
| `maui_tap` | Tap an element or a point within it. |
| `maui_fill` | Replace text in an input element. |
| `maui_clear` | Clear text from an input element. |
| `maui_key` | Send a key press to an element. |
| `maui_focus` | Set focus to an element. |
| `maui_scroll` | Scroll a container or bring an element into view. |
| `maui_gesture` | Perform a gesture such as a swipe, pinch, or drag. |
| `maui_batch` | Execute a sequence of UI actions. |

### Navigation tools

| Tool | Description |
|------|-------------|
| `maui_navigate` | Navigate to a Shell route. |
| `maui_back` | Navigate back. |
| `maui_resize` | Resize an app window. |

### Screenshot tools

| Tool | Description |
|------|-------------|
| `maui_screenshot` | Capture the app or an element as a PNG image. |

### Native dialog tools

| Tool | Description |
|------|-------------|
| `maui_native_dialog_detect` | Detect a supported native dialog without changing host focus or sending host input. |
| `maui_native_dialog_respond` | Request confirmation and press a button on a detected dialog. |

### CDP tools

| Tool | Description |
|------|-------------|
| `maui_cdp_webviews` | List registered Blazor WebViews. |
| `maui_cdp_source` | Get the HTML source of a Blazor WebView. |
| `maui_cdp_evaluate` | Evaluate JavaScript in a Blazor WebView. |
| `maui_cdp_screenshot` | Capture a Blazor WebView screenshot. |

`maui_cdp_evaluate`, `maui_cdp_source`, and `maui_cdp_screenshot` accept a string `webviewId` to target a WebView by its index, `AutomationId`, or registered element ID. Call `maui_cdp_webviews` to list the available WebViews. If you omit `webviewId`, the default can change as pages and WebViews load, so specify a target when the app has more than one. A numeric `webviewId` matches an index before an `AutomationId`. If evaluation fails or the selected WebView returns no HTML source, the corresponding tool returns an error.

### Log tools

| Tool | Description |
|------|-------------|
| `maui_logs` | Retrieve app and WebView console logs. |

### Network tools

| Tool | Description |
|------|-------------|
| `maui_network` | List captured HTTP requests. |
| `maui_network_detail` | Get details of a captured request. |
| `maui_network_clear` | Clear captured requests. |

### Preferences tools

| Tool | Description |
|------|-------------|
| `maui_preferences_list` | List preference keys and values. |
| `maui_preferences_get` | Read a preference by key. |
| `maui_preferences_set` | Write a preference. |
| `maui_preferences_delete` | Delete a preference. |
| `maui_preferences_clear` | Clear preferences. |

### Storage tools

| Tool | Description |
|------|-------------|
| `maui_secure_storage_get` | Read an encrypted secure-storage value. |
| `maui_secure_storage_set` | Write an encrypted secure-storage value. |
| `maui_secure_storage_delete` | Delete a secure-storage value. |
| `maui_secure_storage_clear` | Clear secure storage. |
| `maui_storage_roots` | List the app's advertised file storage roots. |
| `maui_files_list` | List files and directories under a storage root. |
| `maui_files_download` | Download a file as base64. |
| `maui_files_upload` | Upload a base64-encoded file. |
| `maui_files_delete` | Delete a file. |

### Platform tools

| Tool | Description |
|------|-------------|
| `maui_app_info` | Get app and package information. |
| `maui_device_info` | Get device and platform information. |
| `maui_display_info` | Get display dimensions, density, and orientation. |
| `maui_battery_info` | Get battery and charging information. |
| `maui_connectivity` | Get network connectivity information. |
| `maui_geolocation` | Get the current GPS location. |
| `maui_get_theme` | Get the app theme. |
| `maui_set_theme` | Set the app or supported virtual device theme. |

### Sensor tools

| Tool | Description |
|------|-------------|
| `maui_sensors_list` | List sensors and their status. |
| `maui_sensors_start` | Start a sensor. |
| `maui_sensors_stop` | Stop a sensor. |

### Fold tools

| Tool | Description |
|------|-------------|
| `maui_fold_capabilities` | Discover fold support on an Android emulator or iOS simulator. |
| `maui_get_fold_state` | Read the fold state of an emulator or simulator. |
| `maui_set_fold_state` | Set the fold state of an emulator or simulator. |

The fold tools don't control physical devices. iOS Simulator fold support is experimental and requires `allowExperimentalIos=true`. Changing the fold state affects the whole virtual device.

### Background jobs tools

| Tool | Description |
|------|-------------|
| `maui_jobs_list` | List supported Android WorkManager jobs or Apple BGTaskScheduler requests. |
| `maui_jobs_run` | Request a supported iOS background task launch. |

### Recording tools

| Tool | Description |
|------|-------------|
| `maui_recording_start` | Start a screen recording. |
| `maui_recording_status` | Check screen-recording status. |
| `maui_recording_stop` | Stop the screen recording and save the video. |
| `maui_flow_list` | List recorded workflow files. |
| `maui_flow_validate` | Validate a workflow without running it. |
| `maui_flow_replay` | Replay a workflow against the running app. |
| `maui_flow_record_start` | Start or resume a workflow recording. |
| `maui_flow_record_status` | Check workflow-recording status. |
| `maui_flow_record_step` | Report the automatically recorded step count for older callers. |
| `maui_flow_record_stop` | Validate and save the recorded workflow. |
| `maui_flow_record_cancel` | Discard an in-progress workflow recording. |

Replaying a workflow drives and changes the running app. Replay only trusted files, or use `maui_flow_validate` to check a workflow without running it.

### Actions, extensions, and menus

| Tool | Description |
|------|-------------|
| `maui_list_actions` | List app-registered DevFlow actions. |
| `maui_invoke_action` | Invoke an app-registered action. |
| `maui_extension_list` | List registered agent extensions. |
| `maui_extension_call` | Call an extension tool. |
| `maui_menu_list` | Inspect app and supported native menus. |
| `maui_menu_invoke` | Invoke a menu item. |

## Tool details

The following sections describe behavior and limits for selected tools.

### Native dialogs and screenshots

`maui_native_dialog_detect` supports app-owned dialogs and some system permission dialogs on macOS AppKit and Mac Catalyst, Windows, Android, iOS Simulator, and Linux when safe platform automation is available. On Linux, detection is limited to the app's visual tree; external system prompts require manual handling. Detection doesn't change host focus or send host keyboard or pointer input. Physical iOS devices aren't supported for native dialog automation.

| Tool | Parameter | Required | Description |
|------|-----------|----------|-------------|
| `maui_native_dialog_detect` | `agentPort` | No | Agent HTTP port. |
| `maui_native_dialog_detect` | `androidDevice` | No | Android device serial to validate against the device mapped to the agent. |
| `maui_native_dialog_detect` | `simulatorUdid` | No | iOS Simulator UDID to validate against the simulator reported by the agent. |
| `maui_native_dialog_respond` | `promptId` | Yes | One-time token returned by detection. |
| `maui_native_dialog_respond` | `buttonLabel` | Yes | Exact visible button label selected by the user. |
| `maui_native_dialog_respond` | `agentPort` | No | Agent HTTP port for the same app. |

To respond to a dialog:

1. Call `maui_native_dialog_detect` and inspect `detected`, `canRespond`, `userActionRequired`, `title`, and `buttons`. If `userActionRequired` is true and `canRespond` is false, follow the returned instruction and ask the user to respond manually. This can happen even when `detected` is false.
1. Ask the user which exact button to press. If `canRespond` is true, call `maui_native_dialog_respond` with that label and the returned `promptId`.
1. The MCP host requests separate confirmation for that exact prompt and button. Check the response's `success` and `userActionRequired` fields before continuing.

The prompt token expires after two minutes and can be used once. A response attempt can consume it even if the attempt fails or confirmation is declined. If the dialog changes or the user wants to retry, detect the dialog again. A host without form-elicitation support can't confirm the action, so the user must respond manually.

The confirmation gate applies to the MCP tool. The CLI command `maui devflow ui alert dismiss` doesn't use this gate; obtain the user's approval for the exact button before using it.

On macOS, grant Accessibility access to the terminal or host running `maui`. Android detection requires `adb`. iOS Simulator detection uses the `apple` CLI; if it isn't available or fails, a dialog might not be reported. Android and iOS Simulator actions are scoped to the connected device; a supplied device identifier must match it. Some surfaces aren't safely automatable, including the iOS 18 and later contact-selection surface after the initial permission sheet.

`maui_screenshot` returns a PNG image to the MCP client. On macOS AppKit, full-window capture uses ScreenCaptureKit on macOS 14 or later when available to include native chrome without activating the app or changing keyboard focus. Native window capture might require **Screen & System Audio Recording** permission, but the agent doesn't trigger a permission prompt. Fallback captures can differ or omit layer-backed controls. On failure, the tool reports the agent's error message when available and any suggestions, such as restoring a hidden or minimized window before retrying.

The tool's optional arguments include `agentPort`, `window`, `elementId`, `selector` for a Blazor WebView, `maxWidth`, and `scale`. You can also pass `captureEpoch` and `registryGeneration` from `maui_tree` to require a matching snapshot.

### List preferences

`maui_preferences_list` accepts optional `agentPort` and `sharedName` arguments. It enumerates the native preference store where supported, including keys written directly by the app. Native enumeration is supported for macOS, iOS, tvOS, Mac Catalyst, Android, Windows, Linux, and WPF stores.

The response includes:

| Field | Description |
|-------|-------------|
| `keys` | Preference entries containing `key`, `value`, `type`, and `sharedName`. |
| `source` | `native` for the backing store, or `registry` for DevFlow-tracked keys. |
| `complete` | `true` when native enumeration succeeded; `false` for the registry fallback. |
| `sharedName` | The requested shared container name, or `null` for the default store. |
| `enumerationError` | An error string if native enumeration failed or is unavailable; otherwise, `null`. |

For example:

```json
{
  "keys": [
    { "key": "theme", "value": "dark", "type": "string", "sharedName": null }
  ],
  "source": "native",
  "complete": true,
  "sharedName": null,
  "enumerationError": null
}
```

Check `complete` before treating the list of keys as exhaustive. When it is `false`, keys written directly by the app might be missing. Completeness applies to keys, not value accuracy. Values and types are inferred and can be incorrect, including on Linux. If you know a value's type, use `maui_preferences_get` with the appropriate `type`.

### Batch actions and commands

`maui_batch` accepts an `actionsJson` string containing a JSON array of UI actions. It runs actions sequentially and stops at the first failure by default. Set `continueOnError` to `true` to continue after an action fails. Actions aren't transactional: earlier changes remain applied.

For example, the following MCP tool arguments fill an entry and then tap a button. Obtain the element IDs from `maui_tree`:

```json
{
  "actionsJson": "[{\"action\":\"fill\",\"elementId\":\"entry1\",\"text\":\"hello\"},{\"action\":\"tap\",\"elementId\":\"button1\"}]"
}
```

When actions run, the response includes an overall `success` and per-action `results`. Request validation or transport failures can return a plain-text error, and a rejected batch can omit `results`. Inspect the response before assuming actions ran.

Optional `captureEpoch` and `registryGeneration` bind the actions to a tree snapshot. The agent validates the referenced element IDs before running a capture-bound batch and can reject the entire batch if an ID is stale or unknown. After a batch, call `maui_tree` again before another capture-bound action.

The CLI command `maui devflow batch` is separate from the MCP tool. It reads UI or WebView command lines, such as `ui tap button1`, from standard input and emits a JSON Lines result for each command by default. It stops on the first failure unless you pass `--continue-on-error`. In either case, it exits with code `1` if any command fails, even when later commands succeed.

### maui_jobs_list

`maui_jobs_list` accepts an optional `agentPort` and reports background jobs where supported. The response includes `platform`, `supported`, and `jobs`. When available, it also includes `type`, `runSupported`, and an `error` or `reason` if the listing fails or isn't supported.

Android WorkManager entries include `identifier`, `tags`, `state`, `generation`, and `runAttemptCount`. Apple BGTaskScheduler entries include `identifier`, `type`, and `earliestBeginDate`. Android jobs can be listed but can't be run through `maui_jobs_run`.

### maui_jobs_run

`maui_jobs_run` accepts a required `identifier`, an optional iOS task `type` (`processing` or `refresh`), and an optional `agentPort`. It requests a launch on a physical iOS device with a Debug-built agent library. Release-built NuGet agents can't trigger the task. A successful response acknowledges the launch request, not execution or completion; verify the task handler's outcome separately.

## See also

- [MCP server for AI agents](mcp-server.md)
- [DevFlow overview](index.md)
- [Agent HTTP API reference](agent-api.md)
- [AgentClient API reference](agent-client.md)
