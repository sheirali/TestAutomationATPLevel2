﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;

namespace WinAppDriverTests.Pages
{
    public class TemperatureCalculatorPage : CalculatorBasePage
    {
        public TemperatureCalculatorPage(WindowsDriver<WindowsElement> driver) : base(driver)
        {
        }

        private string ResultText =>
            GetResultElement().Text
                .Replace("Converts into ", string.Empty)
                .Replace("Fahrenheit", string.Empty)
                .Trim();

        public void ConvertCelsiusToFahrenheit(int value)
        {
            SelectConverterCalculator(CalculatorType.Temperature, "Celsius", "Fahrenheit", value);
        }

        public void AssertResult(double expectedResult)
        {
            Assert.AreEqual(expectedResult.ToString(), ResultText, "The calculation result wasn't correct.");
        }
    }
}
