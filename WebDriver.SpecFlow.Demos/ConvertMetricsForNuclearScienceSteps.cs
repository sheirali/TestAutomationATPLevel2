using System;
using WebDriver.SpecFlow.Demos.Core;
using WebDriver.SpecFlow.Demos.Pages;
using TechTalk.SpecFlow;
using WebDriver.SpecFlow.Demos.Pages.ItemPage;
using WebDriver.SpecFlow.Demos.Pages.SecondsToMinutesPage;
using TechTalk.SpecFlow.Assist;

namespace WebDriver.SpecFlow.Demos
{
    [Binding]
    public class ConvertMetricsForNuclearScienceSteps
    {
        private HomePage _homePage;
        private KilowattHoursPage _kilowattHoursPage;
        private readonly SecondsToMinutesPage _secondsToMinutesPage;

        public ConvertMetricsForNuclearScienceSteps()
        {
            _homePage = new HomePage(Driver.Browser);
            _kilowattHoursPage = new KilowattHoursPage(Driver.Browser);
            _secondsToMinutesPage = new SecondsToMinutesPage(Driver.Browser);
        }

        [Given(@"web browser is opened")]
        public void GivenWebBrowserIsOpened()
        {
            Driver.StartBrowser(BrowserTypes.Chrome);
        }

        [Then(@"close web browser")]
        public void ThenCloseWebBrowser()
        {
            Driver.StopBrowser();
        }

        [When(@"I navigate to Metric Conversions")]
        public void WhenINavigateToMetricConversions_()
        {
            _homePage = new HomePage(Driver.Browser);
            _homePage.Open();
        }

        [When(@"navigate to Energy and power section")]
        public void WhenNavigateToEnergyAndPowerSection()
        {
            _homePage.EnergyAndPowerAnchor.Click();
        }

        [When(@"navigate to Kilowatt-hours")]
        public void WhenNavigateToKilowatt_Hours()
        {
            _homePage.KilowattHours.Click();
        }

        [When(@"choose conversions to Newton-meters")]
        public void WhenChooseConversionsToNewton_Meters()
        {
            _kilowattHoursPage = new KilowattHoursPage(Driver.Browser);
            _kilowattHoursPage.KilowatHoursToNewtonMetersAnchor.Click();
        }

        [When(@"type (.*) kWh")]
        public void WhenTypeKWh(double kWh)
        {
            _kilowattHoursPage.ConvertKilowattHoursToNewtonMeters(kWh);
        }

        [Then(@"assert that (.*) Nm are displayed as answer")]
        public void ThenAssertThatENmAreDisplayedAsAnswer(string expectedNewtonMeters)
        {
            _kilowattHoursPage.AssertFahrenheit(expectedNewtonMeters);
        }

        [When(@"I navigate to Seconds to Minutes Page")]
        public void WhenINavigateToSecondsToMinutesPage()
        {
            _secondsToMinutesPage.Open();
        }

        [When(@"type (.*) kWh in (.*) format")]
        public void WhenTypeKWhInFormat(double kWh, Format format)
        {
            _kilowattHoursPage.ConvertKilowattHoursToNewtonMeters(kWh, format);
        }

        [When(@"type seconds for (.*)")]
        public void WhenTypeSeconds(TimeSpan seconds)
        {
            _secondsToMinutesPage.ConvertSecondsToMintes(seconds.TotalSeconds);
        }

        [Then(@"assert that (.*) minutes are displayed as answer")]
        public void ThenAssertThatSecondsAreDisplayedAsAnswer(int expectedMinutes)
        {
            _secondsToMinutesPage.AssertMinutes(expectedMinutes.ToString());
        }

        [StepArgumentTransformation(@"(?:(\d*) day(?:s)?(?:, )?)?(?:(\d*) hour(?:s)?(?:, )?)?(?:(\d*) minute(?:s)?(?:, )?)?(?:(\d*) second(?:s)?(?:, )?)?")]
        public TimeSpan TimeSpanTransform(string days, string hours, string minutes, string seconds)
        {
            int daysParsed;
            int hoursParsed;
            int minutesParsed;
            int secondsParsed;

            int.TryParse(days, out daysParsed);
            int.TryParse(hours, out hoursParsed);
            int.TryParse(minutes, out minutesParsed);
            int.TryParse(seconds, out secondsParsed);

            return new TimeSpan(daysParsed, hoursParsed, minutesParsed, secondsParsed);
        }

        [When(@"add products")]
        public void NavigateToItemUrl(Table productsTable)
        {
            var itemPage = new ItemPage(Driver.Browser);
            var products = productsTable.CreateDynamicSet();
            foreach (var product in products)
            {
                itemPage.Navigate(string.Concat(product.Url, "?", product.AffilicateCode));
                itemPage.ClickBuyNowButton();
            }
        }
    }
}