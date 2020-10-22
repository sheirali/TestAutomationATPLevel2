using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;


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

        protected void ClearEntryInput()
        {
            var clear = _driver.FindElementByName("Clear entry");
            clear?.Click();
        }

        protected void SelectCalculator(CalculatorType calculatorType)
        {
            _driver.FindElementByAccessibilityId("TogglePaneButton").Click();
            _driver.FindElementByAccessibilityId(calculatorType.ToString()).Click();
        }

        protected void PickNumericValue(string numberCharacter)
        {
            //due to how calculator handles negation (ie -2 ), have to put the negation at end
            if (numberCharacter.StartsWith('-'))
            {
                string value = numberCharacter.Substring(1);
                numberCharacter = value + "-";
            }

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

                if (item.Equals('-'))
                {
                    _driver.FindElementByAccessibilityId("negateButton").Click();
                }

                if (item.Equals('.'))
                {
                    _driver.FindElementByAccessibilityId("decimalSeparatorButton").Click();
                }

                //if (char.IsPunctuation(item))//says both . and - are punctuation
                //{
                //    _driver.FindElementByAccessibilityId("decimalSeparatorButton").Click();
                //}
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
            return results;
        }

        protected WindowsElement GetStandardResultElement()
        {
            var results = _driver.FindElementByAccessibilityId("CalculatorResults");
            return results;
        }

        protected void SelectConverterCalculator(CalculatorType calculatorType, string fromUnit, string toUnit, int value)
        {
            SelectCalculator(calculatorType);
            ClearEntryInput();

            _driver.FindElementByAccessibilityId("Units1").Click();
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
            var celsiusButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name(fromUnit)));
            celsiusButton.Click();

            _driver.FindElementByAccessibilityId("Units2").Click();
            var fahrenheitButton = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Name(toUnit)));
            fahrenheitButton.Click();

            PickNumericValue(value.ToString());
        }
    }
}
