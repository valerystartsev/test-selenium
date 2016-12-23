using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
// ReSharper disable once RedundantUsingDirective
using OpenQA.Selenium.Firefox;
// ReSharper disable once RedundantUsingDirective
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Assert = NUnit.Framework.Assert;

// ReSharper disable once CheckNamespace
namespace missions
{
    [TestFixture]
    public class Mission17Tests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Init()
        {

            ChromeOptions options = new ChromeOptions();
            options.SetLoggingPreference(LogType.Browser, LogLevel.All);
             
            _driver = new ChromeDriver(options );
            //_driver = new FirefoxDriver();
            //_driver = new InternetExplorerDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            LoginToPanel();
        }

        [TearDown]
        public void Stop()
        {
            _driver.Quit();
            _driver = null;
        }

        [Test]
        public void Test_BrowserLog()
        {
            /* Проверьте отсутствие сообщений в логе браузера
             * Сделайте сценарий, который проверяет, не появляются ли в логе браузера сообщения при открытии страниц в учебном приложении, а именно -- страниц товаров в каталоге в административной панели.
             *
             * Сценарий должен состоять из следующих частей:
             * 1) зайти в админку
             * 2) открыть каталог, категорию, которая содержит товары (страница http://localhost/litecart/admin/?app=catalog&doc=catalog&category_id=1)
             * 3) последовательно открывать страницы товаров и проверять, не появляются ли в логе браузера сообщения (любого уровня)
             */
            ClickCatalogMenuItem();

            _driver.FindElement(By.CssSelector("a[href*='category_id=1']:not([title=Edit]") ).Click();

            // get elements with links
            IList<IWebElement> linkList = _driver.FindElements(By.CssSelector("a[href*='category_id=1'][href*=edit_product][href*=product_id]:not([title=Edit]"));

            // store links as text in array
            var hRefs = new List<string>();
            foreach (var item in linkList)
            {
                hRefs.Add(item.GetAttribute("href"));
            }

            var log = _driver.Manage().Logs.GetLog(LogType.Browser);

            var logLinesInitial = log.Count; 
            foreach (var item in hRefs)
            {
                var logLinesBefore = log.Count;
                _driver.Url = item;
                IList<IWebElement> tabList = _driver.FindElements(By.CssSelector("div.tabs ul li a"));
                foreach (var tab in tabList)
                {
                    tab.Click();
                }

                var logLinesAfter = log.Count;
                Console.WriteLine("{0} new records added to log for product {1}",logLinesAfter - logLinesBefore,item );

            }
            
            
                            
            // check browser log
            foreach (LogEntry l in log) //_driver.Manage().Logs.GetLog("browser"))
            {            
                Console.WriteLine(l);
            }
            Assert.True(logLinesInitial == log.Count, "New lines were added to log during test, see output for details");


        }


        private void ClickCatalogMenuItem()
        {
            _wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("div#box-apps-menu-wrapper ul#box-apps-menu li a[href*=catalog]")));

            _driver.FindElement(By.CssSelector("div#box-apps-menu-wrapper ul#box-apps-menu li a[href*=catalog]")).Click();

            _wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("td#content a.button[href*=edit_product]")));
        }


        private void LoginToPanel()
        {
            _driver.Url = "http://litecart:81/admin/login.php";

            // login box is visible
            _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("box-login")));
            // fill the fields and click login
            _driver.FindElement(By.Name("username")).SendKeys("admin");
            _driver.FindElement(By.Name("password")).SendKeys("admin");
            _driver.FindElement(By.Name("login")).Click();

            // wait login is sucessfull and logout icon is visible
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("i.fa.fa-sign-out.fa-lg")));
        }


        public bool AreElementsPresent(IWebDriver driver, By locator)
        {
            driver.FindElement(locator);
            return driver.FindElements(locator).Count > 0;
        }

        public bool IsElementPresent(IWebElement driver, By locator)
        {
            try
            {
                driver.FindElement(locator);
                return true;
            }
            // ReSharper disable once UnusedVariable
            catch (NoSuchElementException ex)
            {
                return false;
            }
        }
    }
}
