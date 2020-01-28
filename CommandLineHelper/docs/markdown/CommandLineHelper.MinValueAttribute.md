﻿# MinValueAttribute Class

> Namespace: [CommandLineHelper](_toc.CommandLineHelper.md#commandlinehelper-namespace)\
> Assembly: [CommandLineHelper](_toc.CommandLineHelper.md) (CommandLineHelper.dll) version 1.0.0.0\
> Inheritance: [object](https://docs.microsoft.com/en-us/dotnet/api/system.object) `→` [Attribute](https://docs.microsoft.com/en-us/dotnet/api/system.attribute) `→` [ParameterAttributeBase](CommandLineHelper.ParameterAttributeBase.md) `→` [ValidationAttributeBase](CommandLineHelper.ValidationAttributeBase.md) `→` MinValueAttribute

The 'MinValueAttribute' attribute constraints the value of the parameter property it is attached to, to a minimum numeric value. If the parameter property fails validation because the value undercuts the minimum value, an error message is created which holds the provided message. 
```csharp
[MinValue( 2, "The value of 'SeatsPerCar' must be a value greater or equal 2.")]
public int SeatsPerCar
{
  get;
  set;
}
```


## Syntax

```csharp
public class MinValueAttribute : ValidationAttributeBase
```

## Constructors

Constructor | Description
--- | ---
[MinValueAttribute(object, string)](CommandLineHelper.MinValueAttribute.-ctor.md) | Constructor of the 'MinValueAttribute' class.

## Properties

Property | Description
--- | ---
[MinValue](CommandLineHelper.MinValueAttribute.MinValue.md) | The value constraint for the number.

## Methods

Method | Description
--- | ---
[Validate(PropertyMetaInfo, ParameterBase)](CommandLineHelper.MinValueAttribute.Validate.md) | Implementation of the abstract base function 'Validate'.

## See Also

- [CommandLineHelper Namespace](_toc.CommandLineHelper.md#commandlinehelper-namespace)

---

_This document is generated by [DG](https://github.com/Khojasteh/dg)._