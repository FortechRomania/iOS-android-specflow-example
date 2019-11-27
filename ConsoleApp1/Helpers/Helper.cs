using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Just4Fun.ConsoleApp1.CosmeticConditionConstants;
using Just4Fun.ConsoleApp1.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

using static BritishCarAuctions.DealerProApp.Api.IntegrationTests.Constants;
using PointerInputDevice = OpenQA.Selenium.Appium.Interactions.PointerInputDevice;

namespace Just4Fun.ConsoleApp1.Helpers
{
    public class Helper
    {
        public static void WaitUntilElementIsNotDisplayed(By locator)
        {
            var wait = new WebDriverWait(AppiumManager.Driver, TimeSpan.FromSeconds(20));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        public static void OptionalWaitFor(Func<bool> condition, long seconds)
        {
            var optionalWait = new WebDriverWait(AppiumManager.Driver, TimeSpan.FromSeconds(seconds));

            try
            {
                optionalWait.Until(drv => EvaluateConditionWithoutThrowing(condition));
            }
            catch (WebDriverTimeoutException e)
            {
                if (e.InnerException != null)
                {
                    Console.WriteLine(e.InnerException.Message);
                }
            }
        }

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

        public static void ScrollLeftHorizontalUntilCondition(Func<bool> condition, long seconds = 30)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!EvaluateConditionWithoutThrowing(condition))
            {
                ScrollLeftHorizontal();

                if (stopwatch.Elapsed.Seconds > seconds)
                {
                    try
                    {
                        condition();

                        throw new WebDriverException("Scrolling left timed out as condition was not met");
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public static void ScrollLeftHorizontal()
        {
            var startx = AppiumManager.Driver.Manage().Window.Size.Width / 2;
            var starty = AppiumManager.Driver.Manage().Window.Size.Height / 2;

            new TouchAction(AppiumManager.Driver).Press(startx, starty).Wait(ms: 1000).MoveTo(startx - 200, starty).Release().Perform();
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
            else if (AppiumManager.IsOnUWP)
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

        public static void ScrollUpUntilCondition(Func<bool> condition, long seconds = 30)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!EvaluateConditionWithoutThrowing(condition))
            {
                ScrollUp();

                if (stopwatch.Elapsed.Seconds > seconds)
                {
                    try
                    {
                        condition();

                        throw new WebDriverException("Scrolling up timed out as condition was not met");
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public static void ScrollUp(int? offset = null)
        {
            var startx = AppiumManager.Driver.Manage().Window.Size.Width / 2;
            var starty = AppiumManager.Driver.Manage().Window.Size.Height / 2;
            offset = offset ?? (AppiumManager.Driver.Manage().Window.Size.Height / 2) - 10;
            var endy = starty + offset.Value;

            if (AppiumManager.IsOnIOS)
            {
                new TouchAction(AppiumManager.Driver).Press(startx, starty).Wait(ms: 1000).MoveTo(startx, endy).Release().Perform();
            }
            else if (AppiumManager.IsOnUWP)
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

        public static string GenerateRadomString(int size)
        {
            var builder = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * random.NextDouble()) + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static Point GetPointForLocation(string tapLocation)
        {
            var kipperViewConstants = ScreenSpecificKipperviewConstants.KipperviewConstantsForScreen(GlobalSettings.ScreenType);

            switch (tapLocation)
            {
                case "Bonnet":
                    return kipperViewConstants.Bonnet;
                case "Screen Front":
                    return kipperViewConstants.ScreenFront;
                case "Bumper Front":
                    return kipperViewConstants.BumperFront;
                case "Door osr":
                    return kipperViewConstants.DoorOsr;
                case "Wheel nsf":
                    return kipperViewConstants.WheelAndWheelTrimNsf;
                case "Roof":
                    return kipperViewConstants.Roof;
                case "Seat Back Cover osf":
                    return kipperViewConstants.SeatBackAndBaseCoverOsf;
                case "Tailgate Glass":
                    return kipperViewConstants.TailgateGlass;
                case "Door Pad osf":
                    return kipperViewConstants.DoorPadOsf;
                case "Door Pad nsr":
                    return kipperViewConstants.DoorPadNsr;
                case "Qtr Panel Trim os":
                    return kipperViewConstants.QtrPanelTrimOs;
                case "Carpets Rear":
                    return kipperViewConstants.CarpetsRear;
                case "Roof Lining":
                    return kipperViewConstants.RoofLiningAndSunvisor;
                default:
                    throw new Exception("Cannot find a location to tap");
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

        public static void TapOnPoint(Point point)
        {
            if (AppiumManager.IsOnIOS)
            {
                new TouchAction(AppiumManager.Driver).Tap(point.X, point.Y).Perform();
            }
            else if (AppiumManager.IsOnUWP)
            {
                var touchDevice = new PointerInputDevice(PointerKind.Touch);
                var actionSequence = new ActionSequence(touchDevice, 0);

                actionSequence.AddAction(touchDevice.CreatePointerMove(CoordinateOrigin.Viewport, point.X, point.Y, TimeSpan.Zero));
                actionSequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
                actionSequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));

                AppiumManager.Driver.PerformActions(new List<ActionSequence> { actionSequence });
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