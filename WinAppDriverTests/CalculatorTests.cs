using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace WinAppDriverTests
{
    [TestClass]
    public class CalculatorTests
    {
        private static WindowsDriver<WindowsElement> _driver;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");

            _driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        ////[TestInitialize]
        ////public void TestInitialize()
        ////{
        ////    ////_driver?.LaunchApp();
        ////}

        [TestCleanup]
        public void TestCleanup()
        {
            _driver?.Quit();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestMethod]
        [TestCategory("WinAppDriver_Calc")]
        public void Addition()
        {
            _driver.FindElementByName("Five").Click();
            _driver.FindElementByName("Plus").Click();
            _driver.FindElementByName("Seven").Click();
            _driver.FindElementByName("Equals").Click();

            Assert.IsTrue(_driver.FindElementByAccessibilityId("CalculatorResults").Text.EndsWith("12"), "The calculation result wasn't correct.");
        }
    }
}
