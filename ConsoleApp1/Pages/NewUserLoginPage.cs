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
    public class NewUserLoginPage : BasePage
    {
        [FindsByIOSUIAutomation(ID = "Log in using your BCA Dealer Pro Credentials")]
        private IMobileElement<AppiumWebElement> _newUserLoginScreenHeader;

        [FindsByIOSUIAutomation(ID = "Log in")]
        [FindsByWindowsAutomation(Accessibility = "LoginButton")]
        private IMobileElement<AppiumWebElement> _loginButton;

        [FindsByIOSUIAutomation(ID = "Username")]
        [FindsByWindowsAutomation(Accessibility = "Username")]
        private IMobileElement<AppiumWebElement> _userNameField;

        [FindsByIOSUIAutomation(ID = "Password")]
        [FindsByWindowsAutomation(Accessibility = "Password")]
        private IMobileElement<AppiumWebElement> _passField;

        [FindsByIOSUIAutomation(ID = "BackButton")]
        [FindsByWindowsAutomation(Accessibility = "BackButton")]
        private IMobileElement<AppiumWebElement> _backButton;

        [FindsByIOSUIAutomation(ID = "LoginErrorView")]
        [FindsByWindowsAutomation(Accessibility = "ErrorTextBlock")]
        private IMobileElement<AppiumWebElement> _loginErrorMessage;

        [FindsByIOSUIAutomation(ID = "ForgotPasswordButton")]
        [FindsByWindowsAutomation(Accessibility = "ForgotPasswordButton")]
        private IMobileElement<AppiumWebElement> _forgotPasswordButton;

        public NewUserLoginPage()
        {
            PageFactory.InitElements(Driver, this, new AppiumPageObjectMemberDecorator());
        }

        public void TapLoginButton()
        {
            _loginButton.Click();
        }

        public void SetUser(string value)
        {
            try
            {
                Helper.WaitFor(() => _userNameField.Displayed);
            }
            catch
            {
            }

            _userNameField.SendKeysWithFocus(value, Platform);
        }

        public void SetPassword(string value)
        {
            _passField.SendKeysWithFocus(value, Platform);
        }

        public void ClearUser()
        {
            _userNameField.Clear();
        }

        public void TapOnBackButton()
        {
            Helper.WaitFor(() => _backButton.Displayed);
            Thread.Sleep(2000);
            _backButton.Click();
        }

        public void TapOnForgotPasswordButton()
        {
            Helper.WaitFor(() => _forgotPasswordButton.Displayed);
            _forgotPasswordButton.Click();
        }

        public bool IsLoginButtonEnabled()
        {
            return _loginButton.Enabled;
        }

        public void WaitUntilNewUserLoginScreenIsDisplayed()
        {
            Helper.WaitFor(() => _newUserLoginScreenHeader.Displayed);
        }

        public void WaitUntilLoginErrorMessageIsDisplayed()
        {
            Helper.WaitFor(() => _loginErrorMessage.Displayed);
        }

        public string GetLoginErrorMessage()
        {
            return _loginErrorMessage.Text;
        }
    }
}