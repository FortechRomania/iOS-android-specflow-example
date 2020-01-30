using IosAndroidSpecflowExample.Extensions;
using IosAndroidSpecflowExample.Helpers;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.PageObjects;
using OpenQA.Selenium.Appium.PageObjects.Attributes;
using SeleniumExtras.PageObjects;

namespace IosAndroidSpecflowExample.Pages
{
#pragma warning disable 0649
    public class AddAcquaintancePage : BasePage
    {
        [FindsByAndroidUIAutomator(ID = "firstNameField")]
        [FindsByIOSUIAutomation(ID = "_FirstNameField")]
        private IMobileElement<AppiumWebElement> _firstNameFieldElement;

        [FindsByAndroidUIAutomator(ID = "lastNameField")]
        [FindsByIOSUIAutomation(ID = "_LastNameField")]
        private IMobileElement<AppiumWebElement> _lastNameFieldElement;

        [FindsByAndroidUIAutomator(ID = "acquaintanceSaveButton")]
        [FindsByIOSUIAutomation(ID = "Save")]
        private IMobileElement<AppiumWebElement> _saveButtonElement;

        public AddAcquaintancePage()
        {
            PageFactory.InitElements(Driver, this, new AppiumPageObjectMemberDecorator());
        }

        public void FillInRequiredFields(string firstName, string lastName)
        {
            _firstNameFieldElement.Clear();
            _firstNameFieldElement.FastClick();
            _firstNameFieldElement.SendKeys(firstName);

            _lastNameFieldElement.Clear();
            _lastNameFieldElement.FastClick();
            _lastNameFieldElement.SendKeys(lastName);
        }

        public void TapOnSaveButton()
        {
            _saveButtonElement.FastClick();
        }

        public void WaitUntilPageLoads()
        {
            Helper.WaitFor(() => _firstNameFieldElement.Displayed);
        }
    }
}
