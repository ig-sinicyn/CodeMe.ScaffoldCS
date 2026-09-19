# Summary

Render logic is based on c# [interpolated dtring handler](https://learn.microsoft.com/en-us/dotnet/csharp/advanced-topics/performance/interpolated-string-handler) feature.

Also, we use ambient template context (based on [ScopedAsyncLocal&lt;T&gt;](https://github.com/ig-sinicyn/CodeMe/tree/master/docs/Basics/AsyncLocal)) to ease passing of the current writer to the nested parts.

We do support conditional evaluation by IIF/IF-ELSE-ENDIF macro.

We do support conditional line trim by RL (RemoveLine macro)
