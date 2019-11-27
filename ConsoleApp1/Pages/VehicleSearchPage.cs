using System;
using System.Collections.Generic;
using System.Linq;
using Just4Fun.ConsoleApp1.Extensions;
using Just4Fun.ConsoleApp1.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.PageObjects;
using OpenQA.Selenium.Appium.PageObjects.Attributes;
using SeleniumExtras.PageObjects;

namespace Just4Fun.ConsoleApp1.Pages
{
#pragma warning disable 0649
    public class VehicleSearchPage : BasePage
    {
        [FindsByIOSUIAutomation(ID = "AccountButton")]
        [FindsByWindowsAutomation(Accessibility = "AccountButton")]
        private IMobileElement<AppiumWebElement> _myAccountButton;

        [FindsByIOSUIAutomation(ID = "SearchPlateTextField")]
        [FindsByWindowsAutomation(Accessibility = "SearchPlateField")]
        private IMobileElement<AppiumWebElement> _searchVehiclesField;

        [FindsByIOSUIAutomation(ID = "RecentlyAddedVehicleViewCell")]
        [FindsByWindowsAutomation(Accessibility = "VehicleImage")]
        private IList<AppiumWebElement> _recentlyAddedVehicle;

        [FindsByWindowsAutomation(Accessibility = "PlateLabel")]
        private IList<AppiumWebElement> _recentlyAddedVehicleVRM;

        [FindsByIOSUIAutomation(ID = "Find Vehicle")]
        [FindsByWindowsAutomation(Accessibility = "SearchVehicleButton")]
        private IMobileElement<AppiumWebElement> _searchVehiclesButton;

        [FindsByIOSUIAutomation(ID = "ErrorView")]
        [FindsByWindowsAutomation(Accessibility = "ErrorTextBlock")]
        private IMobileElement<AppiumWebElement> _vrmNotFoundErrorMessage;

        [FindsByIOSUIAutomation(ID = "Sync complete")]
        private IMobileElement<AppiumWebElement> _syncCompleteMenuButton;

        [FindsByIOSUIAutomation(ID = "Sync")]
        private IMobileElement<AppiumWebElement> _syncMenuButton;

        [FindsByWindowsAutomation(Accessibility = "SyncIconTitle")]
        private IMobileElement<AppiumWebElement> _syncIconTitleText;

        [FindsByWindowsAutomation(Accessibility = "SynchronizationButton")]
        private IMobileElement<AppiumWebElement> _syncButton;

        public VehicleSearchPage()
        {
            PageFactory.InitElements(Driver, this, new AppiumPageObjectMemberDecorator());
        }

        public string SyncIconTitleText => _syncIconTitleText.Text;

        public bool IsMyAccountButtonDisplayed => _myAccountButton.Displayed;

        public bool IsSearchVehicleButtonDisplayed => _searchVehiclesButton.Displayed;

        public void WaitUntilRecentlyAddedVehicleIsDisplayed()
        {
            Helper.WaitFor(() => _recentlyAddedVehicle.Any());
        }

        public void TapOnMyAccountButton()
        {
            Helper.WaitFor(() => _myAccountButton.Enabled);

            _myAccountButton.Click();
        }

        public void TapOnSyncCompleteMenuButton()
        {
            if (Platform == PlatformEnum.IOS)
            {
                Helper.WaitFor(() => _syncCompleteMenuButton.Displayed);

                _syncCompleteMenuButton.Click();
            }
            else if (Platform == PlatformEnum.UWP)
            {
                Helper.WaitFor(() => _syncButton.Displayed);
                Helper.WaitFor(() => SyncIconTitleText.Equals("Sync complete"));
                _syncButton.Click();
            }
        }

        public void TapOnSyncMenuButton()
        {
            if (Platform == PlatformEnum.IOS)
            {
                Helper.WaitFor(() => _syncMenuButton.Displayed);
                _syncMenuButton.Click();
            }
            else if (Platform == PlatformEnum.UWP)
            {
                Helper.WaitFor(() => _syncButton.Displayed);
                _syncButton.Click();
            }
        }

        public void WaitUntilSearchVehiclesFieldIsDisplayed()
        {
            Helper.WaitFor(() => _searchVehiclesField.Displayed);
        }

        public void WaitUntilSyncCompleteMenuButtonIsDisplayed()
        {
            if (Platform == PlatformEnum.IOS)
            {
                Helper.WaitFor(() => _syncCompleteMenuButton.Displayed);
            }
            else if (Platform == PlatformEnum.UWP)
            {
                Helper.WaitFor(() => _syncButton.Displayed);

                Helper.WaitFor(() => SyncIconTitleText.Equals("Sync complete"));
            }
        }

        public void SetSearchField(string searchVehicle)
        {
            Helper.WaitFor(() => _searchVehiclesField.Displayed);
            _searchVehiclesField.SendKeysWithFocus(searchVehicle, Platform);
        }

        public void ClearSearchVehicleField()
        {
            _searchVehiclesField.ClearUsingBackspace();
        }

        public string GetSearchVehicleText()
        {
            WaitUntilSearchVehiclesFieldIsDisplayed();

            return _searchVehiclesField.Text;
        }

        public bool IsSearchVehiclesButtonEnabled()
        {
            Helper.WaitFor(() => _searchVehiclesButton.Displayed);

            return _searchVehiclesButton.Enabled;
        }

        public void TapOnSearchVehicleButton()
        {
            Helper.WaitFor(() => _searchVehiclesButton.Displayed);

            _searchVehiclesButton.Click();
        }

        public void TapOnRecentlyAddedVehicleAtIndex(int index)
        {
            _recentlyAddedVehicle[index].Click();
        }

        public string GetVRMForRecentlyAddedVehicleAtIndex(int index)
        {
            if (Platform == PlatformEnum.IOS)
            {
                return _recentlyAddedVehicle[index].FindElementById("PlateView").Text;
            }
            else if (Platform == PlatformEnum.UWP)
            {
                return _recentlyAddedVehicleVRM[index].Text;
            }
            else
            {
                throw new Exception("Platform not recognized");
            }
        }

        public string GetVRMNotFoundErrorMessage()
        {
            Helper.WaitFor(() => _vrmNotFoundErrorMessage.Displayed);

            return _vrmNotFoundErrorMessage.Text;
        }

        public void WaitUntilVRMNotFoundErrorMessageIsNotDisplayed()
        {
            if (Platform == PlatformEnum.IOS)
            {
                Helper.WaitUntilElementIsNotDisplayed(By.Id("ErrorView"));
            }
            else if (Platform == PlatformEnum.UWP)
            {
                Helper.WaitUntilElementIsNotDisplayed(By.Id("ErrorTextBlock"));
            }
        }
    }
}
