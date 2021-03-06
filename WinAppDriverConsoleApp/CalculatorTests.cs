﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.IO;

namespace WinAppDriverConsoleApp
{
    [TestClass]
    public class CalculatorTests
    {
        //private static WindowsDriver<WindowsElement> _driver;
        private static RemoteWebDriver _driver;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //var appiumOptions = new DesiredCapabilities();
            //appiumOptions.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            //appiumOptions.SetCapability("deviceName", "WindowsPC");
            //_driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), options);

            var options = new ChromeOptions();
            options.AddAdditionalCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            options.AddAdditionalCapability("deviceName", "WindowsPC");
            _driver = new RemoteWebDriver(new Uri("http://127.0.0.1:4723"), options);


            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            if (_driver != null)
            {
                throw new Exception("driver is null!");
                //_driver.star();
            }
        }

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

            //Assert.AreEqual("12", _driver.
            //    FindElementByAccessibilityId("CalculatorResults").Text);
        }
    }
}
