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
    public class Mission3Tests
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
        public void Test1_Login2AdminPanel()
        {
            _driver.Url = "http://litecart:81/admin/login.php";
            
            // login box is visible
            _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("box-login")));
            // fill the fields and click login
            _driver.FindElement(By.Name("username")).SendKeys("admin");
            _driver.FindElement(By.Name("password")).SendKeys("admin");
            _driver.FindElement(By.Name("login")).Click();
            
            // wait login is sucessfull and logout icon is visible
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".fa-sign-out")));
        }

        [TearDown]
        public void Stop()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
