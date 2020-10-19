using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;
using System;
using System.IO;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Interactions;

namespace AppiumGettingStartedTests
{
    [TestClass]
    public class AppiumTests
    {
        private static AndroidDriver<AndroidElement> _driver;
        private static AppiumLocalService _appiumLocalService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ////var args = new OptionCollector().AddArguments(GeneralOptionList.PreLaunch());
            ////_appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            ////_appiumLocalService.Start();
            string testAppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ApiDemos-debug.apk");
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "android25-test");
            appiumOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, "io.appium.android.apis");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "7.1");
            appiumOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, ".ApiDemos");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App, testAppPath);

            _driver = new AndroidDriver<AndroidElement>(_appiumLocalService, appiumOptions);
            _driver.CloseApp();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            if (_driver != null)
            {
                _driver.LaunchApp();
                _driver.StartActivity("io.appium.android.apis", ".ApiDemos");
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _driver?.CloseApp();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _appiumLocalService.Dispose();
        }

        [TestMethod]
        public void PerformActionsButtons()
        {
            By byScrollLocator = new ByAndroidUIAutomator("new UiSelector().text(\"Views\");");
            var viewsButton = _driver.FindElement(byScrollLocator);
            viewsButton.Click();

            var controlsViewButton = _driver.FindElementByXPath("//*[@text='Controls']");
            controlsViewButton.Click();

            var lightThemeButton = _driver.FindElementByXPath("//*[@text='1. Light Theme']");
            lightThemeButton.Click();
            _driver.HideKeyboard();

            // rotate the device
            ////var rotatable = (IRotatable)_driver;
            ////rotatable.Orientation = ScreenOrientation.Landscape;
            ////_driver.ToggleWifi();

            var saveButton = _driver.FindElementByXPath("//*[@text='Save']");

            Assert.IsTrue(saveButton.Enabled);
        }

        [TestMethod]
        public void LightTheme()
        {
            //var viewButton = _driver.FindElementByXPath("//android.widget.View[@content-desc='View']");

            //https://qavalidation.com/2016/07/scrolling-in-appium.html/ -- Utils
            var winSize = _driver.Manage().Window.Size;
            int scrollStart = (int)(winSize.Height * 0.5);
            int scrollEnd = (int)(winSize.Height * 0.2);

            //Message=The IWebDriver object must implement or wrap a driver that implements IHasTouchScreen. (Parameter 'driver')
            ITouchAction touchActions = new OpenQA.Selenium.Appium.MultiTouch.TouchAction(_driver);
            ////touchActions.
            ////touchActions.Perform();

            
            By byScrollLocator = new ByAndroidUIAutomator("new UiSelector().text(\"Views\");");
            //Message = An element could not be located on the page using the given search parameters.
            var viewsButton = _driver.FindElement(byScrollLocator);
            viewsButton.Click();

            
            var demoTitle = _driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.view.ViewGroup/android.widget.FrameLayout[1]/android.view.ViewGroup");
            Assert.AreEqual<string>("API Demos", demoTitle.Text, "Not on Controls page");
            
            var lightThemeButton = _driver.FindElementByXPath("//android.widget.TextView[@content-desc='1. Light Theme']");
            lightThemeButton.Click();

            var themeTitle = _driver.FindElementByXPath("/hierarchy/android.widget.FrameLayout/android.widget.LinearLayout/android.widget.FrameLayout[1]/android.widget.TextView");
            Assert.AreEqual<string>("Views/Controls/1. Light Theme", themeTitle.Text, "Not on Light Theme page");
        }
    }
}
