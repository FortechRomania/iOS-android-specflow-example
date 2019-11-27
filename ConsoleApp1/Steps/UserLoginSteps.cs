using System;
using Just4Fun.ConsoleApp1.Pages;
using FluentAssertions;
using SpecNuts;
using TechTalk.SpecFlow;

namespace Just4Fun.ConsoleApp1.Steps
{
    [Binding]
    public class UserLoginSteps : ReportingStepDefinitions
    {
        [StepDefinition(@"I navigate back from User Login screen")]
        public void NavigateBackFromUserLoginScreen()
        {
            var userLoginPage = new UserLoginPage();

            userLoginPage.TapBackButton();
        }

        [StepDefinition(@"I type one charracter on pass field on Login User screen")]
        public void TypeOneCharacterOnPassField()
        {
            var userLoginPage = new UserLoginPage();

            userLoginPage.SetPassword("T");
        }

        [StepDefinition(@"Login button from Login User screen should be (active|inactive)")]
        public void LoginButtonShouldBeActive(string status)
        {
            var userLoginPage = new UserLoginPage();

            if (status == "active")
            {
                userLoginPage.IsLoginButtonEnabled().Should().BeTrue();
            }
            else
            {
                userLoginPage.IsLoginButtonEnabled().Should().BeFalse();
            }
        }

        [StepDefinition(@"I delete the typed character from password field")]
        public void DeleteTypeCharacterFromPass()
        {
            var userLoginPage = new UserLoginPage();

            userLoginPage.ClearPass();
        }

        [StepDefinition(@"I should be redirected to User Login screen")]
        public void IShouldBeRedirectedToUserLoginScreen()
        {
            var userLoginPage = new UserLoginPage();

            userLoginPage.IsLoginButtonEnabled();
        }
    }
}
