using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;

namespace WinAppDriverTests.Pages
{
    public class CalculatorBasePage
    {
        protected WindowsDriver<WindowsElement> _driver;

        public CalculatorBasePage(WindowsDriver<WindowsElement> driver)
        {
            _driver = driver ?? throw new ArgumentNullException("Driver cannot be null.");
        }

        protected void ClearCalcInput()
        {
            var clear = _driver.FindElementByName("Clear");
            clear?.Click();
        }

        protected void SelectCalculator(CalculatorType calculatorType)
        {
            _driver.FindElementByAccessibilityId("TogglePaneButton").Click();
            _driver.FindElementByAccessibilityId(calculatorType.ToString()).Click();
        }

        protected void PickNumericValue(string numberCharacter)
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

                if (char.IsPunctuation(item))
                {
                    _driver.FindElementByAccessibilityId("decimalSeparatorButton").Click();
                }
            }
        }

        protected void PickOperator(string operation)
        {
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

 

        protected WindowsElement GetResultElement()
        {
            var results = _driver.FindElementByAccessibilityId("Value2");
            ////Assert.IsNotNull(results);
            return results;
        }
    }
}
