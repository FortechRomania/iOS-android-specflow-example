using System.Threading;
using System.Threading.Tasks;
using Just4Fun.ConsoleApp1.Helpers;
using Just4Fun.ConsoleApp1.Pages;
using BritishCarAuctions.DealerProApp.Api.IntegrationTests;
using FluentAssertions;
using SpecNuts;
using TechTalk.SpecFlow;
using static BritishCarAuctions.DealerProApp.Api.IntegrationTests.Constants.Users;

namespace Just4Fun.ConsoleApp1.Steps
{
    [Binding]
    public class VehicleSearchSteps : ReportingStepDefinitions
    {
        public VehicleSearchSteps(ScenarioState scenarioState)
        {
            ScenarioState = scenarioState;
        }

        private ScenarioState ScenarioState { get; set; }

        [StepDefinition(@"(?:Dealer Pro app is opened and |)Search Vehicle screen is displayed")]
        public void SearchVehicleScreenIsDisplayed()
        {
            var searchVehiclesPage = new VehicleSearchPage();

            searchVehiclesPage.WaitUntilSearchVehiclesFieldIsDisplayed();
        }

        [StepDefinition(@"The (.*) recently added vehicle is displayed")]
        public async Task RecentlyAddedVehiclesAre(string vehicleVRM)
        {
            var searchVehiclesPage = new VehicleSearchPage();

            var recentlyAddedVehicles = await ScenarioState.FetchRecentlyAddedVehicleListAsync();
            recentlyAddedVehicles.RecentVehicles.Should().NotBeEmpty();

            searchVehiclesPage.WaitUntilRecentlyAddedVehicleIsDisplayed();
            searchVehiclesPage.GetVRMForRecentlyAddedVehicleAtIndex(0).Should().Be(vehicleVRM);
        }

        [StepDefinition(@"I type (.*) in search vehicle field")]
        public void SetSearchField(string searchVehicle)
        {
            var searchVehiclesPage = new VehicleSearchPage();

            searchVehiclesPage.SetSearchField(searchVehicle);
        }

        [StepDefinition(@"Search Vehicle button is (enabled|disabled)")]
        public void SearchVehicleButtonIsEnabled(string status)
        {
            var searchVehiclesPage = new VehicleSearchPage();

            if (status == "enabled")
            {
                searchVehiclesPage.IsSearchVehiclesButtonEnabled().Should().BeTrue();
            }
            else
            {
                searchVehiclesPage.IsSearchVehiclesButtonEnabled().Should().BeFalse();
            }
        }

        [StepDefinition(@"I clear search vehicle field")]
        public void ClearSearchVehicleField()
        {
            var searchVehiclesPage = new VehicleSearchPage();

            searchVehiclesPage.ClearSearchVehicleField();
        }

        [StepDefinition(@"I search for (.*) VRM")]
        public void SearchVRM(string vrm)
        {
            FindVehicle(vrm);
        }

        [StepDefinition(@"I search for (.*) VRM and create it if doesn't exist")]
        public async Task SearchVRMAndCreateIfDoesNotExistAsync(string vrm)
        {
            await ScenarioState.DealerProApi.CreateVehicleIfDoesNotExists(vrm);

            FindVehicle(vrm);
        }

        [StepDefinition(@"An error message is displayed: '(.*)'")]
        public void DisplayedErrorMessage(string errorMessage)
        {
            var searchVehiclesPage = new VehicleSearchPage();

            searchVehiclesPage.GetVRMNotFoundErrorMessage().Should().Be(errorMessage);
        }

        [StepDefinition(@"The error message should not be displayed")]
        public void ErrorMessageShouldNotBeDisplayed()
        {
            var searchVehiclesPage = new VehicleSearchPage();

            searchVehiclesPage.WaitUntilVRMNotFoundErrorMessageIsNotDisplayed();
        }

        [StepDefinition(@"Search field should be pre-populate with the previous searched VRM")]
        public void SearchFieldShouldBePopulateWithPreviousSearchedVRM()
        {
            var searchVehiclesPage = new VehicleSearchPage();

            searchVehiclesPage.GetSearchVehicleText().Should().Be(ScenarioState.LastSearchedVrm);
        }

