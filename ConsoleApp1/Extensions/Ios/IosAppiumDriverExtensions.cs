using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;

namespace Just4Fun.ConsoleApp1.Extensions.Ios
{
    public static class XCUIElementType
    {
        public const string XCUIElementTypeOther = "XCUIElementTypeOther";
        public const string XCUIElementTypeCell = "XCUIElementTypeCell";
        public const string XCUIElementTypeButton = "XCUIElementTypeButton";
        public const string XCUIElementTypeStaticText = "XCUIElementTypeStaticText";
        public const string XCUIElementTypeAlert = "XCUIElementTypeAlert";
    }

    public static class IosAppiumDriverExtensions
    {
        private const string IosClassChain = "-ios class chain";

        public static TWebElement FindElementByIosNsPredicate<TWebElement>(this AppiumDriver<TWebElement> driver, string elementType, string name)
            where TWebElement : IWebElement
        {
            return driver.FindElementByIosNsPredicate($"type == \"{elementType}\" AND name == \"{name}\"");
        }

        public static IReadOnlyCollection<TWebElement> FindElementsByIosNsPredicate<TWebElement>(this AppiumDriver<TWebElement> driver, string elementType, string name)
            where TWebElement : IWebElement
        {
            return driver.FindElementsByIosNsPredicate($"type == \"{elementType}\" AND name == \"{name}\"");
        }

        public static TWebElement FindElementByIosNsPredicate<TWebElement>(this AppiumDriver<TWebElement> driver, string selector)
            where TWebElement : IWebElement
        {
            return driver.FindElement(MobileSelector.iOSPredicateString, selector);
        }

        public static IReadOnlyCollection<TWebElement> FindElementsByIosNsPredicate<TWebElement>(this AppiumDriver<TWebElement> driver, string selector)
            where TWebElement : IWebElement
        {
            return driver.FindElements(MobileSelector.iOSPredicateString, selector);
        }

        public static TWebElement FindElementByIosClassChain<TWebElement>(this AppiumDriver<TWebElement> driver, string elementType, string name, int index)
            where TWebElement : IWebElement
        {
            return driver.FindElementByIosClassChain($"**/{elementType}[`name == \"{name}\"`][{index + 1}]");
        }

        public static TWebElement FindStaticTextElementInCell<TWebElement>(this AppiumDriver<TWebElement> driver, string cellName, int index, string childName)
            where TWebElement : IWebElement
        {
            return driver.FindElementByIosClassChain(XCUIElementType.XCUIElementTypeCell, cellName, index, XCUIElementType.XCUIElementTypeStaticText, childName);
        }

        public static TWebElement FindButtonElementInCell<TWebElement>(this AppiumDriver<TWebElement> driver, string cellName, int index, string childName)
            where TWebElement : IWebElement
        {
            return driver.FindElementByIosClassChain(XCUIElementType.XCUIElementTypeCell, cellName, index, XCUIElementType.XCUIElementTypeButton, childName);
        }

        public static TWebElement FindStaticTextElementInOther<TWebElement>(this AppiumDriver<TWebElement> driver, string otherName, int index, string childName)
            where TWebElement : IWebElement
        {
            return driver.FindElementByIosClassChain(XCUIElementType.XCUIElementTypeOther, otherName, index, XCUIElementType.XCUIElementTypeStaticText, childName);
        }

        public static TWebElement FindElementByIosClassChain<TWebElement>(this AppiumDriver<TWebElement> driver, string parentElementType, string parentName, int index, string childElementType, string childName)
            where TWebElement : IWebElement
        {
            return driver.FindElementByIosClassChain($"**/{parentElementType}[`name == \"{parentName}\"`][{index + 1}]/{childElementType}[`name == \"{childName}\"`]");
        }

        public static TWebElement FindElementByIosClassChain<TWebElement>(this AppiumDriver<TWebElement> driver, string selector)
            where TWebElement : IWebElement
        {
            return driver.FindElement(IosClassChain, selector);
        }

        public static IReadOnlyCollection<TWebElement> FindElementsByIosClassChain<TWebElement>(this AppiumDriver<TWebElement> driver, string elementType, string name)
            where TWebElement : IWebElement
        {
            return driver.FindElementsByIosClassChain($"**/{elementType}[`name == \"{name}\"`]");
        }

        public static IReadOnlyCollection<TWebElement> FindElementsByIosClassChain<TWebElement>(this AppiumDriver<TWebElement> driver, string selector)
            where TWebElement : IWebElement
        {
            return driver.FindElements(IosClassChain, selector);
        }
    }
}
