using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Linq;

namespace WinAppDriverTests
{
    [TestClass]
    public class CalculatorPageTests
    {
        private static WindowsDriver<WindowsElement> _driver;
        private static CalculatorPage _calcPage;

        [TestInitialize]
        public void TestInit()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");

            _driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);
            Assert.IsNotNull(_driver);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _calcPage = new CalculatorPage(_driver);
            _driver.Manage().Window.Maximize();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _driver?.Quit();
        }

        [TestMethod]
        [TestCategory("Calc Page")]
        public void Return8_When_Sum4And4()
        {
            var scPage = new StandardCalculatorPage(_driver);

            scPage.Sum(4, 4);

            scPage.AssertResult(8);
        }

        [TestMethod]
        [TestCategory("Calc Page")]
        public void Return8_When_Sum2And2And4()
        {
            // Arrange
            var scPage = new StandardCalculatorPage(_driver);

            // Act
            scPage.Sum(2, 2, 4);

            // Assert
            scPage.AssertResult(8);
        }

        [TestMethod]
        [TestCategory("Calc Page")]
        public void Division()
        {
            ////_calcPage.Division();
        }

        [TestMethod]
        [TestCategory("Calc Page")]
        public void Multiplication()
        {
            ////_calcPage.Multiplication();
        }

        [TestMethod]
        [TestCategory("Calc Page")]
        public void Subtraction()
        {
            ////_calcPage.Subtraction();
        }

        [TestCategory("Calc Page")]
        [DataTestMethod]
        [DataRow("One", "Plus", "Seven", "8")]
        [DataRow("Nine", "Minus", "One", "8")]
        [DataRow("Eight", "Divide by", "Eight", "1")]
        public void Template(string input1, string operation, string input2, string expectedResult)
        {
            _calcPage.Template( input1,  operation,  input2,  expectedResult);
        }

        [TestMethod]
        [TestCategory("Calc Page")]
        public void SwitchToTemperatureCalc()
        {
            _calcPage.Temperature();
        }

        [TestMethod]
        [TestCategory("Calc Page")]
        public void SwitchToAreaCalc()
        {
            _calcPage.Area();
        }

        [DataTestMethod]
        [TestCategory("Calc Page")]
        [DataRow("45", "5", "2")]
        [DataRow("6", "2", "6")]
        [DataRow("77", "9.12", "1.6")]
        public void SwitchToScientificCalc(string n, string x, string y)
        {
            _calcPage.Scientific(n, x, y);
        }
    }
}
