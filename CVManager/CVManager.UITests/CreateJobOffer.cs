using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CVManager.UITests
{
    class CreateJobOffer
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var driverFolder = Path.Combine(projectDirectory, @"chromedriver_win32");

            _driver = new ChromeDriver(driverFolder); //Open bew browser
        }

        [Test]
        public void TryToSubmitNewJobOffer()
        {
            //_driver.Navigate().GoToUrl("http://www.google.com"); //navigate to page
            //var query = _driver.FindElement(By.Name("q"));
            //query.SendKeys("Hello Selenium!");
            //query.Submit();

            _driver.Navigate().GoToUrl(@"https://cvmanagerkrzysztofdabrowski.azurewebsites.net/JobOffer"); //Go to index

            var createButton = _driver.FindElement(By.LinkText("Create job offer"));
            Assert.IsNotNull(createButton);

            createButton.Click();

            var headers2 = _driver.FindElements(By.TagName("h2"));

            //On creation form there should be such header
            Assert.IsTrue(headers2.Any(h => h.Text == "New Job Offer"));

            _driver.FindElement(By.Id("JobTitle")).SendKeys("Software tester");
            _driver.FindElement(By.Id("SalaryFrom")).SendKeys("4000");
            _driver.FindElement(By.Id("SalaryTo")).SendKeys("6500");
            _driver.FindElement(By.Id("Description")).SendKeys("LED Solar System to młoda i dynamicznie rozwijająca się firma działająca w obszarze nowoczesnych technologii:  oświetlenia LED, lamp LED, laserów półprzewodnikowych, DAC audio, systemów fotowoltaicznych. Wraz z rozwojem firmy  poszerzamy kanały sprzedaży oraz przygotowujemy się do wdrożenia innowacyjnych produktów. Prowadzimy badania\r\ni jesteśmy na etapie prototypownia innowacyjnych lamp LED dla przemysłu oraz lamp LED do wzrostu roślin. Aktualnie jesteśmy na etapie automatyzacji procesów sprzedażowych\r\ni wdrażania produktów do sklepu internetowego. ");

            //_driver.Close();

            Assert.Pass();
        }
    }
}
