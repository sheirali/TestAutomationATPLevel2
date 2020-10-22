using OpenQA.Selenium;

namespace WebDriver.SpecFlow.Demos.Pages.ItemPage
{
    public partial class ItemPage
    {
        public IWebElement AddToCartButton => Driver.FindElement(By.Id("add-to-cart-button"));

        public IWebElement ProductTitle => Driver.FindElement(By.Id("productTitle"));
    }
}