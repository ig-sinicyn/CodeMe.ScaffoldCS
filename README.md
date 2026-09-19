# CodeMe.ScaffoldCS
Scaffolding framework inspired by CodegenCS

# Template issues

Have a good scenarios:
* Last line behavior (last line is empty)
  * May be fixed if we keep endline in the current line and trim endline on close
    (last line behavior)
  * Will also fix inconsistency with delayed flush on MacOS ('\r' separator)
* Consider to split Template and advanced rendering logic. The goal is to have api that allows to add third-party rendering customisation
* Add api for column-level padding. Func to PaddedTemplatePart?
  * Need both options.
* Split template / text writer options

Waits for grooming:
* Value handling customizations. Interceptors?
  * We need a really good scenario for it. With naive implementation interceptors will slow down each write operation so lets keep it on hold.
* Review behaviors for `// {SummaryWithNewLineAtTheEnd} something` render scenario. Something like NextLineIndentation or EnforceIndentation() maybe?
* Shall we append indentation on whitespace-only lines?
  * Do we need "append indentation on empty lines" option?
* Add support for alignment arg for multiline values?
  * Need a good scenario for this. We have auto-indentation for this
* Multiline value nahdling.
  * Add option for Template to disable multiline indentation. Add .RenderLiteral() / .RenderMultiline()
  * Or just add .RenderLiteral() and no option

Delayed:
* Docs are missing
* No XML comments

# Metamodel issues