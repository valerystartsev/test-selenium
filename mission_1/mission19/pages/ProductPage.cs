using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Support.PageObjects;

namespace mission19
{
    internal class ProductPage : Page
    {
        public ProductPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        internal ProductPage Open(string click)
        {
            //click.Click();
            driver.Url = click;//"http://litecart:81/en/create_account";
            return this;
        }

        [FindsBy(How = How.CssSelector , Using = "div#box-product.box h1.title")]
        internal IWebElement ProductName;
        [FindsBy(How = How.CssSelector, Using = "div#box-product.box div.content div.information div.price-wrapper")]
        internal IWebElement ProductPrice;

        [FindsBy(How = How.CssSelector, Using = "div.buy_now tbody input[name=quantity]")]
        internal IWebElement SetQuantity;

        [FindsBy(How = How.CssSelector , Using = "div.buy_now tbody button[name=add_cart_product]")]
        internal IWebElement AddToCartButton;

        [FindsBy(How = How.CssSelector, Using = "div.buy_now td.options select[name*=options]")]
        internal IWebElement SizeSelector;

        [FindsBy(How = How.CssSelector, Using = "span.quantity")]
        internal IWebElement CurrentQuantity;


        internal bool IsElementPresent(By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }

        internal void ClickAddToCart(int amout)
        {
            SetQuantity.SendKeys(Keys.Home);
            SetQuantity.SendKeys(Keys.Control + "a");
            SetQuantity.SendKeys(amout.ToString());
            AddToCartButton.Click(); 
        }

        internal void ClickAddToCart()
        {
            // need select Size if product has a size selection
            //if (SizeSelector != null)
            if (IsElementPresent(By.CssSelector("div.buy_now td.options select[name*=options]")))

            {
                var select = new SelectElement(SizeSelector);
                @select.SelectByIndex(@select.Options.Count - 1);
                
            } 
            // remember current number of items in cart
            var prevQuantity = CurrentQuantity.Text;
            
            AddToCartButton.Click();

            wait.Until(d => CurrentQuantity.Text != prevQuantity);
            // wait until number of items changed
            //_wait.Until(d => d.FindElement(By.CssSelector("span.quantity")).Text != prevQuantity);

        }
    }
}