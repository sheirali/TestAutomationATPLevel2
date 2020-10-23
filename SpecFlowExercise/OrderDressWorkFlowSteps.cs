using System.Diagnostics;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System;
using SpecFlowExercise.Pages;
//using WebDriver.SpecFlow.Demos.Core;
//using WebDriver.SpecFlow.Demos.Pages;
//using WebDriver.SpecFlow.Demos.Pages.ItemPage;
//using WebDriver.SpecFlow.Demos.Pages.SecondsToMinutesPage;


namespace SpecFlowExercise
{
    [Binding]
    public class OrderDressWorkFlowSteps
    {
        private DressPage _dressPage;

        public OrderDressWorkFlowSteps()
        {
            _dressPage = new DressPage();
        }

        ~OrderDressWorkFlowSteps()
        {
            _dressPage = null;
        }

        [When(@"I navigate to Dress Page")]
        public void WhenINavigateToDressPage()
        {
            _dressPage.Open();
        }

        [When(@"buy dresses")]
        [When(@"add dresses")]
        public void WhenAddDresses(Table dressesTable)
        {
            var dressInfo = dressesTable.CreateDynamicSet();
            foreach (var info in dressInfo)
            {
                //string navigateTo = string.Concat(info.Url, info.ProductId);
                ////Debug.WriteLine($"URL={info.Url} DressID={info.ProductId}");
                //Debug.WriteLine($"navigateTo={navigateTo}");
                //navigateTo=/index.php?controller=product&id_product=3
                //_dressPage.NavigateTo(navigateTo);

                //its a hyperlink that I need to click to add to Compare
                _dressPage.AddToCompare(info.ProductId);
                ////a[@data-id-product='5'][contains(text(), 'Add to Compare')]
            }
        }

        [Then(@"assert (.*) dresses added to compare")]
        public void ThenAssertDressesAddedToCompare(int dressCountToCompare)
        {
            _dressPage.AssertCompareCount(dressCountToCompare);
        }


        [When(@"pick dresses for quickView")]
        public void WhenPickDressesForQuickView(Table dressesTable)
        {
            var dressInfo = dressesTable.CreateDynamicSet();
            foreach (var info in dressInfo)
            {
                _dressPage.QuickView(info.ProductId);
            }
        }
    }
}
