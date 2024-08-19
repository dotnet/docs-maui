---
title: Android asset packs
description: Learn how to place your large Android app assets into asset packs to increase the size of the package that you upload to Google Play.
ms.topic: concept-article
ms.date: 08/19/2024
monikerRange: ">=net-maui-9.0"

#customer intent: As a developer, I want to bundle my app assets separately from my code so that I can upload a bigger package to Google Play.
---

# Android asset packs

Assets such as video, audio, or graphics can occupy a lot of space inside your app package. For example, consider the following files in an example project:

```
MyProject.csproj
MainActivity.cs
Assets/
    MyLargeAsset.mp4
    MySmallAsset.json
AndroidManifest.xml
```

*MyLargeAsset.mp4* and *MySmallAsset.json* will both be placed into the Android App Bundle (AAB) package when the app is published. However, if *MyLargeAsset.mp4* is greater than 200Mb it can't be placed into the main app bundle due to the package size restrictions enforced by Google Play.

.NET for Android 9 introduces the ability to place assets into separate packages, known as *asset packs*. This enables you to upload apps that would normally be larger than the basic package size allowed by Google Play. By putting these assets into a separate package you gain the ability to upload a package which is up to 2Gb in size, rather than the basic package size of 200Mb.

> [!IMPORTANT]
> Asset packs can only contain assets. In the case of .NET for Android this means items that have the `AndroidAsset` build action.

## Asset pack metadata

To support asset packages in .NET for Android apps, the `AndroidAsset` item group supports two new metadata attributes:

