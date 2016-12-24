using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace mission19
{
    internal class CartPage : Page
    {
        public CartPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        internal CartPage Open()
        {

            driver.Url = "http://litecart:81/en/checkout";
            //driver.FindElement(By.CssSelector("div#checkout-cart-wrapper ul.shortcuts")).Click();
            IList<IWebElement> itemList = driver.FindElements(By.CssSelector("div#checkout-cart-wrapper ul.shortcuts"));
            // click on first to stop carusel
            itemList[0].Click();
            return this;
        }

        //var items = _driver.FindElements(By.CssSelector("td.item"));
        [FindsBy(How = How.CssSelector, Using = "td.item")]
        IList<IWebElement> itemsList;

        //var btnRemove = _driver.FindElement(By.CssSelector("button[name=remove_cart_item]"));
        [FindsBy(How = How.CssSelector, Using = "button[name=remove_cart_item]")]
        internal IWebElement RemoveFromCartButton;

        internal void RemoveAllItemsFromCart()
        {
            /*
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

             */
            //foreach (var item in itemsList)
            while (itemsList.Count > 0)  
            {
                RemoveFromCartButton.Click();
                wait.Until(ExpectedConditions.StalenessOf(itemsList[0]));
            }

        }
    }
}