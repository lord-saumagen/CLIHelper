# CommandLineHelper Assembly (version 1.0.0.0)

The `CommandLineHelper` assembly exports types in the following namespaces.

## CommandLineHelper Namespace

The `CommandLineHelper` namespace exposes the following types.

### Classes

Class | Description
--- | ---
[DefaultValueAttribute](CommandLineHelper.DefaultValueAttribute.md) | The 'DefaultValueAttribute' attribute allows to set default values for the parameter properties the attribute is attached to. 
```csharp
[DefaultValue((Object) 42)]
public int NumberOfGuests
{
  get;
  set;
}
```
 The value of the command line argument 'DefaultValue' will be 42 even if the command line argument wasn't provided. The value in the 'DefaultValueAttribute' must be cast to an object and the base type must match with the type of the property the attribute is attached to.
[DescriptionAttribute](CommandLineHelper.DescriptionAttribute.md) | The 'DescriptionAttribute' attribute should be used to add a meaningful description to a parameter attribute and in consequence to the corresponding command line argument. The description should be shown on the command line after a help request. 
```csharp
[Description("The 'Email' argument value must be a valid e-mail address.")]
public string Email
{
  get;
  set;
}
```

[HelpAttribute](CommandLineHelper.HelpAttribute.md) | The 'HelpAttribute' attribute provides information which should be shown on a help request. 
```csharp
[Help(@"Write any information which will help a user to understand your
command line application. It can be anything from 'intended use',
'examples', 'common misconceptions' or any other helpful information
you are able to provide.")]
class ParameterExtended : ParameterBase
{
}
```

[InternalAttribute](CommandLineHelper.InternalAttribute.md) | The 'InternalAttribute' attribute is a marker attribute. Every public property with a public get and set accessor of the 'ParameterBase' class or it's derived classes is considered a parameter property. Those parameter properties are subjects of the parse and validation process. Properties which have the 'InternalAttribute' attribute attached will never be processed during the parse or validation process. 
```csharp
[Internal]
public int LoopCount
{
  get;
  set;
}
```

[MandatoryAttribute](CommandLineHelper.MandatoryAttribute.md) | The 'MandatoryAttribute' attribute is a marker attribute. Every parameter property which has a 'MandatoryAttribute' attribute attached will result in a mandatory command line argument. 
```csharp
[Mandatory]
public int LoopCount
{
  get;
  set;
}
```
 A mandatory parameter property will result in a command line argument which must be provided on the command line. Missing mandatory command line arguments will result in an error message.
[MaxStringLengthAttribute](CommandLineHelper.MaxStringLengthAttribute.md) | The 'MaxStringLengthAttribute' attribute constraints the input length of the parameter property it is attached to. If the parameter property fails validation because the string length exceeded the maximum length, an error message is created which shows the provided message. 
```csharp
[MaxStringLength(25, "The value of 'FavoriteTVShow' must not be longer than 25 characters.")]
public string FavoriteTVShow
{
  get;
  set;
}
```

[MaxValueAttribute](CommandLineHelper.MaxValueAttribute.md) | The 'MaxValueAttribute' attribute constraints the value of the parameter property it is attached to, to a maximum numeric value. If the parameter property fails validation because the value exceeds the maximum value, an error message is created which holds the provided message. 
```csharp
[MaxValue(10, "The value of 'SeatsPerCar' must be a value less or equal 10.")]
public int SeatsPerCar
{
  get;
  set;
}
```

[MinStringLengthAttribute](CommandLineHelper.MinStringLengthAttribute.md) | The 'MinStringLengthAttribute' attribute constraints the input length of the parameter property it is attached to. If the parameter property fails validation because the string length undercut the minimum length, an error message is created which holds the provided message. 
```csharp
[MinStringLength(5, "The value of 'FavoriteTVShow' must at least have a length of 5 characters.")]
public string FavoriteTVShow
{
  get;
  set;
}
```

