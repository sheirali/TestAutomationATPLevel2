using WebDriver.SpecFlow.Demos.Base;
using OpenQA.Selenium;

namespace WebDriver.SpecFlow.Demos.Pages
{
    public partial class HomePage : BasePage
    {
        public HomePage(IWebDriver driver) 
            : base(driver)
        {
        }

        public override string Url => "http://www.metric-conversions.org/";
    }
}