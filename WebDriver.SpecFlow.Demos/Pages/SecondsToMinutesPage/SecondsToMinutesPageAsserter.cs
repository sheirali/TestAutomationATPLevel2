using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebDriver.SpecFlow.Demos.Pages.SecondsToMinutesPage
{
    public static class SecondsToMinutesPageAsserter
    {
        public static void AssertMinutes(this SecondsToMinutesPage page, string expectedMinutes)
        {
            Assert.IsTrue(page.Answer.Text.Contains(string.Format("{0}min", expectedMinutes)));
        }
    }
}