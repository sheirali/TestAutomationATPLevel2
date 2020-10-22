using OpenQA.Selenium;

namespace WebDriver.SpecFlow.Demos.Pages
{
    public partial class HomePage
    {
        public IWebElement EnergyAndPowerAnchor => Driver.FindElement(By.XPath("//a[contains(@title,'Energy Conversion')]"));

        public IWebElement KilowattHours => Driver.FindElement(By.XPath("//a[contains(text(),'Kilowatt-hours')]"));
    }
}