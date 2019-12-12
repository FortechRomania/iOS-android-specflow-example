#load "./cake_utils.cake"
#load "../build.cake"

public void DisableiOSKeyboardSettings(string simulatorName, string iOSVersion)
{
    iOSVersion = iOSVersion.Replace(".", "-");

    StartProcessWithOutput("xcrun", $"simctl list devices \"{simulatorName}\" -j", out IEnumerable<string> output);
    var jsonOutput = string.Join(string.Empty, output);

    var devices = DeserializeJson<DevicesResponse>(jsonOutput).Devices
        .Where(keyValuePair => keyValuePair.Key.Contains(iOSVersion))
        .SelectMany(keyValuePair => keyValuePair.Value)
        .Where(device => device.IsAvailable);

    var KEYBOARD_SETTINGS = new List<String> {  "KeyboardAllowPaddle",
                                                "KeyboardAssistant",
                                                "KeyboardAutocapitalization",
                                                "KeyboardAutocorrection",
                                                "KeyboardCapsLock",
                                                "KeyboardCheckSpelling",
                                                "KeyboardPeriodShortcut",
                                                "KeyboardPrediction",
                                                "KeyboardShowPredictionBar" };

    devices.ToList().ForEach(device => {
        StartProcess("xcrun", $"simctl boot {device.UDID}");

        Thread.Sleep(TimeSpan.FromSeconds(30));

        var simulatorPreferencesPath = $"{HOME}/Library/Developer/CoreSimulator/Devices/{device.UDID}/data/Library/Preferences/com.apple.Preferences.plist";
        KEYBOARD_SETTINGS.ForEach(setting => {
            StartProcess("plutil", $"-replace {setting} -bool NO {simulatorPreferencesPath}");
        });

        StartProcess("xcrun", $"simctl shutdown {device.UDID}");
    });
}

private sealed class DevicesResponse
{
    public Dictionary<string, List<Device>> Devices { get; set; }
}
public sealed class Device
{
    public string State { get; set; }

    public bool IsAvailable { get; set; }

    public string UDID { get; set; }

    public string Name { get; set; }
    
    public string AvailabilityError { get; set; }
}