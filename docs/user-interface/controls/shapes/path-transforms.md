---
title: "Path transforms"
description: "A .NET MAUI transform defines how to transform a Path object from one coordinate space to another coordinate space."
ms.date: 01/12/2022
---

# Path transforms

[![Browse sample.](~/media/code-sample.png) Browse the sample](/samples/dotnet/maui-samples/userinterface-shapes)

A .NET Multi-platform App UI (.NET MAUI) `Transform` defines how to transform a <xref:Microsoft.Maui.Controls.Shapes.Path> object from one coordinate space to another coordinate space. When a transform is applied to a <xref:Microsoft.Maui.Controls.Shapes.Path> object, it changes how the object is rendered in the UI.

Transforms can be categorized into four general classifications: rotation, scaling, skew, and translation. .NET MAUI defines a class for each of these transform classifications:

- `RotateTransform`, which rotates a <xref:Microsoft.Maui.Controls.Shapes.Path> by a specified `Angle`.
- `ScaleTransform`, which scales a <xref:Microsoft.Maui.Controls.Shapes.Path> object by specified `ScaleX` and `ScaleY` amounts.
- `SkewTransform`, which skews a <xref:Microsoft.Maui.Controls.Shapes.Path> object by specified `AngleX` and `AngleY` amounts.
- `TranslateTransform`, which moves a <xref:Microsoft.Maui.Controls.Shapes.Path> object by specified `X` and `Y` amounts.

.NET MAUI also provides the following classes for creating more complex transformations:

- `TransformGroup`, which represents a composite transform composed of multiple transform objects.
- `CompositeTransform`, which applies multiple transform operations to a <xref:Microsoft.Maui.Controls.Shapes.Path> object.
- `MatrixTransform`, which creates custom transforms that are not provided by the other transform classes.

