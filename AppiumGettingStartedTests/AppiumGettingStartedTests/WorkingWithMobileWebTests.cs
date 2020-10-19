using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Service;
using System;

namespace AppiumGettingStartedTests
{
    [TestClass]
    public class WorkingWithMobileWebTests
    {
private static AndroidDriver<AppiumWebElement> _driver;
private static AppiumLocalService _appiumLocalService;

[ClassInitialize]
public static void ClassInitialize(TestContext context)
{
    _appiumLocalService = new AppiumServiceBuilder().UsingAnyFreePort().Build();
    _appiumLocalService.Start();
    var appiumOptions = new AppiumOptions();
    appiumOptions.AddAdditionalCapability(MobileCapabilityType.DeviceName, "android25-test");
    appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformName, "Android");
    appiumOptions.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, "7.1");
    appiumOptions.AddAdditionalCapability(MobileCapabilityType.BrowserName, "Chrome");

    _driver = new AndroidDriver<AppiumWebElement>(_appiumLocalService, appiumOptions);
    _driver.CloseApp();
}

[TestInitialize]
public void TestInitialize()
{
    _driver?.LaunchApp();
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
        public void GoToWebSite()
        {
            _driver.Navigate().GoToUrl("http://demos.bellatrix.solutions/");
            Console.WriteLine(_driver.PageSource);
            Assert.IsTrue(_driver.PageSource.Contains("Shop"));
        }
    }
}
