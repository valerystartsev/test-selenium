using System;

using NUnit.Framework;
using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
//using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

// ReSharper disable once CheckNamespace
namespace missions
{
    [TestFixture]
    public class Mission1Tests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Init()
        {
            //_driver = new ChromeDriver();
            _driver = new FirefoxDriver();
            //_driver = new InternetExplorerDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test1_SearchInYandex()
        {
            _driver.Url = "http://ya.ru";
            _driver.FindElement(By.Name("text")).SendKeys("webdriver");
            _driver.FindElement(By.XPath("//button[@type='submit']")).Click();
            
            _wait.Until(ExpectedConditions.TitleContains("webdriver — Яндекс: нашлось"));
        }

        [TearDown]
        public void Stop()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
