using System;
using WebDriver.SpecFlow.Demos.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using WebDriver.SpecFlow.Demos.Core;

namespace WebDriver.SpecFlow.Demos.Pages
{
    public partial class KilowattHoursPage : BasePage
    {
        public KilowattHoursPage(IWebDriver driver) 
            : base(driver)
        {
        }

        public override string Url => "http://www.metric-conversions.org/temperature/celsius-to-fahrenheit.htm";

        public void ConvertKilowattHoursToNewtonMeters(double kWh)
        {
            CelsiusInput.SendKeys(kWh.ToString());
            DriverWait.Until(drv => Answer != null);
        }

        public void ConvertKilowattHoursToNewtonMeters(double kWh, Format format = Format.Decimal)
        {
            CelsiusInput.SendKeys(kWh.ToString());
            if (format != Format.Decimal)
            {
                var formatText = Enum.GetName(typeof(Format), format);
                new SelectElement(FormatField).SelectByText(formatText);
            }

            DriverWait.Until(drv => Answer != null);
        }
    }
}