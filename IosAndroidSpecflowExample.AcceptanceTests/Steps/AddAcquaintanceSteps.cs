using IosAndroidSpecflowExample.Helpers;
using IosAndroidSpecflowExample.Pages;
using SpecNuts;
using TechTalk.SpecFlow;

namespace IosAndroidSpecflowExample.Steps
{
    [Binding]
    public class AddAcquaintanceSteps : ReportingStepDefinitions
    {
        public AddAcquaintanceSteps(ScenarioState scenarioState)
        {
            ScenarioState = scenarioState;
        }

        private ScenarioState ScenarioState { get; set; }

        [StepDefinition(@"I fill in the required fields and tap on the Save button")]
        public void IFillInTheRequiredFieldsAndTapOnTheSaveButton()
        {
            FillInRequiredFieldsAndTapSave(Constants.FirstName, Constants.LastName);
        }

        [StepDefinition(@"I set the first name to: '(.*)' and last name to: '(.*)' and tap the Save button")]
        public void ISetTheFirstNameToAndLastNameToAndTapTheSaveButton(string firstName, string lastName)
        {
            FillInRequiredFieldsAndTapSave(firstName, lastName);
        }

        private void FillInRequiredFieldsAndTapSave(string firstName, string lastName)
        {
            var addAcquaintancePage = new AddAcquaintancePage();

            ScenarioState.FirstName = firstName;
            ScenarioState.LastName = lastName;

            addAcquaintancePage.WaitUntilPageLoads();
            addAcquaintancePage.FillInRequiredFields(firstName, lastName);
            addAcquaintancePage.TapOnSaveButton();
        }
    }
}
