using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        private ScenarioState _scenarioState;

        private ScenarioContext _currentScenarioContext;

        public Hooks(ScenarioState scenarioState, ScenarioContext currentScenarioContext)
        {
            _scenarioState = scenarioState;
            _currentScenarioContext = currentScenarioContext;
        }

        private static List<ScenarioScreenshotInfo> ScenarioScreenshots { get; set; } = new List<ScenarioScreenshotInfo>();

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
        public async Task BeforeScenario()
        {
            TestLogger.CurrentTextWriter = TestContext.Out;

            AppiumManager.ResetApp();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (_currentScenarioContext.TestError != null)
            {
                try
                {
                    var screenshot = AppiumManager.Driver.GetScreenshot();
                    ScenarioScreenshots.Add(new ScenarioScreenshotInfo(_currentScenarioContext.ScenarioInfo.Title, screenshot.AsBase64EncodedString));

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
