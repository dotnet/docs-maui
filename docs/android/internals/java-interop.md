---
title: "Java to managed code interoperability"
description: "Learn about XXXXXX"
ms.date: 06/21/2023
no-loc: [ "Java", "Kotlin" ]
---

# Java to managed code interoperability

.NET for Android interoperates with the Java and Kotlin APIs that are provided by Android. App developers expect to be able to call native Android APIs and receive calls, or react to events, from the Android APIs using code written in one of the .NET managed languages. This requires mechanisms to bridge the the Java VM (ART in Android OS), and the Managed VM (MonoVM). .NET for Android employs a number of mechanisms, both a build and at runtime, to do this.

Java VM and Managed VM are two separate entities that co-exist in the same process/app. Despite sharing the same process resources, there's no direct way to call Java/Kotlin from .NET, and there's no direct way for Java/Kotlin code to invoke managed code. To enable communication, .NET for Android uses the [Java Native Interface (JNI)](https://docs.oracle.com/javase/8/docs/technotes/guides/jni/spec/jniTOC.html), which is a mechanism that allows native code (.NET managed code being native in this context) to register implementations of Java methods, written outside the Java VM and in languages other than Java/Kotlin. Such methods need to be appropriately declared in Java code, for example:

```java
class MainActivity
  extends androidx.appcompat.app.AppCompatActivity
{
  public void onCreate (android.os.Bundle p0)
  {
    n_onCreate (p0);
  }

  private native void n_onCreate (android.os.Bundle p0);
}
```

Each native method is declared using the `native` keyword, and whenever it is invoked from Java code, the Java VM will use the JNI to invoke the target method.

Native methods can be registered dynamically by calling the [`RegisterNatives`](https://docs.oracle.com/javase/8/docs/technotes/guides/jni/spec/functions.html#RegisterNatives) JNI function, or statically by providing a native shared library which exports a symbol with an appropriate name that points to the native function implementing the Java method.

## Java callable wrappers

.NET for Android wraps the Android API by generating C# code that mirrors the Java/Kotlin APIs. Each generated class that corresponds to a Java/Kotlin type is derived from the `Java.Lang.Object` class (implemented in the `Mono.Android` assembly), which marks it as a Java interoperable type. This means that it can implement or override virtual Java methods.  To make registration and invocation of such methods possible, it'd necessary to generate a Java class that mirrors the managed class and provides an entry point to the Java to managed transition. Java classes are generated during app build, as well as during the .NET for Android build, and are known as *Java Callable Wrappers* (JCW). The following example shows a managed class that overrides two Java virtual methods:

```csharp
public class MainActivity : AppCompatActivity
{
  public override Android.Views.View? OnCreateView (Android.Views.View? parent, string name, Android.Content.Context context, Android.Util.IAttributeSet attrs)
  {
    return base.OnCreateView (parent, name, context, attrs);
  }

  protected override void OnCreate (Bundle savedInstanceState)
  {
     base.OnCreate(savedInstanceState);
     DoSomething (savedInstanceState);
  }

  void DoSomething (Bundle bundle)
  {
     // do something with the bundle
  }
}
```

In this example, the managed class overrides two Java virtual methods found in the `AppCompatActivity` type - `OnCreateView` and `OnCreate`. The `DoSomething` method doesn't correspond to any method found in the base Java type, and therefore it won't be included in the JCW.

The following example shows the JCW generated for the above class, with a few generated methods omitted for brevity:

```java
public class MainActivity
        extends androidx.appcompat.app.AppCompatActivity
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

Both mechanisms of method registration rely on generation of JCWs, with dynamic registration requiring more code to be generated so that the registration can be performed at the runtime.

JCWs are generated only for types that derive from the `Java.Lang.Object` type. Finding such types is the task of Java.Interop's `JavaTypeScanner`, which uses `Mono.Cecil` to read all the assemblies referenced by the app and its libraries. The returned list of assemblies is then used by a variety of tasks, with JCW being one of them.

After all types are found, `JavaCallableWrapperGenerator` is invoked to analyze each method in each type, looking for those that override a virtual Java method and therefore need to be included in the wrapper class code. The generator optionally passes each method to an implementation of `JavaCallableMethodClassifier` abstract class to check whether the given method can be registered statically.

The `JavaCallableWrapperGenerator` type looks for methods decorated with the `[Register]` attribute, which most frequently are created by invoking its constructor with three parameters:

1. Java method name
1. JNI method signature
1. Connector method name

The connector is a static method that creates a delegate that subsequently allows invocation of the native callback method:

```csharp
public class MainActivity : AppCompatActivity
{
  // Connector backing field
  static Delegate? cb_onCreate_Landroid_os_Bundle_;

  // Connector method
  static Delegate GetOnCreate_Landroid_os_Bundle_Handler ()
  {
    if (cb_onCreate_Landroid_os_Bundle_ == null)
      cb_onCreate_Landroid_os_Bundle_ = JNINativeWrapper.CreateDelegate ((_JniMarshal_PPL_V) n_OnCreate_Landroid_os_Bundle_);
    return cb_onCreate_Landroid_os_Bundle_;
  }

  // Native callback
  static void n_OnCreate_Landroid_os_Bundle_ (IntPtr jnienv, IntPtr native__this, IntPtr native_savedInstanceState)
  {
    var __this = global::Java.Lang.Object.GetObject<Android.App.Activity> (jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
    var savedInstanceState = global::Java.Lang.Object.GetObject<Android.OS.Bundle> (native_savedInstanceState, JniHandleOwnership.DoNotTransfer);
    __this.OnCreate (savedInstanceState);
  }

  // Target method
  [Register ("onCreate", "(Landroid/os/Bundle;)V", "GetOnCreate_Landroid_os_Bundle_Handler")]
  protected virtual unsafe void OnCreate (Android.OS.Bundle? savedInstanceState)
  {
    const string __id = "onCreate.(Landroid/os/Bundle;)V";
    try {
      JniArgumentValue* __args = stackalloc JniArgumentValue [1];
      __args [0] = new JniArgumentValue ((savedInstanceState == null) ? IntPtr.Zero : ((global::Java.Lang.Object) savedInstanceState).Handle);
      _members.InstanceMethods.InvokeVirtualVoidMethod (__id, this, __args);
    } finally {
      global::System.GC.KeepAlive (savedInstanceState);
    }
  }
}
```

The above code is generated in the `Android.App.Activity` class when .NET for Android is built. What happens with this code depends on the registration mechanism.

### Dynamic registration

This registration mechanism is used by .NET for Android when an app is built in the Debug configuration or when [marshal methods](#marshal-methods) is turned off.

With dynamic registration, the following Java code is generated for the C# example shown in [Java callable wrappers](#java-callable-wrappers), with only the code relevant to registration being shown:

```java
public class MainActivity
        extends androidx.appcompat.app.AppCompatActivity
{
        public static final String __md_methods;
        static {
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

The code that takes part in registration is the class's static constructor. For each method registered for the type (that is, implemented or overridden in the managed code), the JCW generator outputs a single string that contains information about the type and method to register. Each registration string is terminated with the newline character and the entire sequence ends with an empty string. All the registration strings are concatenated and placed in the `__md_methods` static variable. The `mono.android.Runtime.register` method is then invoked to register all the methods.

### Dynamic Registration call sequence

All the native methods declared in the generated Java type are registered when the type is constructed or accessed for the first time. This is when the Java VM invokes the type's static constructor, initiating a sequence of calls that ends with all of the type's methods being registered with the JNI:

1. `mono.android.Runtime.register` is a native method, declared in the `Runtime` class of .NET for Android's Java runtime code, and implemented in the native .NET Android runtime. The purpose of this method is to prepare a call into .NET for Android's managed runtime code.
1. `Android.Runtime.JNIEnv::RegisterJniNatives` is passed the name of the managed type for which to register Java methods, and uses .NET reflection to load that type followed by a call to cache the type. It ends with a call to the `Android.Runtime.AndroidTypeManager::RegisterNativeMembers` method.
1. `Android.Runtime.AndroidTypeManager::RegisterNativeMembers` calls the `Java.Interop.JniEnvironment.Types::RegisterNatives` method which first generates a delegate to the native callback method, using `System.Reflection.Emit`, and invokes Java JNI's `RegisterNatives` function to register the native methods for a managed type.

For more information about Java type registration, see [Java Type Registration](https://github.com/xamarin/xamarin-android/wiki/Blueprint#java-type-registration).

### Marshal methods

The marshal methods registration mechanism takes advantage of the JNI's ability to look up implementations of `native` Java methods in native libraries. Such symbols must have names that follow a set of rules, so that the JNI is able to properly locate them.

The goal of marshal methods is to completely bypass the dynamic registration sequence, replacing it with native code generated and compiled during app build. This reduces app startup time. To achieve this goal, the marshal methods mechanism uses a number of classes which [generate native](#llvm-ir-code-generation) code and [modify assemblies](#assembly-rewriting) that contain the registered methods.

The current implementation of the marshal methods classifier recognizes the standard method registration pattern, using the example of the `OnCreate` method shown in [Registration](#registration). This standard pattern consists of:

- The connector method, which is `GetOnCreate_Landroid_os_Bundle_Handler` in the example in [Registration](#registration).
- The delegate backing field, which is `cb_onCreate_Landroid_os_Bundle_` in the example in [Registration](#registration).
- The native callback method, which is `n_OnCreate_Landroid_os_Bundle_` in the example in [Registration](#registration).
- The virtual target method which dispatches the call to the actual object, `OnCreate` in the example in [Registration](#registration).

Whenever the classifier's `ShouldBeDynamicallyRegistered` method is called, it's passed the method's declaring type amd the `Register` attribute instance which it uses to check whether the method being registered conforms to the standard registration pattern. The connector, native callback methods, and backing field must be private and static for the registered method to be considered as a candidate for static registration.

> [!NOTE]
> Registered methods that don't follow the standard pattern will be registered dynamically.

With marshal methods, the following Java code is generated for the C# example shown in [Java callable wrappers](#java-callable-wrappers), with only the code relevant to registration being shown:

```java
public class MainActivity
        extends androidx.appcompat.app.AppCompatActivity
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

> [!NOTE]
> Compared to the code generated for dynamic registration, there is no static constructor. However, the rest of the code is identical.

#### JNI requirements

JNI [specifies](https://docs.oracle.com/javase/8/docs/technotes/guides/jni/spec/design.html#resolving_native_method_names) a number of rules that govern how the native symbol name is constructed. This enables a mapping of object-oriented Java code (with its package names, class names and overloadable methods) into the essentially "flat" procedural "namespace" of the lowest common denominator C code.

The precise rules are outlined in the URL above, and their short version is as follows:

- Each symbol starts with the `Java_` prefix.
- Next follows the mangled *fully qualified class name*.
- Next, the `_` character serves as a separator.
- Next, a mangled *method* name.
- Optionally, a double underscore `__` and the mangled method argument signature.

Mangling is a way of encoding certain characters that are not directly representable both in the source code and in the native symbol name. The JNI specification allows for direct use of ASCII letters (capital and lowercase) and digits, while all the other characters are either represented by placeholders or encoded as 16-bit hexadecimal Unicode character codes:

| Escape sequence | Denotes                                  |
|-----------------|------------------------------------------|
| _0XXXX          | a Unicode character XXXX, all lower case |
| _1              | The `_` character                        |
| _2              | The `;` character in signatures          |
| _3              | The `[` character in signatures          |
| _               | The `.` or `/` characters                |

Generation of JNI symbol names is performed by the `MarshalMethodsNativeAssemblyGenerator` class while generating the native function source code.

JNI supports two forms of the native symbol name - a short and a long one. The former is looked up first by the Java VM, followed by the latter. The latter needs to be used only for overloaded methods.

#### LLVM intermediate representation code generation

`MarshalMethodsNativeAssemblyGenerator` uses the LLVM intermediate representation (IR) generator infrastructure to output both data and executable code for all the marshal methods wrappers. It's not necessary to understand the generated code unless you need to modify it, so this article shows the equivalent C++ code that can serve as a guide to understanding how the marshal method runtime invocation works:

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

The `xamarin_app_init` function is output only once and is called by the .NET for Android runtime twice during app startup. The first time it's called to pass `get_function_pointer_fn`, and the second time it's called just before handing control over to the MonoVM, to pass a pointer to `get_function_pointer_fn`.

The `Java_helloandroid_MainActivity_n_1onCreate__Landroid_os_Bundle_2` function is a template which is repeated for each Java native function, with each function having its own set of arguments and its own callback backing field (`android_app_activity_on_create_bundle` here).

The `get_function_pointer` function takes as parameters indexes into several tables. One is for `MonoImage*` pointers and the other for `MonoClass*` pointers - both of which are generated by the `MarshalMethodsNativeAssemblyGenerator` class at app build time and allow for very fast lookup at runtime. Target methods are retrieved by their token value, within the specified `MonoImage*` (essentially a pointer to managed assembly image in memory) and class.

The method identified using this approach must be decorated in the managed code with the [`UnmanagedCallersOnly`](xref:System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute) attribute so that it can be invoked directly, as if it was a native method itself, with minimal managed marshalling overhead.

#### Assembly rewriting

Managed assemblies, including `Mono.Android.dll`, that contain Java types need to be usable with dynamic registration and with marshal methods. However, both of these mechanisms have different requirements. It can't be assumed that every assembly will have marshal methods compliant code. Therefore, a mechanism is required that ensures that code meets this requirement.

This is achieved by reading assemblies and modifying them by altering the definition of the native callbacks and removing the code that's no longer used by marshal methods. This task is performed by the `MarshalMethodsAssemblyRewriter` class that's invoked during app build after all the assemblies are linked but before type maps are generated.

The exact modifications that are applied by `Mono.Cecil` are:

- Removal of the *connector backing field*.
- Removal of the *connector method*.
- Generation of a *native callback wrapper* method, which catches and propagates unhandled exceptions thrown by the native callback or the target method. This method is decorated with the `UnmanagedCallersOnly` attribute and called directly from the native code.
- Optionally, generate code in the *native callback wrapper* to handle [non-blittable types](#wrappers-for-methods-with-non-blittable-types).

After modifications, the assembly contains the equivalent of the following C# code for each marshal method:

```csharp
public class MainActivity : AppCompatActivity
{
  // Native callback
  static void n_OnCreate_Landroid_os_Bundle_ (IntPtr jnienv, IntPtr native__this, IntPtr native_savedInstanceState)
  {
    var __this = global::Java.Lang.Object.GetObject<Android.App.Activity> (jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
    var savedInstanceState = global::Java.Lang.Object.GetObject<Android.OS.Bundle> (native_savedInstanceState, JniHandleOwnership.DoNotTransfer);
    __this.OnCreate (savedInstanceState);
  }

  // Native callback exception wrapper
  [UnmanagedCallersOnly]
  static void n_OnCreate_Landroid_os_Bundle__mm_wrapper (IntPtr jnienv, IntPtr native__this, IntPtr native_savedInstanceState)
  {
    try {
      n_OnCreate_Landroid_os_Bundle_ (jnienv, native__this, native_savedInstanceState)
    } catch (Exception ex) {
      Android.Runtime.AndroidEnvironmentInternal.UnhandledException (ex);
    }
  }

  // Target method
  [Register ("onCreate", "(Landroid/os/Bundle;)V", "GetOnCreate_Landroid_os_Bundle_Handler")]
  protected virtual unsafe void OnCreate (Android.OS.Bundle? savedInstanceState)
  {
    const string __id = "onCreate.(Landroid/os/Bundle;)V";
    try {
      JniArgumentValue* __args = stackalloc JniArgumentValue [1];
      __args [0] = new JniArgumentValue ((savedInstanceState == null) ? IntPtr.Zero : ((global::Java.Lang.Object) savedInstanceState).Handle);
      _members.InstanceMethods.InvokeVirtualVoidMethod (__id, this, __args);
    } finally {
      global::System.GC.KeepAlive (savedInstanceState);
    }
  }
}
```

##### Wrappers for methods with non-blittable types

The [`UnmanagedCallersOnly`](xref:System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute) attribute requires that all the argument types as well as the method return type are [blittable](/dotnet/framework/interop/blittable-and-non-blittable-types).

Among these types is one that's commonly used by the managed classes implementing Java methods: `bool`. This is currently the only non-blittable type encountered in bindings, and so is the only one supported by the assembly rewriter.

Whenever a method with a non-blittable type is encountered, a wrapper is generated for it, so that it can be decorated with the `UnmanagedCallersOnly` attribute. This is less error prone than modifying the native callback method's IL stream to implement the necessary conversion.

An example of such a method is `Android.Views.View.IOnTouchListener::OnTouch`:

```csharp
static bool n_OnTouch_Landroid_view_View_Landroid_view_MotionEvent_ (IntPtr jnienv, IntPtr native__this, IntPtr native_v, IntPtr native_e)
{
  var __this = global::Java.Lang.Object.GetObject<Android.Views.View.IOnTouchListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
  var v = global::Java.Lang.Object.GetObject<Android.Views.View> (native_v, JniHandleOwnership.DoNotTransfer);
  var e = global::Java.Lang.Object.GetObject<Android.Views.MotionEvent> (native_e, JniHandleOwnership.DoNotTransfer);
  bool __ret = __this.OnTouch (v, e);
  return __ret;
}
```

This method returns a `bool` value, and so needs a wrapper to cast the return value correctly. Each wrapper method retains the native callback method name, but appends the `_mm_wrapper` suffix to it:

```csharp
[UnmanagedCallersOnly]
static byte n_OnTouch_Landroid_view_View_Landroid_view_MotionEvent__mm_wrapper (IntPtr jnienv, IntPtr native__this, IntPtr native_v, IntPtr native_e)
{
  try {
    return n_OnTouch_Landroid_view_View_Landroid_view_MotionEvent_(jnienv, native__this, native_v, native_e) ? 1 : 0;
  } catch (Exception ex) {
    Android.Runtime.AndroidEnvironmentInternal.UnhandledException (ex);
    return default;
  }
}
```

The wrapper's return statement uses the ternary operator to cast the boolean value to 1 or 0 because the value of `bool` across the managed runtime can take a range of values:

- `0` for `false`
- `-1` or `1` for `true`
- `!= 0` for true

Because the `bool` type in C# can be 1, 2 or 4 bytes long, it's cast to a type of a known and static size. The `byte` managed type is used as it corresponds to the Java/JNI `jboolean` type, defined as an unsigned 8-bit type.

Whenever an argument value needs to be converted between `byte` and `bool`, code is generated that's equivalent to the `argument != 0` comparison. For example, for the `Android.Views.View.IOnFocusChangeListener::OnFocusChange` method:

```csharp
[UnmanagedCallersOnly]
static void n_OnFocusChange_Landroid_view_View_Z (IntPtr jnienv, IntPtr native__this, IntPtr native_v, byte hasFocus)
{
  n_OnFocusChange_Landroid_view_View_Z (jnienv, native__this, native_v, hasFocus != 0);
}

static void n_OnFocusChange_Landroid_view_View_Z (IntPtr jnienv, IntPtr native__this, IntPtr native_v, bool hasFocus)
{
  var __this = global::Java.Lang.Object.GetObject<Android.Views.View.IOnFocusChangeListener> (jnienv, native__this, JniHandleOwnership.DoNotTransfer)!;
  var v = global::Java.Lang.Object.GetObject<Android.Views.View> (native_v, JniHandleOwnership.DoNotTransfer);
  __this.OnFocusChange (v, hasFocus);
}
```

##### UnmanagedCallersOnly attribute

Each marshal methods native callback method is decorated with the [`UnmanagedCallersOnly`](xref:System.Runtime.InteropServices.UnmanagedCallersOnlyAttribute) attribute, to be able to invoke the callback directly from native code with minimal overhead compared.

### Marshal Methods Registration call sequence

The sequence described in [dynamic registration sequence](#dynamic-registration-call-sequence) is completely removed for the marshal methods approach. What's common for dynamic and marshal methods registration is the resolution of the native function target performed by the Java VM runtime. In both cases, the method declared in a Java class as `native` is looked up by the Java VM when first JIT-ing the code. The difference lies in the way this lookup is performed.

Dynamic registration uses the [`RegisterNatives`](https://docs.oracle.com/javase/8/docs/technotes/guides/jni/spec/functions.html#RegisterNatives) JNI function at runtime, which stores a pointer to the registered method inside the structure that describes a Java class in the Java VM.

Marshal methods, however, don't register anything with the JNI. Instead they rely on the symbol lookup mechanism of the Java VM. Whenever a call to a `native` Java method is JIT'd and isn't registered previously using the `RegisterNatives` JNI function, the Java VM will proceed to look for symbols in the process runtime image and, having found a matching symbol, use a pointer to it as the target of the `native` Java method call.
