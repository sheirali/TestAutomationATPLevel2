using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;

namespace WinAppDriverTests.Pages
{
    public class ScientificCalculatorPage : CalculatorBasePage
    {
        public ScientificCalculatorPage(WindowsDriver<WindowsElement> driver) 
            : base(driver)
        {
        }

        private string ResultText =>
            GetStandardResultElement().Text
                .Replace("Display is", string.Empty)
                .Replace("point", string.Empty)
                .Trim();

        public void ExecuteFormulaCalculation(string n, string x, string y)
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
        }

        public void AssertResultStartsWith(double expectedResult)
        {
            Assert.IsTrue(ResultText.StartsWith(expectedResult.ToString()), "The calculation result wasn't correct.");
        }
    }
}