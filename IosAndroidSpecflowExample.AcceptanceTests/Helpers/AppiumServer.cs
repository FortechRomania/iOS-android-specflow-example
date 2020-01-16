using System;
using System.Diagnostics;
using IosAndroidSpecflowExample.IosAndroidSpecflowExample.AcceptanceTests.Settings;
using OpenQA.Selenium.Appium.Service;

namespace IosAndroidSpecflowExample.Helpers
{
    public class AppiumServer
    {
        private static AppiumLocalService _localService;

        public static event DataReceivedEventHandler OutputDataReceived;

        public static Uri ServerUri { get; set; }

        public static bool IsServerRunning => ServerUri != null;

        public static void StartServerIfShouldRunLocally()
        {
            if (string.IsNullOrEmpty(GlobalSettings.ServerUri))
            {
                _localService = new AppiumServiceBuilder()
                    .UsingAnyFreePort()
                    .WithArguments(new OpenQA.Selenium.Appium.Service.Options.OptionCollector().AddArguments(new System.Collections.Generic.KeyValuePair<string, string>("--log-no-colors", string.Empty)))
                    .Build();

                _localService.OutputDataReceived += OnOutputDataReceived;

                _localService.Start();

                ServerUri = _localService.ServiceUrl;
            }
            else
            {
                ServerUri = new Uri(GlobalSettings.ServerUri);
            }
        }

        public static void StopLocalService()
        {
            if (_localService != null && _localService.IsRunning)
            {
                _localService?.Dispose();
                _localService = null;
            }
        }

        private static void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            OutputDataReceived?.Invoke(sender, e);
        }
    }
}