- `AssetPack`. If present, this attribute controls which asset pack an asset is placed into. If not present, the asset will be placed into the main app bundle.

    The name of the asset pack will be `$(AndroidPackage).%(AssetPack)`. Therefore, if your package name is `com.mycompany.myproject` and the `AssetPack` value is `myassets`, the asset pack name would be `com.mycompany.myproject.myassets`.

    > [!NOTE]
    > The `AssetPack` value can also be `base`, which indicates that the asset should be placed into the main app bundle rather than an asset pack. For more information, see [Force an asset into the main app bundle](#force-an-asset-into-the-main-app-bundle).

- `DeliveryType`. If present, this attribute controls the type of asset pack is created. Valid values for this are `InstallTime`, `FastFollow`, and `OnDemand`. If not present, the default value is `InstallTime`. For more information, see [Asset delivery options](#asset-delivery-options).

Returning to the previous example, *MyLargeAsset.mp4* can be moved into its own asset pack by specifying the following `<ItemGroup>` element in the project's *.csproj* file:

```xml
<ItemGroup>
    <AndroidAsset Update="Assets/MyLargeAsset.mp4" AssetPack="myassets" />
</ItemGroup>
```

This item group will cause the .NET for Android build system to create a new asset pack called `myassets` and include *MyLargeAsset.mp4* in the pack. The pack will automatically be included in the AAB file. There's no need to specify the delivery type, provided that the default value of `InstallTime` is the required delivery option.

> [!NOTE]
> This `AndroidAsset` is using the `Update` attribute rather than `Include` because .NET for Android supports auto-import of assets.

In scenarios where you have a large number of assets you can use wildcards to update the auto-imported assets:

```xml
<ItemGroup>
    <AndroidAsset Update="Assets/*" AssetPack="myassets" />
</ItemGroup>
```

In this example, all of the assets in the *Assets* folder will be placed into an asset pack named `myassets`.

### Force an asset into the main app bundle

In scenarios where you have a large number of assets, but you don't want all of them to be placed into an asset pack, you can specify an `AssetPack` value of `base` to force an asset into the main app bundle:

```xml
<ItemGroup>
    <AndroidAsset Update="Assets/*" AssetPack="myassets" />
    <AndroidAsset Update="Assets/myimportantfile.json" AssetPack="base" />    
</ItemGroup>
```

In this example, all of the assets in the *Assets* folder except *myimportantfile.json* will be placed into an asset pack named `myassets`. However, *myimportantfile.json* is placed into the main app bundle via the `AssetPack="base"` value, rather than the `myassets` asset pack.

## Asset delivery options

Asset packs can have different delivery options, which control when your assets will install on the device:

- Install time packs are installed at the same time as the app. This pack type can be up to 1Gb in size, but you can only have one of them. This delivery type is specified with `InstallTime` metadata.
- Fast follow packs will install at some point shortly after the app has finished installing. The app will be able to start while this type of pack is being installed so you should check it has finished installing before trying to use the assets. This pack type can be up to 512Mb in size. This delivery type is specified with `FastFollow` metadata.
- On demand packs will never be downloaded to the device unless the app specifically requests it. This delivery type is specified with `OnDemand` metadata.

> [!IMPORTANT]
> The total size of all your asset packs can't exceed 2Gb, and you can have up to 50 separate asset packs.

For more information about asset delivery, see [Play Asset Delivery](https://developer.android.com/guide/playcore/asset-delivery) on developer.android.com.

## Asset packs in .NET MAUI apps

.NET MAUI apps define assets via the `MauiAsset` build action. An asset pack can be specified via the `AssetPack` attribute:

```xml
<MauiAsset
    Include="Resources\Raw\**"
    LogicalName="%(RecursiveDir)%(Filename)%(Extension)"
    AssetPack="myassetpack" />
```

> [!NOTE]
> The additional metadata will be ignored by other platforms.

If you have specific items you want to place in an asset pack you can use the `Update` attribute to define the `AssetPack` metadata:

```xml
<MauiAsset Update="Resources\Raw\myvideo.mp4" AssetPack="myassets" />
```

In .NET MAUI apps, the delivery type can be specified with the `DeliveryType` attribute on a `MauiAsset`:

```xml
<MauiAsset Update="Resources\Raw\myvideo.mp4" AssetPack="myassets" DeliveryType="FastFollow" />
```

## Check the status of `FastFollow` asset packs

If your app uses a `FastFollow` asset pack, it'll need to check that the pack is installed before trying to access its contents.

To do this, add the [Xamarin.Google.Android.Play.Asset.Delivery](https://www.nuget.org/packages/Xamarin.Google.Android.Play.Asset.Delivery) NuGet package to your project. This NuGet package provides access to the `AssetPackManager` type, which provides the ability to query the location of the asset pack:

```csharp
using Xamarin.Google.Android.Play.Core.AssetPacks;

var assetPackManager = AssetPackManagerFactory.GetInstance(this);
AssetPackLocation assetPackPath = assetPackManager.GetPackLocation("myfastfollowpack");
string assetsFolderPath = assetPackPath?.AssetsPath() ?? null;
if (assetsFolderPath is null)
{
    // FastFollow asset pack isn't installed.
}
```

The location of the `FastFollow` asset pack is queried with the `GetPackLocation` method, which returns an `AssetPackLocation` object. If the `AssetsPath` method returns `null` for the `AssetPackLocation` object this indicates that the pack hasn't yet been installed. If the `AssetsPath` method returns a value, it represents the install location of the pack.

## Download `OnDemand` asset packs

If you app uses an `OnDemand` asset pack, it'll need to download it manually. To do this, add the [Xamarin.Google.Android.Play.Asset.Delivery](https://www.nuget.org/packages/Xamarin.Google.Android.Play.Asset.Delivery) NuGet package to your project. This NuGet package provides access to the `AssetPackStateUpdateListener` type, that enables you to monitor the progress of the download. However, in .NET this type is wrapped by the `AssetPackStateUpdateListenerWrapper` type, which can be used to register an event handler to monitor the download progress.

To monitor the download progress you'll need to declare some fields:

```csharp
using Xamarin.Google.Android.Play.Core.AssetPacks;

IAssetPackManager assetPackManager;
AssetPackStateUpdateListenerWrapper listener;
```

Then, declare an event handler to monitor the download:

```csharp
using Xamarin.Google.Android.Play.Core.AssetPacks.Model;

void Listener_StateUpdate(object? sender, AssetPackStateUpdateListenerWrapper.AssetPackStateEventArgs e)
{
    var status = e.State.Status();
    switch (status)
    {
        case AssetPackStatus.Downloading:
            long downloaded = e.State.BytesDownloaded();
            long totalSize = e.State.TotalBytesToDownload();
            double percent = 100.0 * downloaded / totalSize;
            Android.Util.Log.Info ("Listener_StateUpdate", $"Downloading {percent}");
            break;
        case AssetPackStatus.Completed:
            break;
        case AssetPackStatus.WaitingForWifi:
                assetPackManager.ShowConfirmationDialog(this);
            break;
    }
}
```

For information about which `AssetPackStatus` values to use, see [AssetPackStatus](https://developer.android.com/reference/com/google/android/play/core/assetpacks/model/AssetPackStatus) on developer.android.com.

Then, create an `IAssetPackManager` instance via the `AssetPackManagerFactory.GetInstance` method and register the `Listener_StateUpdate` event handler against an `AssetPackStateUpdateListenerWrapper` object:

```csharp
assetPackManager = AssetPackManagerFactory.GetInstance (this);
listener = new AssetPackStateUpdateListenerWrapper();
listener.StateUpdate += Listener_StateUpdate;
```

You'll also need to register the listener when the app resumes, and unregister the listener when the app pauses:

```csharp
protected override void OnResume()
{
    assetPackManager.RegisterListener(listener.Listener);
    base.OnResume();
}

protected override void OnPause()
{
    assetPackManager.UnregisterListener(listener.Listener);
    base.OnPause();
}
```

Finally, before you download the `OnDemand` asset pack you'll need to check if it's already been installed. This can be accomplished with the `AssetPackManager.GetPackLocation` method:

```csharp
using Android.Gms.Extensions;

var assetPackPath = assetPackManager.GetPackLocation ("myondemandpack");
string assetsFolderPath = assetPackPath?.AssetsPath() ?? null;
if (assetsFolderPath is null)
{
    await assetPackManager.Fetch(new string[] { "myondemandpack" }).AsAsync<AssetPackStates>();
}
```

The location of the `OnDemand` asset pack is queried with the `GetPackLocation` method, which returns an `AssetPackLocation` object. If the `AssetsPath` method returns `null` for the `AssetPackLocation` object this indicates that the pack hasn't yet been installed. `assetPackManager.Fetch` can then be called to start the download. If the `AssetsPath` method returns a value, it represents the install location of the pack.

> [!NOTE]
> The `AsAsync<T>` extension method returns a `Task<T>` object, and is available in the `Android.Gms.Extensions` namespace.

The status of the download can then be monitored via the `AssetPackStateUpdateListenerWrapper`.

## Test asset packs locally

By default, .NET for Android uses the Android Application Package (APK) format for debugging. However, to test asset packs locally you'll need to ensure you're using the Android App Bundle (AAB) package format. To debug your asset packs update your *.csproj* file with the following build properties:

```xml
<PropertyGroup Condition="'$(Configuration)' == 'Debug'">
    <AndroidPackageFormat>aab</AndroidPackageFormat>
    <EmbedAssembliesIntoApk>true</EmbedAssembliesIntoApk>
    <AndroidBundleToolExtraArgs>--local-testing</AndroidBundleToolExtraArgs>
</PropertyGroup>
```

The `--local-testing` flag is required for testing on your local device. It tells the `bundletool` app that all the asset packs should be installed in a cached location on the device. It also sets up the `IAssetPackManager` to use a mock downloader that will use the cache. This enables you to test installing `OnDemand` and `FastFollow` asset packs in a `Debug` environment.
