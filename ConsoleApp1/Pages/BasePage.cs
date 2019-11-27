using Just4Fun.ConsoleApp1.Helpers;
using OpenQA.Selenium.Appium;

namespace Just4Fun.ConsoleApp1.Pages
{
    public class BasePage
    {
        protected AppiumDriver<AppiumWebElement> Driver => AppiumManager.Driver;

        protected PlatformEnum Platform => AppiumManager.IsOnIOS ? PlatformEnum.IOS : PlatformEnum.UWP;
    }
}