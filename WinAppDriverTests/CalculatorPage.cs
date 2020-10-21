using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WinAppDriverTests.Pages;

namespace WinAppDriverTests
{
    public class CalculatorPage
    {
        private  WindowsDriver<WindowsElement> _driver;


        public CalculatorPage(WindowsDriver<WindowsElement> driver)
        {
            _driver = driver ?? throw new ArgumentNullException("Driver cannot be null.");
        }


       

        internal void Area()
        {
            //converting square centimeters and asserting that valid square feets are calculated
            //_driver.FindElementByAccessibilityId("ClearEntryButtonPos0").Click();
            SelectCalculator(CalculatorType.Area);
            ClearArea();
            _driver.FindElementByAccessibilityId("Units1").Click();
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            var squaresmButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name("Square centimeters")));
            squaresmButton.Click();
            _driver.FindElementByAccessibilityId("Units2").Click();
            _driver.FindElementByName("Square feet").Click();

            _driver.FindElementByName("Two").Click();
            _driver.FindElementByName("Zero").Click();
            _driver.FindElementByName("Two").Click();
            _driver.FindElementByName("Zero").Click();

            Assert.IsTrue(GetAreaResultText().EndsWith("2.17431"));
        }

        internal void Scientific(string n, string x, string y)
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

        internal void Temperature()
        {
            //_driver.FindElementByAccessibilityId("ClearEntryButtonPos0").Click();
            SelectCalculator(CalculatorType.Temperature);
            ClearTemperature();
            _driver.FindElementByAccessibilityId("Units1").Click();
            _driver.FindElementByName("Celsius").Click();
            _driver.FindElementByAccessibilityId("Units2").Click();
            _driver.FindElementByName("Fahrenheit").Click();

            _driver.FindElementByName("Three").Click();
            _driver.FindElementByName("Two").Click();

            Assert.AreEqual("89.6", GetFahrenheitResultText());
        }

        internal void Template(string input1, string operation, string input2, string expectedResult)
        {
            SelectCalculator(CalculatorType.Standard);
            ClearCalcInput();
            _driver.FindElementByName(input1).Click();
            _driver.FindElementByName(operation).Click();
            _driver.FindElementByName(input2).Click();
            _driver.FindElementByName("Equals").Click();

            Assert.AreEqual(expectedResult, GetStandardCalculatorResultText());
        }


        #region Private Methods
        private void SelectCalculator(CalculatorType calculatorType)
        {
            _driver.FindElementByAccessibilityId("TogglePaneButton").Click();
            _driver.FindElementByAccessibilityId(calculatorType.ToString()).Click();
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
            var results = _driver.FindElementByAccessibilityId("CalculatorResults");
            Assert.IsNotNull(results);

            return results.Text
                .Replace("Display is", string.Empty)
                .Trim();
        }

        private WindowsElement GetResultElement()
        {
            var results = _driver.FindElementByAccessibilityId("Value2");
            Assert.IsNotNull(results);
            return results;
        }


        private string GetFahrenheitResultText()
        {
            WindowsElement results = GetResultElement();

            return results.Text
                .Replace("Converts into ", string.Empty)
                .Replace("Fahrenheit", string.Empty)
                .Trim();
        }

        private string GetAreaResultText()
        {
            WindowsElement results = GetResultElement();

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
        #endregion //Private Methods    
    }
}
