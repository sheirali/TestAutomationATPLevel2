using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using WinAppDriverTests.Pages;

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
            var calcPage = new StandardCalculatorPage(_driver);

            calcPage.Sum(4, 4);

            calcPage.AssertResult(8);
        }

        [TestMethod]
        [TestCategory("Calc Page")]
        public void Return8_When_Sum2And2And4()
        {
            // Arrange
            var calcPage = new StandardCalculatorPage(_driver);

            // Act
            calcPage.Sum(2, 2, 4);

            // Assert
            calcPage.AssertResult(8);
        }


        [DataTestMethod]
        [TestCategory("Calc Page")]
        [DataRow(8, 2, 2, 2, 2)]
        [DataRow(8, 10, -2)]
        [DataRow(8, -2, 10)]
        [DataRow(-2, -10, 8)]
        public void Sum(int expectedResult, params int[] values)
        {
            var calcPage = new StandardCalculatorPage(_driver);

            calcPage.Sum(values);

            calcPage.AssertResult(expectedResult);
        }

        [DataTestMethod]
        [TestCategory("Calc Page")]
        [DataRow(8, 14, 2, 2, 2)]
        [DataRow(12, 10, -2)]
        [DataRow(-12, -2, 10)]
        [DataRow(2, 10, 8)]
        public void Subtract(int expectedResult, params int[] values)
        {
            var calcPage = new StandardCalculatorPage(_driver);

            calcPage.Subtract(values);

            calcPage.AssertResult(expectedResult);
        }

        [DataTestMethod]
        [TestCategory("Calc Page")]
        [DataRow(2, 16, 2, 2, 2)]
        [DataRow(-5, 10, -2)]
        [DataRow(1, -2, -2)]
        public void Divide(int expectedResult, params int[] values)
        {
            var calcPage = new StandardCalculatorPage(_driver);

            calcPage.Divide(values);

            calcPage.AssertResult(expectedResult);
        }

        [DataTestMethod]
        [TestCategory("Calc Page")]
        [DataRow(16, 2, 2, 2, 2)]
        [DataRow(-20, 10, -2)]
        [DataRow(4, -2, -2)]
        public void Multiply(int expectedResult, params int[] values)
        {
            var calcPage = new StandardCalculatorPage(_driver);

            calcPage.Multiply(values);

            calcPage.AssertResult(expectedResult);
        }


        [DataTestMethod]
        [TestCategory("Calc Page")]
        [DataRow(32, 89.6)]
        public void ConvertCelsiusToFahrenheit(int value, double expectedResult)
        {
            var calcPage = new TemperatureCalculatorPage(_driver);

            calcPage.ConvertCelsiusToFahrenheit(value);

            calcPage.AssertResult(expectedResult);
        }


        [DataTestMethod]
        [TestCategory("Calc Page")]
        [DataRow(2020, 2.17431)]        
        public void CalculateSquareAreaCentimetreToFeet(int value, double expectedResult)
        {
            var calcPage = new AreaCalculatorPage(_driver);

            calcPage.ConvertSqCmToSqFt(value);

            calcPage.AssertResult(expectedResult);
        }


        [DataTestMethod]
        [TestCategory("Calc Page")]
        [DataRow("45", "5", "2", -8.88)]
        [DataRow("6", "2", "6", -3.94)]
        [DataRow("77", "9.12", "1.6", -5.87)]
        public void ScientificFormula(string n, string x, string y, double expectedResult)
        {
            var calcPage = new ScientificCalculatorPage(_driver);

            calcPage.ExecuteFormulaCalculation(n, x, y);

            calcPage.AssertResultStartsWith(expectedResult);
        }
    }
}
