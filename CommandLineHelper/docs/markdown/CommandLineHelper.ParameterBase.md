# ParameterBase Class

> Namespace: [CommandLineHelper](_toc.CommandLineHelper.md#commandlinehelper-namespace)\
> Assembly: [CommandLineHelper](_toc.CommandLineHelper.md) (CommandLineHelper.dll) version 1.0.0.0\
> Inheritance: [object](https://docs.microsoft.com/en-us/dotnet/api/system.object) `→` ParameterBase



The base class of all parameter classes. Implements the parse and validation function.

 

In order to use the parameter class in your own code, you have to create a subclass which inherits from 'ParameterBase' and add the parameter properties which are needed in your CLI application.



## Syntax

```csharp
[Usage("A\x20description\x20of\x20the\x20command\x20usage\x20should\x20be\x20assigned\x20to\x20the\x20parameter\x20object.\r\nE.g.:\x20command\x20[help]\x20[version]\x20parameter=<parameterValue>\x20...")]
public class ParameterBase
```

## Constructors

Constructor | Description
--- | ---
[ParameterBase(string, Assembly, IDisplayHelper)](CommandLineHelper.ParameterBase.-ctor.md) | The constructor of the 'ParameterBase' class.

## Fields

Field | Description
--- | ---
[HelpIndicatorList](CommandLineHelper.ParameterBase.HelpIndicatorList.md) | The 'HelpIndicatorList' is a list of command line arguments which are considered a help request when detected during parse.
[VersionIndicatorList](CommandLineHelper.ParameterBase.VersionIndicatorList.md) | The 'VersionIndicatorList' is a list of command line arguments which are considered a version request when detected during parse.

## Properties

Property | Description
--- | ---
[Arguments](CommandLineHelper.ParameterBase.Arguments.md) | Returns a dictionary which holds the command line arguments from the last parse operation.
[CommandAssembly](CommandLineHelper.ParameterBase.CommandAssembly.md) | The assembly of the attached command. The 'CommandAssembly' is set in the constructor of the parameter class.
[CommandName](CommandLineHelper.ParameterBase.CommandName.md) | The command name as used on the command line interface. That might not necessarily be the same as your program name. The 'CommandName' is set in the constructor of the parameter class.
[IsHelpRequest](CommandLineHelper.ParameterBase.IsHelpRequest.md) | Returns true if a help request has been detected during parsing.
[IsParsed](CommandLineHelper.ParameterBase.IsParsed.md) | Returns true if a parse attempt has been made and either a help request or a version request has been detected or at least one property has been successfully parsed.
[IsValid](CommandLineHelper.ParameterBase.IsValid.md) | Returns true if all parameter properties passed the validation.
[IsVersionRequest](CommandLineHelper.ParameterBase.IsVersionRequest.md) | Returns true if a version request has been detected during parsing.
[PropertyMetaInfoList](CommandLineHelper.ParameterBase.PropertyMetaInfoList.md) | A list of 'PropertyMetaInfo' objects which hold additional information for each parameter property.
[ValidationErrorList](CommandLineHelper.ParameterBase.ValidationErrorList.md) | A read only collection of 'ValidationError' objects which shows the validation errors from the last validation process. The list might be empty.

## Methods

Method | Description
--- | ---
[CreateHelp(int)](CommandLineHelper.ParameterBase.CreateHelp.md) | If a 'DisplayHelper' was provided during construction, the help screen for this parameter object will be created. Otherwise an empty string will be returned.
[CreateUsage()](CommandLineHelper.ParameterBase.CreateUsage.md) | If a 'DisplayHelper' was provided during construction, a usage screen for this parameter object will be created. Otherwise an empty string will be returned.
[CreateValidationSummary(int)](CommandLineHelper.ParameterBase.CreateValidationSummary.md#createvalidationsummaryint) | If a 'DisplayHelper' was provided during construction, a validation summary screen for this parameter object will be created. Otherwise an empty string will be returned. 

The message provided in argument 'message' will be shown on top of the summary.



The summary will be returned as a string with a line length matching with the value of argument screenWidth.


[CreateValidationSummary(string, int)](CommandLineHelper.ParameterBase.CreateValidationSummary.md#createvalidationsummarystring-int) | If a 'DisplayHelper' was provided during construction, a validation summary screen for this parameter object will be created. Otherwise an empty string will be returned. 

The message provided in argument 'message' will be shown on top of the summary.



The summary will be returned as a string with a line length matching with the value of argument screenWidth.


[CreateVersion(Assembly)](CommandLineHelper.ParameterBase.CreateVersion.md#createversionassembly) | If a 'DisplayHelper' was provided during construction, a version screen for the provided 'commandAssembly' will be returned. Otherwise an empty string will be returned. 

The command object should be the command line program who's version should be displayed.


[CreateVersion(object)](CommandLineHelper.ParameterBase.CreateVersion.md#createversionobject) | If a 'DisplayHelper' was provided during construction, a version screen for the provided 'commandObject' will be returned. Otherwise an empty string will be returned. 

The command object should be the command line program who's version should be displayed.


[Parse(String[])](CommandLineHelper.ParameterBase.Parse.md) | Parses the arguments provided in argument 'args' and assigns the values of those arguments, which have corresponding parameter properties, to the properties. The parse function handles the parsing for the following types only:

Type | Description
--- | ---
Boolean & Boolean? | Parses the strings: true, false, yes, no. (Not case sensitive)
Char & Char? | Parses a single character
Number types & nullable number types | Parses number strings for the types: Int16, UInt16, Int32, UInt32, Int64, UInt64, Single, Double, Decimal and their nullable counterparts.
 | 

[PrintToConsole(string, ConsoleOutputStream, ConsoleColor, ConsoleColor)](CommandLineHelper.ParameterBase.PrintToConsole.md) | The function will write the provided 'output' to the specified console stream using the provided 'foregroundColor' and 'backgroundColor'. After printing the original console colors get restored. 

The default colors are:



The output stream to use. Default is the standard output.



The backgroundColor default is ConsoleColor.Black.



The foregroundColor default is ConsoleColor.White.


[Process(String[], bool)](CommandLineHelper.ParameterBase.Process.md) | Processes the command line by calling the parse and validation function. 

If the 'showUsageOnEmptyArgs' argument is true and the 'args' argument has no argument which qualifies as parameter argument, the usage screen will be rendered to the console error stream and the return value will be false;



If the validation fails because it was a help request, the help screen will be rendered to the console standard stream and the return value will be false.



If the validation fails because it was a version request, the version screen will be rendered to the console standard stream and the return value will be false.



If the validation fails because of invalid arguments, the validation summary screen will be rendered to the console error stream the return value will be false.



The function returns true if the process was successful and no screen was rendered.


[Validate()](CommandLineHelper.ParameterBase.Validate.md) | Validates all parmeter properties of the current 'ParameterBase' class and returns true if the validation for all properties succeeded, otherwise false.

## See Also

- [CommandLineHelper Namespace](_toc.CommandLineHelper.md#commandlinehelper-namespace)

---

_This document is generated by [DG](https://github.com/Khojasteh/dg)._
