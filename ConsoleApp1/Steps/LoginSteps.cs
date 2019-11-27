using BritishCarAuctions.DealerProApp.Api.IntegrationTests;
using Just4Fun.ConsoleApp1.Helpers;
using Just4Fun.ConsoleApp1.Pages;
using FluentAssertions;
using SpecNuts;
using TechTalk.SpecFlow;
using static BritishCarAuctions.DealerProApp.Api.IntegrationTests.Constants;

namespace BritishCarAuctions.DealerProApp.AcceptanceTests.Steps
{
    [Binding]
    public class LoginSteps : ReportingStepDefinitions
    {
        private ScenarioState _scenarioState;

        public LoginSteps(ScenarioState scenarioState)
        {
            _scenarioState = scenarioState;
        }

        [StepDefinition(@"I login with valid credentials from New User Login screen")]
        public void LoginWithValidCredentialsFromNewUserScreen()
        {
            LoginNewUser(Constants.Users.AutomationUser1);
        }

        [StepDefinition(@"I login with previously used user from New User Login screen")]
        public void LoginWithPreviouslyUsedUserFromNewUserScreen()
        {
            LoginNewUser(_scenarioState.Credentials);
        }

        [StepDefinition(@"I reach the New User Login screen")]
        public void ReachLoginNewUserScreen()
        {
            var loginPage = new LoginPage();

            loginPage.WaitUntilStartUpAnimationEnds();
            loginPage.TapNewUser();
        }

        [StepDefinition(@"I should be redirected to Login screen")]
        public void IShouldBeRedirectedToLoginscreen()
        {
            var loginPage = new LoginPage();

            loginPage.WaitUntilAddNewUserButtonIsDisplayed();
        }

        [StepDefinition(@"I have placed (.*) users in Multi Login screen")]
        public void PlaceUsersInMultiLoginScreen(int numberOfUsers)
        {
            var vehicleSearchPage = new VehicleSearchPage();
            var newUserLoginPage = new NewUserLoginPage();
            //var myAccountMenuPage = new MyAccountMenuPage();
            int countUsers = 0;

            foreach (Credentials credential in Users.AllAutomationUsersList)
            {
                LoginNewUser(credential);
                vehicleSearchPage.TapOnMyAccountButton();
                //myAccountMenuPage.TapOnLogOutButton();
                newUserLoginPage.TapOnBackButton();

                countUsers++;

                if (countUsers == numberOfUsers)
                {
                    break;
                }
            }
        }

        [StepDefinition(@"Login with User option is displayed")]
        public void LoginWithUserOptionIsDisplayed()
        {
            var loginPage = new LoginPage();

            loginPage.IsUserLoginListEmpty();
        }

        [StepDefinition(@"(?:Dealer Pro app is opened and |)Login with New User option is displayed")]
        public void LoginWithNewUserOptionIsDisplayed()
        {
            var loginPage = new LoginPage();

            loginPage.WaitUntilAddNewUserButtonIsDisplayed();
        }

        [StepDefinition(@"I login with User option")]
        public void LoginWithUserOption()
        {
            var loginPage = new LoginPage();
            var userLoginPage = new UserLoginPage();

            _scenarioState.Credentials = Users.AutomationUser2;

            const int FirstUser = 0;

            loginPage.TapOnUserNameAtIndex(FirstUser);
            userLoginPage.SetPassword(Users.AutomationUser1.Password);
            userLoginPage.TapLoginButton();
        }

        [StepDefinition(@"I remove the user from Multi Login screen")]
        public void RemoveUserFromMultiLoginScreen()
        {
            var loginPage = new LoginPage();
            const int FirstUser = 0;

            loginPage.TapOnRemoveUserAtIndex(FirstUser);
        }

        [StepDefinition(@"User will be removed from Multi Login screen")]
        public void UserWillBeRemovedFromMultiLoginScreen()
        {
            var loginPage = new LoginPage();

            loginPage.IsUserLoginListEmpty().Should().BeFalse();
        }

        [StepDefinition(@"I reach the Login User screen")]
        public void IReachTheLoginUserScreen()
        {
            var loginPage = new LoginPage();
            const int FirstUser = 0;

            loginPage.TapOnUserNameAtIndex(FirstUser);
        }

        [StepDefinition(@"I login with the second user")]
        public void LoginWithTheSecondUser()
        {
            var loginPage = new LoginPage();
            var userLoginPage = new UserLoginPage();
            var vehicleSearchPage = new VehicleSearchPage();
            //var myAccountMenuPage = new MyAccountMenuPage();

            _scenarioState.Credentials = Users.AutomationUser2;

            const int SecondUser = 1;

            loginPage.TapOnUserNameAtIndex(SecondUser);
            userLoginPage.SetPassword(Users.AutomationUser2.Password);
            userLoginPage.TapLoginButton();
            vehicleSearchPage.TapOnMyAccountButton();
            //myAccountMenuPage.TapOnLogOutButton();
            userLoginPage.TapBackButton();
        }

        [StepDefinition(@"the second user will be the first in User Login list")]
        public void TheSecondUserWillBeTheFirstInUserLoginList()
        {
            var loginPage = new LoginPage();

            loginPage.GetUserNameAtIndex(0).Should().Be(Users.AutomationUser2.Account);
        }

        [StepDefinition(@"I horizontally scroll the user list")]
        public void HorizontallyScrollTheUserList()
        {
            var loginPage = new LoginPage();

            loginPage.WaitUntilAddNewUserButtonIsDisplayed();
            Helper.ScrollLeftHorizontal();
        }

        [StepDefinition(@"the list should be scrollable")]
        public void ListShouldBeScrollable()
        {
            var loginPage = new LoginPage();

            Helper.ScrollLeftHorizontalUntilCondition(() => loginPage.IsUserAtIndexDisplayed(6));
        }

        private void LoginNewUser(Credentials credentials)
        {
            var loginPage = new LoginPage();
            var loginNewUserPage = new NewUserLoginPage();

            _scenarioState.Credentials = credentials;

            loginPage.WaitUntilStartUpAnimationEnds();

            loginPage.TapNewUser();
            loginNewUserPage.SetUser(credentials.Account);
            loginNewUserPage.SetPassword(credentials.Password);

            loginNewUserPage.TapLoginButton();
        }
    }
}
