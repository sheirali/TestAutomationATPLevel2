using OpenQA.Selenium;
using WebDriver.SpecFlow.Demos.Base;

namespace WebDriver.SpecFlow.Demos.Pages.SecondsToMinutesPage
{
    public partial class SecondsToMinutesPage : BasePage
    {
        public SecondsToMinutesPage(IWebDriver driver) 
            : base(driver)
        {
        }

        public override string Url => "http://www.metric-conversions.org/time/seconds-to-minutes.htm";

        public void ConvertSecondsToMintes(double seconds)
        {
            SecondsInput.SendKeys(seconds.ToString());
            DriverWait.Until(drv => Answer != null);
        }
    }
}