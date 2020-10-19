using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Service.Options;
using System;
using System.Drawing;
using System.IO;
using OpenQA.Selenium.Appium.MultiTouch;

namespace AppiumGettingStartedTests
{
    [TestClass]
    public class GestureTests
    {
        private static AndroidDriver<AppiumWebElement> _driver;
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
            appiumOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, ".graphics.TouchRotateActivity");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.App, testAppPath);

            _driver = new AndroidDriver<AppiumWebElement>(_appiumLocalService, appiumOptions);
            _driver.CloseApp();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            if (_driver != null)
            {
                _driver.LaunchApp();
                _driver.StartActivity("io.appium.android.apis", ".graphics.TouchRotateActivity");
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (_driver != null)
            {
                _driver.CloseApp();
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _appiumLocalService.Dispose();
        }

        [TestMethod]
        public void SwipeTest()
        {
            _driver.StartActivity("io.appium.android.apis", ".graphics.FingerPaint");
            var element = _driver.FindElementById("android:id/content");
            Point point = element.Coordinates.LocationInDom;
            Size size = element.Size;

            new TouchAction(_driver)
                .Press(point.X + 5, point.Y + 5)
                .Wait(200)
                .MoveTo(point.X + size.Width - 5, point.Y + size.Height - 5)
                .Release()
                .Perform();

        }
    }
}
