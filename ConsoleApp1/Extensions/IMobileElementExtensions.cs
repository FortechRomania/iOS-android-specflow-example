using Just4Fun.ConsoleApp1.Helpers;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Interfaces;

namespace Just4Fun.ConsoleApp1.Extensions
{
    public static class IMobileElementExtensions
    {
        public static void SendKeysWithFocus(
            this IMobileElement<AppiumWebElement> mobileElement,
            string keys,
            PlatformEnum platform)
        {
            if (platform == PlatformEnum.UWP)
            {
                mobileElement.Click();
            }

            mobileElement.SendKeys(keys);
        }
    }
}