using IosAndroidSpecflowExample.Helpers;
using OpenQA.Selenium.Appium;

namespace IosAndroidSpecflowExample.Pages
{
    public class BasePage
    {
        protected AppiumDriver<AppiumWebElement> Driver => AppiumManager.Driver;

        protected PlatformEnum Platform => AppiumManager.IsOnIOS ? PlatformEnum.IOS : PlatformEnum.Android;
    }
}
