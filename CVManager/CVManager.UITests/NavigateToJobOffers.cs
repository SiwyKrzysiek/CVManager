using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CVManager.UITests
{
    public class NavigateToJobOffers
    {
        private IWebDriver _driver;
        string _applicationName = "CVManager";
        const int iisPort = 2020;
        private Process _iisProcess;

        [SetUp]
        public void Setup()
        {
            //StartIIS();


            var projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.Parent.FullName;
            var driverFolder = Path.Combine(projectDirectory, @"chromedriver_win32");

            _driver = new ChromeDriver(driverFolder); //Open bew browser
        }

        [TearDown]
        public void TestCleanup()
        {
            //// Ensure IISExpress is stopped
            //if (_iisProcess.HasExited == false)
            //{
            //    _iisProcess.Kill();
            //}
        }

        private void StartIIS()
        {
            var applicationPath = GetApplicationPath(_applicationName);
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

            _iisProcess = new Process();
            _iisProcess.StartInfo.FileName = programFiles + @"\IIS Express\iisexpress.exe";
            _iisProcess.StartInfo.Arguments = string.Format("/path:\"{0}\" /port:{1}", applicationPath, iisPort);
            _iisProcess.Start();
        }

        protected virtual string GetApplicationPath(string applicationName)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)));
            return Path.Combine(solutionFolder, applicationName);
        }

        public string GetAbsoluteUrl(string relativeUrl)
        {
            if (!relativeUrl.StartsWith("/"))
            {
                relativeUrl = "/" + relativeUrl;
            }
            return String.Format("http://localhost:{0}{1}", iisPort, relativeUrl);
        }

        [Test]
        public void TryToNavigateToJobOffers()
        {
            //_driver.Navigate().GoToUrl("http://www.google.com"); //navigate to page
            //var query = _driver.FindElement(By.Name("q"));
            //query.SendKeys("Hello Selenium!");
            //query.Submit();

            //_driver.Navigate().GoToUrl(GetAbsoluteUrl(@"/index"));

            _driver.Navigate().GoToUrl(@"https://cvmanagerkrzysztofdabrowski.azurewebsites.net"); //Go to index

            var jobOffersButton = _driver.FindElement(By.LinkText("Job offers"));
            Assert.IsNotNull(jobOffersButton);

            jobOffersButton.Click();


            var headers2 = _driver.FindElements(By.TagName("h2"));

            //On offers page there should be such header
            Assert.IsTrue(headers2.Any(h => h.Text == "Job Offer list"));

            _driver.Close();

            Assert.Pass();
        }
    }
}