---
title: "AI requirements and prerequisites"
description: "Learn about minimum OS versions, device requirements, and prerequisites for using Microsoft.Maui.Essentials.AI on iOS, macOS, Android, and Windows."
ms.date: 02/02/2026
---

# AI requirements and prerequisites

This article describes the minimum requirements for using Microsoft.Maui.Essentials.AI on each platform.

## Quick reference

| Platform | Chat Client | Embeddings |
|----------|-------------|------------|
| iOS | 26.0+ | 13.0+ |
| macOS | 26.0+ | 10.15+ |
| Mac Catalyst | 26.0+ | 13.1+ |
| tvOS | 26.0+ | 13.0+ |
| Android | API 24+ (with ML Kit GenAI) | — |
| Windows | 10.0.26100.0+ (Windows 11 24H2) | 10.0.26100.0+ |

## Apple platforms

### AppleIntelligenceChatClient requirements

The `AppleIntelligenceChatClient` uses Apple's Foundation Models framework, which requires Apple Intelligence availability.

#### Minimum OS versions

| Platform | Minimum version | Notes |
|----------|-----------------|-------|
| iOS | 26.0 | iPhone with Apple Intelligence support |
| macOS | 26.0 | Mac with Apple Silicon |
| Mac Catalyst | 26.0 | Mac with Apple Silicon |
| tvOS | 26.0 | Apple TV with A-series chip |

#### Device requirements

Apple Intelligence requires specific hardware capabilities:

**iPhone**:
- iPhone 15 Pro, iPhone 15 Pro Max, or later
- iPhone 16 series or later
- Minimum 8GB RAM

**iPad**:
- iPad Pro (M1 or later)
- iPad Air (M1 or later)
- Minimum 8GB RAM

**Mac**:
- Any Mac with Apple Silicon (M1, M2, M3, M4 series)
- macOS 26.0 or later

**Apple TV**:
- Apple TV 4K (3rd generation) or later with tvOS 26.0

> [!NOTE]
> Apple Intelligence must be enabled in device settings. Users may need to join a waitlist depending on region and language availability.

#### Model availability

On-device models are downloaded automatically when Apple Intelligence is enabled. No manual model management is required.

