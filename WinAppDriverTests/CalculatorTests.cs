using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using System.Linq;

namespace WinAppDriverTests
{
    [TestClass]
    public class CalculatorTests
    {
        private static WindowsDriver<WindowsElement> _driver;
        private static WindowsElement _calculatorResult;
        //private static WebDriverWait _waiter;

        private enum CalculatorType
        {
            Standard,
            Scientific,
            Temperature,
            Area
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");

            _driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //_waiter = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _driver?.Quit();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            //var appiumOptions = new AppiumOptions();
            //appiumOptions.AddAdditionalCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            //appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");

            //_driver = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appiumOptions);
            //Assert.IsNotNull(_driver);
            //_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            //_waiter = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            //_driver?.Quit();
        }


        //[DataTestMethod]
        //[TestCategory("WinAppCalc")]
        //[DataRow("+", true)]
        //[DataRow("-", true)]
        //[DataRow("/", true)]
        //[DataRow("*", true)]
        //[DataRow("=", true)]
        //[DataRow("%", false)]
        //[DataRow("!", false)]
        //[DataRow("#", false)]
        //[DataRow(".", false)]
        //[DataRow("0", false)]
        //public void IsBasicOperator(string op, bool expected)
        //{
        //    string[] opeators = new string[] { "+", "-", "*", "/", "=" };

        //    bool actual = opeators.Any(o => o == op);

        //    Assert.AreEqual<bool>(expected, actual , $"Specified value '{op}' is not a valid operator.");
        //}


        [TestMethod]
        [TestCategory("WinAppCalc")]
        public void Addition()
        {
            SelectCalculator(CalculatorType.Standard);
            ClearCalcInput();
            PickNumericValue("5");
            PickOperator("+");
            PickNumericValue("7");
            PickOperator("=");

            Assert.AreEqual("12", GetStandardCalculatorResultText(), "The calculation result wasn't correct.");
        }

        [TestMethod]
        [TestCategory("WinAppCalc")]
        public void Division()
        {
            SelectCalculator(CalculatorType.Standard);
            ClearCalcInput();
            _driver.FindElementByAccessibilityId("num8Button").Click();
            _driver.FindElementByAccessibilityId("num8Button").Click();
            _driver.FindElementByAccessibilityId("divideButton").Click();
            _driver.FindElementByAccessibilityId("num1Button").Click();
            _driver.FindElementByAccessibilityId("num1Button").Click();
            _driver.FindElementByAccessibilityId("equalButton").Click();

            Assert.AreEqual("8", GetStandardCalculatorResultText());
        }

        [TestMethod]
        [TestCategory("WinAppCalc")]
        public void Multiplication()
        {
            SelectCalculator(CalculatorType.Standard);
            ClearCalcInput();
            _driver.FindElementByXPath("//Button[@Name=\"Nine\"]").Click();
            _driver.FindElementByXPath("//Button[@Name='Multiply by']").Click();
            _driver.FindElementByXPath("//Button[@Name='Nine']").Click();
            _driver.FindElementByXPath("//Button[@Name='Equals']").Click();

            Assert.AreEqual("81", GetStandardCalculatorResultText());
        }

        [TestMethod]
        [TestCategory("WinAppCalc")]
        public void Subtraction()
        {
            SelectCalculator(CalculatorType.Standard);
            ClearCalcInput();
            _driver.FindElementByXPath("//Button[@AutomationId='num9Button']").Click();
            _driver.FindElementByXPath("//Button[@AutomationId='minusButton']").Click();
            _driver.FindElementByXPath("//Button[@AutomationId='num1Button']").Click();
            _driver.FindElementByXPath("//Button[@AutomationId='equalButton']").Click();

            Assert.AreEqual("8", GetStandardCalculatorResultText());
        }

        [TestCategory("WinAppCalc")]
        [DataTestMethod]
        [DataRow("One", "Plus", "Seven", "8")]
        [DataRow("Nine", "Minus", "One", "8")]
        [DataRow("Eight", "Divide by", "Eight", "1")]
        public void Template(string input1, string operation, string input2, string expectedResult)
        {
            SelectCalculator(CalculatorType.Standard);
            ClearCalcInput();
            _driver.FindElementByName(input1).Click();
            _driver.FindElementByName(operation).Click();
            _driver.FindElementByName(input2).Click();
            _driver.FindElementByName("Equals").Click();

            Assert.AreEqual(expectedResult, GetStandardCalculatorResultText());
        }


