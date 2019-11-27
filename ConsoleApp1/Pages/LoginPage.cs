using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Just4Fun.ConsoleApp1.Extensions;
using Just4Fun.ConsoleApp1.Helpers;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.PageObjects;
using OpenQA.Selenium.Appium.PageObjects.Attributes;
using SeleniumExtras.PageObjects;

namespace Just4Fun.ConsoleApp1.Pages
{
#pragma warning disable 0649
    public class LoginPage : BasePage
    {
        private static readonly TimeSpan StartUpAnimationDuration = TimeSpan.FromSeconds(2);

        [FindsByIOSUIAutomation(ID = "+")]
        [FindsByWindowsAutomation(Accessibility = "NewUserTextBlock")]
        private IMobileElement<AppiumWebElement> _addNewUserButton;

        [FindsByIOSUIAutomation(ID = "UsernameLabel")]
        [FindsByWindowsAutomation(Accessibility = "ExistingUsernameTextBlock")]
        private IList<AppiumWebElement> _userNameList;

        [FindsByIOSUIAutomation(ID = "CloseButton")]
        [FindsByWindowsAutomation(Accessibility = "RemoveUserButton")]
        private IList<AppiumWebElement> _removeUserButton;

        public LoginPage()
        {
            PageFactory.InitElements(Driver, this, new AppiumPageObjectMemberDecorator());
        }

        public bool IsAddNewUserButtonDisplayed => _addNewUserButton.IsElementPresent();

        public bool IsUserAtIndexDisplayed(int index) => _userNameList[index].Displayed;

        public void WaitUntilStartUpAnimationEnds()
        {
            Thread.Sleep(StartUpAnimationDuration);
        }

        public void TapNewUser()
        {
            Helper.WaitFor(() => _addNewUserButton.Displayed);

            _addNewUserButton.Click();
        }

        public void TapOnUserNameAtIndex(int index)
        {
            Helper.WaitFor(_userNameList.Any);

            _userNameList[index].Click();
        }

        public void TapOnRemoveUserAtIndex(int index)
        {
            Helper.WaitFor(_removeUserButton.Any);

            _removeUserButton[index].Click();
        }

        public bool IsUserLoginListEmpty()
        {
            return _userNameList.Any();
        }

        public void WaitUntilAddNewUserButtonIsDisplayed()
        {
            Helper.WaitFor(() => IsAddNewUserButtonDisplayed);
        }

        public void WaitUntilAddNewUserButtonIsNotDisplayed()
        {
            Helper.WaitFor(() => !IsAddNewUserButtonDisplayed);
        }

        public string GetUserNameAtIndex(int index)
        {
            Helper.WaitFor(_userNameList.Any);

            return _userNameList[index].Text;
        }
    }
}