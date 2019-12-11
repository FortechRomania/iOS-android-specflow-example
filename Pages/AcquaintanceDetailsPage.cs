using IosAndroidSpecflowExample.Helpers;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.PageObjects;
using OpenQA.Selenium.Appium.PageObjects.Attributes;
using SeleniumExtras.PageObjects;

namespace IosAndroidSpecflowExample.Pages
{
    public class AcquaintanceDetailsPage : BasePage
    {
        [FindsByAndroidUIAutomator(ID = "acquaintanceEditButton")]
        [FindsByIOSUIAutomation(ID = "Edit")]
        private IMobileElement<AppiumWebElement> _editAcquintanceButton;

        [FindsByAndroidUIAutomator(ID = "acquaintanceDeleteButton")]
        [FindsByIOSUIAutomation(ID = "Delete")]
        private IMobileElement<AppiumWebElement> _deleteAcquintanceButton;

        [FindsByAndroidUIAutomator(ID = "toolbar")]
        [FindsByIOSUIAutomation(ClassName = "XCUIElementTypeNavigationBar")]
        private IMobileElement<AppiumWebElement> _toolbar;

        [FindsByAndroidUIAutomator(ID = "Navigate up")]
        [FindsByIOSUIAutomation(ID = "List")]
        private IMobileElement<AppiumWebElement> _backToAcquaintanceListButton;

        public AcquaintanceDetailsPage()
        {
            PageFactory.InitElements(Driver, this, new AppiumPageObjectMemberDecorator());
        }

        public void TapOnEditAcquintanceButton() => _editAcquintanceButton.Click();

        public void TapOnDeleteAcquintanceButton() => _deleteAcquintanceButton.Click();

        public string GetToolbarTitle() => _toolbar.Text;

        public void WaitUntilPageLoads()
        {
            Helper.WaitFor(() => _editAcquintanceButton.Displayed);
        }

        public void NavigateBackToAcquaintanceList()
        {
            if (Platform == PlatformEnum.IOS)
            {
                _backToAcquaintanceListButton.Click();
            }
            else
            {
                Driver.Navigate().Back();
            }
        }
    }
}
