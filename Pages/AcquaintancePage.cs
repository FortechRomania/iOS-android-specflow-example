using System.Collections.Generic;
using AndroidIosCucumberExampleNetcore.Helpers;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.PageObjects;
using OpenQA.Selenium.Appium.PageObjects.Attributes;
using SeleniumExtras.PageObjects;

namespace AndroidIosCucumberExampleNetcore.Pages
{
    public class AcquaintancePage : BasePage
    {
        [FindsByAndroidUIAutomator(ID = "acquaintanceListFloatingActionButton")]
        [FindsByIOSUIAutomation(ID = "Add")]
        private IMobileElement<AppiumWebElement> _addAcquaintanceButton;

        [FindsByAndroidUIAutomator(ID = "nameTextView")]
        [FindsByIOSUIAutomation(ID = "NameLabel")]
        private IList<AppiumWebElement> _iosAcquaintances;

        public AcquaintancePage()
        {
            PageFactory.InitElements(Driver, this, new AppiumPageObjectMemberDecorator());
        }

        public string GetNameOfAcquaintanceAtIndex(int index) => _iosAcquaintances[index].Text;

        public void TapOnAddAcquaintanceButton()
        {
            Helper.WaitFor(() => _addAcquaintanceButton.Displayed);
            _addAcquaintanceButton.Click();
        }

        public void TapOnAcquaintance(string formattedAcquaintanceName)
        {
            for (int i = 0; i <= _iosAcquaintances.Count; i++)
            {
                if (GetNameOfAcquaintanceAtIndex(i).Contains(formattedAcquaintanceName))
                {
                    _iosAcquaintances[i].Click();
                    break;
                }
            }
        }

        public void WaitUntilPageLoads()
        {
            Helper.WaitFor(() => _addAcquaintanceButton.Displayed);
        }

        public void WaitUntilListIsNotEmpty()
        {
            Helper.WaitFor(() => _iosAcquaintances.Count != 0);
        }

        public bool IsAcquaintanceWithNameDisplayed(string formattedAcquaintanceName)
        {
            foreach (var acquaintance in _iosAcquaintances)
            {
                if (acquaintance.Text == formattedAcquaintanceName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
