---
title: ".NET MAUI semantics for accessibility"
description: "Learn how to use the SemanticProperties class in a .NET MAUI app, so that a screen reader can speak about the user interface elements on a page."
ms.date: 10/11/2021
---

# Semantics

<!-- Sample link goes here -->

Semantics is concerned with building experiences that make your apps inclusive of people who use technology in a wide range of environments and approach your UI with a range of needs and experiences. In many situations, legal requirements for accessibility may provide an impetus for developers to address accessibility issues. Regardless, it's advisable to build inclusive and accessible apps so that your apps reach the largest possible audience.

The [Web Content Accessibility Guidelines (WCAG)](https://www.w3.org/WAI/standards-guidelines/wcag/) are the global accessibility standard and legal benchmark for web and mobile. These guidelines describe the various ways in which apps can be made more perceivable, operable, understandable, and robust, for all.

Many user accessibility needs are met by assistive technology products installed by the user or by tools and settings provided by the operating system. This includes functionality such as screen readers, screen magnification, and high-contrast settings.

Screen readers typically provide auditory descriptions of controls that are displayed on the screen. These descriptions help users navigate through the app and provide references to controls, such as images, that have no input or text. Screen readers are often controlled through gestures on the touchscreen, trackpad, or keyboard. For information about enabling screen readers, see [Enable screen readers](#enabling-screen-readers).

Each operating system has its own screen reader with its own unique behavior and configuration. For example, most screen readers read the text associated with a control when it receives focus, enabling users to orient themselves as they navigate through the app. However, some screen readers can also read the entire app user interface when a page appears, which enables the user to receive all of the page's available informational content before attempting to navigate it.

Most screen readers will automatically read any text associated with a control that receives accessibility focus. This means that controls, such as `Label` or `Button`, that have a `Text` property set will be accessible for the user. However, `Image`, `ImageButton`, `ActivityIndicator`, and others might not be in the accessibility tree because no text is associated with them.

.NET Multi-platform App UI (.NET MAUI) supports two approaches to providing access to the accessibility experience of the underlying native platform. *Semantic properties* are the .NET MAUI approach to providing accessibility values in apps, and are the recommended approach. *Automation properties* are the Xamarin.Forms approach to providing accessibility values in apps, and have been superseded by semantic properties. In both cases, the default accessibility order of controls is the same order in which they're listed in XAML or added to the layout. However, different layouts might have additional factors that influence accessibility order. For example, the accessibility order of `StackLayout` is also based on its orientation, and the accessibility order of `Grid` is based on its row and column arrangement.

## Semantic properties

Semantic properties are used tp define information about which controls should receive accessibility focus and which text should be read aloud to the user. Semantic properties are attached properties that can be added to any element to set the underlying accessibility APIs on the native platforms.

> [!IMPORTANT]
> Semantic properties don't try to force equivalent behavior on each platform. Instead, they rely on the accessibility experience provided by each platform.

The `SemanticProperties` class defines the following attached properties:

- `Description`, of type `string`, which represents a description that will be read aloud by the screen reader. For more information, see [Description](#description).
- `Hint`, of type `string`, which is similar to `Description`, but provides additional context such as the purpose of a control. For more information, see [Hint](#hint).
- `HeadingLevel`, of type `SemanticHeadingLevel`, which enables an element to be marked as a heading to organize the UI and make it easier to navigate. For more information, see [Heading levels](#heading-levels).

These attached properties set native accessibility values so that a screen reader can speak about the element. <!-- For more information about attached properties, see [Attached properties]().-->

### Description

The `SemanticProperties.Description` attached property represents a short, descriptive `string` that a screen reader uses to announce an element. This property should be set for elements that have a meaning that's important for understanding the content or interacting with the user interface. Setting this property can be accomplished in XAML:

```xaml
<Image Source="dotnet_bot.png"
       SemanticProperties.Description="Image of dotnet bot." />
```

Alternatively, it can be set in C#:

```csharp
Image image = new Image();
SemanticProperties.SetDescription(image, "Image of dotnet bot.");
```

In addition, the `SetValue` method can also be used to set the `SemanticProperties.Description` attached property:

```csharp
image.SetValue(SemanticProperties.DescriptionProperty, "Image of dotnet bot.");
```

> [!WARNING]
> Don't set the `SemanticProperties.Description` attached property on a `Label`. This will stop the `Text` property being spoken by the screen reader. Instead, set the `Text` property, and the `SemanticProperties.Hint` attached property if required.

The accessibility information for an element can also be defined on another element. For example, a `Label` next to an `Entry` can be used to describe what the `Entry` represents. This can be accomplished in XAML as follows:

```xaml
<Label x:Name="label"
       Text="Enter your name: " />
<Entry SemanticProperties.Description="{Binding Source={x:Reference label} Path=Text}" />
```

Alternatively, it can be set in C# as follows:

```csharp
Label label = new Label
{
    Text = "Enter your name: "
};
Entry entry = new Entry();
SemanticProperties.SetDescription(entry, label.Text);
```

### Hint

The `SemanticProperties.Hint` attached property represents a `string` that provides additional context to the `SemanticProperties.Description` attached property, such as the purpose of a control. Setting this property can be accomplished in XAML:

```xaml
<ActivityIndicator IsRunning="true"
                   SemanticProperties.Description="Progress indicator."
                   SemanticProperties.Hint="Saving your data to the cloud." />
```

Alternatively, it can be set in C#:

```csharp
ActivityIndicator activityIndicator = new ActivityIndicator
{
    IsRunning = true
};
SemanticProperties.SetDescription(activityIndicator, "Progress indicator.");
SemanticProperties.SetHint(activityIndicator, "Saving your data to the cloud.");
```

In addition, the `SetValue` method can also be used to set the `SemanticProperties.Hint` attached property:

```csharp
activityIndicator.SetValue(SemanticProperties.HintProperty, "Saving your data to the cloud.");
```

On Android, this property behaves slightly differently depending on the control it's attached to. For example, for controls without text values, such as `Switch` and `CheckBox`, the controls will display the hint with the control. However, for controls with text values, the hint is not displayed and is read after the text value.

> [!WARNING]
> The `SemanticProperties.Hint` property conflicts with the `Entry.Placeholder` property on Android, which both map to the same native property. Therefore, setting a different `SemanticProperties.Hint` value to the `Entry.Placeholder` value isn't recommended.

<!-- ^ and Editor? -->

### Heading levels

The `SemanticProperties.HeadingLevel` attached property enables an element to be marked as a heading to organize the UI and make it easier to navigate. Some screen readers enable users to quickly jump between headings.

Headings have a level from 1 to 9, and are represented by the `SemanticHeadingLevel` enumeration, which defines `None`, and `Level1` through to `Level9` members.

> [!IMPORTANT]
> While Windows offers 9 levels of headings, Android and iOS only offer a single heading. Therefore, when `SemanticProperties.HeadingLevel` is set on Windows it maps to the correct heading level. However, when set on Android and iOS it maps to a single heading level.

The following example demonstrates setting this attached property:

```xaml
<StackLayout SemanticProperties.Description="Main page layout"
             SemanticProperties.HeadingLevel="Level1">
    ...
</StackLayout>
```

Alternatively, it can be set in C#:

```csharp
StackLayout stackLayout = new StackLayout();
SemanticProperties.SetDescription(stackLayout, "Main page layout.");
SemanticProperties.SetHeadingLevel(stackLayout, SemanticHeadingLevel.Level1);
```

In addition, the `SetValue` method can also be used to set the `SemanticProperties.HeadingLevel` attached property:

```csharp
stackLayout.SetValue(SemanticProperties.HeadingLevelProperty, SemanticHeadingLevel.Level1);
```

<!-- Todo: is this right?
### WebView    

The native WebView control on each platform is responsible for determining which elements of its content are accessible, and each WebView control should report its HTML content as accessibility elements.

Alternatively, if the content in your `WebView` is simple, you could provide a description of the content for the `WebView` with the `SemanticProperties.Description` and `SemanticProperties.Hint` attached properties.

-->

## Semantic focus

Controls have a `SetSemanticFocus` extension method, defined in the `Microsoft.Maui` namespace, which forces screen reader focus to a specified element. For example, given a `Label` named `label`, screen reader focus can be forced to the element with the following code:

```csharp
using Microsoft.Maui;
...

label.SetSemanticFocus();
```

<!-- Todo: Any scenarios for calling UpdateSemantics or UpdateSemanticNodeInfo? -->

## Semantic screen reader

.NET MAUI Essentials includes a `SemanticScreenReader` class that enables you to instruct a screen reader to announce specified text. <!-- For more information, see [SemanticScreenReader]().-->

<!--
### Semantic effects

Link to semantic effects (.NET MAUI Community Toolkit) which combines `IsInAccessibleTree` and `ExcludedWithChildren` support, if docs are ever created, + SemanticOrderView.
-->

## Automation properties

Automation properties are attached properties that can be added to any element to indicate how the element is reported to the underlying platform's accessibility framework.

The `AutomationProperties` class defines the following attached properties:

- `ExcludedWithChildren`, of type `bool?`, determines if an element and its children should be excluded from the accessibility tree. For more information, see [ExcludedWithChildren](#excludedwithchildren).
- `IsInAccessibleTree`, of type `bool?`, indicates whether the element is available in the accessibility tree. For more information, see [IsInAccessibleTree](#isinaccessibletree).
- `Name`, of type `string`, represents a short description of the element that serves as a speakable identifier for that element. For more information, see [Name](#name).
- `HelpText`, of type `string`, represents a longer description of the element, which can be thought of as tooltip text that's associated with the element. For more information, see [HelpText](#helptext).
- `LabeledBy`, of type `VisualElement`, which enables another element to define accessibility information for the current element. For more information, see [LabeledBy](#labeledby).

These attached properties set native accessibility values so that a screen reader can speak about the element. <!-- For more information about attached properties, see [Attached Properties](~/xamarin-forms/xaml/attached-properties.md). -->

> [!IMPORTANT]
> Using the `AutomationProperties` attached properties may impact UI Test execution on Android. The `AutomationId`, `AutomationProperties.Name` and `AutomationProperties.HelpText` properties will both set the native `ContentDescription` property, with the `AutomationProperties.Name` and `AutomationProperties.HelpText` property values taking precedence over the `AutomationId` value (if both `AutomationProperties.Name` and `AutomationProperties.HelpText` are set, the values will be concatenated). This means that any tests looking for `AutomationId` will fail if `AutomationProperties.Name` or `AutomationProperties.HelpText` are also set on the element. In this scenario, UI Tests should be altered to look for the value of `AutomationProperties.Name` or `AutomationProperties.HelpText`, or a concatenation of both.

Different screen readers read different accessibility values. For example, on Windows, Narrator will prioritize `AutomationProperties.Name`, `AutomationProperties.LabeledBy`, and then `AutomationProperties.HelpText`. On Android, TalkBack may combine the `AutomationProperties.Name` and `AutomationProperties.HelpText` values. Therefore, it's recommended that thorough accessibility testing is carried out on each platform to ensure an optimal experience.

### ExcludedWithChildren

The `AutomationProperties.ExcludeWithChildren` attached property, of type `bool?`, determines if an element and its children should be excluded from the accessibility tree. This enables scenarios such as displaying an `AbsoluteLayout` over another layout such as a `StackLayout`, with the `StackLayout` being excluded from the accessibility tree when it's not visible. It can be used from XAML as follows:

```xaml
<StackLayout AutomationProperties.ExcludedWithChildren="true">
...
</StackLayout>
```

Alternatively, it can be set in C# as follows:

```csharp
StackLayout stackLayout = new StackLayout();
...
AutomationProperties.SetExcludedWithChildren(stackLayout, true);
```

When this attached property is set, .NET MAUI sets the `AutomationProperties.IsInAccessibleTree` attached property to `false` on the specified element and its children.

### IsInAccessibleTree

> [!WARNING]
> This attached property should typically remain unset. The majority of controls should be present in the accessibility tree, and the `AutomationProperties.ExcludedWithChildren` attached property can be set in scenarios where an element and its children need removing from the accessibility tree.

The `AutomationProperties.IsInAccessibleTree` attached property, of type `bool?`, determines if the element is visible to screen readers. It must be set to `true` to use the other automation properties. This can be accomplished in XAML as follows:

```xaml
<Entry AutomationProperties.IsInAccessibleTree="true" />
```

Alternatively, it can be set in C# as follows:

```csharp
Entry entry = new Entry();
AutomationProperties.SetIsInAccessibleTree(entry, true);
```

### Name

> [!IMPORTANT]
> The `AutomationProperties.Name` attached property has been superseded by the `SemanticProperties.Description` attached property.

The `AutomationProperties.Name` attached property value should be a short, descriptive text string that a screen reader uses to announce an element. This property should be set for elements that have a meaning that is important for understanding the content or interacting with the user interface. This can be accomplished in XAML as follows:

```xaml
<ActivityIndicator AutomationProperties.IsInAccessibleTree="true"
                   AutomationProperties.Name="Progress indicator" />
```

Alternatively, it can be set in C# as follows:

```csharp
ActivityIndicator activityIndicator = new ActivityIndicator();
AutomationProperties.SetIsInAccessibleTree(activityIndicator, true);
AutomationProperties.SetName(activityIndicator, "Progress indicator");
```

### HelpText

> [!IMPORTANT]
> The `AutomationProperties.HelpText` attached property has been superseded by the `SemanticProperties.Hint` attached property.

The `AutomationProperties.HelpText` attached property should be set to text that describes the user interface element, and can be thought of as tooltip text associated with the element. This can be accomplished in XAML as follows:

```xaml
<Button Text="Toggle ActivityIndicator"
        AutomationProperties.IsInAccessibleTree="true"
        AutomationProperties.HelpText="Tap to toggle the activity indicator" />
```

Alternatively, it can be set in C# as follows:

```csharp
Button button = new Button { Text = "Toggle ActivityIndicator" };
AutomationProperties.SetIsInAccessibleTree(button, true);
AutomationProperties.SetHelpText(button, "Tap to toggle the activity indicator");
```

On some platforms, for edit controls such as an `Entry`, the `HelpText` property can sometimes be omitted and replaced with placeholder text. For example, "Enter your name here" is a good candidate for the `Entry.Placeholder` property that places the text in the control prior to the user's actual input.

### LabeledBy

> [!IMPORTANT]
> The `AutomationProperties.LabeledBy` attached property has been superseded by bindings. For more information, see [SemanticProperties: Description](#description).

The `AutomationProperties.LabeledBy` attached property allows another element to define accessibility information for the current element. For example, a `Label` next to an `Entry` can be used to describe what the `Entry` represents. This can be accomplished in XAML as follows:

```xaml
<Label x:Name="label" Text="Enter your name: " />
<Entry AutomationProperties.IsInAccessibleTree="true"
       AutomationProperties.LabeledBy="{x:Reference label}" />
```

Alternatively, it can be set in C# as follows:

```csharp
Label label = new Label { Text = "Enter your name: " };
Entry entry = new Entry();
AutomationProperties.SetIsInAccessibleTree(entry, true);
AutomationProperties.SetLabeledBy(entry, label);
```

> [!IMPORTANT]
> The `AutomationProperties.LabeledByProperty` is not supported on iOS.

## Testing accessibility

.NET MAUI apps typically target multiple platforms, which means testing the accessibility features according to the platform. Follow these links to learn how to test accessibility on each platform:

- [Test your app's accessibility](https://developer.android.com/guide/topics/ui/accessibility/testing) on Android.
- [Verifying app accessibility on iOS](https://developer.apple.com/library/archive/technotes/TestingAccessibilityOfiOSApps/TestAccessibilityiniOSSimulatorwithAccessibilityInspector/TestAccessibilityiniOSSimulatorwithAccessibilityInspector.html).
- [Testing for accessibility on OS X](https://developer.apple.com/library/archive/documentation/Accessibility/Conceptual/AccessibilityMacOSX/OSXAXTestingApps.html)
- [Accessibility testing](/windows/apps/design/accessibility/accessibility-testing) on Windows.

The following tools can assist with your accessibility testing:

- [Accessibility Insights](https://accessibilityinsights.io) for Android and Windows apps.
- [Accessibility Scanner](https://support.google.com/accessibility/android/answer/6376570) for Android apps.
- [Accessibility Inspector](https://developer.apple.com/library/archive/documentation/Accessibility/Conceptual/AccessibilityMacOSX/OSXAXTestingApps.html) for iOS and macOS apps.
- [Android Studio Layout Inspector](https://developer.android.com/studio/debug/layout-inspector) for Android apps.
- [Xcode View Debugger](https://developer.apple.com/library/archive/documentation/DeveloperTools/Conceptual/debugging_with_xcode/chapters/special_debugging_workflows.html#//apple_ref/doc/uid/TP40015022-CH9-SW2) for iOS and macOS apps.

However, none of these tools can perfectly emulate the screen reader user experience, and the best way to test and troubleshoot your apps for accessibility will always be manually with screen readers.

## Enabling screen readers

Each platform has a different default screen reader to narrate accessibility values:

- Android has TalkBack. For information on enabling TalkBack, see [Enable TalkBack](#enable-talkback).
- iOS and macOS have VoiceOver. For information on enabling VoiceOver, see [Enable Narrator](#enable-voiceover).
- Windows has Narrator. For information on enabling Narrator, see [Enable Narrator](#enable-narrator).

### Enable TalkBack

TalkBack is the primary screen reader used on Android. How it's enabled depends on the device manufacturer, Android version, and TalkBack version. However, TalkBack can typically be enabled on your Android device via the device settings:

1. Open the **Settings** app.
1. Select **Accessibility** > **TalkBack**.
1. Turn **Use TalkBack** on.
1. Select **OK**.

> [!NOTE]
> While these steps apply to most devices, you might experience some differences.

A TalkBack tutorial opens automatically the first time you enable TalkBack.

For alternative methods of enabling TalkBack, see [Turn Talkback on or off](https://support.google.com/accessibility/android/answer/6007100?hl=en-GB&ref_topic=10601570).

### Enable VoiceOver

VoiceOver is the primary screen reader used on iOS and macOS. On iOS, VoiceOver can be enabled as follows:

1. Open the **Settings** app.
1. Select **Accessibility** > **TalkBack**.
1. Turn **VoiceOver** on.
1. Select **OK**.

A VoiceOver tutorial can be opened by selecting **VoiceOver Practice**, once VoiceOver is enabled.

For alternative methods of enabling VoiceOver, see [Turn on and practice VoiceOver on iPhone](https://support.apple.com/guide/iphone/iph3e2e415f/ios) and [Turn on and practice VoiceOver on iPad](https://support.apple.com/guide/ipad/ipad9a246898/ipados).

On macOS, VoiceOver can be enabled as follows:

1. Open the **System Preferences**.
1. Select **Accessibility** > **VoiceOver**.
1. Select **Enable VoiceOver**.
1. Select **Use VoiceOver**.

A VoiceOver tutorial can be opened by selecting **Open VoiceOver Training...**.

For alternative methods of enabling VoiceOver, see [Turn VoiceOver on or off on Mac](https://support.apple.com/guide/voiceover/vo2682/mac).

### Enable Narrator

Narrator is the primary screen reader used on Windows. Narrator can be enabled by pressing the **Windows logo key** + **Ctrl** + **Enter** together. These keys can be pressed again to stop Narrator.

For more information about Narrator, see [Complete guide to Narrator](https://support.microsoft.com/windows/complete-guide-to-narrator-e4397a0d-ef4f-b386-d8ae-c172f109bdb1).
