using IosAndroidSpecflowExample.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.MultiTouch;

namespace IosAndroidSpecflowExample.Extensions
{
    public static class IWebElementExtensions
    {
        public static void FastClick(this IWebElement webElement)
        {
            if (AppiumManager.IsOnIOS)
            {
                new TouchAction(AppiumManager.Driver).Tap(webElement).Perform();
            }
            else
            {
                webElement.Click();
            }
        }
    }
}
