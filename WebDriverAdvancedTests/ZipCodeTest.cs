using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WebDriverAdvancedTests
{
    [TestClass]
    public class ZipCodeTest
    {
        private IWebDriver _driver;
        //need to wait
        private WebDriverWait _waiter;
        

        [TestInitialize]
        public void TestInit()
        {
            //_driver = new ChromeDriver();
            //_driver.Navigate().GoToUrl("https://www.zip-codes.com/search.asp?selectTab=3");
            //_waiter = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));

            var sauceOptions = new Dictionary<string, object>();
            sauceOptions.Add("screenResolution", "1280x1024");

            var browserOptions = new ChromeOptions();
            browserOptions.UseSpecCompliantProtocol = true;
            browserOptions.PlatformName = "Windows 10";
            browserOptions.BrowserVersion = "latest";
            browserOptions.AddAdditionalOption("sauce:options", sauceOptions);

            //browserOptions.AddAdditionalOption("browserstack.debug", "true");
            //browserOptions.AddAdditionalOption("build", "1.0");
            //browserOptions.AddAdditionalOption("browserName", "Chrome");
            //browserOptions.AddAdditionalOption("platform", "Windows 8.1");
            //browserOptions.AddAdditionalOption("version", "49.0");
            //browserOptions.AddAdditionalOption("screenResolution", "1280x800");
            browserOptions.AddAdditionalOption("username", "sheir1");
            browserOptions.AddAdditionalOption("accessKey", "1280dc44-dc9d-4dbf-b854-c7db46451293");
            browserOptions.AddAdditionalOption("name", "ZipCodeTest");
            _driver = new RemoteWebDriver(new Uri("http://ondemand.saucelabs.com:80/wd/hub"), browserOptions);
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            _driver.Navigate().GoToUrl("https://www.zip-codes.com/search.asp?selectTab=3");

            _waiter = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _driver.Quit();
        }

        [TestMethod]
        [TestCategory("ZipCodes Exercise")]
        public void TownSearch()
        {
            var town = _driver.FindElement(By.XPath(@"//*[@id='ui-id-8']/form/input[2]"));
            var submitBtn = _driver.FindElement(By.XPath(@"//*[@id='ui-id-8']/form/input[6]"));
            
            Assert.IsNotNull(town);
            Assert.IsNotNull(submitBtn);

            town.SendKeys("ant");            
            submitBtn.Click();
            Debug.WriteLine(_driver.Title);
            Assert.AreEqual<string>("Free Zip Code Finder and Lookup", _driver.Title);
        }

        [TestMethod]
        [TestCategory("ZipCodes Exercise")]
        public void GridSearch()
        {
            var town = _driver.FindElement(By.XPath(@"//*[@id='ui-id-8']/form/input[2]"));
            var submitBtn = _driver.FindElement(By.XPath(@"//*[@id='ui-id-8']/form/input[6]"));

            Assert.IsNotNull(town);
            Assert.IsNotNull(submitBtn);

            town.SendKeys("ant");
            submitBtn.Click();


            string gridXpath = @"//table[@class='statTable']";
            var resultGrid = _waiter.Until(ExpectedConditions.ElementExists(By.XPath(gridXpath)));
            //now have results in a grid
            //var resultGrid = _driver.FindElement(By.XPath());
            Assert.IsNotNull(resultGrid);
            var rows = resultGrid.FindElements(By.XPath("tbody/tr"));
            Assert.IsNotNull(rows);
            ////table[@class="statTable"]/tbody/tr[2]/td[1]

            List<string> zipUrls = new List<string>();  //was upto 10
            for (int i = 1; i < 2; i++)//grid has header row, so skip
            {
                ////table[@class="statTable"]/tbody/tr[2]/td[1]/a[@href]
                var zipCell = rows[i].FindElement(By.XPath("td[1]/a[@href]"));
                //Console.WriteLine(zipCell.Text);
                //Debug.WriteLine(zipCell.Text);
                var zipUrl = zipCell.GetAttribute("href");
                //Console.WriteLine(zipUrl);
                //Debug.WriteLine(zipUrl);
                zipUrls.Add(zipUrl);
            }

            foreach (string url in zipUrls)
            {
                //For every one of them save City name, State, ZipCode, Longitude and Latitude
                _driver.Navigate().GoToUrl(url);

                //to help speed up finding of elements just do 1 wait
                string cityXpath = "//table[@class='statTable']/tbody/tr/td[1]/span[text() = 'City:']/parent::td/following-sibling::td[1]";
                var el = _waiter.Until(ExpectedConditions.ElementExists(By.XPath(cityXpath)));

                //var elCity = _waiter.Until(ExpectedConditions.ElementExists(By.XPath(cityXpath)));
                //Console.WriteLine(elCity.Text);
                //Debug.WriteLine(elCity.Text);

                string cityInfo = this.ZipInfo("City:");
                string stateInfo = this.ZipInfo("State:");
                string postalCodeInfo = this.ZipInfo("Zip Code:");
                string latitudeInfo = this.ZipInfo("Latitude:");
                string longitudeInfo = this.ZipInfo("Longitude:");
                string mapUrl0 = this.MapUrl(latitudeInfo, longitudeInfo);

                string output = $"City='{cityInfo}'  State='{stateInfo}'  Zip='{postalCodeInfo}'  Latitude='{latitudeInfo}'  Longitude='{longitudeInfo}'  mapUrl='{mapUrl0}'"  ;
                //City='Guayanilla'  State='PR [Puerto Rico]'  Zip='00656'  Latitude='18.037324'  Longitude='-66.796287'  mapUrl='https://www.google.com/maps/place/18.037324+-66.796287'
                Debug.WriteLine(output);
            }

            //var tableRows = _driver.FindElement(By.ClassName("startTable")).FindElements(By.TagName("//tr"));
            //List<string> zipCodeUrls = new List<string>();
            //foreach (var tableRow in tableRows)
            //{
            //    var firstAnchor = tableRow.FindElement(By.TagName("a"));
            //    zipCodeUrls.Add(firstAnchor.GetAttribute("href"));
            //}

            //take a screenshot
            //Save each screenshot on the disk with file name <CityName>-<State>-<ZipCode>.jpg, for example Paauilo- Hawaii- 96776.jpg
            string mapUrl = "https://www.google.com/maps/place/18.037324+-66.796287";
            _driver.SwitchTo().NewWindow(WindowType.Tab);
            _driver.Navigate().GoToUrl(mapUrl);
            string tempFilePath = Path.GetTempPath();
            string city = "Guayanilla", state = "PR [Puerto Rico]", zip = "00656";

            //can only do PNG and not JPEG
            string filename = $"{city}-{state}-{zip}.{ScreenshotImageFormat.Png}";
            string fullPath = Path.Combine(tempFilePath, filename);
            Debug.WriteLine(fullPath);
            TakeFullScreenshot(_driver, fullPath, ScreenshotImageFormat.Png);

            Assert.IsTrue(File.Exists(fullPath), "The screenshot map file was not found.");
        }

        [TestMethod]
        [Ignore]
        [TestCategory("ZipCodes Exercise")]
        public void SearchPageModel()
        {            
            ZipSearchPage page = new ZipSearchPage(_driver);
            int rowCount = page.SearchForTown("ant");

            Debug.WriteLine(rowCount);
            Assert.AreEqual<int>(1102, rowCount);           
        }


        [TestMethod]
        [Ignore]
        [TestCategory("ZipCodes Exercise")]
        public void SearchPageModelForResults()
        {
            int count = 10;
            ZipSearchPage page = new ZipSearchPage(_driver);
            int rowCount = page.SearchForTownTopResults("ant", count);

            Debug.WriteLine(rowCount);
            Assert.AreEqual<int>(1102, rowCount);

            Assert.AreEqual<int>(count, page.Results.Count);
        }

        private string ZipInfo(string itemName)
        {
            string itemXpath = $"//table[@class='statTable']/tbody/tr/td[1]/span[text() = '{itemName}']/parent::td/following-sibling::td[1]";
            //var el = _waiter.Until(ExpectedConditions.ElementExists(By.XPath(itemXpath)));
            var el = _driver.FindElement(By.XPath(itemXpath));

            return el.Text;
        }

        private string MapUrl(string latitude, string longitude)
        {
            return $"https://www.google.com/maps/place/{latitude}+{longitude}";
        }

        private void TakeFullScreenshot(IWebDriver driver, string filename, ScreenshotImageFormat format)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(filename, format);
        }
    }
}