        [TestMethod]
        [TestCategory("WinAppCalc")]
        public void SwitchToTemperatureCalc()
        {
            //_driver.FindElementByAccessibilityId("ClearEntryButtonPos0").Click();
            ClearTemperature();
            SelectCalculator(CalculatorType.Temperature);
            _driver.FindElementByAccessibilityId("Units1").Click();
            _driver.FindElementByName("Celsius").Click();
            _driver.FindElementByAccessibilityId("Units2").Click();
            _driver.FindElementByName("Fahrenheit").Click();

            _driver.FindElementByName("Three").Click();
            _driver.FindElementByName("Two").Click();

            Assert.AreEqual("89.6", GetFahrenheitResultText());
        }

        [TestMethod]
        [TestCategory("WinAppCalc")]
        public void SwitchToAreaCalc()
        {
            //converting square centimeters and asserting that valid square feets are calculated
            //_driver.FindElementByAccessibilityId("ClearEntryButtonPos0").Click();
            ClearArea();
            SelectCalculator(CalculatorType.Area);
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

        [DataTestMethod]
        [TestCategory("WinAppCalc")]
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
            SelectCalculator(CalculatorType.Scientific);
            ClearCalcInput();
            _driver.FindElementByAccessibilityId("piButton").Click();
            _driver.FindElementByAccessibilityId("plusButton").Click();
            PickNumericValue(n);
            _driver.FindElementByName("Log").Click();
            _driver.FindElementByName("Minus").Click();
            PickNumericValue(x);
            _driver.FindElementByAccessibilityId("powerButton").Click();
            PickNumericValue(y);
            _driver.FindElementByAccessibilityId("powerButton").Click();
            _driver.FindElementByAccessibilityId("equalButton").Click();
            _driver.FindElementByAccessibilityId("decimalSeparatorButton").Click();

            string result = GetStandardCalculatorResultText();
            Assert.AreNotEqual("0", result);
        }

        private void SelectCalculator(CalculatorType calculatorType)
        {
            _driver.FindElementByAccessibilityId("TogglePaneButton").Click();
            _driver.FindElementByAccessibilityId(calculatorType.ToString()).Click();

            //var el = _waiter.Until(ExpectedConditions.ElementExists(By.Name("One")));
            //Assert.IsNotNull(el);
        }

        private void PickNumericValue(string numberCharacter)
        {
            //assume "valid" input, it 77 or 9.12
            foreach (char item in numberCharacter)
            {                
                if (char.IsDigit(item))
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

                if (char.IsPunctuation(item))   //ie .
                {
                    _driver.FindElementByAccessibilityId("decimalSeparatorButton").Click();
                }
            }
        }

        private void PickOperator(string operation)
        {
            //string[] opeators = new string[] { "+", "-", "*", "/", "=" };
            //bool actual = opeators.Any(o => o == operation);

            switch (operation)
            {
                case "+":
                    _driver.FindElementByName("Plus").Click();
                    break;
                case "-":
                    _driver.FindElementByName("Minus").Click();
                    break;
                case "*":
                    _driver.FindElementByName("Multiply by").Click();
                    break;
                case "/":
                    _driver.FindElementByName("Divide by").Click();
                    break;
                case "=":
                    _driver.FindElementByName("Equals").Click();
                    break;
                default:
                    break;
            }
        }

        private string GetStandardCalculatorResultText()
        {
            _calculatorResult = _driver.FindElementByAccessibilityId("CalculatorResults");
            Assert.IsNotNull(_calculatorResult);

            return _calculatorResult.Text
                .Replace("Display is", string.Empty)
                .Trim();
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

        private void ClearTemperature()
        {
            var clear = _driver.FindElementByName("Clear entry");
            clear?.Click();
        }

        private void ClearArea()
        {
            var clear = _driver.FindElementByName("Clear entry");
            clear?.Click();
        }

        private void ClearCalcInput()
        {
            //var clear = _waiter.Until(ExpectedConditions.ElementExists(By.Name("Clear")));
            //Assert.IsNotNull(clear);

            var clear = _driver.FindElementByName("Clear");
            //var clear = _driver.FindElementByAccessibilityId("clearButton");
            clear?.Click();
        }
    }
}
