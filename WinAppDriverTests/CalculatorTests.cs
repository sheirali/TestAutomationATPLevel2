using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Diagnostics;

namespace WinAppDriverTests
{
    [TestClass]
    public class CalculatorTests
    {
        private static WindowsDriver<WindowsElement> _driver;
        private static WindowsElement _calculatorResult;

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

        [TestMethod]
        [TestCategory("CalculatorTest")]
        public void Division()
        {
            ClearCalcInput();

            _driver.FindElementByAccessibilityId("num8Button").Click();
            _driver.FindElementByAccessibilityId("num8Button").Click();
            _driver.FindElementByAccessibilityId("divideButton").Click();
            _driver.FindElementByAccessibilityId("num1Button").Click();
            _driver.FindElementByAccessibilityId("num1Button").Click();
            _driver.FindElementByAccessibilityId("equalButton").Click();

            Assert.AreEqual("8", GetCalculatorResultText());
        }

        [TestMethod]
        [TestCategory("CalculatorTest")]
        public void Multiplication()
        {
            ClearCalcInput();

            //OpenQA.Selenium.WebDriverException: Unexpected error. Unimplemented Command: xpath locator strategy is not supported

            _driver.FindElementByXPath("//Button[@Name=\"Nine\"]").Click();
            _driver.FindElementByXPath("//Button[@Name='Multiply by']").Click();
            _driver.FindElementByXPath("//Button[@Name='Nine']").Click();
            _driver.FindElementByXPath("//Button[@Name='Equals']").Click();

            Assert.AreEqual("81", GetCalculatorResultText());
        }

        [TestMethod]
        [TestCategory("CalculatorTest")]
        public void Subtraction()
        {
            ClearCalcInput();

            //OpenQA.Selenium.WebDriverException: Unexpected error. Unimplemented Command: xpath locator strategy is not supported

            _driver.FindElementByXPath("//Button[@AutomationId='num9Button']").Click();
            _driver.FindElementByXPath("//Button[@AutomationId='minusButton']").Click();
            _driver.FindElementByXPath("//Button[@AutomationId='num1Button']").Click();
            _driver.FindElementByXPath("//Button[@AutomationId='equalButton']").Click();

            Assert.AreEqual("8", GetCalculatorResultText());
        }

        [TestCategory("CalculatorTest")]
        [DataTestMethod]
        [DataRow("One", "Plus", "Seven", "8")]
        [DataRow("Nine", "Minus", "One", "8")]
        [DataRow("Eight", "Divide by", "Eight", "1")]
        public void Template(string input1, string operation, string input2, string expectedResult)
        {
            ClearCalcInput();

            _driver.FindElementByName(input1).Click();
            _driver.FindElementByName(operation).Click();
            _driver.FindElementByName(input2).Click();
            _driver.FindElementByName("Equals").Click();

            Assert.AreEqual(expectedResult, GetCalculatorResultText());
        }


        [TestMethod]
        [TestCategory("CalculatorTest")]
        public void SwitchToTemperatureCalc()
        {
            //_driver.FindElementByAccessibilityId("ClearEntryButtonPos0").Click();
            ClearCalcInput();

            _driver.FindElementByAccessibilityId("TogglePaneButton").Click();
            _driver.FindElementByAccessibilityId("Temperature").Click();
            _driver.FindElementByAccessibilityId("Units1").Click();
            _driver.FindElementByName("Celsius").Click();
            _driver.FindElementByAccessibilityId("Units2").Click();
            _driver.FindElementByName("Fahrenheit").Click();

            _driver.FindElementByName("Three").Click();
            _driver.FindElementByName("Two").Click();

            Assert.AreEqual("89.6", GetFahrenheitResultText());
        }

        [TestMethod]
        [TestCategory("CalculatorTest")]
        public void SwitchToAreaCalc()
        {
            //converting square centimeters and asserting that valid square feets are calculated
            //_driver.FindElementByAccessibilityId("ClearEntryButtonPos0").Click();
            ClearCalcInput();
            //Square centimetres
            _driver.FindElementByAccessibilityId("TogglePaneButton").Click();
            _driver.FindElementByAccessibilityId("Area").Click();
            _driver.FindElementByAccessibilityId("Units1").Click();
            _driver.FindElementByName("Square centimetres").Click();
            _driver.FindElementByAccessibilityId("Units2").Click();
            _driver.FindElementByName("Square feet").Click();

            _driver.FindElementByName("Two").Click();
            _driver.FindElementByName("Zero").Click();
            _driver.FindElementByName("Two").Click();
            _driver.FindElementByName("Zero").Click();

            Assert.AreEqual("2.17431", GetAreaResultText());
        }

        private string GetCalculatorResultText()
        {
            _calculatorResult = _driver.FindElementByAccessibilityId("CalculatorResults");
            Assert.IsNotNull(_calculatorResult);

            return _calculatorResult.Text.Replace("Display is", string.Empty).Trim();
        }

        private string GetFahrenheitResultText()
        {
            var results = _driver.FindElementByAccessibilityId("Value2");
            Assert.IsNotNull(results);
            Debug.WriteLine(results.Text);//Converts into 89.6 Fahrenheit

            return results.Text
                .Replace("Converts into ", string.Empty)
                .Replace("Fahrenheit", string.Empty)
                .Trim();
        }

        private string GetAreaResultText()
        {
            var results = _driver.FindElementByAccessibilityId("Value2");
            Assert.IsNotNull(results);
            Debug.WriteLine(results.Text); //Convert from 2.17431 Square feet

            return results.Text
                .Replace("Convert from ", string.Empty)
                .Replace("Square feet", string.Empty)
                .Trim();
        }


        private void ClearCalcInput()
        {
            var clear = _driver.FindElementByName("Clear entry");
            clear?.Click();
        }
    }
}
