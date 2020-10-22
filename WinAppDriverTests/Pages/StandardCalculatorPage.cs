using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;

namespace WinAppDriverTests.Pages
{
    public class StandardCalculatorPage : CalculatorBasePage
    {
        public StandardCalculatorPage(WindowsDriver<WindowsElement> driver) : base(driver)
        {
        }

        private string ResultText =>
            GetStandardResultElement().Text
                .Replace("Display is", string.Empty)
                .Trim();

        public void Sum(params int[] values)
        {
            ExecuteBasicMathOperation("+", "0", values);
        }

        public void Subtract(params int[] values)
        {
            ExecuteBasicMathOperation("-", "0", values);
        }

        public void Divide(params int[] values)
        {
            ExecuteBasicMathOperation("/", "1", values);
        }

        public void Multiply(params int[] values)
        {
            ExecuteBasicMathOperation("*", "1", values);
        }

        public void AssertResult(int expectedResult)
        {
            Assert.AreEqual(expectedResult.ToString(), ResultText, "The calculation result wasn't correct.");
        }
    }
}
