using System;

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

// ReSharper disable once CheckNamespace
namespace missions
{
    [TestFixture]
    public class Mission4Tests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Init()
        {
            //_driver = new ChromeDriver();
            //_driver = new FirefoxDriver();
            //_driver = new InternetExplorerDriver();
            //_wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void Test1_ChromeCapabilities()
        {

            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            _driver.Url = "http://ya.ru";
            _driver.FindElement(By.Name("text")).SendKeys("webdriver");
            _driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            _wait.Until(ExpectedConditions.TitleContains("webdriver — Яндекс: нашлось"));
        }
        [Test]
        public void Test2_FireFoxCapabilities()
        {
            FirefoxOptions options = new FirefoxOptions();
            _driver = new FirefoxDriver(options);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            var strOptions = options.ToCapabilities().ToString();
            Console.WriteLine("IE started, caps are:" + strOptions);
            Console.WriteLine("line2="+options.ToString());
            //var caps = ((IHasCapabilities) _driver).Capabilities;
            //var isproxy = caps.GetCapability("proxy");
            //Console.WriteLine("line3="+caps.ToString()+" :"+isproxy.ToString());
            _driver.Url = "http://ya.ru";
            //_driver.FindElement(By.Name("text")).SendKeys("webdriver");
            //_driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //_wait.Until(ExpectedConditions.TitleContains("webdriver — Яндекс: нашлось"));
        }

        [Test]
        public void Test3_IECapabilities()
        {

            InternetExplorerOptions options = new InternetExplorerOptions();
            
            _driver = new InternetExplorerDriver(options);
            options.UnexpectedAlertBehavior = InternetExplorerUnexpectedAlertBehavior.Ignore;
            //options.SetLoggingPreference(LogType.);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            var strOptions = options.ToCapabilities().ToString();
            Console.WriteLine("IE started, caps are:"+strOptions);
            _driver.Url = "http://ya.ru";
            //_driver.FindElement(By.Name("text")).SendKeys("webdriver");
            //_driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            //_wait.Until(ExpectedConditions.TitleContains("webdriver — Яндекс: нашлось"));
        }

        [TearDown]
        public void Stop()
        {
            _driver.Quit();
            _driver = null;
        }
    }
}
