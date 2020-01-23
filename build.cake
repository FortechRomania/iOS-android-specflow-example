// Loads our list of addins from the utils folder
// this assumes using the new bootstrapper build.sh in the root folder
// which downloads the required files
#load "./utils/cake_utils.cake"
#load "./utils/utils.cake"
#load "./utils/ios_utils.cake"
#load "./utils/android_utils.cake"

using System.Threading;
using System.Diagnostics;

// GENERAL ARGUMENTS
var TARGET = Argument("target", "Build");
var CONFIGURATION = Argument("configuration", "Release");
var HOME = EnvironmentVariable("HOME");

// CONSTANTS
var MS_BUILD_LOG_FILE = $"{Environment.CurrentDirectory}/msbuild.log";

// ANDROID APK ARGUMENTS
var ANDROID_HOME = EnvironmentVariable("ANDROID_HOME");

// IOS UI TESTS ARGUMENTS
var APP_PATH = Argument("app-path", "");
var SIMULATOR_NAME = Argument("sim-name", "iPad Air 2");
var IOS_PLATFORM_VERSION = Argument("ios-version", "12.2");

// ANDROID UI TESTS ARGUMENTS   
var AVD_HOME = $"{HOME}/.android/avd";
var APK_PATH = Argument("apk-path", "");
var AVD_NAME = Argument("avd-name", "AppEmulator");
var API_LEVEL = Argument("emulator-api-level", "23");
var SDK_MANAGER = $"{ANDROID_HOME}/tools/bin/sdkmanager";
var AVD_MANAGER = $"{ANDROID_HOME}/tools/bin/avdmanager";
var EMULATOR = $"{ANDROID_HOME}/tools/emulator";

IProcess EMULATOR_PROCESS = null;

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

Task("Clean")
.Does(() => {
    var buildSettings = new MSBuildSettings()
     .WithTarget("Clean");
         
    MSBuild(Project.Solution, buildSettings);
});

Task("NuGetRestore")
.IsDependentOn("Clean")
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
.IsDependentOn("NuGetRestore")
.Does(() => {
    DisableiOSKeyboardSettings(SIMULATOR_NAME, IOS_PLATFORM_VERSION);

    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/IosSettings.resx", "/root/data[@name='DevicePlatform']/value", IOS_PLATFORM_VERSION);
    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/IosSettings.resx", "/root/data[@name='DeviceIdentifier']/value", SIMULATOR_NAME);
    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/IosSettings.resx", "/root/data[@name='AppPath']/value", APP_PATH);

    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/GlobalSettings.resx", "/root/data[@name='Platform']/value", "iOS");

    MSBuild(Project.AcceptanceTests, new MSBuildSettings().SetConfiguration(CONFIGURATION));
    DotNetCoreTest(Project.AcceptanceTests, new DotNetCoreTestSettings { NoBuild = true, Logger = "trx", Configuration = CONFIGURATION });
});

string DEVICE_SERIAL;
Task("AndroidEmulatorAcceptanceTests")
.IsDependentOn("Clean")
.IsDependentOn("NugetRestore")
.Does(() => {
    var fullAvdName = $"{AVD_NAME}_{API_LEVEL}";
    CreateAndroidAvdIfNeeded(API_LEVEL, fullAvdName);
    (EMULATOR_PROCESS, DEVICE_SERIAL) = LaunchAndroidEmulator(fullAvdName);

    Adb($"-s {DEVICE_SERIAL} shell settings put secure show_ime_with_hard_keyboard 0");

    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/AndroidSettings.resx", "/root/data[@name='DevicePlatform']/value", versionNames[API_LEVEL]);
    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/AndroidSettings.resx", "/root/data[@name='ApkPath']/value", APK_PATH);
    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/AndroidSettings.resx", "/root/data[@name='DeviceIdentifier']/value", fullAvdName);
    XmlPoke($"{Project.AcceptanceTestsPath}/Settings/GlobalSettings.resx", "/root/data[@name='Platform']/value", "Android");
    
    MSBuild(Project.AcceptanceTests, new MSBuildSettings().SetConfiguration(CONFIGURATION));
    DotNetCoreTest(Project.AcceptanceTests, new DotNetCoreTestSettings { NoBuild = true, Logger = "trx", Configuration = CONFIGURATION });

    Information("Emulator will be killed");
    Adb($"-s {DEVICE_SERIAL} emu kill");
    EMULATOR_PROCESS?.Kill();
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
.IsDependentOn("Build")
.IsDependentOn("ReportBuildWarningsToVsts");

RunTarget(TARGET);