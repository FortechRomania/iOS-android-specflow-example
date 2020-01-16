using System;
using System.Net;
using System.Net.Sockets;
using IosAndroidSpecflowExample.IosAndroidSpecflowExample.AcceptanceTests.Settings;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;

namespace IosAndroidSpecflowExample.Helpers
{
    public class AppiumManager
    {
        private const int NewCommandTimeoutSeconds = 120;

        private static readonly TimeSpan AppiumDriverStartupTimeout = TimeSpan.FromSeconds(180);

        public static AppiumDriver<AppiumWebElement> Driver { get; private set; }

        public static PlatformEnum Platform { get; set; }

        public static bool IsOnIOS => Platform == PlatformEnum.IOS;

        public static bool IsOnAndroid => Platform == PlatformEnum.Android;

        public static string PlatformSpecificNewLine => IsOnIOS ? "\n" : "\r";

        public static void ResetApp()
        {
            if (!AppiumServer.IsServerRunning)
            {
                AppiumServer.StartServerIfShouldRunLocally();
            }

            if (Driver != null)
            {
                Driver.ResetApp();
            }
            else
            {
                CreateDriver();
            }
        }

        public static void CreateDriver()
        {
            var options = new AppiumOptions();

            options.AddAdditionalCapability(MobileCapabilityType.PlatformName, GlobalSettings.Platform);

            options.AddAdditionalCapability(MobileCapabilityType.NewCommandTimeout, NewCommandTimeoutSeconds);

            if (Platform == PlatformEnum.IOS)
            {
                options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, IosSettings.DevicePlatform);
                options.AddAdditionalCapability(MobileCapabilityType.DeviceName, IosSettings.DeviceIdentifier);
                options.AddAdditionalCapability(MobileCapabilityType.App, IosSettings.AppPath);

                if (!string.IsNullOrEmpty(IosSettings.UDID))
                {
                    options.AddAdditionalCapability(MobileCapabilityType.Udid, IosSettings.UDID);
                    options.AddAdditionalCapability("xcodeOrgId", IosSettings.XcodeOrgId);
                    options.AddAdditionalCapability("xcodeSigningId", IosSettings.XcodeSigningId);
                    options.AddAdditionalCapability("updatedWDABundleId", IosSettings.UpdatedWDABundleId);
                    options.AddAdditionalCapability("wdaLocalPort", GetAvailablePort());
                }

                Driver = new IOSDriver<AppiumWebElement>(AppiumServer.ServerUri, options, AppiumDriverStartupTimeout);
            }
            else if (Platform == PlatformEnum.Android)
            {
                options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, AndroidSettings.DevicePlatform);
                options.AddAdditionalCapability(MobileCapabilityType.DeviceName, AndroidSettings.DeviceIdentifier);
                options.AddAdditionalCapability(MobileCapabilityType.App, AndroidSettings.ApkPath);
                options.AddAdditionalCapability(MobileCapabilityType.AutomationName, "uiautomator2");
                options.AddAdditionalCapability(AndroidMobileCapabilityType.AppWaitActivity, "*");

                Driver = new AndroidDriver<AppiumWebElement>(AppiumServer.ServerUri, options, AppiumDriverStartupTimeout);
            }
        }

        public static void CloseDriver()
        {
            Helper.ExecuteSwallowingExceptions(() => Driver?.Quit());
        }

        private static int GetAvailablePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);

            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();

            return port;
        }
    }
}
