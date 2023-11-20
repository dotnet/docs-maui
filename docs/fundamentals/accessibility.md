---
title: "Build accessible apps with semantic properties"
description: "Learn how to use the SemanticProperties class in a .NET MAUI app, so that a screen reader can speak about the user interface elements on a page."
ms.date: 10/03/2023
---

# Build accessible apps with semantic properties

Semantics for accessibility is concerned with building experiences that make your apps inclusive for people who use technology in a wide range of environments and approach your UI with a range of needs and experiences. In many situations, legal requirements for accessibility may provide an impetus for developers to address accessibility issues. Regardless, it's advisable to build inclusive and accessible apps so that your apps reach the largest possible audience.

The [Web Content Accessibility Guidelines (WCAG)](https://www.w3.org/WAI/standards-guidelines/wcag/) are the global accessibility standard and legal benchmark for web and mobile. These guidelines describe the various ways in which apps can be made more perceivable, operable, understandable, and robust, for all.

Many user accessibility needs are met by assistive technology products installed by the user or by tools and settings provided by the operating system. This includes functionality such as screen readers, screen magnification, and high-contrast settings.

Screen readers typically provide auditory descriptions of controls that are displayed on the screen. These descriptions help users navigate through the app and provide references to controls, such as images, that have no input or text. Screen readers are often controlled through gestures on the touchscreen, trackpad, or keyboard. For information about enabling screen readers, see [Enable screen readers](#enabling-screen-readers).

Operating systems have their own screen readers with their own unique behavior and configuration. For example, most screen readers read the text associated with a control when it receives focus, enabling users to orient themselves as they navigate through the app. However, some screen readers can also read the entire app user interface when a page appears, which enables the user to receive all of the page's available informational content before attempting to navigate it.

Most screen readers will automatically read any text associated with a control that receives accessibility focus. This means that controls, such as <xref:Microsoft.Maui.Controls.Label> or <xref:Microsoft.Maui.Controls.Button>, that have a `Text` property set will be accessible for the user. However, <xref:Microsoft.Maui.Controls.Image>, <xref:Microsoft.Maui.Controls.ImageButton>, <xref:Microsoft.Maui.Controls.ActivityIndicator>, and others might not be in the accessibility tree because no text is associated with them.

.NET Multi-platform App UI (.NET MAUI) supports two approaches to providing access to the accessibility experience of the underlying platform. *Semantic properties* are the .NET MAUI approach to providing accessibility values in apps, and are the recommended approach. *Automation properties* are the Xamarin.Forms approach to providing accessibility values in apps, and have been superseded by semantic properties. In both cases, the default accessibility order of controls is the same order in which they're listed in XAML or added to the layout. However, different layouts might have additional factors that influence accessibility order. For example, the accessibility order of <xref:Microsoft.Maui.Controls.StackLayout> is also based on its orientation, and the accessibility order of <xref:Microsoft.Maui.Controls.Grid> is based on its row and column arrangement. For more information about content ordering, see [Meaningful Content Ordering](https://devblogs.microsoft.com/xamarin/the-journey-to-accessible-apps-meaningful-content-ordering/) on the Xamarin blog.

> [!NOTE]
> When a <xref:Microsoft.Maui.Controls.WebView> displays a website that's accessible, it will also be accessible in a .NET MAUI app. Conversely, when a <xref:Microsoft.Maui.Controls.WebView> displays a website that's not accessible, it won't be accessible in a .NET MAUI app.

## Semantic properties

Semantic properties are used to define information about which controls should receive accessibility focus and which text should be read aloud to the user. Semantic properties are attached properties that can be added to any element to set the underlying platform accessibility APIs.

> [!IMPORTANT]
> Semantic properties don't try to force equivalent behavior on each platform. Instead, they rely on the accessibility experience provided by each platform.

The <xref:Microsoft.Maui.Controls.SemanticProperties> class defines the following attached properties:

- [`Description`](xref:Microsoft.Maui.Controls.SemanticProperties.DescriptionProperty), of type `string`, which represents a description that will be read aloud by the screen reader. For more information, see [Description](#description).
- [`Hint`](xref:Microsoft.Maui.Controls.SemanticProperties.HintProperty), of type `string`, which is similar to `Description`, but provides additional context such as the purpose of a control. For more information, see [Hint](#hint).
- [`HeadingLevel`](xref:Microsoft.Maui.Controls.SemanticProperties.HeadingLevelProperty), of type <xref:Microsoft.Maui.SemanticHeadingLevel>, which enables an element to be marked as a heading to organize the UI and make it easier to navigate. For more information, see [Heading levels](#heading-levels).

These attached properties set platform accessibility values so that a screen reader can speak about the element. For more information about attached properties, see [Attached properties](~/fundamentals/attached-properties.md).

### Description

The [`Description`](xref:Microsoft.Maui.Controls.SemanticProperties.DescriptionProperty) attached property represents a short, descriptive `string` that a screen reader uses to announce an element. This property should be set for elements that have a meaning that's important for understanding the content or interacting with the user interface. Setting this property can be accomplished in XAML:

```xaml
<Image Source="dotnet_bot.png"
       SemanticProperties.Description="Cute dot net bot waving hi to you!" />
```

Alternatively, it can be set in C#:

```csharp
Image image = new Image { Source = "dotnet_bot.png" };
SemanticProperties.SetDescription(image, "Cute dot net bot waving hi to you!");
```

In addition, the <xref:Microsoft.Maui.Controls.BindableObject.SetValue%2A> method can also be used to set the [`Description`](xref:Microsoft.Maui.Controls.SemanticProperties.DescriptionProperty) attached property:

```csharp
image.SetValue(SemanticProperties.DescriptionProperty, "Cute dot net bot waving hi to you!");
```

The accessibility information for an element can also be defined on another element. For example, a <xref:Microsoft.Maui.Controls.Label> next to an <xref:Microsoft.Maui.Controls.Switch> can be used to describe what the <xref:Microsoft.Maui.Controls.Switch> represents. This can be accomplished in XAML as follows:

```xaml
<Label x:Name="label"
       Text="Enable dark mode: " />
<Switch SemanticProperties.Description="{Binding Source={x:Reference label} Path=Text}" />
```

Alternatively, it can be set in C# as follows:

```csharp
Label label = new Label
{
    Text = "Enable dark mode: "
};
Switch mySwitch = new Switch();
SemanticProperties.SetDescription(mySwitch, label.Text);
```

<!-- markdownlint-disable MD032 -->

> [!WARNING]
> - Avoid setting the [`Description`](xref:Microsoft.Maui.Controls.SemanticProperties.DescriptionProperty) attached property on a <xref:Microsoft.Maui.Controls.Label>. This will stop the `Text` property being spoken by the screen reader. This is because the visual text should ideally match the text read aloud by the screen reader.
> - Avoid setting the [`Description`](xref:Microsoft.Maui.Controls.SemanticProperties.DescriptionProperty) attached property on an <xref:Microsoft.Maui.Controls.Entry> or <xref:Microsoft.Maui.Controls.Editor> on Android. Doing so will stop Talkback actions functioning. Instead, use the <xref:Microsoft.Maui.Controls.InputView.Placeholder> property or the [`Hint`](xref:Microsoft.Maui.Controls.SemanticProperties.HintProperty) attached property.
> - On iOS, if you set the [`Description`](xref:Microsoft.Maui.Controls.SemanticProperties.DescriptionProperty) property on any control that has children the screen reader will be unable to reach the children. This is because iOS doesn't provide accessibility features that allow the navigation from a parent element into a child element.

<!-- markdownlint-enable MD032 -->

### Hint

The [`Hint`](xref:Microsoft.Maui.Controls.SemanticProperties.HintProperty) attached property represents a `string` that provides additional context to the [`Description`](xref:Microsoft.Maui.Controls.SemanticProperties.DescriptionProperty) attached property, such as the purpose of a control. Setting this property can be accomplished in XAML:

```xaml
<Image Source="like.png"
       SemanticProperties.Description="Like"
       SemanticProperties.Hint="Like this post." />
```

Alternatively, it can be set in C#:

```csharp
Image image = new Image { Source = "like.png" };
SemanticProperties.SetDescription(image, "Like");
SemanticProperties.SetHint(image, "Like this post.");
```

In addition, the <xref:Microsoft.Maui.Controls.BindableObject.SetValue%2A> method can also be used to set the [`Hint`](xref:Microsoft.Maui.Controls.SemanticProperties.HintProperty) attached property:

```csharp
image.SetValue(SemanticProperties.HintProperty, "Like this post.");
```

On Android, this property behaves slightly differently depending on the control it's attached to. For example, for controls without text values, such as <xref:Microsoft.Maui.Controls.Switch> and <xref:Microsoft.Maui.Controls.CheckBox>, the controls will display the hint with the control. However, for controls with text values, the hint is not displayed and is read after the text value.

> [!WARNING]
> The [`Hint`](xref:Microsoft.Maui.Controls.SemanticProperties.HintProperty) property conflicts with the `Entry.Placeholder` property on Android, which both map to the same platform property. Therefore, setting a different [`Hint`](xref:Microsoft.Maui.Controls.SemanticProperties.HintProperty) value to the `Entry.Placeholder` value isn't recommended.

<!-- ^ and Editor? -->

### Heading levels

The [`HeadingLevel`](xref:Microsoft.Maui.Controls.SemanticProperties.HeadingLevelProperty) attached property enables an element to be marked as a heading to organize the UI and make it easier to navigate. Some screen readers enable users to quickly jump between headings.

Headings have a level from 1 to 9, and are represented by the <xref:Microsoft.Maui.SemanticHeadingLevel> enumeration, which defines `None`, and `Level1` through `Level9` members.

> [!IMPORTANT]
> While Windows offers 9 levels of headings, Android and iOS only offer a single heading. Therefore, when [`HeadingLevel`](xref:Microsoft.Maui.Controls.SemanticProperties.HeadingLevelProperty) is set on Windows it maps to the correct heading level. However, when set on Android and iOS it maps to a single heading level.

The following example demonstrates setting this attached property:

```xaml
<Label Text="Get started with .NET MAUI"
       SemanticProperties.HeadingLevel="Level1" />
<Label Text="Paragraphs of text go here." />
<Label Text="Installation"
       SemanticProperties.HeadingLevel="Level2" />
<Label Text="Paragraphs of text go here." />    
<Label Text="Build your first app"
       SemanticProperties.HeadingLevel="Level3" />
<Label Text="Paragraphs of text go here." />     
<Label Text="Publish your app"
       SemanticProperties.HeadingLevel="Level4" />
<Label Text="Paragraphs of text go here." />   
```

Alternatively, it can be set in C#:

```csharp
Label label1 = new Label { Text = "Get started with .NET MAUI" };
Label label2 = new Label { Text = "Paragraphs of text go here." };
Label label3 = new Label { Text = "Installation" };
Label label4 = new Label { Text = "Paragraphs of text go here." };
Label label5 = new Label { Text = "Build your first app" };
Label label6 = new Label { Text = "Paragraphs of text go here." };
Label label7 = new Label { Text = "Publish your app" };
Label label8 = new Label { Text = "Paragraphs of text go here." };
SemanticProperties.SetHeadingLevel(label1, SemanticHeadingLevel.Level1);
SemanticProperties.SetHeadingLevel(label3, SemanticHeadingLevel.Level1);
SemanticProperties.SetHeadingLevel(label5, SemanticHeadingLevel.Level1);
SemanticProperties.SetHeadingLevel(label7, SemanticHeadingLevel.Level1);
```

In addition, the <xref:Microsoft.Maui.Controls.BindableObject.SetValue%2A> method can also be used to set the [`HeadingLevel`](xref:Microsoft.Maui.Controls.SemanticProperties.HeadingLevelProperty) attached property:

```csharp
label1.SetValue(SemanticProperties.HeadingLevelProperty, SemanticHeadingLevel.Level1);
```

## Semantic focus

Controls have a <xref:Microsoft.Maui.SemanticExtensions.SetSemanticFocus%2A> extension method which forces screen reader focus to a specified element. For example, given a <xref:Microsoft.Maui.Controls.Label> named `label`, screen reader focus can be forced to the element with the following code:

```csharp
label.SetSemanticFocus();
```

## Semantic screen reader

.NET MAUI provides the <xref:Microsoft.Maui.Accessibility.ISemanticScreenReader> interface, with which you can instruct a screen reader to announce text to the user. The interface is exposed through the <xref:Microsoft.Maui.Accessibility.SemanticScreenReader.Default> property, and is available in the <xref:Microsoft.Maui.Accessibility> namespace.

To instruct a screen reader to announce text, use the <xref:Microsoft.Maui.Accessibility.SemanticScreenReader.Announce%2A> method, passing a `string` argument that represents the text. The following example demonstrates using this method:

```csharp
SemanticScreenReader.Default.Announce("This is the announcement text.");
```

### Limitations

The default platform screen reader must be enabled for text to be read aloud.

## Automation properties

Automation properties are attached properties that can be added to any element to indicate how the element is reported to the underlying platform's accessibility framework.

The <xref:Microsoft.Maui.Controls.AutomationProperties> class defines the following attached properties:

- [`ExcludedWithChildren`](xref:Microsoft.Maui.Controls.AutomationProperties.ExcludedWithChildrenProperty), of type `bool?`, determines if an element and its children should be excluded from the accessibility tree. For more information, see [ExcludedWithChildren](#excludedwithchildren).
- [`IsInAccessibleTree`](xref:Microsoft.Maui.Controls.AutomationProperties.IsInAccessibleTreeProperty), of type `bool?`, indicates whether the element is available in the accessibility tree. For more information, see [IsInAccessibleTree](#isinaccessibletree).
- [`Name`](xref:Microsoft.Maui.Controls.AutomationProperties.NameProperty), of type `string`, represents a short description of the element that serves as a speakable identifier for that element. For more information, see [Name](#name).
- [`HelpText`](xref:Microsoft.Maui.Controls.AutomationProperties.HelpTextProperty), of type `string`, represents a longer description of the element, which can be thought of as tooltip text that's associated with the element. For more information, see [HelpText](#helptext).
- [`LabeledBy`](xref:Microsoft.Maui.Controls.AutomationProperties.LabeledByProperty), of type <xref:Microsoft.Maui.Controls.VisualElement>, which enables another element to define accessibility information for the current element. For more information, see [LabeledBy](#labeledby).

These attached properties set platform accessibility values so that a screen reader can speak about the element. For more information about attached properties, see [Attached properties](~/fundamentals/attached-properties.md).

Different screen readers read different accessibility values. Therefore, when using automation properties it's recommended that thorough accessibility testing is carried out on each platform to ensure an optimal experience.

> [!IMPORTANT]
> Automation properties are the Xamarin.Forms approach to providing accessibility values in apps, and have been superseded by semantic properties. For more information about semantic properties, see [Semantic properties](#semantic-properties).

### ExcludedWithChildren

The [`ExcludedWithChildren`](xref:Microsoft.Maui.Controls.AutomationProperties.ExcludedWithChildrenProperty) attached property, of type `bool?`, determines if an element and its children should be excluded from the accessibility tree. This enables scenarios such as displaying an <xref:Microsoft.Maui.Controls.AbsoluteLayout> over another layout such as a <xref:Microsoft.Maui.Controls.StackLayout>, with the <xref:Microsoft.Maui.Controls.StackLayout> being excluded from the accessibility tree when it's not visible. It can be used from XAML as follows:

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

When this attached property is set, .NET MAUI sets the [`IsInAccessibleTree`](xref:Microsoft.Maui.Controls.AutomationProperties.IsInAccessibleTreeProperty) attached property to `false` on the specified element and its children.

### IsInAccessibleTree

> [!WARNING]
> This attached property should typically remain unset. The majority of controls should be present in the accessibility tree, and the `AutomationProperties.ExcludedWithChildren` attached property can be set in scenarios where an element and its children need removing from the accessibility tree.

The [`IsInAccessibleTree`](xref:Microsoft.Maui.Controls.AutomationProperties.IsInAccessibleTreeProperty) attached property, of type `bool?`, determines if the element is visible to screen readers. It must be set to `true` to use the other automation properties. This can be accomplished in XAML as follows:

```xaml
<Entry AutomationProperties.IsInAccessibleTree="true" />
```

Alternatively, it can be set in C# as follows:

```csharp
Entry entry = new Entry();
AutomationProperties.SetIsInAccessibleTree(entry, true);
```

> [!WARNING]
> On iOS, if the `IsInAccessibleTree` property is `true` on any control that has children the screen reader will be unable to reach the children. This is because iOS doesn't provide accessibility features that allow the navigation from a parent element into a child element.

### Name

> [!IMPORTANT]
> The [`Name`](xref:Microsoft.Maui.Controls.AutomationProperties.NameProperty) attached property has been deprecated in .NET 8. Instead, use the [`Description`](xref:Microsoft.Maui.Controls.SemanticProperties.DescriptionProperty) attached property.

The [`Name`](xref:Microsoft.Maui.Controls.AutomationProperties.NameProperty) attached property value should be a short, descriptive text string that a screen reader uses to announce an element. This property should be set for elements that have a meaning that is important for understanding the content or interacting with the user interface. This can be accomplished in XAML as follows:

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
> The [`HelpText`](xref:Microsoft.Maui.Controls.AutomationProperties.HelpTextProperty) attached property has been deprecated in .NET 8. Instead, use the [`Hint`](xref:Microsoft.Maui.Controls.SemanticProperties.HintProperty) attached property.

The [`HelpText`](xref:Microsoft.Maui.Controls.AutomationProperties.HelpTextProperty) attached property should be set to text that describes the user interface element, and can be thought of as tooltip text associated with the element. This can be accomplished in XAML as follows:

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

On some platforms, for edit controls such as an <xref:Microsoft.Maui.Controls.Entry>, the `HelpText` property can sometimes be omitted and replaced with placeholder text. For example, "Enter your name here" is a good candidate for the `Entry.Placeholder` property that places the text in the control prior to the user's actual input.

### LabeledBy

> [!IMPORTANT]
> The [`LabeledBy`](xref:Microsoft.Maui.Controls.AutomationProperties.LabeledByProperty) attached property has been deprecated in .NET 8. Instead, use a `SemanticProperties.Description` binding. For more information, see [SemanticProperties: Description](#description).

The [`LabeledBy`](xref:Microsoft.Maui.Controls.AutomationProperties.LabeledByProperty) attached property allows another element to define accessibility information for the current element. For example, a <xref:Microsoft.Maui.Controls.Label> next to an <xref:Microsoft.Maui.Controls.Entry> can be used to describe what the <xref:Microsoft.Maui.Controls.Entry> represents. This can be accomplished in XAML as follows:

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

However, none of these tools can perfectly emulate the screen reader user experience, and the best way to test and troubleshoot your apps for accessibility will always be manually on physical devices with screen readers.

## Enabling screen readers

Each platform has a different default screen reader to narrate accessibility values:

- Android has TalkBack. For information on enabling TalkBack, see [Enable TalkBack](#enable-talkback).
- iOS and macOS have VoiceOver. For information on enabling VoiceOver, see [Enable VoiceOver](#enable-voiceover).
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
1. Select **Accessibility** > **VoiceOver**.
1. Turn **VoiceOver** on.

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

## Accessibility checklist

Follow these tips to ensure that your .NET MAUI apps are accessible to the widest audience possible:

<!-- markdownlint-disable MD032 -->

> [!div class="checklist"]
> - Ensure your app is perceivable, operable, understandable, and robust for all by following the Web Content Accessibility Guidelines (WCAG). WCAG is the global accessibility standard and legal benchmark for web and mobile. For more information, see [Web Content Accessibility Guidelines (WCAG) Overview](https://www.w3.org/WAI/standards-guidelines/wcag/).
> - Make sure the user interface is self-describing. Test that all the elements of your user interface are screen reader accessible. Add descriptive text and hints when necessary.
> - Ensure that images and icons have alternate text descriptions.
> - Support large fonts and high contrast. Avoid hardcoding control dimensions, and instead prefer layouts that resize to accommodate larger font sizes. Test color schemes in high-contrast mode to ensure they are readable.
> - Design the visual tree with navigation in mind. Use appropriate layout controls so that navigating between controls using alternate input methods follows the same logical flow as using touch. In addition, exclude unnecessary elements from screen readers (for example, decorative images or labels for fields that are already accessible).
> - Don't rely on audio or color cues alone. Avoid situations where the sole indication of progress, completion, or some other state is a sound or color change. Either design the user interface to include clear visual cues, with sound and color for reinforcement only, or add specific accessibility indicators. When choosing colors, try to avoid a palette that is hard to distinguish for users with color blindness.
> - Provide captions for video content and a readable script for audio content. It's also helpful to provide controls that adjust the speed of audio or video content, and ensure that volume and transport controls are easy to find and use.
> - Localize your accessibility descriptions when the app supports multiple languages.
> - Test the accessibility features of your app on each platform it targets. For more information, see [Testing accessibility](#testing-accessibility).

<!-- markdownlint-enable MD032 -->
