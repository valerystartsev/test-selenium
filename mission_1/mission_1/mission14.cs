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
    public class Mission14Tests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Init()
        {
            /* // only for Chrome
            ChromeOptions options = new ChromeOptions();
            options.SetLoggingPreference(LogType.Browser, LogLevel.All);
            */

            //_driver = new ChromeDriver(options);
            _driver = new FirefoxDriver();
            //_driver = new InternetExplorerDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            LoginToPanel();
        }

        [TearDown]
        public void Stop()
        {
            // check browser log
            /* // only for Chrome
            foreach (LogEntry l in _driver.Manage().Logs.GetLog("browser"))
            {
                Console.WriteLine(l);
            }
            */
            _driver.Quit();
            _driver = null;
        }

        [Test]
        public void Test_OpenWindows()
        {
            /* 
             * сценарий, который проверяет, что ссылки на странице редактирования страны открываются в новом окне.
             * Сценарий должен состоять из следующих частей:
             * 1) зайти в админку
             * 2) открыть пункт меню Countries (или страницу http://localhost/litecart/admin/?app=countries&doc=countries)
             * 3) открыть на редактирование какую-нибудь страну или начать создание новой
             * 4) возле некоторых полей есть ссылки с иконкой в виде квадратика со стрелкой -- они ведут на внешние страницы и 
             *    открываются в новом окне, именно это и нужно проверить.
             * Конечно, можно просто убедиться в том, что у ссылки есть атрибут target="_blank". Но в этом упражнении 
             * требуется именно кликнуть по ссылке, чтобы она открылась в новом окне, потом переключиться в новое окно, 
             * закрыть его, вернуться обратно, и повторить эти действия для всех таких ссылок.
             * Не забудьте, что новое окно открывается не мгновенно, поэтому требуется ожидание открытия окна.
             */

            ClickCountryMenuItem();
            // open new country 
            var btn = _driver.FindElement(By.CssSelector("td#content a.button[href*=edit_country]"));
            btn.Click();
            _wait.Until(ExpectedConditions.StalenessOf(btn));
            // list of hrefs
            IList<IWebElement> listHrefs = _driver.FindElements(By.CssSelector("td#content table a[href][target=_blank]"));

            foreach (var item in listHrefs)
            {

                var mainWindow = _driver.CurrentWindowHandle;
                ICollection<string> oldWindows = _driver.WindowHandles;

                Console.WriteLine(item.GetAttribute("href"));
                // open new window
                item.Click();
                // wait new window is opened
                _wait.Until( (d)=>_driver.WindowHandles.Count==(oldWindows.Count+1) );
                // switch to new window
                _driver.SwitchTo().Window(_driver.WindowHandles.Last());
                // do something in the new window
                //_driver.SwitchTo().ActiveElement().SendKeys(Keys.Control+Keys.End);
                // close new window
                _driver.Close();
                // switch back to old window
                _driver.SwitchTo().Window(mainWindow);

            }



        }



        private void ClickCountryMenuItem()
        {
            _wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("div#box-apps-menu-wrapper ul#box-apps-menu li a[href*=countries]")));

            _driver.FindElement(By.CssSelector("div#box-apps-menu-wrapper ul#box-apps-menu li a[href*=countries]")).Click();

            _wait.Until(
                ExpectedConditions.ElementToBeClickable(
                    By.CssSelector("td#content a.button[href*=edit_country]")));
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
