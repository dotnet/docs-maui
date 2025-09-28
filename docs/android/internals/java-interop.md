---
title: "Java and .NET managed code interoperability"
description: "Learn how .NET for Android interoperates with the Java and Kotlin APIs that are provided by Android."
ms.date: 06/21/2023
no-loc: [ "Java", "Kotlin" ]
---

# Java and .NET managed code interoperability

App developers expect to be able to call native Android APIs and receive calls, or react to events, from the Android APIs using code written in one of the .NET managed languages. .NET for Android employs a number of approaches, at build and at runtime, to bridge the Java VM (ART in Android OS) and the Managed VM (MonoVM).

The Java VM and the Managed VM co-exist in the same process or app as separate entities. Despite sharing the same process resources, there's no direct way to call Java/Kotlin APIs from .NET, and there's no direct way for Java/Kotlin code to invoke managed code APIs. To enable this communication, .NET for Android uses the [Java Native Interface (JNI)](https://docs.oracle.com/javase/8/docs/technotes/guides/jni/spec/jniTOC.html). This is an approach that enables native code (.NET managed code being native in this context) to register implementations of Java methods, which are written outside the Java VM and in languages other than Java or Kotlin. These methods need to be declared in Java code, for example:

```java
class MainActivity extends androidx.appcompat.app.AppCompatActivity
{
  public void onCreate (android.os.Bundle p0)
  {
    n_onCreate (p0);
  }

  private native void n_onCreate (android.os.Bundle p0);
}
```

Each native method is declared using the `native` keyword, and whenever it is invoked from Java code, the Java VM will use the JNI to invoke the target method.

Native methods can be registered dynamically or statically, by providing a native shared library which exports a symbol with an appropriate name that points to the native function implementing the Java method.

## Java callable wrappers

.NET for Android wraps Android APIs by generating C# code that mirrors the Java/Kotlin APIs. Each generated class that corresponds to a Java/Kotlin type is derived from the `Java.Lang.Object` class (implemented in the `Mono.Android` assembly), which marks it as a Java interoperable type. This means that it can implement or override virtual Java methods.  To make registration and invocation of these methods possible, it's necessary to generate a Java class that mirrors the managed class and that provides an entry point to the Java to managed transition. Java classes are generated during app build, as well as during the .NET for Android build, and are known as *Java Callable Wrappers* (JCW). The following example shows a managed class that overrides two Java virtual methods:

```csharp
public class MainActivity : AppCompatActivity
{
    public override Android.Views.View? OnCreateView(Android.Views.View? parent, string name, Android.Content.Context context, Android.Util.IAttributeSet attrs)
    {
        return base.OnCreateView(parent, name, context, attrs);
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        DoSomething(savedInstanceState);
    }

    void DoSomething(Bundle bundle)
    {
        // do something with the bundle
    }
}
```

In this example, the managed class overrides the `OnCreateView` and `OnCreate` Java virtual methods that are found in the `AppCompatActivity` type. The `DoSomething` method doesn't correspond to any method found in the base Java type, and therefore it won't be included in the JCW.

The following example shows the JCW generated for the above class (with some generated methods omitted for clarity):

```java
public class MainActivity extends androidx.appcompat.app.AppCompatActivity
{
  public android.view.View onCreateView (android.view.View p0, java.lang.String p1, android.content.Context p2, android.util.AttributeSet p3)
  {
    return n_onCreateView (p0, p1, p2, p3);
  }
  private native android.view.View n_onCreateView (android.view.View p0, java.lang.String p1, android.content.Context p2, android.util.AttributeSet p3);

  public void onCreate (android.os.Bundle p0)
  {
    n_onCreate (p0);
  }
  private native void n_onCreate (android.os.Bundle p0);
}
```

## Registration

Both method registration approaches rely on generation of JCWs, with dynamic registration requiring more code to be generated so that the registration can be performed at runtime.

JCWs are generated only for types that derive from the `Java.Lang.Object` type. Finding such types is the task of [Java.Interop](https://github.com/xamarin/java.interop), which reads all the assemblies referenced by the app and its libraries. The returned list of assemblies is then used by a variety of tasks, with JCW being one of them.

After all types are found, each method in each type is analyzed to look for those that override a virtual Java method and therefore need to be included in the wrapper class code. The generator also optionally checks whether the given method can be registered statically.

The generator looks for methods decorated with the `Register` attribute, which most frequently are created by invoking its constructor with the Java method name, the JNI method signature, and the connector method name. The connector is a static method that creates a delegate that subsequently allows invocation of the native callback method:

```csharp
public class MainActivity : AppCompatActivity
{
    // Connector backing field
    static Delegate? cb_onCreate_Landroid_os_Bundle_;

    // Connector method
    static Delegate GetOnCreate_Landroid_os_Bundle_Handler()
    {
        if (cb_onCreate_Landroid_os_Bundle_ == null)
            cb_onCreate_Landroid_os_Bundle_ = JNINativeWrapper.CreateDelegate((_JniMarshal_PPL_V)n_OnCreate_Landroid_os_Bundle_);
        return cb_onCreate_Landroid_os_Bundle_;
    }

    // Native callback
    static void n_OnCreate_Landroid_os_Bundle_(IntPtr jnienv, IntPtr native__this, IntPtr native_savedInstanceState)
    {
        var __this = global::Java.Lang.Object.GetObject<Android.App.Activity>(jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
        var savedInstanceState = global::Java.Lang.Object.GetObject<Android.OS.Bundle>(native_savedInstanceState, JniHandleOwnership.DoNotTransfer);
        __this.OnCreate(savedInstanceState);
    }

    // Target method
    [Register("onCreate", "(Landroid/os/Bundle;)V", "GetOnCreate_Landroid_os_Bundle_Handler")]
    protected virtual unsafe void OnCreate(Android.OS.Bundle? savedInstanceState)
    {
        const string __id = "onCreate.(Landroid/os/Bundle;)V";
        try
        {
            JniArgumentValue* __args = stackalloc JniArgumentValue[1];
            __args[0] = new JniArgumentValue((savedInstanceState == null) ? IntPtr.Zero : ((global::Java.Lang.Object)savedInstanceState).Handle);
            _members.InstanceMethods.InvokeVirtualVoidMethod(__id, this, __args);
        }
        finally
        {
            global::System.GC.KeepAlive(savedInstanceState);
        }
    }
}
```

The above code is generated in the `Android.App.Activity` class when .NET for Android is built. What happens with this code depends on the registration approach.

## Dynamic registration

This registration approach is used by .NET for Android when an app is built in the Debug configuration, or when [marshal methods](#marshal-methods) is turned off.

With dynamic registration, the following Java code is generated for the C# example shown in [Java callable wrappers](#java-callable-wrappers) (with some generated methods omitted for clarity):

```java
public class MainActivity extends androidx.appcompat.app.AppCompatActivity
{
  public static final String __md_methods;
  static
  {
    __md_methods =
            "n_onCreateView:(Landroid/view/View;Ljava/lang/String;Landroid/content/Context;Landroid/util/AttributeSet;)Landroid/view/View;:GetOnCreateView_Landroid_view_View_Ljava_lang_String_Landroid_content_Context_Landroid_util_AttributeSet_Handler\n" +
            "n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
            "";
    mono.android.Runtime.register ("HelloAndroid.MainActivity, HelloAndroid", MainActivity.class, __md_methods);
  }

  public android.view.View onCreateView (android.view.View p0, java.lang.String p1, android.content.Context p2, android.util.AttributeSet p3)
  {
    return n_onCreateView (p0, p1, p2, p3);
  }

  private native android.view.View n_onCreateView (android.view.View p0, java.lang.String p1, android.content.Context p2, android.util.AttributeSet p3);

  public void onCreate (android.os.Bundle p0)
  {
    n_onCreate (p0);
  }

  private native void n_onCreate (android.os.Bundle p0);
}
```

The code that takes part in registration is the class's static constructor. For each method registered for the type (that is, implemented or overridden in managed code), the JCW generator outputs a single string that contains information about the type and method to register. Each registration string is terminated with the newline character and the entire sequence ends with an empty string. All the registration strings are concatenated and placed in the `__md_methods` static variable. The `mono.android.Runtime.register` method is then invoked to register all the methods.

### Dynamic registration call sequence

All the native methods declared in the generated Java type are registered when the type is constructed or accessed for the first time. This is when the Java VM invokes the type's static constructor, initiating a sequence of calls that ends with all of the type's methods being registered with the JNI:

1. `mono.android.Runtime.register` is a native method, declared in the `Runtime` class of .NET for Android's Java runtime code, and implemented in the native .NET for Android runtime. The purpose of this method is to prepare a call into .NET for Android's managed runtime code.
1. `Android.Runtime.JNIEnv::RegisterJniNatives` is passed the name of the managed type for which to register Java methods, and uses .NET reflection to load that type followed by a call to cache the type. It ends with a call to the `Android.Runtime.AndroidTypeManager::RegisterNativeMembers` method.
1. `Android.Runtime.AndroidTypeManager::RegisterNativeMembers` calls the `Java.Interop.JniEnvironment.Types::RegisterNatives` method which first generates a delegate to the native callback method, using `System.Reflection.Emit`, and invokes Java JNI's `RegisterNatives` function to register the native methods for a managed type.

> [!NOTE]
> The `System.Reflection.Emit` call is a costly operation, and is repeated for each registered method.

For more information about Java type registration, see [Java Type Registration](https://github.com/xamarin/xamarin-android/wiki/Blueprint#java-type-registration).

## Marshal methods

The marshal methods registration approach takes advantage of the JNIs ability to look up implementations of `native` Java methods in native libraries. Such symbols must have names that follow a set of rules, so that the JNI is able to locate them.

The goal of this approach is to bypass the dynamic registration approach, replacing it with native code generated and compiled during app build. This reduces app startup time. To achieve this goal, this approach uses classes that [generate native code](#llvm-intermediate-representation-code-generation) and [modify assemblies](#assembly-rewriting) which contain the registered methods.

The marshal methods classifier recognizes the standard method registration pattern, which consists of:

- The connector method, which is `GetOnCreate_Landroid_os_Bundle_Handler` in the example in [Registration](#registration).
- The delegate backing field, which is `cb_onCreate_Landroid_os_Bundle_` in the example in [Registration](#registration).
- The native callback method, which is `n_OnCreate_Landroid_os_Bundle_` in the example in [Registration](#registration).
- The virtual target method that dispatches the call to the actual object, which is `OnCreate` in the example in [Registration](#registration).

Whenever the classifier runs, it's passed the methods declaring type and the `Register` attribute instance which it uses to check whether the method being registered conforms to the registration pattern. The connector, native callback methods, and backing field must be private and static for the registered method to be considered as a candidate for static registration.

> [!NOTE]
> Registered methods that don't follow the standard pattern will be registered dynamically.

With marshal methods, the following Java code is generated for the C# example shown in [Java callable wrappers](#java-callable-wrappers) (with some generated methods omitted for clarity):

```java
public class MainActivity extends androidx.appcompat.app.AppCompatActivity
{
  public android.view.View onCreateView (android.view.View p0, java.lang.String p1, android.content.Context p2, android.util.AttributeSet p3)
  {
    return n_onCreateView (p0, p1, p2, p3);
  }

  private native android.view.View n_onCreateView (android.view.View p0, java.lang.String p1, android.content.Context p2, android.util.AttributeSet p3);

  public void onCreate (android.os.Bundle p0)
  {
    n_onCreate (p0);
  }

  private native void n_onCreate (android.os.Bundle p0);
}
```

Compared to the code generated for dynamic registration, there is no static constructor. However, the rest of the code is identical.

### JNI requirements

The JNI specifies a [series of rules](https://docs.oracle.com/javase/8/docs/technotes/guides/jni/spec/design.html#resolving_native_method_names) that govern how the native symbol name is constructed. A native method name is concatenated from the following components:

- Each symbol starts with the `Java_` prefix.
- A mangled *fully qualified class name*.
- A `_` character serves as a separator.
- A mangled *method* name.
- Optionally, a double underscore `__` and the mangled method argument signature.

Mangling is a way of encoding certain characters that aren't directly representable in the source code and in the native symbol name. The JNI specification allows for direct use of ASCII letters (capital and lowercase) and digits, while all the other characters are either represented by placeholders or encoded as 16-bit hexadecimal Unicode character codes:

| Escape sequence | Denotes                                   |
|-----------------|-------------------------------------------|
| _0XXXX          | a Unicode character XXXX, all lower case. |
| _1              | The `_` character.                        |
| _2              | The `;` character in signatures.          |
| _3              | The `[` character in signatures.          |
| _               | The `.` or `/` characters.                |

Generation of JNI symbol names is performed by one of the .NET for Android build tasks while generating the native function source code.

JNI supports a short and long form of the native symbol name. The short form is looked up first by the Java VM, followed by the long form. The long forms needs to be used only for overloaded methods.

### LLVM intermediate representation code generation

One of the .NET for Android build tasks uses the LLVM intermediate representation (IR) generator infrastructure to output data and executable code for all the marshal methods wrappers. Consider the following C++ code, which serves as a guide to understanding how the marshal method runtime invocation works:

```C++
using get_function_pointer_fn = void(*)(uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr);

static get_function_pointer_fn get_function_pointer;

void xamarin_app_init (get_function_pointer_fn fn) noexcept
{
  get_function_pointer = fn;
}

using android_app_activity_on_create_bundle_fn = void (*) (JNIEnv *env, jclass klass, jobject savedInstanceState);
static android_app_activity_on_create_bundle_fn android_app_activity_on_create_bundle = nullptr;

extern "C" JNIEXPORT void
JNICALL Java_helloandroid_MainActivity_n_1onCreate__Landroid_os_Bundle_2 (JNIEnv *env, jclass klass, jobject savedInstanceState) noexcept
{
  if (android_app_activity_on_create_bundle == nullptr) {
    get_function_pointer (
      16, // mono image index
      0,  // class index
      0x0600055B, // method token
      reinterpret_cast<void*&>(android_app_activity_on_create_bundle) // target pointer
    );
  }

  android_app_activity_on_create_bundle (env, klass, savedInstanceState);
}
```

The `xamarin_app_init` function is output only once and is called by the .NET for Android runtime twice during app startup. The first time it's called is to pass `get_function_pointer_fn`, and the second time it's called is just before handing control over to the Mono VM, to pass a pointer to `get_function_pointer_fn`.

The `Java_helloandroid_MainActivity_n_1onCreate__Landroid_os_Bundle_2` function is a template that's repeated for each Java native function, with each function having its own set of arguments and its own callback backing field (`android_app_activity_on_create_bundle` here).

The `get_function_pointer` function takes indexes into several tables as parameters. One is for `MonoImage*` pointers and the other for `MonoClass*` pointers - both of which are generated at app build time and allow for very fast lookup at runtime. Target methods are retrieved by their token value, within the specified `MonoImage*` (essentially a pointer to managed assembly image in memory) and class.

The method identified using this approach must be decorated in managed code with the [`UnmanagedCallersOnly`](xref:System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute) attribute so that it can be invoked directly, as if it was a native method itself, with minimal managed marshalling overhead.

### Assembly rewriting

Managed assemblies, including `Mono.Android.dll`, that contain Java types need to be usable with dynamic registration and with marshal methods. However, both of these approaches have different requirements. It can't be assumed that every assembly will have marshal methods compliant code. Therefore, an approach is required that ensures that code meets this requirement. This is achieved by reading assemblies and modifying them by altering the definition of the native callbacks and removing the code that's no longer used by marshal methods. This task is performed by one of the .NET for Android build tasks that's invoked during app build after all the assemblies are linked but before type maps are generated.

The exact modifications that are applied are:

- Removal of the *connector backing field*.
- Removal of the *connector method*.
- Generation of a *native callback wrapper* method, which catches and propagates unhandled exceptions thrown by the native callback or the target method. This method is decorated with the [`UnmanagedCallersOnly`](xref:System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute) attribute and called directly from the native code.
- Optionally, generate code in the *native callback wrapper* to handle [non-blittable types](#wrappers-for-methods-with-non-blittable-types).

After modifications, the assembly contains the equivalent of the following C# code for each marshal method:

```csharp
public class MainActivity : AppCompatActivity
{
    // Native callback
    static void n_OnCreate_Landroid_os_Bundle_(IntPtr jnienv, IntPtr native__this, IntPtr native_savedInstanceState)
    {
        var __this = global::Java.Lang.Object.GetObject<Android.App.Activity>(jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
        var savedInstanceState = global::Java.Lang.Object.GetObject<Android.OS.Bundle>(native_savedInstanceState, JniHandleOwnership.DoNotTransfer);
        __this.OnCreate(savedInstanceState);
    }

    // Native callback exception wrapper
    [UnmanagedCallersOnly]
    static void n_OnCreate_Landroid_os_Bundle__mm_wrapper(IntPtr jnienv, IntPtr native__this, IntPtr native_savedInstanceState)
    {
        try
        {
            n_OnCreate_Landroid_os_Bundle_(jnienv, native__this, native_savedInstanceState)
        }
        catch (Exception ex)
        {
            Android.Runtime.AndroidEnvironmentInternal.UnhandledException(ex);
        }
    }

    // Target method
    [Register("onCreate", "(Landroid/os/Bundle;)V", "GetOnCreate_Landroid_os_Bundle_Handler")]
    protected virtual unsafe void OnCreate(Android.OS.Bundle? savedInstanceState)
    {
        const string __id = "onCreate.(Landroid/os/Bundle;)V";
        try
        {
            JniArgumentValue* __args = stackalloc JniArgumentValue[1];
            __args[0] = new JniArgumentValue((savedInstanceState == null) ? IntPtr.Zero : ((global::Java.Lang.Object)savedInstanceState).Handle);
            _members.InstanceMethods.InvokeVirtualVoidMethod(__id, this, __args);
        }
        finally
        {
            global::System.GC.KeepAlive(savedInstanceState);
        }
    }
}
```

#### Wrappers for methods with non-blittable types

The [`UnmanagedCallersOnly`](xref:System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute) attribute requires that all the argument types, and the method return type, are [blittable](/dotnet/framework/interop/blittable-and-non-blittable-types). Among these types is the `bool` type that's commonly used by managed classes implementing Java methods. This is currently the only non-blittable type encountered in bindings, and therefore is the only one supported by the assembly rewriter.

Whenever a method with a non-blittable type is encountered, a wrapper is generated for it so that it can be decorated with the [`UnmanagedCallersOnly`](xref:System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute) attribute. This is less error prone than modifying the native callback method's IL stream to implement the necessary conversion. An example of such a method is `Android.Views.View.IOnTouchListener::OnTouch`:

```csharp
static bool n_OnTouch_Landroid_view_View_Landroid_view_MotionEvent(IntPtr jnienv, IntPtr native__this, IntPtr native_v, IntPtr native_e)
{
    var __this = global::Java.Lang.Object.GetObject<Android.Views.View.IOnTouchListener>(jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
    var v = global::Java.Lang.Object.GetObject<Android.Views.View>(native_v, JniHandleOwnership.DoNotTransfer);
    var e = global::Java.Lang.Object.GetObject<Android.Views.MotionEvent>(native_e, JniHandleOwnership.DoNotTransfer);
    bool __ret = __this.OnTouch(v, e);
    return __ret;
}
```

This method returns a `bool` value, and so needs a wrapper to cast the return value correctly. Each wrapper method retains the native callback method name, but appends the `_mm_wrapper` suffix to it:

```csharp
[UnmanagedCallersOnly]
static byte n_OnTouch_Landroid_view_View_Landroid_view_MotionEvent__mm_wrapper(IntPtr jnienv, IntPtr native__this, IntPtr native_v, IntPtr native_e)
{
    try
    {
        return n_OnTouch_Landroid_view_View_Landroid_view_MotionEvent_(jnienv, native__this, native_v, native_e) ? 1 : 0;
    }
    catch (Exception ex)
    {
        Android.Runtime.AndroidEnvironmentInternal.UnhandledException(ex);
        return default;
    }
}
```

The wrapper's return statement uses the ternary operator to cast the boolean value to 1 or 0 because the value of `bool` across the managed runtime can take a range of values - `0` for `false`, `-1` or `1` for `true`, and `!= 0` for `true`.

Because the `bool` type in C# can be 1, 2 or 4 bytes long, it's cast to a type of a known and static size. The `byte` managed type is used as it corresponds to the Java/JNI `jboolean` type, defined as an unsigned 8-bit type.

Whenever an argument value needs to be converted between `byte` and `bool`, code is generated that's equivalent to the `argument != 0` comparison. For example, for the `Android.Views.View.IOnFocusChangeListener::OnFocusChange` method:

```csharp
[UnmanagedCallersOnly]
static void n_OnFocusChange_Landroid_view_View_Z(IntPtr jnienv, IntPtr native__this, IntPtr native_v, byte hasFocus)
{
    n_OnFocusChange_Landroid_view_View_Z(jnienv, native__this, native_v, hasFocus != 0);
}

static void n_OnFocusChange_Landroid_view_View_Z(IntPtr jnienv, IntPtr native__this, IntPtr native_v, bool hasFocus)
{
    var __this = global::Java.Lang.Object.GetObject<Android.Views.View.IOnFocusChangeListener>(jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
    var v = global::Java.Lang.Object.GetObject<Android.Views.View>(native_v, JniHandleOwnership.DoNotTransfer);
    __this.OnFocusChange(v, hasFocus);
}
```

#### UnmanagedCallersOnly attribute

Each marshal methods native callback method is decorated with the [`UnmanagedCallersOnly`](xref:System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute) attribute, to be able to invoke the callback directly from native code with minimal overhead.

### Marshal methods registration call sequence

What's common for dynamic and marshal methods registration is the resolution of the native function target performed by the Java VM runtime. In both cases, the method declared in a Java class as `native` is looked up by the Java VM when first JIT-ing the code. The difference is in the way this lookup is performed.

Dynamic registration uses the [`RegisterNatives`](https://docs.oracle.com/javase/8/docs/technotes/guides/jni/spec/functions.html#RegisterNatives) JNI function at runtime, which stores a pointer to the registered method inside the structure that describes a Java class in the Java VM.

Marshal methods, however, don't register anything with the JNI. Instead, they rely on the symbol lookup approach of the Java VM. Whenever a call to a `native` Java method is JIT'd and isn't registered previously using the `RegisterNatives` JNI function, the Java VM will proceed to look for symbols in the process runtime image and, having found a matching symbol, use a pointer to it as the target of the `native` Java method call.
