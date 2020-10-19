using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using System;
using System.IO;

namespace GettingStartedAppiumAndroidWindows
{
    [TestClass]
    public class LocatingElements
    {
        private static AndroidDriver<AndroidElement> _driver;
        private static AppiumLocalService _appiumLocalService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            _appiumLocalService.Start();
            string testAppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "ApiDemos-debug.apk");
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "android25-test");
            appiumOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, "com.example.android.apis");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "7.1");
            appiumOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, ".view.ControlsMaterialDark");
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
                _driver.StartActivity("com.example.android.apis", ".view.ControlsMaterialDark");
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
        public void LocatingElementsTest()
        {
            AndroidElement button = _driver.FindElementById("button");
            button.Click();

            AndroidElement checkBox = _driver.FindElementByClassName("android.widget.CheckBox");
            checkBox.Click();

            AndroidElement secondButton = _driver.FindElementByAndroidUIAutomator("new UiSelector().textContains(\"BUTTO\");");
            secondButton.Click();

            AndroidElement thirdButton = _driver.FindElementByXPath("//*[@resource-id='com.example.android.apis:id/button']");
            thirdButton.Click();
        }

        [TestMethod]
        public void LocatingElementInsideAnotherElementTest()
        {
            var mainElement = _driver.FindElementById("decor_content_parent");

            var button = mainElement.FindElementById("button");
            button.Click();

            var checkBox = mainElement.FindElementByClassName("android.widget.CheckBox");
            checkBox.Click();

            var thirdButton = mainElement.FindElementByXPath("//*[@resource-id='com.example.android.apis:id/button']");
            thirdButton.Click();
        }
    }
}
