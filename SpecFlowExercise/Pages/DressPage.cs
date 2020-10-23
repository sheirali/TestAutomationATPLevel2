using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SpecFlowExercise.Pages
{
    public class DressPage
    {
        private IWebDriver _driver;
        private WebDriverWait _waiter;
        private readonly string _baseUrl = "http://automationpractice.com/";
        private readonly string _dressUrl = "http://automationpractice.com/index.php?id_category=8&controller=category";

        public DressPage()
        {
            _driver = new ChromeDriver();
            // wait 30 seconds.
            _waiter = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
        }

        ~DressPage()
        {
            _waiter = null;
            _driver?.Quit();
        }

        public void Open()
        {
            _driver.Navigate().GoToUrl(_dressUrl);
            _driver.Manage().Window.Maximize();
        }

        public void NavigateTo(string partUrl)
        {
            string fullPath = string.Concat(_baseUrl, partUrl);
            _driver.Navigate().GoToUrl(fullPath);
        }

        public IWebElement CompareCount => 
            _waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//button[contains(@class, 'bt_compare')][last()]/span/strong")));

        public void AssertCompareCount(int dressCountToCompare)
        {
            //string compareXpath = @"//*[@id='center_column']/div[3]/div[2]/form/button/span/strong";
            //string compareXpath = "//button[contains(@class, 'bt_compare')][last()]/span/strong";
            //var elCompare = _waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(compareXpath)));

            //var el = _driver.FindElement(By.XPath(path));
            Assert.IsNotNull(CompareCount);
            Debug.WriteLine($"Dress Compare Count is {CompareCount.Text}");
            Assert.AreEqual<string>(dressCountToCompare.ToString(), CompareCount.Text);
        }

        public void AddToCompare(int productId)
        {
            Debug.WriteLine($"AddToCompare dressID {productId}");

            //lets try to wait for compare count to be updated
            _waiter.Until(el => CompareCount != null);
            int currentCount = int.Parse(CompareCount.Text);

            string compareXpath = $"//a[@data-id-product='{productId}'][contains(text(), 'Add to Compare')]";
            var elCompare = _waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(compareXpath)));
            Assert.IsNotNull(elCompare);
            elCompare.Click();

            _waiter.Until(el => CompareCount != null && int.Parse(CompareCount.Text) > currentCount);
        }

        public void QuickView(int productId)
        {
//Add tests for navigation to Quick View and verification of the info there.
//Add tests to change the quantity, size, color from a quick view, and add the item to the cart.
//Verify that the correct item was added.

            Debug.WriteLine($"QuickView dressID {productId}");
            string productKey = "id_product=" + productId;


            //sigh...keep trying to get the QuickView popup but failing
            //valid xPath but error on Click -- element not interactable


            //  //div[@class='quick-view-wrapper-mobile']  
            //string qviewXpath = $"//a[@class='quick-view-mobile'][contains(@href, '{productKey}')]/parent::div";

            string qviewXpath = $"//a[@class='quick-view-mobile'][contains(@href, '{productKey}')]/i";
            var elQuickView = _waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath(qviewXpath)));

            if (elQuickView.Enabled && elQuickView.Displayed)//false
            {
                Debug.WriteLine($"elQuickView = {elQuickView.Text} gonna click via JS");//no name
                ((IJavaScriptExecutor)_driver).ExecuteScript("argument[0].click();", elQuickView);
            }
            else
            {
                Debug.WriteLine($"elQuickView = {elQuickView.Text} gonna click via Click");
                elQuickView.Click();
            }


            _waiter.Until(el => CompareCount != null);

            //launches a popup - iframe ??
            var popup = _driver.SwitchTo().Frame(1);
            
            //just a delay
            var elAddToCart = _waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//button[@class='exclusive']")));
            _waiter.Until(el => elAddToCart != null);


            var elName = _driver.FindElement(By.XPath("//h1[@itemprop='name']"));
            Debug.WriteLine($"elName = {elName.Text}");
            //<h1 itemprop='name'>Printed Dress</h1>
            //  //*[@id="product"]/div/div/div[2]/h1

            var elPrice = _driver.FindElement(By.Id("our_price_display"));
            Debug.WriteLine($"elPrice = {elPrice.Text}");
            //< span id = "our_price_display" itemprop = "price" >$26.00 </ span >
            //  //*[@id="our_price_display"]

            var elQuantity = _driver.FindElement(By.Id("quantity_wanted"));
            Debug.WriteLine($"elQuantity = {elQuantity.Text}");
            //<input type="text" name="qty" id="quantity_wanted" class="text" value="1" style="border: 1px solid rgb(189, 194, 201);">
            //  //*[@id="quantity_wanted"]

            var elSize = _driver.FindElement(By.Id("group_1")); //combobox / drowndown / select
            Debug.WriteLine($"elQuantity = {elSize.Text}");
            var cboSize = new SelectElement(elSize);
            var opts = cboSize.Options;
            Debug.WriteLine($"opts.Count = {opts.Count}");
            var selected = cboSize.SelectedOption;
            Debug.WriteLine($"selected = {selected.Text}");
            //<span style="width: 218.009px; user-select: none;">S</span>
            //  //*[@id="uniform-group_1"]/span
            //  //*[@id="group_1"]      the dropdown for size

            var elColour = _driver.FindElement(By.Id("color_to_pick_list"));    //UL
            Debug.WriteLine($"elColour = {elColour.Text}");
            var lstColours = elColour.FindElements(By.TagName("//li"));
            //<ul id="color_to_pick_list" class="clearfix">
            //  //ul[@id='color_to_pick_list']
            //ul[@id='color_to_pick_list']/child::li
            //  //ul[@id='color_to_pick_list']/child::li/a
            //< a href = "http://automationpractice.com/index.php?id_product=3&amp;controller=product" id = "color_13" name = "Orange" class="color_pick selected" style="background: rgb(243, 156, 17); opacity: 1;" title="Orange">
            //</a>
            //driver.findElement(By.cssSelector("ul. clearfix li:last-child");

            popup.Close();
            _driver.SwitchTo().DefaultContent();
        }
    }
}
