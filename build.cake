// Loads our list of addins from the tools folder
// this assumes using the new bootstrapper build.sh in the root folder
// which downloads the required files
#load "./utils/cake_utils.cake"
#load "./utils/utils.cake"
#load "./utils/ios_utils.cake"

using System.Threading;
using System.Diagnostics;

// GENERAL ARGUMENTS
var TARGET = Argument("target", "Build");
var CONFIGURATION = Argument("configuration", "Release");
var HOME = EnvironmentVariable("HOME");

var VSTS_USERNAME = Argument("vsts_username", EnvironmentVariable("VSTS_USERNAME"));
var VSTS_ACCESS_TOKEN = Argument("vsts_token", EnvironmentVariable("VSTS_ACCESS_TOKEN"));

// CONSTANTS
var NUGET_SOURCE_NAME = "practicePurposeOrg";
var NUGET_FEED_URL = "https://pkgs.dev.azure.com/practicePurposeOrg/_packaging/practicePurposeOrg/nuget/v3/index.json";

var MS_BUILD_LOG_FILE = $"{Environment.CurrentDirectory}/msbuild.log";

// IOS UI TESTS ARGUMENTS
var APP_PATH = Argument("app-path", "");
var SIMULATOR_NAME = Argument("sim-name", "iPad Air 2");
var IOS_PLATFORM_VERSION = Argument("ios-version", "12.2");

// PROJECT
public static class Project
{
    public static string Solution = "IosAndroidSpecflowExample.sln";
    public static string AcceptanceTestsPath = "./IosAndroidSpecflowExample.AcceptanceTests";
    public static string AcceptanceTests = $"{AcceptanceTestsPath}/IosAndroidSpecflowExample.csproj";
}

public static class ApplicationsInfo
{
    public static string iOSAppName = "AcquaintanceNativeiOS-shortlist.app";
}

Task("AddNugetSource")
.Does(() => {
    if (string.IsNullOrEmpty(VSTS_ACCESS_TOKEN) || string.IsNullOrEmpty(VSTS_USERNAME))
    {
        Information("Can't add NuGet source since no VSTS credentials were found");
        return;
    }

    if (!NuGetHasSource(NUGET_FEED_URL))
    {
        NuGetAddSource(NUGET_SOURCE_NAME, NUGET_FEED_URL, new NuGetSourcesSettings { UserName = VSTS_USERNAME, Password = VSTS_ACCESS_TOKEN });
    }
});

Task("Clean")
.Does(() => {
    var buildSettings = new MSBuildSettings()
     .WithTarget("Clean");
         
    MSBuild(Project.Solution, buildSettings);
});

Task("NuGetRestore")
.IsDependentOn("Clean")
.IsDependentOn("AddNugetSource")
.Does(() => {
    NuGetRestore(Project.Solution);
});

Task("Build")
.IsDependentOn("NuGetRestore")
.Does(() => {
    var buildSettings = new MSBuildSettings()
        .SetConfiguration("Debug")
        .WithLogger(
            Context.Tools.Resolve("MSBuild.ExtensionPack.Loggers.dll").FullPath,
            "XmlFileLogger",
            string.Format($"logfile=\"{MS_BUILD_LOG_FILE}\";verbosity=Detailed;encoding=UTF-8"));
    MSBuild(Project.Solution, buildSettings);
});

Task("iOSSimulatorAcceptanceTests")
.IsDependentOn("SetXamarinSdkVersionIfRunningOnVsts")
.IsDependentOn("NuGetRestore")
.Does(() => {
    DisableiOSKeyboardSettings(SIMULATOR_NAME, IOS_PLATFORM_VERSION);

    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/IosSettings.resx", "/root/data[@name='DevicePlatform']/value", IOS_PLATFORM_VERSION);
    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/IosSettings.resx", "/root/data[@name='DeviceIdentifier']/value", SIMULATOR_NAME);
    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/IosSettings.resx", "/root/data[@name='AppPath']/value", APP_PATH);

    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/GlobalSettings.resx", "/root/data[@name='Platform']/value", "iOS");
    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/GlobalSettings.resx", "/root/data[@name='ScreenType']/value", "IpadRegularScreen");

    MSBuild(Project.AcceptanceTests, new MSBuildSettings().SetConfiguration(CONFIGURATION));
});

Task("ReportBuildWarningsToVsts")
.Does(() => {
    var issuesSettings = new MsBuildIssuesSettings(MS_BUILD_LOG_FILE, MsBuildXmlFileLoggerFormat);
    var issues = ReadIssues(MsBuildIssues(issuesSettings), Environment.CurrentDirectory);

    foreach (var issue in issues)
    {
        Information($"##vso[task.logissue type=warning;sourcepath={issue.AffectedFileRelativePath};linenumber={issue.Line};code={issue.Rule};]{issue.MessageText}");
    }
});

Task("HealthCheck")
.IsDependentOn("SetXamarinSdkVersionIfRunningOnVsts")
.IsDependentOn("Build")
.IsDependentOn("ReportBuildWarningsToVsts");

Task("SetXamarinSdkVersionIfRunningOnVsts")
.Does(() => {
    var currentUser = EnvironmentVariable("USER");
    Information($"Current User is {currentUser}");

    if (currentUser == "runner")
    {
        StartProcessWithException("sh", "xamarin_sdk_version.sh");
    }
});

RunTarget(TARGET);