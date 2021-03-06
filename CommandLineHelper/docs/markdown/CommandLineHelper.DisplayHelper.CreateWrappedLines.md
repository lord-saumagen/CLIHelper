﻿# DisplayHelper.CreateWrappedLines(string, int, int, bool) Method

> Namespace: [CommandLineHelper](_toc.CommandLineHelper.md#commandlinehelper-namespace)\
> Assembly: [CommandLineHelper](_toc.CommandLineHelper.md) (CommandLineHelper.dll) version 1.0.0.0

Wraps the string provided in argument 'text' at least at the maximum line length provided in argument 'maxLineLength'. The function respects the line breaks which might already part of the text. The function returns the result as a list of strings.

## Syntax

```csharp
public static List<string> CreateWrappedLines(
    string text, 
    int maxLineLength, 
    int leadingSpaces = 1, 
    bool keepEmptyLines = false
)
```

### Parameters

`text`: [string](https://docs.microsoft.com/en-us/dotnet/api/system.string)\


`maxLineLength`: [int](https://docs.microsoft.com/en-us/dotnet/api/system.int32)\


`leadingSpaces`: [int](https://docs.microsoft.com/en-us/dotnet/api/system.int32)\


`keepEmptyLines`: [bool](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)\


### Return Value

[List\<string>](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1)\


## See Also

- [CommandLineHelper Namespace](_toc.CommandLineHelper.md#commandlinehelper-namespace)
- [DisplayHelper Class](CommandLineHelper.DisplayHelper.md)

---

_This document is generated by [DG](https://github.com/Khojasteh/dg)._
