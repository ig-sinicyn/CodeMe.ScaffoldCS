namespace CodeMe.ScaffoldCS.Tool;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        args =
        [
            "AllowOverwrite=true",
            "ModelPath=AccountInfo.cs",
            "ModelBasePath=..\\..\\..\\..\\examples\\SampleApps.Desired\\Accounts\\",
            "OutputPath=..\\..\\..\\..\\examples\\SampleApps.Scaffolded\\Accounts\\"
        ];
        return await RunCommand.RunAsync(
            @"..\..\SampleTemplates\debug\SampleTemplates.dll",
            "Requests",
            args);
    }
}