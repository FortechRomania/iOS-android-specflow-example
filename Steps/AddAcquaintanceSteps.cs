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

        public ScenarioState ScenarioState { get; set; }

        [StepDefinition(@"I fill in the required fields and tap on the Save button")]
        public void IFillInTheRequiredFieldsAndTapOnTheSaveButton()
        {
            FillInRequiredFieldsAndTapSave(ScenarioState, Constants.FirstName, Constants.LastName);
        }

        [StepDefinition(@"I set the first name to: '(.*)' and last name to: '(.*)' and tap the Save button")]
        public void ISetTheFirstNameToAndLastNameToAndTapTheSaveButton(string firstName, string lastName)
        {
            FillInRequiredFieldsAndTapSave(ScenarioState, firstName, lastName);
        }

        private void FillInRequiredFieldsAndTapSave(ScenarioState scenarioState, string firstName, string lastName)
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
