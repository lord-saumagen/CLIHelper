﻿# ValidationAttributeBase.Validate(PropertyMetaInfo, ParameterBase) Method

> Namespace: [CommandLineHelper](_toc.CommandLineHelper.md#commandlinehelper-namespace)\
> Assembly: [CommandLineHelper](_toc.CommandLineHelper.md) (CommandLineHelper.dll) version 1.0.0.0

This function must be implemented by every subclass. 

The function must set the 'propertyMetaInfo.ValidationError' property if the validation failed.



If the validation succeeded, the 'propertyMetaInfo.ValidationError' should be set to 'null'.



An example string length validation attribute implementation:


```csharp
public class MaxStringLengthAttribute : ValidationAttributeBase
{
  public int MaxLength
  {
    get;
    private set;
  }

  public MaxStringLengthAttribute(int maxLength, string validationErrorMessage) : base(validationErrorMessage)
  {
     this.MaxLength = maxLength;
     this.ValidationErrorMessage = validationErrorMessage;
  }
  
  //
  // Create an implementation of the abstact base function 'Validate'
  //
  public override void Validate(PropertyMetaInfo propertyMetaInfo, ParameterBase parameterObject)
  {
    MethodBase? methodBase = MethodBase.GetCurrentMethod();
    //
    // Make sure the 'ValidationAttribute' is assigned to the right 
    // property type. (A string in this case)
    //
    if(propertyMetaInfo.Type.ToLower().IndexOf("string") < 0)
    {
      propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString(), $"The attribute '{methodBase?.DeclaringType}' is not allowed on properties of type: '{propertyMetaInfo.Type}'.");
      return; 
    }

    //
    // The actual validation. Set the 'ValidationError' if the validation fails.
    //
    if(propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString()?.Length > this.MaxLength)
    {
      propertyMetaInfo.ValidationError = new ValidationError(propertyMetaInfo, propertyMetaInfo.PropertyInfo.GetValue(parameterObject)?.ToString(), $"{ValidationErrorMessage}");
      return; 
    }

    //
    // The validation passed. Clear the 'ValidationError'.
    //
    propertyMetaInfo.ValidationError = null;
  }

}// END class
```


## Syntax

```csharp
public abstract void Validate(PropertyMetaInfo propertyMetaInfo, ParameterBase parameterObject)
```

### Parameters

`propertyMetaInfo`: [PropertyMetaInfo](CommandLineHelper.PropertyMetaInfo.md)\


`parameterObject`: [ParameterBase](CommandLineHelper.ParameterBase.md)\


## See Also

- [CommandLineHelper Namespace](_toc.CommandLineHelper.md#commandlinehelper-namespace)
- [ValidationAttributeBase Class](CommandLineHelper.ValidationAttributeBase.md)

---

_This document is generated by [DG](https://github.com/Khojasteh/dg)._
