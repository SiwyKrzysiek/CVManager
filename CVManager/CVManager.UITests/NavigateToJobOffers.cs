using System;
using System.Diagnostics;
using System.IO;
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
            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var driverFolder = projectDirectory + @"\chromedriver_win32";

            Debug.WriteLine(projectDirectory);
            _driver = new ChromeDriver(driverFolder); //Open bew browser
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