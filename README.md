# CodeMe.ScaffoldCS
Scaffolding framework inspired by CodegenCS

# Template issues

Have a good scenarios:
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
* Raw literal?
  * Postproned as no use case
* Collection render options, preffix and suffix
* Collection render options, wrap on length
* Dictionary render?
    * Need good scenario, current render works fine

Delayed:
* Docs are missing
* No XML comments

# Metamodel issues

* Metadata accessibility modifiers
* Metadata default values
* Metadata arg comments (records)
* Implicit usings + using declarations for type aliases.
* ToString methods to ease metadata model append

# Scaffold API
* Last line behavior option
* Simplify template code. Less usings?
* CSharp render for DTO?
* Backport string helpers such as trim suffix / prefis, snake_case and so on

# Scaffold tool issues
* Something like dotnet tool, modules as templates
* Needs designdoc.