[MinValueAttribute](CommandLineHelper.MinValueAttribute.md) | The 'MinValueAttribute' attribute constraints the value of the parameter property it is attached to, to a minimum numeric value. If the parameter property fails validation because the value undercuts the minimum value, an error message is created which holds the provided message. 
```csharp
[MinValue( 2, "The value of 'SeatsPerCar' must be a value greater or equal 2.")]
public int SeatsPerCar
{
  get;
  set;
}
```

[NameAttribute](CommandLineHelper.NameAttribute.md) | The 'NameAttribute' attribute allows to change the name of a parameter attribute for it's corresponding command line argument. This way it's possible to use easy to memorize argument names on the command line which otherwis would collide with your style guide naming rules. 
```csharp
[Name("email")]
public string RecipientEmailAddress
{
  get;
  set;
}
```
 The command line argument would be `email` and corresponding property name would be `RecipientEmailAddress`.
[ParameterAttributeBase](CommandLineHelper.ParameterAttributeBase.md) | The base class of all parameter attribute classes.
[UsageAttribute](CommandLineHelper.UsageAttribute.md) | The 'UsageAttribute' attribute provides the usage information for a given command. It should be assigned to the parameter property class. 
```csharp
[Usage(@"<CommandName> [number=<Int32 number>] name=<String>
The 'number' value must be in the range [0..100]. The default value is 0.
The 'name' must not be an empty string and the length must be less or equal 50.")]
class ParameterExtended : ParameterBase
{
}
```

[ValidationAttributeBase](CommandLineHelper.ValidationAttributeBase.md) | The base class of all validation attributes. In order to create your own validaton attribute do the following. Create a subclass which inherits from 'ValidationAttributeBase'. Overwrite the 'Validate' function to implement your own validation logic. 

See: [`Validate(PropertyMetaInfo, ParameterBase)`](CommandLineHelper.ValidationAttributeBase.Validate.md)


[ValueSetAttribute](CommandLineHelper.ValueSetAttribute.md) | The 'ValueSetAttribute' attribute constraints the input of the parameter property it is attached to. Only the values which are provided in the 'ValueSetAttribute' constructor will be considered valid parameter property values during validation. Make sure the 'ValueSet' holds only objects which are assignable to the target parameter property. If you intent to combine the 'ValueSetAttribute' and the 'DefaultValueAttribute' make sure the default value is one of the elements of the 'ValueSet'. 
```csharp
[ValueSet(new object[] {"Left", "Right", "Top", "Down"})]
public string NavigateDirection
{
  get;
  set;
}
```

[DisplayHelper](CommandLineHelper.DisplayHelper.md) | The 'DisplayHelper' class is a reference implementation of the 'IDisplayHelper' interface. 

The purpose of the 'DisplayHelper' class is to simplify the screen rendering. The class offers functions to create the output strings for the help screen, usage screen, the validation summary screen and the version screen.



[`IDisplayHelper`](CommandLineHelper.IDisplayHelper.md)


[ParameterBase](CommandLineHelper.ParameterBase.md) | 

The base class of all parameter classes. Implements the parse and validation function.

 

In order to use the parameter class in your own code, you have to create a subclass which inherits from 'ParameterBase' and add the parameter properties which are needed in your CLI application.


[PropertyMetaInfo](CommandLineHelper.PropertyMetaInfo.md) | The 'PropertyMetaInfo' class is a facade class which combines the parameter property 'PropertyInfo' with an interface to access the parse and validation results as well as the attached custom attributes of a parameter property.
[ValidationError](CommandLineHelper.ValidationError.md) | The 'ValidationError' class holds a reference to the 'PropertyMetaInfo' which failed validation as well as the value which couldn't be validated and the error message.

### Interfaces

Interface | Description
--- | ---
[IDisplayHelper](CommandLineHelper.IDisplayHelper.md) | The interface every 'DisplayHelper' class must implement in order to work with a 'ParameterProperty' class.

### Enumerations

Enumeration | Description
--- | ---
[ConsoleOutputStream](CommandLineHelper.ConsoleOutputStream.md) | Enumeration of console output streams
[ParseResultEnum](CommandLineHelper.ParseResultEnum.md) | This enumeration is used to describe the result of a parse operation.

---

_This document is generated by [DG](https://github.com/Khojasteh/dg)._
