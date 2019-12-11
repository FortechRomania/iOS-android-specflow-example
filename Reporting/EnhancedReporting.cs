using System.Collections.Generic;
using System.Linq;
using SpecNuts.Model;

namespace AndroidIosCucumberExampleNetcore.Reporting
{
    public class ScreenshotEmbedder
    {
        public static void EmbedScreenshotsForFailedScenarios(List<Feature> features, List<ScenarioScreenshotInfo> scenariosWithScreenshots)
        {
            features.ForEach(feature =>
            {
                feature.Elements.ForEach(scenario =>
                {
                    var scenarioWithScreenshot = scenariosWithScreenshots.Find(x => x.Title == scenario.Name);
                    if (scenarioWithScreenshot != null)
                    {
                        var firstFailedStep = scenario.Steps.First((step) => step.Result.Status == TestResult.failed);
                        firstFailedStep.AddEmbedding("image/png", scenarioWithScreenshot.Base64Data);
                    }
                });
            });
        }
    }

    public class ScenarioScreenshotInfo
    {
        public ScenarioScreenshotInfo(string title, string base64Data)
        {
            Title = title;
            Base64Data = base64Data;
        }

        public string Title { get; set; }

        public string Base64Data { get; set; }
    }
}
