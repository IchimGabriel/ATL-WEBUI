using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System;
using Xunit;

namespace ATL.WebUI.Tests.SeleniumTests
{
    /// <summary>
    /// 
    /// </summary>
    public class WebDriverPageTests
    {
        private IWebDriver _driver;

        [Theory]
        [InlineData("Edge")]
        public void Search_For_DotNet_Core(string browserName)
        {
            string url = "https://google.com";

            using (_driver = CreateWebDriver(browserName))
            {
                _driver.Navigate().GoToUrl(url);
                _driver.FindElement(By.Name("q")).SendKeys("dotnet core" + Keys.Enter);
                _driver.Close();
            }
        }


        public IWebDriver CreateWebDriver(string browserName)
        {
            switch (browserName.ToLowerInvariant())
            {
                case "edge":
                    return new EdgeDriver();

                default:
                    throw new NotSupportedException($"The browser '{browserName}' is not supported.");
            }
        }

        [Fact]
        public void Assert_True_Page_Title_Privacy()
        {
            _driver = new EdgeDriver();
            try
            {
                _driver.Navigate().GoToUrl("http://localhost:5004/Home/Privacy");
                Assert.Equal("Privacy Policy - ATL-FF", _driver.Title);
            }
            finally
            {
                _driver.Quit();
            }
        }

        [Fact]
        public void Assert_False_Page_Title_Privacy()
        {
            _driver = new EdgeDriver();
            try
            {
                _driver.Navigate().GoToUrl("http://localhost:5004/Home/Privacy");
                Assert.NotEqual("Policy - ATL-FF App", _driver.Title);
            }
            finally
            {
                _driver.Quit();
            }
        }

    }
}
