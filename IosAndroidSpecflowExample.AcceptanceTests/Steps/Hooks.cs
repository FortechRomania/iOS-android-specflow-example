using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using IosAndroidSpecflowExample.Helpers;
using IosAndroidSpecflowExample.Reporting;
using IosAndroidSpecflowExample.Settings;
using NUnit.Framework;
using SpecNuts;
using SpecNuts.Json;
using TechTalk.SpecFlow;

namespace IosAndroidSpecflowExample.Steps
{
    [Binding]
    public class Hooks : ReportingStepDefinitions
    {
        public Hooks(ScenarioContext currentScenarioContext)
        {
            CurrentScenarioContext = currentScenarioContext;
        }

        private static List<ScenarioScreenshotInfo> ScenarioScreenshots { get; set; } = new List<ScenarioScreenshotInfo>();

        private ScenarioContext CurrentScenarioContext { get; set; }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Reporters.Add(new JsonReporter());

            Reporters.FinishedReport += (sender, args) =>
            {
                var reportsPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

                try
                {
                    ScreenshotEmbedder.EmbedScreenshotsForFailedScenarios(args.Reporter.Report.Features, ScenarioScreenshots);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                args.Reporter.WriteToFile($"{reportsPath}/TestReports.json");
            };

            AppiumManager.Platform = GlobalSettings.Platform == "iOS" ? PlatformEnum.IOS : PlatformEnum.Android;
            AppiumServer.OutputDataReceived += OnOutputDataReceived;
            AppiumServer.StartServerIfShouldRunLocally();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            AppiumManager.CloseDriver();
            AppiumServer.StopLocalService();
            AppiumServer.OutputDataReceived -= OnOutputDataReceived;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            TestLogger.CurrentTextWriter = TestContext.Out;

            AppiumManager.ResetApp();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (CurrentScenarioContext.TestError != null)
            {
                try
                {
                    var screenshot = AppiumManager.Driver.GetScreenshot();
                    ScenarioScreenshots.Add(new ScenarioScreenshotInfo(CurrentScenarioContext.ScenarioInfo.Title, screenshot.AsBase64EncodedString));

                    string screenshotPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, $"{TestContext.CurrentContext.Test.Name}.png");
                    File.WriteAllBytes(screenshotPath, screenshot.AsByteArray);

                    TestContext.AddTestAttachment(screenshotPath);
                }
                catch
                {
                    if (AppiumManager.Platform == PlatformEnum.IOS)
                    {
                        AppiumManager.CreateDriver();
                    }
                }
            }
        }

        private static void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            TestLogger.WriteLine(e.Data);
        }
    }
}
