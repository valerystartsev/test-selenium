using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
// ReSharper disable once RedundantUsingDirective
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Assert = NUnit.Framework.Assert;

// ReSharper disable once CheckNamespace
namespace missions
{
    [TestFixture]
    public class Mission8Tests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Init()
        {
            _driver = new ChromeDriver();
            //_driver = new FirefoxDriver();
            //_driver = new InternetExplorerDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
        }

        [TearDown]
        public void Stop()
        {
            _driver.Quit();
            _driver = null;
        }

        [Test]
        public void Test_CountStickers()
        {
            /*
             * Tect выполняет следующие действия в учебном приложении litecart.
             * Сделайте сценарий, проверяющий наличие стикеров у всех товаров 
             * в учебном приложении litecart на главной странице. 
             * Стикеры -- это полоски в левом верхнем углу изображения товара, 
             * на которых написано New или Sale или что-нибудь другое. 
             * Сценарий должен проверять, что у каждого товара имеется ровно один стикер.
             */

            LoginToMainPage();

            // find Most Popular
            // IList<IWebElement> productList = _driver.FindElements(By.CssSelector("div.box#box-most-popular ul.listing-wrapper.products li.product"));
            //
            CheckStickers("box-most-popular", By.CssSelector("div.box#box-most-popular ul.listing-wrapper.products li.product"));

            CheckStickers("box-campaigns", By.CssSelector("div.box#box-campaigns ul.listing-wrapper.products li.product"));

            CheckStickers("box-latest-products", By.CssSelector("div.box#box-latest-products ul.listing-wrapper.products li.product"));

        }

        private void CheckStickers(string id, By locator)
        {
            if (AreElementsPresent(_driver, locator))
            {
                IList<IWebElement> list = _driver.FindElements(locator);
                for (int i = 0; i < list.Count; i++)
                {
                    IList<IWebElement> stickers = list[i].FindElements(By.CssSelector("div[class*='sticker']"));
                    Assert.True(stickers.Count == 1, "id {0}, product {1} has more one sticker", id, list[i].FindElement(By.CssSelector("div.name")).Text);
                }
                Console.WriteLine(id + ","+list.Count );
            }
        }

        

        private void LoginToMainPage()
        {
            _driver.Url = "http://litecart:81";

            // wait login is sucessfull and logout icon is visible
            _wait.Until(ExpectedConditions.TitleContains("Online Store"));
        }

        private bool AreElementsPresent(IWebDriver  driver, By locator)
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
