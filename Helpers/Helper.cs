using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

using PointerInputDevice = OpenQA.Selenium.Appium.Interactions.PointerInputDevice;

namespace IosAndroidSpecflowExample.Helpers
{
    public class Helper
    {
        public static void WaitFor(Func<bool> condition, long seconds = 30)
        {
            var wait = new WebDriverWait(AppiumManager.Driver, TimeSpan.FromSeconds(seconds));

            try
            {
                wait.Until(drv => EvaluateConditionWithoutThrowing(condition));
            }
            catch (WebDriverTimeoutException)
            {
                // Force same exception
                condition();
            }
        }

        public static void ScrollDownUntilCondition(Func<bool> condition, long seconds = 30)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!EvaluateConditionWithoutThrowing(condition))
            {
                ScrollDown();

                if (stopwatch.Elapsed.Seconds > seconds)
                {
                    try
                    {
                        condition();

                        throw new WebDriverException("Scrolling down timed out as condition was not met");
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public static void ScrollDown(int? offset = null)
        {
            var startx = AppiumManager.Driver.Manage().Window.Size.Width / 2;
            var starty = AppiumManager.Driver.Manage().Window.Size.Height / 2;
            offset = offset ?? (AppiumManager.Driver.Manage().Window.Size.Height / 2) - 10;
            var endy = starty - offset.Value;

            if (AppiumManager.IsOnIOS)
            {
                new TouchAction(AppiumManager.Driver).Press(startx, starty).Wait(ms: 1000).MoveTo(startx, endy).Release().Perform();
            }
            else if (AppiumManager.IsOnAndroid)
            {
                var touchDevice = new PointerInputDevice(PointerKind.Touch);
                var actionSequence = new ActionSequence(touchDevice, 0);

                actionSequence.AddAction(touchDevice.CreatePointerMove(CoordinateOrigin.Viewport, startx, starty, TimeSpan.Zero));
                actionSequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
                actionSequence.AddAction(touchDevice.CreatePointerMove(CoordinateOrigin.Viewport, startx, endy, TimeSpan.Zero));
                actionSequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));

                AppiumManager.Driver.PerformActions(new List<ActionSequence> { actionSequence });
            }
        }

        public static void ExecuteSwallowingExceptions(Action action)
        {
            try
            {
                action();
            }
            catch
            {
            }
        }

        private static bool EvaluateConditionWithoutThrowing(Func<bool> condition)
        {
            try
            {
                return condition();
            }
            catch
            {
                return false;
            }
        }
    }
}