For current Apple Intelligence availability and supported languages, see [Apple Intelligence availability](https://support.apple.com/en-us/120898).

### NLEmbeddingGenerator requirements

The `NLEmbeddingGenerator` uses Apple's Natural Language framework, which has been available since iOS 13.

#### Minimum OS versions

| Platform | Minimum version | Notes |
|----------|-----------------|-------|
| iOS | 13.0 | All devices with iOS 13+ |
| macOS | 10.15 (Catalina) | All Macs with macOS 10.15+ |
| Mac Catalyst | 13.1 | All Macs with macOS 10.15+ |
| tvOS | 13.0 | All Apple TV devices with tvOS 13+ |

#### Device requirements

The Natural Language framework works on all devices meeting the minimum OS requirements. No special hardware is required.

#### Language model availability

Embedding models for different languages may need to be downloaded on first use. The system handles this automatically, but:

- Initial embedding generation may be slower while models download
- Adequate storage space is required for language models
- Network connectivity is needed for initial model download

For supported languages, see [NLLanguage](https://developer.apple.com/documentation/naturallanguage/nllanguage).

---

## Android

Android AI capabilities use Google's ML Kit GenAI APIs with Gemini Nano for on-device inference.

### GeminiNanoChatClient requirements

The `GeminiNanoChatClient` uses ML Kit GenAI, which requires specific device capabilities.

#### Minimum requirements

| Requirement | Value |
|-------------|------------------|
| Minimum Android API | API 24 (Android 7.0) |
| Target Android API | API 35 recommended |
| Google Play Services | Required |
| Architecture | ARM64 or x86_64 |

#### Device requirements

ML Kit GenAI (Gemini Nano) requires devices with:
- Sufficient NPU or GPU capabilities for on-device inference
- Google Play Services installed and updated
- Adequate storage for on-device models (~1-2GB)
- Minimum 4GB RAM (6GB+ recommended)

**Supported devices** include:
- Google Pixel 8, Pixel 8 Pro, and later
- Samsung Galaxy S24 series and later
- Other flagship devices with Gemini Nano support

> [!NOTE]
> The list of supported devices is expanding. Check [ML Kit GenAI documentation](https://developers.google.com/ml-kit/genai) for the latest compatibility list.

#### Model availability

Models are downloaded automatically through Google Play Services. Initial download requires:
- Active internet connection
- Adequate storage space
- Automatic updates via Play Services

For more information, see [ML Kit GenAI requirements](https://developers.google.com/ml-kit/genai).

---

## Windows

Windows AI capabilities use the Windows Copilot Runtime with Phi Silica for on-device inference.

### PhiSilicaChatClient and PhiSilicaEmbeddingGenerator requirements

Both implementations use `Microsoft.Windows.AI.Text.LanguageModel` from Windows Copilot Runtime.

#### Minimum requirements

| Requirement | Value |
|-------------|------------------|
| Windows version | Windows 10.0.26100.0 (Windows 11 24H2) |
| Architecture | x64, ARM64 |
| NPU | 40+ TOPS recommended |
| RAM | 16GB+ recommended |

#### Device requirements

Phi Silica performs best on **Copilot+ PCs** with NPU acceleration:

**Qualcomm**:
- Snapdragon X Elite
- Snapdragon X Plus

**Intel**:
- Core Ultra (Lunar Lake)
- Core Ultra (Arrow Lake)

**AMD**:
- Ryzen AI 300 series

> [!NOTE]
> Phi Silica can run on other Windows 11 24H2 devices without an NPU, but performance will be significantly slower as inference runs on CPU/GPU instead.

#### Model availability

The Phi Silica model is included with Windows 11 24H2 on Copilot+ PCs. On other devices:
- Model may need to be downloaded via Windows Update
- Requires adequate storage (~2-3GB)
- First-time initialization may be slow

For more information, see [Windows Copilot Runtime - Phi Silica](/windows/ai/apis/phi-silica).

---

## NuGet package requirements

### Package dependencies

```xml
<PackageReference Include="Microsoft.Maui.Essentials.AI" Version="x.x.x" />
```

This package depends on:
- `Microsoft.Extensions.AI.Abstractions` (for `IChatClient`, `IEmbeddingGenerator`)
- .NET 9.0 or later

### Target framework requirements

Your project must target the appropriate platforms:

```xml
<TargetFrameworks>net9.0-ios;net9.0-maccatalyst;net9.0-android;net9.0-windows10.0.19041.0</TargetFrameworks>
```

---

## Checking availability at runtime

### Check for Apple Intelligence availability

```csharp
#if IOS || MACCATALYST
// Check if Apple Intelligence is available on this device
if (OperatingSystem.IsIOSVersionAtLeast(26) || OperatingSystem.IsMacCatalystVersionAtLeast(26))
{
    var chatClient = new AppleIntelligenceChatClient();
}
#endif
```

### Check for NL embeddings availability

```csharp
#if IOS || MACCATALYST
// NL embeddings available on iOS 13+ / macOS 10.15+
if (OperatingSystem.IsIOSVersionAtLeast(13) || OperatingSystem.IsMacCatalystVersionAtLeast(13, 1))
{
    var embeddingGenerator = new NLEmbeddingGenerator();
}
#endif
```

### Check for Android AI availability

```csharp
#if ANDROID
// Check if ML Kit GenAI is available
try
{
    var chatClient = new GeminiNanoChatClient();
    // Model available
}
catch (Exception ex)
{
    // ML Kit GenAI not available on this device
    // Consider fallback to cloud AI
}
#endif
```

### Check for Windows AI availability

```csharp
#if WINDOWS
// Check OS version for Phi Silica
if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 26100))
{
    var chatClient = new PhiSilicaChatClient();
    var embeddingGenerator = new PhiSilicaEmbeddingGenerator();
}
#endif
```

### Feature detection pattern

```csharp
public interface IAIFeatureDetector
{
    bool IsChatAvailable { get; }
    bool IsEmbeddingsAvailable { get; }
}

public class AIFeatureDetector : IAIFeatureDetector
{
    public bool IsChatAvailable =>
#if IOS || MACCATALYST
        OperatingSystem.IsIOSVersionAtLeast(26) || 
        OperatingSystem.IsMacCatalystVersionAtLeast(26);
#elif ANDROID
        true; // Runtime check needed for device support
#elif WINDOWS
        OperatingSystem.IsWindowsVersionAtLeast(10, 0, 26100);
#else
        false;
#endif

    public bool IsEmbeddingsAvailable =>
#if IOS || MACCATALYST
        OperatingSystem.IsIOSVersionAtLeast(13) || 
        OperatingSystem.IsMacCatalystVersionAtLeast(13, 1);
#elif WINDOWS
        OperatingSystem.IsWindowsVersionAtLeast(10, 0, 26100);
#else
        false;
#endif
}
```

## Troubleshooting

### Apple Intelligence not available

If `AppleIntelligenceChatClient` throws an exception:

1. **Verify device compatibility**: Check that your device meets the hardware requirements (iPhone 15 Pro or later, M1 Mac or later)
2. **Check OS version**: Ensure iOS/macOS 26.0 or later is installed
3. **Enable Apple Intelligence**: Go to Settings > Apple Intelligence & Siri and enable Apple Intelligence
4. **Wait for model download**: On-device models may take time to download initially
5. **Check region/language**: Apple Intelligence may not be available in all regions

### NL embeddings not generating

If `NLEmbeddingGenerator` returns null or throws:

1. **Check language support**: Not all languages have embedding models available
2. **Wait for model download**: Language models download on first use
3. **Check storage space**: Ensure adequate storage for language models
4. **Verify connectivity**: Initial model download requires internet

### Android Gemini Nano not available

If `GeminiNanoChatClient` throws an exception:

1. **Check device support**: Gemini Nano requires specific device hardware (Pixel 8+, Galaxy S24+, etc.)
2. **Update Play Services**: Ensure Google Play Services is up to date
3. **Check storage**: Model requires ~1-2GB of storage
4. **Wait for download**: Initial model download happens automatically via Play Services
5. **Region availability**: ML Kit GenAI may not be available in all regions

### Windows Phi Silica not available

If `PhiSilicaChatClient` or `PhiSilicaEmbeddingGenerator` throws an exception:

1. **Check OS version**: Windows 11 24H2 (10.0.26100) or later is required
2. **Run Windows Update**: Phi Silica model may need to be downloaded
3. **Check device**: Best performance on Copilot+ PCs with NPU
4. **Verify resources**: Ensure adequate RAM (16GB+) and storage

## See also

- [Platform APIs](platform-apis.md)
- [Feature comparison](feature-comparison.md)
- [Getting started](getting-started.md)
- [Apple Intelligence availability](https://support.apple.com/en-us/120898)
- [ML Kit requirements](https://developers.google.com/ml-kit/migration/android)
- [Windows Copilot Runtime](/windows/ai/apis/)
