using System;
using AndroidIosCucumberExampleNetcore.Helpers;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.PageObjects;
using OpenQA.Selenium.Appium.PageObjects.Attributes;
using SeleniumExtras.PageObjects;

namespace AndroidIosCucumberExampleNetcore.Pages
{
#pragma warning disable 0649
    public class AlertConfirmationPage : BasePage
    {
        [FindsByAndroidUIAutomator(ID = "android:id/button1")]
        private IMobileElement<AppiumWebElement> _deleteButton;

        [FindsByIOSUIAutomation(ID = "Delete?")]
        private IMobileElement<AppiumWebElement> _alertConfirmationModal;

        public AlertConfirmationPage()
        {
            PageFactory.InitElements(Driver, this, new AppiumPageObjectMemberDecorator());
        }

        public void TapOnConfirmButton()
        {
            if (Platform == PlatformEnum.IOS)
            {
                Helper.WaitFor(() => _alertConfirmationModal.FindElementByAccessibilityId("Delete").Displayed);

                _alertConfirmationModal.FindElementByAccessibilityId("Delete").Click();
            }
            else
            {
                Helper.WaitFor(() => _deleteButton.Displayed);

                _deleteButton.Click();
            }
        }
    }
}
