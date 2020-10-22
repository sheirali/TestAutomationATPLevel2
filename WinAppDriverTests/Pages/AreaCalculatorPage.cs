using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;

namespace WinAppDriverTests.Pages
{
    public class AreaCalculatorPage : CalculatorBasePage
    {
        public AreaCalculatorPage(WindowsDriver<WindowsElement> driver) : base(driver)
        {
        }

        private string ResultText =>
            GetResultElement().Text
                .Replace("Converts into ", string.Empty)
                .Replace("Square feet", string.Empty)
                .Trim();

        public void ConvertSqCmToSqFt(int value)
        {
            SelectConverterCalculator(CalculatorType.Area, "Square centimetres", "Square feet", value);
        }

        public void AssertResult(double expectedResult)
        {
            Assert.AreEqual(expectedResult.ToString(), ResultText, "The calculation result wasn't correct.");
        }
    }
}
