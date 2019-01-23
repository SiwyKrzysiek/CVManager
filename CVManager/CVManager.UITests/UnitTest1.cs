using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Tests
{
    public class Tests
    {
        private IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver(@"D:\Studia\ASP.NETCore_MiNI2018\CVManager\chromedriver_win32"); //Open bew browser
        }

        [Test]
        public void Test1()
        {
            driver.Navigate().GoToUrl("http://www.google.com"); //navigate to page
            var query = driver.FindElement(By.Name("q"));
            query.SendKeys("Hello Selenium!");
            query.Submit();

            driver.Close();

            Assert.Pass();
        }
    }
}