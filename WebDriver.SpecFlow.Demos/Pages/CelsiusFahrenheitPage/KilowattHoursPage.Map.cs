using OpenQA.Selenium;

namespace WebDriver.SpecFlow.Demos.Pages
{
    public partial class KilowattHoursPage
    {
        public IWebElement CelsiusInput => Driver.FindElement(By.Id("argumentConv"));

        public IWebElement FormatField => Driver.FindElement(By.Id("format"));

        public IWebElement Answer => Driver.FindElement(By.Id("answer"));

        public IWebElement KilowatHoursToNewtonMetersAnchor => Driver.FindElement(By.XPath("//a[contains(text(),'Kilowatt-hours to Newton-meters')]"));
    }
}