using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;
using System;
using System.IO;

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
            var args = new OptionCollector().AddArguments(GeneralOptionList.PreLaunch());
            _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            _appiumLocalService.Start();
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
            var saveButton = _driver.FindElementByXPath("//*[@text='Save']");

            Assert.IsTrue(saveButton.Enabled);
        }
    }
}
