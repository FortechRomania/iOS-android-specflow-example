using FluentAssertions;
using IosAndroidSpecflowExample.Helpers;
using IosAndroidSpecflowExample.Pages;
using SpecNuts;
using TechTalk.SpecFlow;

namespace IosAndroidSpecflowExample.Steps
{
    [Binding]
    public class AcquaintanceDetailsSteps : ReportingStepDefinitions
    {
        public AcquaintanceDetailsSteps(ScenarioState scenarioState)
        {
            ScenarioState = scenarioState;
        }

        public ScenarioState ScenarioState { get; set; }

        [StepDefinition(@"I tap on the delete acquaintance button and confirm my action")]
        public void LoginWithValidCredentialsFromNewUserScreen()
        {
            var acquaintanceDetailsPage = new AcquaintanceDetailsPage();
            var alertConfirmationPage = new AlertConfirmationPage();

            acquaintanceDetailsPage.WaitUntilPageLoads();
            acquaintanceDetailsPage.TapOnDeleteAcquintanceButton();
            alertConfirmationPage.TapOnConfirmButton();
        }

        [StepDefinition(@"I tap on the edit acquaintance button")]
        public void ITapOnTheEditAcquaintanceButton()
        {
            var acquaintanceDetailsPage = new AcquaintanceDetailsPage();

            acquaintanceDetailsPage.WaitUntilPageLoads();
            acquaintanceDetailsPage.TapOnEditAcquintanceButton();
        }

        [StepDefinition(@"I should see that the first name and last name changed")]
        public void IShouldSeeThatTheFirstNameAndLastNameChanged()
        {
            var acquaintanceDetailsPage = new AcquaintanceDetailsPage();
            var acquaintancePage = new AcquaintancePage();

            string formattedAcquaintanceName = $"{ScenarioState.LastName}, {ScenarioState.FirstName}";

            Helper.WaitFor(() => acquaintanceDetailsPage.GetToolbarTitle() == formattedAcquaintanceName);

            acquaintanceDetailsPage.NavigateBackToAcquaintanceList();

            acquaintancePage.WaitUntilListIsNotEmpty();
            acquaintancePage.IsAcquaintanceWithNameDisplayed(formattedAcquaintanceName).Should().BeTrue();
        }
    }
}
