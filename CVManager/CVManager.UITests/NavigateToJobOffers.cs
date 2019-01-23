using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tests
{
    public class NavigateToJobOffers
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver(@"D:\Studia\ASP.NETCore_MiNI2018\CVManager\chromedriver_win32"); //Open bew browser
        }

        [Test]
        public void Test1()
        {
            _driver.Navigate().GoToUrl("http://www.google.com"); //navigate to page
            var query = _driver.FindElement(By.Name("q"));
            query.SendKeys("Hello Selenium!");
            query.Submit();

            _driver.Close();

            Assert.Pass();
        }
    }
}