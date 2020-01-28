﻿# DisplayHelper Class

> Namespace: [CommandLineHelper](_toc.CommandLineHelper.md#commandlinehelper-namespace)\
> Assembly: [CommandLineHelper](_toc.CommandLineHelper.md) (CommandLineHelper.dll) version 1.0.0.0\
> Implements: [IDisplayHelper](CommandLineHelper.IDisplayHelper.md)\
> Inheritance: [object](https://docs.microsoft.com/en-us/dotnet/api/system.object) `→` DisplayHelper

The 'DisplayHelper' class is a reference implementation of the 'IDisplayHelper' interface. 

The purpose of the 'DisplayHelper' class is to simplify the screen rendering. The class offers functions to create the output strings for the help screen, usage screen, the validation summary screen and the version screen.



[`IDisplayHelper`](CommandLineHelper.IDisplayHelper.md)



## Syntax

```csharp
public class DisplayHelper : IDisplayHelper
```

## Constructors

Constructor | Description
--- | ---
[DisplayHelper()](CommandLineHelper.DisplayHelper.-ctor.md) | Initializes a new instance of [DisplayHelper](CommandLineHelper.DisplayHelper.md) class.

## Methods

Method | Description
--- | ---
[BreakTextAtLineBreak(string, bool, bool)](CommandLineHelper.DisplayHelper.BreakTextAtLineBreak.md) | The function breaks the text provided in argument 'text' at the line breaks found in that text and returns the result as a list of strings.
[CreateHelp(ParameterBase, int)](CommandLineHelper.DisplayHelper.CreateHelp.md) | Creates the help screen for the parameter object provided in argument 'parameterObject'. 

The line length of the resulting help text will match with value provide in argument 'screenWidth' as long a the value is greater than the minimum screen line length. Otherwise the minimum screen line length will be used.



The result string is supposed to be rendered on the command line.


[CreateUsage(ParameterBase, int)](CommandLineHelper.DisplayHelper.CreateUsage.md) | Creates a usage description for the command associated with the provided 'parameterObject'. 

The usage description is either the one provided with the 'UsageAttribute' or a generic description created from the known parameter attributes.



The result string is supposed to be rendered on the command line.


[CreateValidationSummary(ParameterBase, string, int)](CommandLineHelper.DisplayHelper.CreateValidationSummary.md) | Creates a summary of the validation errors of the parameter object provided in argument 'parameterObject'. 

The message provided in argument 'message' will be shown on top of the summary.



The summary will be returned as a string with a line length matching with the value of argument screenWidth.



The result string is supposed to be rendered on the command line.


[CreateVersion(Assembly)](CommandLineHelper.DisplayHelper.CreateVersion.md#createversionassembly) | Returns the version of the provided 'commandObjectAssembly' as string. 

The command object should be the assembly of the command line program who's version should be displayed.



The result string is supposed to be rendered on the command line.


[CreateVersion(object)](CommandLineHelper.DisplayHelper.CreateVersion.md#createversionobject) | Returns the version of the provided 'commandObject' as string. 

The command object should be the command line program who's version should be displayed.



The result string is supposed to be rendered on the command line.


[CreateWrappedLines(string, int, int, bool)](CommandLineHelper.DisplayHelper.CreateWrappedLines.md) | Wraps the string provided in argument 'text' at least at the maximum line length provided in argument 'maxLineLength'. The function respects the line breaks which might already part of the text. The function returns the result as a list of strings.

## See Also

- [CommandLineHelper Namespace](_toc.CommandLineHelper.md#commandlinehelper-namespace)

---

_This document is generated by [DG](https://github.com/Khojasteh/dg)._