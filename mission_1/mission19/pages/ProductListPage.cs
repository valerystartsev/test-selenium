using System.Linq;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace mission19
{
    internal class ProductListPage : Page
    {
        public ProductListPage(IWebDriver driver)
            : base(driver)
        {
            PageFactory.InitElements(driver, this);
        }

        internal ProductListPage Open()
        {
            driver.Url = "http://litecart:81/en/";
            return this;
        }

        [FindsBy(How = How.CssSelector, Using = "div#box-product.box h1.title")]
        internal IWebElement CheckouBtn;

        [FindsBy(How = How.CssSelector, Using = "div#box-most-popular ul.listing-wrapper.products li.product.column.shadow.hover-light")]
        IList<IWebElement> productElements;

        internal ISet<string> GetMostPopularLinks()
        {
            foreach (var item in productElements)
            {
                var link = item.FindElement(By.CssSelector("a[href^=http]"));
                var href = link.GetAttribute("href");
            }

            ISet<string> list = new HashSet<string>(productElements.Select(e => e.FindElement(By.CssSelector("a.link[href^=http]")).GetAttribute("href")));
            return list;
            //return (IList<string>) list; //productElements ;
            //return new HashSet<string>(productElements.Select(e => e.FindElements(By.CssSelector("a.link[href^=http]")))..GetAttribute("href")));
        }

        internal void DoCheckout()
        {
            
        }

        /*
        [FindsBy(How = How.CssSelector, Using = "table.dataTable tr.row")]
        IList<IWebElement> customerRows;
        */
        /*
        internal ISet<string> GetMostPopularIds()
        {
            return new HashSet<string>(
                customerRows.Select(e => e.FindElements(By.TagName("td"))[2].Text).ToList());
        }
         */
    }
}