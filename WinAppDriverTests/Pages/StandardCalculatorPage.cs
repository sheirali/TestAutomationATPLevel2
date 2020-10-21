using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using WinAppDriverTests.Pages;

namespace WinAppDriverTests
{
    public class StandardCalculatorPage : CalculatorBasePage
    {
        public StandardCalculatorPage(WindowsDriver<WindowsElement> driver) : base(driver)
        {
        }

        public string StandardCalculatorResultText => 
            _driver.FindElementByAccessibilityId("CalculatorResults").Text
                .Replace("Display is", string.Empty)
                .Trim();

        public void Sum(params int[] numbersToBeSum)
        {
            SelectCalculator(CalculatorType.Standard);
            ClearCalcInput();
            foreach (var num in numbersToBeSum)
            {
                PickNumericValue(num.ToString());
                PickOperator("+");
            }

            PickNumericValue("0");
            PickOperator("=");
        }

        internal void Division()
        {
            SelectCalculator(CalculatorType.Standard);
            ClearCalcInput();
            _driver.FindElementByAccessibilityId("num8Button").Click();
            _driver.FindElementByAccessibilityId("num8Button").Click();
            _driver.FindElementByAccessibilityId("divideButton").Click();
            _driver.FindElementByAccessibilityId("num1Button").Click();
            _driver.FindElementByAccessibilityId("num1Button").Click();
            _driver.FindElementByAccessibilityId("equalButton").Click();

            Assert.AreEqual("8", StandardCalculatorResultText);
        }

        internal void Multiplication()
        {
            SelectCalculator(CalculatorType.Standard);
            ClearCalcInput();
            _driver.FindElementByXPath("//Button[@Name=\"Nine\"]").Click();
            _driver.FindElementByXPath("//Button[@Name='Multiply by']").Click();
            _driver.FindElementByXPath("//Button[@Name='Nine']").Click();
            _driver.FindElementByXPath("//Button[@Name='Equals']").Click();

            Assert.AreEqual("81", StandardCalculatorResultText);
        }

        internal void Subtraction()
        {
            SelectCalculator(CalculatorType.Standard);
            ClearCalcInput();
            _driver.FindElementByXPath("//Button[@AutomationId='num9Button']").Click();
            _driver.FindElementByXPath("//Button[@AutomationId='minusButton']").Click();
            _driver.FindElementByXPath("//Button[@AutomationId='num1Button']").Click();
            _driver.FindElementByXPath("//Button[@AutomationId='equalButton']").Click();

            Assert.AreEqual("8", StandardCalculatorResultText);
        }

        public void AssertResult(int expectedResult)
        {
            Assert.AreEqual(expectedResult.ToString(), StandardCalculatorResultText, "The calculation result wasn't correct.");
        }
    }
}
