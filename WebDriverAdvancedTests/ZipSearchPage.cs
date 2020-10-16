using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace WebDriverAdvancedTests
{
    public class ZipSearchPage
    {
        private string _searchUrl = "https://www.zip-codes.com/search.asp?selectTab=3";
        private IWebDriver _driver;
        private WebDriverWait _wait;


        public ZipSearchPage(IWebDriver webDriver)
        {
            _driver = webDriver;
            //tests failed with 2 seconds when trying to take snapshot of Maps page
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));

            Results = new List<ZipSearchResult>();
        }

        public List<ZipSearchResult> Results { get; private set; }

        /// <summary>
        /// Searches for town.
        /// </summary>
        /// <param name="townName">Name of the town.</param>
        /// <returns>int - number of records found</returns>
        public int SearchForTown(string townName)
        {
            _driver.Navigate().GoToUrl(_searchUrl);

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
            IWebElement town = wait.Until((d) => d.FindElement(By.XPath(@"//*[@id='ui-id-8']/form/input[2]")));
            //var town = _driver.FindElement(By.XPath(@"//*[@id='ui-id-8']/form/input[2]"));
            IWebElement submitBtn = _driver.FindElement(By.XPath(@"//*[@id='ui-id-8']/form/input[6]"));
           
            town.SendKeys("ant");
            submitBtn.Click();


            string recordsXpath = "//table[@class='statTable']/following-sibling::div/div/span";
            //(1 - 50 of 1102 Records)
            IWebElement elSpan = wait.Until(ExpectedConditions.ElementExists(By.XPath(recordsXpath)));
            string resultText = elSpan.Text;
            Debug.WriteLine(resultText);
            resultText = resultText.Replace(" Records)", "");
            int indexOf = resultText.IndexOf("of ");
            Debug.WriteLine(resultText);
            resultText = resultText.Substring(indexOf+3);
            Debug.WriteLine(resultText);

            int rows = 0;
            bool isSuccess = int.TryParse(resultText, out rows);
            return rows;
        }


        public int SearchForTownTopResults(string townName, int topCount)
        {
            _driver.Navigate().GoToUrl(_searchUrl);

            IWebElement town = _wait.Until((d) => d.FindElement(By.XPath(@"//*[@id='ui-id-8']/form/input[2]")));
            //var town = _driver.FindElement(By.XPath(@"//*[@id='ui-id-8']/form/input[2]"));
            IWebElement submitBtn = _driver.FindElement(By.XPath(@"//*[@id='ui-id-8']/form/input[6]"));

            town.SendKeys("ant");
            submitBtn.Click();


            string recordsXpath = "//table[@class='statTable']/following-sibling::div/div/span";
            //(1 - 50 of 1102 Records)
            IWebElement elSpan = _wait.Until(ExpectedConditions.ElementExists(By.XPath(recordsXpath)));
            string resultText = elSpan.Text;
            resultText = resultText.Replace(" Records)", "");
            int indexOf = resultText.IndexOf("of ");
             resultText = resultText.Substring(indexOf + 3);
            
            int rowCount = 0;
            bool isSuccess = int.TryParse(resultText, out rowCount);
           

            string gridXpath = @"//table[@class='statTable']";
            IWebElement resultGrid = _wait.Until(ExpectedConditions.ElementExists(By.XPath(gridXpath)));
            
            //now have results in a grid   
            var rows = resultGrid.FindElements(By.XPath("tbody/tr"));

            List<string> zipUrls = new List<string>();
            for (int i = 1; i <= topCount; i++)//grid has header row, so skip
            {
                ////table[@class="statTable"]/tbody/tr[2]/td[1]/a[@href]
                var zipCell = rows[i].FindElement(By.XPath("td[1]/a[@href]"));
               
                var zipUrl = zipCell.GetAttribute("href");
               
                zipUrls.Add(zipUrl);
            }

            foreach (string url in zipUrls)
            {
                //For every one of them save City name, State, ZipCode, Longitude and Latitude
                _driver.Navigate().GoToUrl(url);

                //to help speed up finding of elements just do 1 wait
                string cityXpath = "//table[@class='statTable']/tbody/tr/td[1]/span[text() = 'City:']/parent::td/following-sibling::td[1]";
                var el = _wait.Until(ExpectedConditions.ElementExists(By.XPath(cityXpath)));

                ZipSearchResult result = new ZipSearchResult()
                {
                    City = this.ZipInfo("City:"),
                    State = this.ZipInfo("State:"),
                    ZipCode = this.ZipInfo("Zip Code:"),
                    Latitude = this.ZipInfo("Latitude:"),
                    Longitude = this.ZipInfo("Longitude:")
                };
                Debug.WriteLine(result);
                this.Results.Add(result);

                CreateMapScreenshot(result);
            }

            Debug.WriteLine(this.Results.Count);
            return rowCount;
        }

        private void CreateMapScreenshot(ZipSearchResult result)
        {
            //Save each screenshot on the disk with file name <CityName>-<State>-<ZipCode>.jpg, for example Paauilo- Hawaii- 96776.jpg
            _driver.SwitchTo().NewWindow(WindowType.Tab);
            _driver.Navigate().GoToUrl(result.MapUrl);


            string tempFilePath = Path.GetTempPath();           
            //can only do PNG and not JPEG
            string filename = $"{result.City}-{result.State}-{result.ZipCode}.{ScreenshotImageFormat.Png.ToString()}";
            string fullPath = Path.Combine(tempFilePath, filename);
            //Debug.WriteLine(fullPath);

            //need to wait for google maps to completely load the map before taking screenshot
             //_driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5.0);
            string someElXpath = @"//*[@id='pane']/div/div[1]/div/div/div[5]/div[1]/div/button";
            var el = _wait.Until(ExpectedConditions.ElementExists(By.XPath(someElXpath)));


            TakeFullScreenshot(this._driver, fullPath, ScreenshotImageFormat.Png);
        }

        private string ZipInfo(string itemName)
        {
            string itemXpath = $"//table[@class='statTable']/tbody/tr/td[1]/span[text() = '{itemName}']/parent::td/following-sibling::td[1]";
            var el = _driver.FindElement(By.XPath(itemXpath));

            return el.Text;
        }

        private void TakeFullScreenshot(IWebDriver driver, String filename, ScreenshotImageFormat format)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(filename, format);
        }
    }
}
