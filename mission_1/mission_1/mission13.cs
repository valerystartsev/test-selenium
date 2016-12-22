using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenQA.Selenium;
// ReSharper disable once RedundantUsingDirective
using OpenQA.Selenium.Firefox;
// ReSharper disable once RedundantUsingDirective
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

// ReSharper disable once CheckNamespace
namespace missions
{
    [TestFixture]
    public class Mission13Tests
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Init()
        {
            //_driver = new ChromeDriver();
            _driver = new FirefoxDriver();
            //_driver = new InternetExplorerDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));

            LoginToMainPage();
        }

        [TearDown]
        public void Stop()
        {
            _driver.Quit();
            _driver = null;
        }

        [Test]
        public void Test_Basket_AddRemove()
        {
            /*
             * Tect выполняет следующие действия в учебном приложении litecart.
             * Сделайте сценарий для добавления товаров в корзину и удаления товаров из корзины.
             * Сценарий должен состоять из следующих частей:
             * 1) открыть страницу какого-нибудь товара
             * 2) добавить его в корзину
             * 3) подождать, пока счётчик товаров в корзине обновится
             * 4) вернуться на главную страницу, повторить предыдущие шаги ещё два раза, чтобы в общей сложности в корзине было 3 единицы товара
             * 5) открыть корзину (в правом верхнем углу кликнуть по ссылке Checkout)
             * 6) удалить все товары из корзины один за другим, после каждого удаления подождать, пока внизу обновится таблица
             */
            // 
            for (int i = 0; i < 3; i++)
            {
                AddItemToCart();
            }

            // click Checkoput button
            var btnCart = _driver.FindElement(By.CssSelector("div#cart-wrapper div#cart a.link"));
            btnCart.Click();
            _wait.Until(ExpectedConditions.StalenessOf(btnCart));

            RemoveItemsFromCart();
        }

        private void RemoveItemsFromCart()
        {
            // list of items
            IList<IWebElement> itemList = _driver.FindElements(By.CssSelector("div#checkout-cart-wrapper ul.shortcuts"));
            // click on first to stop carusel
            itemList[0].Click();

            // remember number of lines in the bottom table
            var items = _driver.FindElements(By.CssSelector("td.item"));
            Console.WriteLine("items.count={0}", items.Count);
            while (items.Count != 0)
            {
                // click remove item
                var btnRemove = _driver.FindElement(By.CssSelector("button[name=remove_cart_item]"));

                if (btnRemove.Displayed && btnRemove.Enabled)
                {
                    Console.WriteLine("Click!");
                    btnRemove.Click();
                }

                _wait.Until(ExpectedConditions.StalenessOf(items[0]));
                // reread table after refresh
                items = _driver.FindElements(By.CssSelector("td.item"));
            }
        }

        private void AddItemToCart()
        {
            // click first product in Most-Popular
            if (!AreElementsPresent(_driver, By.CssSelector("div#box-most-popular ul.listing-wrapper.products li a.link")))
                Assert.Fail("can't find product list");

            var firstProduct = _driver.FindElement(By.CssSelector("div#box-most-popular ul.listing-wrapper.products li a.link"));
            firstProduct.Click();
            _wait.Until(ExpectedConditions.StalenessOf(firstProduct));

            // product page is shown
            // need select Size if product has a size selection
            if (IsElementPresent(_driver, By.CssSelector("div.buy_now td.options select[name*=options]")))
            {
                // select last size in the list
                var options = _driver.FindElement(By.CssSelector("div.buy_now td.options select[name*=options]"));
                var select = new SelectElement(options);
                @select.SelectByIndex(@select.Options.Count - 1);
            }
            // remember current number of items in cart
            var prevQuantity = _driver.FindElement(By.CssSelector("span.quantity")).Text;

            // add product to cart
            _driver.FindElement(By.CssSelector("button[name=add_cart_product]")).Click();

            // wait until number of items changed
            _wait.Until(d => d.FindElement(By.CssSelector("span.quantity")).Text != prevQuantity);

            // 
            var div_product = _driver.FindElement(By.CssSelector("div#box-product"));

            // click Home icon
            _driver.FindElement(By.CssSelector("li.general-0 a")).Click();
            // wait product page is closed
            _wait.Until(ExpectedConditions.StalenessOf(div_product));
        }


        private void LoginToMainPage()
        {
            _driver.Url = "http://litecart:81";

            // wait login is sucessfull and logout icon is visible
            _wait.Until(ExpectedConditions.TitleContains("Online Store"));
        }


        public bool AreElementsPresent(IWebDriver driver, By locator)
        {
            driver.FindElement(locator);
            return driver.FindElements(locator).Count > 0;
        }

        public bool IsElementPresent(IWebDriver driver, By locator)
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
