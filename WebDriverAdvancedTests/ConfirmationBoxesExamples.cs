using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace WebDriverAdvancedTests
{
    [TestClass]
    public class ConfirmationBoxesExamples
    {
        private IWebDriver _driver;

        [TestInitialize]
        public void TestInit()
        {
            _driver = new ChromeDriver();
            _driver.Navigate().GoToUrl("https://demoqa.com/alerts");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void Accept_ConfirmationBox()
        {
            var confirmButton = _driver.FindElement(By.Id("confirmButton"));
            confirmButton.Click();

            var alert = _driver.SwitchTo().Alert();

            Assert.AreEqual("Do you confirm action?", alert.Text);

            alert.Accept();
        }

        [TestMethod]
        public void Dismiss_ConfirmationBox()
        {
            var confirmButton = _driver.FindElement(By.Id("confirmButton"));
            confirmButton.Click();

            var alert = _driver.SwitchTo().Alert();

            Assert.AreEqual("Do you confirm action?", alert.Text);

            alert.Dismiss();
        }

        [TestMethod]
        public void TypeText_ConfirmationBox()
        {
            var promtButton = _driver.FindElement(By.Id("promtButton"));
            promtButton.Click();

            var alert = _driver.SwitchTo().Alert();
            
            Assert.AreEqual("Please enter your name", alert.Text);

            alert.SendKeys("Anton");
            alert.Accept();

            Assert.AreEqual("Anton", alert.Text);
        }
    }
}
