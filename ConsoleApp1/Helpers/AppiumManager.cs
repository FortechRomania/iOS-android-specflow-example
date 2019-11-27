using System;
using System.Net;
using System.Net.Sockets;
using Just4Fun.ConsoleApp1.Settings;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Windows;

namespace Just4Fun.ConsoleApp1.Helpers
{
    public class AppiumManager
    {
        private const int NewCommandTimeoutSeconds = 120;

        private static readonly TimeSpan AppiumDriverStartupTimeout = TimeSpan.FromSeconds(180);

        public static AppiumDriver<AppiumWebElement> Driver { get; private set; }

        public static WindowsDriver<WindowsElement> WindowsDesktopDriver { get; private set; }

        public static PlatformEnum Platform { get; set; }

        public static bool IsOnIOS => Platform == PlatformEnum.IOS;

        public static bool IsOnUWP => Platform == PlatformEnum.UWP;

        public static string PlatformSpecificNewLine => IsOnIOS ? "\n" : "\r";

        public static void ResetApp()
        {
            if (!AppiumServer.IsServerRunning)
            {
                AppiumServer.StartServerIfShouldRunLocally();
            }

            if (IsOnIOS)
            {
                if (Driver != null)
                {
                    Driver.ResetApp();
                }
                else
                {
                    CreateDriver();
                }
            }
            else
            {
                CloseDriver();

                //PowerShellHelper.InvokeUWPInstallScript();

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
            else
            {
                options.AddAdditionalCapability(MobileCapabilityType.App, WindowsSettings.AppId);
                options.AddAdditionalCapability(MobileCapabilityType.DeviceName, "WindowsPC");

                Driver = new WindowsDriver<AppiumWebElement>(AppiumServer.ServerUri, options, AppiumDriverStartupTimeout);
            }
        }

        public static void CloseDriver()
        {
            Helper.ExecuteSwallowingExceptions(() => Driver?.Quit());
        }

        public static void StartWindowsDesktopDriver()
        {
            var newCommandTimeout = 600;

            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", "Root");
            options.AddAdditionalCapability(MobileCapabilityType.DeviceName, "WindowsPC");
            options.AddAdditionalCapability(MobileCapabilityType.PlatformName, GlobalSettings.Platform);
            options.AddAdditionalCapability(MobileCapabilityType.NewCommandTimeout, newCommandTimeout);

            WindowsDesktopDriver = new WindowsDriver<WindowsElement>(AppiumServer.ServerUri, options, AppiumDriverStartupTimeout);
        }

        public static void CloseWindowsDesktopDriver()
        {
            WindowsDesktopDriver.Quit();
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
