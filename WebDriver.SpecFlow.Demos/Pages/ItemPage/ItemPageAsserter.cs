using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebDriver.SpecFlow.Demos.Pages.ItemPage
{
    public static class ItemPageAsserter
    {
        public static void ProductTitle(this ItemPage page, string expectedTitle)
        {
            Assert.AreEqual<string>(expectedTitle, page.ProductTitle.Text);
        }
    }
}