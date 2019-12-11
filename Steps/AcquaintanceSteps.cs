using System;
using IosAndroidSpecflowExample.Helpers;
using IosAndroidSpecflowExample.Pages;
using FluentAssertions;
using SpecNuts;
using TechTalk.SpecFlow;

namespace IosAndroidSpecflowExample.Steps
{
    [Binding]
    public class AcquaintanceSteps : ReportingStepDefinitions
    {
        public AcquaintanceSteps(ScenarioState scenarioState)
        {
            ScenarioState = scenarioState;
        }

        public ScenarioState ScenarioState { get; set; }

        [StepDefinition(@"I tap on the add acquaintance button")]
        public void GivenITapOnTheAddAcquaintanceButton()
        {
            var acquaintancesPage = new AcquaintancePage();

            acquaintancesPage.WaitUntilPageLoads();
            acquaintancesPage.TapOnAddAcquaintanceButton();
        }

        [StepDefinition(@"I should see the new acquaintance in the list")]
        public void ThenIShouldSeeTheNewAcquaintanceInTheList()
        {
            var acquaintancePage = new AcquaintancePage();
            string formattedAcquaintanceName = $"{ScenarioState.LastName}, {ScenarioState.FirstName}";

            acquaintancePage.WaitUntilPageLoads();

            acquaintancePage.IsAcquaintanceWithNameDisplayed(formattedAcquaintanceName).Should().BeTrue();
        }

        [StepDefinition(@"I tap on the new acquaintance")]
        public void ITapOnTheNewAcquaintance()
        {
            var acquaintancePage = new AcquaintancePage();
            string formattedAcquaintanceName = $"{ScenarioState.LastName}, {ScenarioState.FirstName}";

            acquaintancePage.WaitUntilListIsNotEmpty();
            acquaintancePage.TapOnAcquaintance(formattedAcquaintanceName);
        }

        [StepDefinition(@"I should see that the acquaintance was deleted")]
        public void IShouldSeeThatTheAcquaintanceWasDeleted()
        {
            var acquaintancePage = new AcquaintancePage();
            string formattedAcquaintanceName = $"{ScenarioState.LastName}, {ScenarioState.FirstName}";

            acquaintancePage.WaitUntilListIsNotEmpty();
            acquaintancePage.IsAcquaintanceWithNameDisplayed(formattedAcquaintanceName).Should().BeFalse();
        }
    }
}
