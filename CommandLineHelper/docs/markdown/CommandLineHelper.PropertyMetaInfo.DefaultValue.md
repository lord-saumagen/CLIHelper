﻿# PropertyMetaInfo.DefaultValue Property

> Namespace: [CommandLineHelper](_toc.CommandLineHelper.md#commandlinehelper-namespace)\
> Assembly: [CommandLineHelper](_toc.CommandLineHelper.md) (CommandLineHelper.dll) version 1.0.0.0

Returns the 'DefaultValueAttribute.Value' of the assigned 'DefaultValueAttribute'. If no 'DefaultValueAttribute' has been assigned to the parameter property the returned value is: 

null : for a nullable parameter property or reference parameter property.



The Type default value: for a none nullable value type parameter property.



## Syntax

```csharp
public object DefaultValue { get; }
```

## Property Value



## See Also

- [CommandLineHelper Namespace](_toc.CommandLineHelper.md#commandlinehelper-namespace)
- [PropertyMetaInfo Class](CommandLineHelper.PropertyMetaInfo.md)

---

_This document is generated by [DG](https://github.com/Khojasteh/dg)._