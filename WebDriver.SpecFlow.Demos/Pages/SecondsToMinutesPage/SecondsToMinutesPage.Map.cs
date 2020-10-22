using OpenQA.Selenium;

namespace WebDriver.SpecFlow.Demos.Pages.SecondsToMinutesPage
{
    public partial class SecondsToMinutesPage
    {
        public IWebElement SecondsInput => Driver.FindElement(By.Id("argumentConv"));

        public IWebElement Answer => Driver.FindElement(By.Id("answer"));
    }
}