#load "./cake_utils.cake"
#load "../build.cake"

public void CreateAndroidAvdIfNeeded(string apiLevel, string avdName)
{
    if (AvdExists(avdName))
    {
        Information($"{avdName} already exists. Won't recreate it.");

        return;
    }

    var package = $"system-images;android-{apiLevel};google_apis;x86";

    Information($"Installing {package}...");
    InstallAndroidSystemImage(package);

    ReplaceRegexInFiles("**/Scripts/config.ini", "AvdId=.*", $"AvdId={avdName}");
    ReplaceRegexInFiles("**/Scripts/config.ini", "avd.ini.displayname=.*", $"avd.ini.displayname={avdName}");
    ReplaceRegexInFiles("**/Scripts/config.ini", "android-[0-9]*", $"android-{apiLevel}");

    Information($"Creating {avdName}...");
    StartProcessWithException(AVD_MANAGER, $"--verbose create avd --name {avdName} --package \"{package}\" --force", "no");
    DeleteFile($"{AVD_HOME}/{avdName}.avd/config.ini");
    CopyFile($"{Project.AcceptanceTestsPath}/Scripts/config.ini", $"{AVD_HOME}/{avdName}.avd/config.ini");
}

public void InstallAndroidSystemImage(string androidSystemImage)
{
    StartProcessWithException(SDK_MANAGER, androidSystemImage, "y");
}

public bool AvdExists(string avdName)
{
    IEnumerable<string> redirectedStandardOutput;
    StartProcessWithOutput(AVD_MANAGER, "list avd", out redirectedStandardOutput);
    return redirectedStandardOutput.Any(line => line.Contains(avdName));
}

public (IProcess, string) LaunchAndroidEmulator(string fullAvdName)
{
    int port = GetAvailablePort();
    var process = StartAndReturnProcess(EMULATOR, new ProcessSettings { Arguments =  $"-port {port} -netdelay none -netspeed full -avd {fullAvdName}" });

    var deviceSerial = $"emulator-{port}";
    WaitUntilEmulatorStarts(deviceSerial);
    
    return (process, deviceSerial);
}

public void Adb(string command)
{
    StartProcessWithException("adb", command);
}

private void WaitUntilEmulatorStarts(string deviceSerial)
{
    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    while(true)
    {
        IEnumerable<string> redirectedStandardOutput;
        StartProcessWithOutput("adb", $"-s {deviceSerial} shell getprop sys.boot_completed", out redirectedStandardOutput);

        if (redirectedStandardOutput?.FirstOrDefault()?.Equals("1") == true) {
            Information("Emulator has booted.");

            return;
        } 
        else 
        {
            Information("Waiting for emulator to boot...");
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        if (stopwatch.Elapsed > TimeSpan.FromSeconds(240)) throw new TimeoutException("Emulator did not start in 240 seconds");
    }
}
public var versionNames = new Dictionary<string, string>
{
    ["21"] ="5.0", 
    ["22"] ="5.1", 
    ["23"] ="6.0", 
    ["24"] ="7.0",
    ["25"] ="7.1", 
    ["26"] ="8.0.0", 
    ["27"] ="8.1.0"
};