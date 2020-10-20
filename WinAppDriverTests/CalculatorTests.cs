using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

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
        [TestCategory("WinAppDriver_Calc")]
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
        [TestCategory("WinAppDriver_Calc")]
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
        [TestCategory("WinAppDriver_Calc")]
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

        [TestCategory("WinAppDriver_Calc")]
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
        [TestCategory("WinAppDriver_Calc")]
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
        [TestCategory("WinAppDriver_Calc")]
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

        [TestCategory("WinAppDriver_Calc")]
        [DataTestMethod]
        [DataRow("45", "5", "2")]
        [DataRow("6", "2", "6")]
        [DataRow("77", "9.12", "1.6")]
        public void SwitchToScientificCalc(string n, string x, string y)
        {
            //data - driven test to calculate the following formula: Pi + log(n) - x ^ y
            //Use the following data:
            //N = 45, x = 5, y = 2
            //N = 6, x = 2, y = 6
            //N = 77, x = 9.12, y = 1.6

            _driver.FindElementByAccessibilityId("TogglePaneButton").Click();
            _driver.FindElementByAccessibilityId("Scientific").Click();

            ClearScientificCalcInput();

            _driver.FindElementByAccessibilityId("piButton").Click();
            _driver.FindElementByAccessibilityId("plusButton").Click();
            //n
            InputNumericValue(n);
            _driver.FindElementByName("Log").Click();

            _driver.FindElementByName("Minus").Click();
            //x
            InputNumericValue(x);
            _driver.FindElementByAccessibilityId("powerButton").Click();
            //y
            InputNumericValue(y);
            _driver.FindElementByAccessibilityId("powerButton").Click();

            _driver.FindElementByAccessibilityId("equalButton").Click();

            _driver.FindElementByAccessibilityId("decimalSeparatorButton").Click();

            string result = GetCalculatorResultText();
            Console.WriteLine(result);
            Assert.AreNotEqual<string>("0", result);
        }

        private void InputNumericValue(string n)
        {
            //assume "valid" input, it 77 or 9.12
            foreach (char item in n.ToCharArray())
            {                
                if (Char.IsDigit(item))
                {
                    switch (item)
                    {
                        case '0':
                            _driver.FindElementByName("Zero").Click();
                            break;
                        case '1':
                            _driver.FindElementByName("One").Click();
                            break;
                        case '2':
                            _driver.FindElementByName("Two").Click();
                            break;
                        case '3':
                            _driver.FindElementByName("Three").Click();
                            break;
                        case '4':
                            _driver.FindElementByName("Four").Click();
                            break;
                        case '5':
                            _driver.FindElementByName("Five").Click();
                            break;
                        case '6':
                            _driver.FindElementByName("Six").Click();
                            break;
                        case '7':
                            _driver.FindElementByName("Seven").Click();
                            break;
                        case '8':
                            _driver.FindElementByName("Eight").Click();
                            break;
                        case '9':
                            _driver.FindElementByName("Nine").Click();
                            break;
                        default:
                            break;
                    }
                }

                if (Char.IsPunctuation(item))   //ie .
                {
                    _driver.FindElementByAccessibilityId("decimalSeparatorButton").Click();
                }
            }
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

        private void ClearScientificCalcInput()
        {
            var clear = _driver.FindElementByAccessibilityId("clearButton");
            clear?.Click();
        }
    }
}
