using System;
using System.Text.RegularExpressions;
using OpenQA.Selenium;

namespace Just4Fun.ConsoleApp1.Extensions
{
    public static class IWebElementExtensions
    {
        public static string ValueOrNull(this IWebElement element)
        {
            try
            {
                return element.Text;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsElementPresent(this IWebElement element)
        {
            try
            {
                if (element != null && element.Displayed)
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }

            return false;
        }

        public static bool IsElementPresent(Func<IWebElement> elementLocator)
        {
            try
            {
                var element = elementLocator();

                if (element != null && element.Displayed)
                {
                    return true;
                }
                else if (element != null && element.Enabled)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return false;
        }

        public static string RemoveBlankCharacters(this string stringToBeChanged)
        {
            return Regex.Replace(stringToBeChanged, @"\s+", string.Empty);
        }

        public static void ClearUsingBackspace(this IWebElement element)
        {
            element.SendKeys($"{Keys.Backspace}{Keys.Backspace}{Keys.Backspace}{Keys.Backspace}{Keys.Backspace}{Keys.Backspace}{Keys.Backspace}");
        }
    }
}
