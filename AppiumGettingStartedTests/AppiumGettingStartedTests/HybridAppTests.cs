using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Appium.Service;
using System;
using System.IO;

namespace AppiumGettingStartedTests
{
    [TestClass]
    public class HybridAppTests
    {
        private static AndroidDriver<AppiumWebElement> _driver;
        private static AppiumLocalService _appiumLocalService;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
            _appiumLocalService.Start();
            string testAppPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "selendroid-test-app-0.10.0.apk");
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "android25-test");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
            appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "7.1");
            appiumOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppPackage, "io.selendroid.testapp");
            appiumOptions.AddAdditionalCapability(AndroidMobileCapabilityType.AppActivity, "HomeScreenActivity");

            _driver = new AndroidDriver<AppiumWebElement>(_appiumLocalService, appiumOptions);
            _driver.CloseApp();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            if (_driver != null)
            {
                _driver.LaunchApp();
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
        public void WebViewTestCase()
        {
            var webButton = _driver.FindElementById("io.selendroid.testapp:id/buttonStartWebview");
            webButton.Click();

            var contexts = ((IContextAware)_driver).Contexts;
            for (int i = 0; i < contexts.Count; i++)
            {
                if (contexts[i].Contains("WEBVIEW"))
                {
                    ((IContextAware)_driver).Context = contexts[i];
                    break;
                }
            }

            var sendMeYourNameButton = _driver.FindElement(By.XPath("/html/body/form/div/input[2]"));
            sendMeYourNameButton.Click();
        }
    }
}
