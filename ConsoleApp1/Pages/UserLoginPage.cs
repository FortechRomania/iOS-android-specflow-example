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
    public class UserLoginPage : BasePage
    {
        [FindsByIOSUIAutomation(ID = "Log in")]
        [FindsByWindowsAutomation(Accessibility = "LoginButton")]
        private IMobileElement<AppiumWebElement> _loginButton;

        [FindsByIOSUIAutomation(ID = "Password")]
        [FindsByWindowsAutomation(Accessibility = "Password")]
        private IMobileElement<AppiumWebElement> _passField;

        [FindsByIOSUIAutomation(ID = "backArrowImage")]
        [FindsByWindowsAutomation(Accessibility = "BackButton")]
        private IMobileElement<AppiumWebElement> _backArrow;

        public UserLoginPage()
        {
            PageFactory.InitElements(Driver, this, new AppiumPageObjectMemberDecorator());
        }

        public void SetPassword(string value)
        {
            Helper.WaitFor(() => _passField.Displayed);

            _passField.SendKeysWithFocus(value, Platform);
        }

        public void TapLoginButton()
        {
            _loginButton.Click();
        }

        public void TapBackButton()
        {
            Helper.WaitFor(() => _backArrow.Displayed);

            _backArrow.Click();
        }

        public bool IsLoginButtonEnabled()
        {
            return _loginButton.Enabled;
        }

        public void ClearPass()
        {
            _passField.Clear();
        }
    }
}