        [StepDefinition(@"I programmatically add a new vehicle with VRM (.*) using a valid user")]
        public async Task AddNewVehicleWithLoggedUserAsync(string vrm)
        {
            await ScenarioState.DealerProApi.CreateVehicle(vrm);
        }

        [StepDefinition(@"I should see the My Account icon displayed in the Search Vehicle screen")]
        public void IshouldSeeTheMyAccountButtonDisplayedInTheSearchVehicleScreen()
        {
            var searchVehiclePage = new VehicleSearchPage();

            searchVehiclePage.IsMyAccountButtonDisplayed.Should().BeTrue();
        }

        [StepDefinition(@"I tap on the Sync Menu icon")]
        public void ITapOnSyncMenu()
        {
            var searchVehiclePage = new VehicleSearchPage();

            searchVehiclePage.TapOnSyncMenuButton();
        }

        [StepDefinition(@"I tap on Sync complete icon")]
        public void ITapOnSyncCompleteIcon()
        {
            var searchVehiclePage = new VehicleSearchPage();

            searchVehiclePage.TapOnSyncCompleteMenuButton();
        }

        [StepDefinition(@"I tap on the My Account (?:icon|icon again)")]
        public void ITapOnTheMyAccountIcon()
        {
            var searchVehiclePage = new VehicleSearchPage();

            searchVehiclePage.TapOnMyAccountButton();
        }

        [StepDefinition(@"I verify if (.*) VRM exists or create it if it doesn't and programatically add a vehicle note")]
        public async Task IVerifyIfVrmExistsOrCreateItIfItDoesntAndProgramaticallyAddAVehicleNote(string vrm)
        {
            await ScenarioState.DealerProApi.VerifyVehicleExistanceAndUpdateItsNote(vrm, Constants.VehicleNote);
            ScenarioState.LastAddedNote = Constants.VehicleNote;
        }

        [StepDefinition(@"I select a vehicle from Recently Added section")]
        public void ISelectAVehicleFromRecentlyAdddedSection()
        {
            var searchVehiclePage = new VehicleSearchPage();

            searchVehiclePage.WaitUntilRecentlyAddedVehicleIsDisplayed();

            ScenarioState.LastSearchedVrm = searchVehiclePage.GetVRMForRecentlyAddedVehicleAtIndex(0);
            searchVehiclePage.TapOnRecentlyAddedVehicleAtIndex(0);
        }

        [StepDefinition(@"I delete the vehicle with VRM (.*) from the dealership using the Dealer Pro web site(?: if exists|)")]
        public async Task IDeleteTheVehicleVRMFromTheDealershipUsingDealerProWebsite(string vehicleVRM)
        {
            if (ScenarioState.Credentials == null)
            {
                ScenarioState.Credentials = AutomationUser1;
            }

            await ScenarioState.DealerProApi.DeleteVehicleIfExists(vehicleVRM);
        }

        [StepDefinition(@"I search for (.*) newly created vehicle(?:| having MOT status (.*))")]
        public async Task ISearchForANewlyCreatedVehicleYhOJO(string vehicleVRM, string motStatus)
        {
            await ScenarioState.DealerProApi.DeleteVehicleIfExists(vehicleVRM);
            await ScenarioState.DealerProApi.CreateVehicle(vehicleVRM, motStatus);
            FindVehicle(vehicleVRM);
        }

        private void FindVehicle(string vrm)
        {
            var searchVehiclesPage = new VehicleSearchPage();

            searchVehiclesPage.SetSearchField(vrm);
            ScenarioState.LastSearchedVrm = vrm;
            Thread.Sleep(2000);

            if (AppiumManager.Platform == PlatformEnum.IOS)
            {
                if (!searchVehiclesPage.IsSearchVehicleButtonDisplayed)
                {
                    AppiumManager.Driver.HideKeyboard();
                }
            }

            searchVehiclesPage.TapOnSearchVehicleButton();
        }
    }
}
