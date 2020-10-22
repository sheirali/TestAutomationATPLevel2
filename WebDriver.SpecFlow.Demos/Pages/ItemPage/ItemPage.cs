using WebDriver.SpecFlow.Demos.Base;
using OpenQA.Selenium;

namespace WebDriver.SpecFlow.Demos.Pages.ItemPage
{
    public partial class ItemPage : BasePage
    {
        public ItemPage(IWebDriver driver) 
            : base(driver)
        {
        }

        public override string Url => "http://www.amazon.com/";

        public void ClickBuyNowButton()
        {
            AddToCartButton.Click();
        }

        public void Navigate(string part)
        {
            Open(part);
        }
    }
}