All of these classes derive from the `Transform` class, which defines a `Value` property of type `Matrix`, which represents the current transformation as a `Matrix` object. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings, and styled. For more information about the `Matrix` struct, see [Transform matrix](#transform-matrix).

To apply a transform to a <xref:Microsoft.Maui.Controls.Shapes.Path>, you create a transform class and set it as the value of the `Path.RenderTransform` property.

## Rotation transform

A rotate transform rotates a <xref:Microsoft.Maui.Controls.Shapes.Path> object clockwise about a specified point in a 2D x-y coordinate system.

The `RotateTransform` class, which derives from the `Transform` class, defines the following properties:

- `Angle`, of type `double`, represents the angle, in degrees, of clockwise rotation. The default value of this property is 0.0.
- `CenterX`, of type `double`, represents the x-coordinate of the rotation center point. The default value of this property is 0.0.
- `CenterY`, of type `double`, represents the y-coordinate of the rotation center point. The default value of this property is 0.0.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The `CenterX` and `CenterY` properties specify the point about which the <xref:Microsoft.Maui.Controls.Shapes.Path> object is rotated. This center point is expressed in the coordinate space of the object that's transformed. By default, the rotation is applied to (0,0), which is the upper-left corner of the <xref:Microsoft.Maui.Controls.Shapes.Path> object.

The following example shows how to rotate a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Center"
      HeightRequest="100"
      WidthRequest="100"
      Data="M13.908992,16.207977L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983z">
    <Path.RenderTransform>
        <RotateTransform CenterX="0"
                         CenterY="0"
                         Angle="45" />
    </Path.RenderTransform>
</Path>
```

In this example, the <xref:Microsoft.Maui.Controls.Shapes.Path> object is rotated 45 degrees about its upper-left corner.

## Scale transform

A scale transform scales a <xref:Microsoft.Maui.Controls.Shapes.Path> object in the 2D x-y coordinate system.

The `ScaleTransform` class, which derives from the `Transform` class, defines the following properties:

- `ScaleX`, of type `double`, which represents the x-axis scale factor. The default value of this property is 1.0.
- `ScaleY`, of type `double`, which represents the y-axis scale factor. The default value of this property is 1.0.
- `CenterX`, of type `double`, which represents the x-coordinate of the center point of this transform. The default value of this property is 0.0.
- `CenterY`, of type `double`, which represents the y-coordinate of the center point of this transform. The default value of this property is 0.0.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

The value of `ScaleX` and `ScaleY` have a huge impact on the resulting scaling:

- Values between 0 and 1 decrease the width and height of the scaled object.
- Values greater than 1 increase the width and height of the scaled object.
- Values of 1 indicate that the object is not scaled.
- Negative values flip the scale object horizontally and vertically.
- Values between 0 and -1 flip the scale object and decrease its width and height.
- Values less than -1 flip the object and increase its width and height.
- Values of -1 flip the scaled object but do not change its horizontal or vertical size.

The `CenterX` and `CenterY` properties specify the point about which the <xref:Microsoft.Maui.Controls.Shapes.Path> object is scaled. This center point is expressed in the coordinate space of the object that's transformed. By default, scaling is applied to (0,0), which is the upper-left corner of the <xref:Microsoft.Maui.Controls.Shapes.Path> object. This has the effect of moving the <xref:Microsoft.Maui.Controls.Shapes.Path> object and making it appear larger, because when you apply a transform you change the coordinate space in which the <xref:Microsoft.Maui.Controls.Shapes.Path> object resides.

The following example shows how to scale a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Center"
      HeightRequest="100"
      WidthRequest="100"
      Data="M13.908992,16.207977L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983z">
    <Path.RenderTransform>
        <ScaleTransform CenterX="0"
                        CenterY="0"
                        ScaleX="1.5"
                        ScaleY="1.5" />
    </Path.RenderTransform>
</Path>
```

In this example, the <xref:Microsoft.Maui.Controls.Shapes.Path> object is scaled to 1.5 times the size.

## Skew transform

A skew transform skews a <xref:Microsoft.Maui.Controls.Shapes.Path> object in the 2D x-y coordinate system, and is useful for creating the illusion of 3D depth in a 2D object.

The `SkewTransform` class, which derives from the `Transform` class, defines the following properties:

- `AngleX`, of type `double`, which represents the x-axis skew angle, which is measured in degrees counterclockwise from the y-axis. The default value of this property is 0.0.
- `AngleY`, of type `double`, which represents the y-axis skew angle, which is measured in degrees counterclockwise from the x-axis. The default value of this property is 0.0.
- `CenterX`, of type `double`, which represents the x-coordinate of the transform center. The default value of this property is 0.0.
- `CenterY`, of type `double`, which represents the y-coordinate of the transform center. The default value of this property is 0.0.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

To predict the effect of a skew transformation, consider that `AngleX` skews x-axis values relative to the original coordinate system. Therefore, for an `AngleX` of 30, the y-axis rotates 30 degrees through the origin and skews the values in x by 30 degrees from that origin. Similarly, an `AngleY` of 30 skews the y values of the <xref:Microsoft.Maui.Controls.Shapes.Path> object by 30 degrees from the origin.

> [!NOTE]
> To skew a <xref:Microsoft.Maui.Controls.Shapes.Path> object in place, set the `CenterX` and `CenterY` properties to the object's center point.

The following example shows how to skew a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Center"
      HeightRequest="100"
      WidthRequest="100"
      Data="M13.908992,16.207977L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983z">
    <Path.RenderTransform>
        <SkewTransform CenterX="0"
                       CenterY="0"
                       AngleX="45"
                       AngleY="0" />
    </Path.RenderTransform>
</Path>
```

In this example, a horizontal skew of 45 degrees is applied to the <xref:Microsoft.Maui.Controls.Shapes.Path> object, from a center point of (0,0).

## Translate transform

A translate transform moves an object in the 2D x-y coordinate system.

The `TranslateTransform` class, which derives from the `Transform` class, defines the following properties:

- `X`, of type `double`, which represents the distance to move along the x-axis. The default value of this property is 0.0.
- `Y`, of type `double`, which represents the distance to move along the y-axis. The default value of this property is 0.0.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

Negative `X` values move an object to the left, while positive values move an object to the right. Negative `Y` values move an object up, while positive values move an object down.

The following example shows how to translate a <xref:Microsoft.Maui.Controls.Shapes.Path> object:

```xaml
<Path Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Center"
      HeightRequest="100"
      WidthRequest="100"
      Data="M13.908992,16.207977L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983z">
    <Path.RenderTransform>
        <TranslateTransform X="50"
                            Y="50" />
    </Path.RenderTransform>
</Path>
```

In this example, the <xref:Microsoft.Maui.Controls.Shapes.Path> object is moved 50 device-independent units to the right, and 50 device-independent units down.

## Multiple transforms

.NET MAUI has two classes that support applying multiple transforms to a <xref:Microsoft.Maui.Controls.Shapes.Path> object. These are `TransformGroup`, and `CompositeTransform`. A `TransformGroup` performs transforms in any desired order, while a `CompositeTransform` performs transforms in a specific order.

### Transform groups

Transform groups represent composite transforms composed of multiple `Transform` objects.

The `TransformGroup` class, which derives from the `Transform` class, defines a `Children` property, of type `TransformCollection`, which represents a collection of `Transform` objects. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings, and styled.

The order of transformations is important in a composite transform that uses the `TransformGroup` class. For example, if you first rotate, then scale, then translate, you get a different result than if you first translate, then rotate, then scale. One reason order is significant is that transforms like rotation and scaling are performed respect to the origin of the coordinate system. Scaling an object that is centered at the origin produces a different result to scaling an object that has been moved away from the origin. Similarly, rotating an object that is centered at the origin produces a different result than rotating an object that has been moved away from the origin.

The following example shows how to perform a composite transform using the `TransformGroup` class:

```xaml
<Path Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Center"
      HeightRequest="100"
      WidthRequest="100"
      Data="M13.908992,16.207977L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983z">
    <Path.RenderTransform>
        <TransformGroup>
            <ScaleTransform ScaleX="1.5"
                            ScaleY="1.5" />
            <RotateTransform Angle="45" />
        </TransformGroup>
    </Path.RenderTransform>
</Path>
```

In this example, the <xref:Microsoft.Maui.Controls.Shapes.Path> object is scaled to 1.5 times its size, and then rotated by 45 degrees.

## Composite transforms

A composite transform applies multiple transforms to an object.

The `CompositeTransform` class, which derives from the `Transform` class, defines the following properties:

- `CenterX`, of type `double`, which represents the x-coordinate of the center point of this transform. The default value of this property is 0.0.
- `CenterY`, of type `double`, which represents the y-coordinate of the center point of this transform. The default value of this property is 0.0.
- `ScaleX`, of type `double`, which represents the x-axis scale factor. The default value of this property is 1.0.
- `ScaleY`, of type `double`, which represents the y-axis scale factor. The default value of this property is 1.0.
- `SkewX`, of type `double`, which represents the x-axis skew angle, which is measured in degrees counterclockwise from the y-axis. The default value of this property is 0.0.
- `SkewY`, of type `double`, which represents the y-axis skew angle, which is measured in degrees counterclockwise from the x-axis. The default value of this property is 0.0.
- `Rotation`, of type `double`, represents the angle, in degrees, of clockwise rotation. The default value of this property is 0.0.
- `TranslateX`, of type `double`, which represents the distance to move along the x-axis. The default value of this property is 0.0.
- `TranslateY`, of type `double`, which represents the distance to move along the y-axis. The default value of this property is 0.0.

These properties are backed by <xref:Microsoft.Maui.Controls.BindableProperty> objects, which means that they can be targets of data bindings, and styled.

A `CompositeTransform` applies transforms in this order:

1. Scale (`ScaleX` and `ScaleY`).
1. Skew (`SkewX` and `SkewY`).
1. Rotate (`Rotation`).
1. Translate (`TranslateX`, `TranslateY`).

If you want to apply multiple transforms to an object in a different order, you should create a `TransformGroup` and insert the transforms in your intended order.

> [!IMPORTANT]
> A `CompositeTransform` uses the same center points, `CenterX` and `CenterY`, for all transformations. If you want to specify different center points per transform, use a `TransformGroup`,

The following example shows how to perform a composite transform using the `CompositeTransform` class:

```xaml
<Path Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Center"
      HeightRequest="100"
      WidthRequest="100"
      Data="M13.908992,16.207977L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983z">
    <Path.RenderTransform>
        <CompositeTransform ScaleX="1.5"
                            ScaleY="1.5"
                            Rotation="45"
                            TranslateX="50"
                            TranslateY="50" />
    </Path.RenderTransform>
</Path>
```

In this example, the <xref:Microsoft.Maui.Controls.Shapes.Path> object is scaled to 1.5 times its size, then rotated by 45 degrees, and then translated by 50 device-independent units.

## Transform matrix

A transform can be described in terms of a 3x3 affine transformation matrix, that performs transformations in 2D space. This 3x3 matrix is represented by the `Matrix` struct, which is a collection of three rows and three columns of `double` values.

The `Matrix` struct defines the following properties:

- `Determinant`, of type `double`, which gets the determinant of the matrix.
- `HasInverse`, of type `bool`, which indicates whether the matrix is invertible.
- `Identity`, of type `Matrix`, which gets an identity matrix.
- `HasIdentity`, of type `bool`, which indicates whether the matrix is an identity matrix.
- `M11`, of type `double`, which represents the value of the first row and first column of the matrix.
- `M12`, of type `double`, which represents the value of the first row and second column of the matrix.
- `M21`, of type `double`, which represents the value of the second row and first column of the matrix.
- `M22`, of type `double`, which represents the value of the second row and second column of the matrix.
- `OffsetX`, of type `double`, which represents the value of the third row and first column of the matrix.
- `OffsetY`, of type `double`, which represents the value of the third row and second column of the matrix.

The `OffsetX` and `OffsetY` properties are so named because they specify the amount to translate the coordinate space along the x-axis, and y-axis, respectively.

In addition, the `Matrix` struct exposes a series of methods that can be used to manipulate the matrix values, including `Append`, `Invert`, `Multiply`, `Prepend` and many more.

The following table shows the structure of a .NET MAUI matrix:

:::row:::
    :::column:::
        M11
    :::column-end:::
    :::column:::
        M12
    :::column-end:::
    :::column:::
        0.0
    :::column-end:::
:::row-end:::
:::row:::
    :::column:::
        M21
    :::column-end:::
    :::column:::
        M22
    :::column-end:::
    :::column:::
        0.0
    :::column-end:::
:::row-end:::
:::row:::
    :::column:::
        OffsetX
    :::column-end:::
    :::column:::
        OffsetY
    :::column-end:::
    :::column:::
        1.0
    :::column-end:::
:::row-end:::

> [!NOTE]
> An affine transformation matrix has its final column equal to (0,0,1), so only the members in the first two columns need to be specified.

By manipulating matrix values, you can rotate, scale, skew, and translate <xref:Microsoft.Maui.Controls.Shapes.Path> objects. For example, if you change the `OffsetX` value to 100, you can use it move a <xref:Microsoft.Maui.Controls.Shapes.Path> object 100 device-independent units along the x-axis. If you change the `M22` value to 3, you can use it to stretch a <xref:Microsoft.Maui.Controls.Shapes.Path> object to three times its current height. If you change both values, you move the <xref:Microsoft.Maui.Controls.Shapes.Path> object 100 device-independent units along the x-axis and stretch its height by a factor of 3. In addition, affine transformation matrices can be multiplied to form any number of linear transformations, such as rotation and skew, followed by translation.

## Custom transforms

The `MatrixTransform` class, which derives from the `Transform` class, defines a `Matrix` property, of type `Matrix`, which represents the matrix that defines the transformation. This property is backed by a <xref:Microsoft.Maui.Controls.BindableProperty> object, which means that it can be the target of data bindings, and styled.

Any transform that you can describe with a `TranslateTransform`, `ScaleTransform`, `RotateTransform`, or `SkewTransform` object can equally be described by a `MatrixTransform`. However, the `TranslateTransform`, `ScaleTransform`, `RotateTransform`, and `SkewTransform` classes are easier to conceptualize than setting the vector components in a `Matrix`. Therefore, the `MatrixTransform` class is typically used to create custom transformations that aren't provided by the `RotateTransform`, `ScaleTransform`, `SkewTransform`, or `TranslateTransform` classes.

The following example shows how to transform a <xref:Microsoft.Maui.Controls.Shapes.Path> object using a `MatrixTransform`:

```xaml
<Path Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Center"
      Data="M13.908992,16.207977L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983z">
    <Path.RenderTransform>
        <MatrixTransform>
            <MatrixTransform.Matrix>
                <!-- M11 stretches, M12 skews -->
                <Matrix OffsetX="10"
                        OffsetY="100"
                        M11="1.5"
                        M12="1" />
            </MatrixTransform.Matrix>
        </MatrixTransform>
    </Path.RenderTransform>
</Path>
```

In this example, the <xref:Microsoft.Maui.Controls.Shapes.Path> object is stretched, skewed, and offset in both the X and Y dimensions.

Alternatively, this can be written in a simplified form that uses a type converter that's built into .NET MAUI:

```xaml
<Path Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Center"
      Data="M13.908992,16.207977L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983z">
    <Path.RenderTransform>
        <MatrixTransform Matrix="1.5,1,0,1,10,100" />
    </Path.RenderTransform>
</Path>
```

In this example, the `Matrix` property is specified as a comma-delimited string consisting of six members: `M11`, `M12`, `M21`, `M22`, `OffsetX`, `OffsetY`. While the members are comma-delimited in this example, they can also be delimited by one or more spaces.

In addition, the previous example can be simplified even further by specifying the same six members as the value of the `RenderTransform` property:

```xaml
<Path Stroke="Black"
      Aspect="Uniform"
      HorizontalOptions="Center"
      RenderTransform="1.5 1 0 1 10 100"
      Data="M13.908992,16.207977L32.000049,16.207977 32.000049,31.999985 13.908992,30.109983z" />
```
