using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace mission19
{
    internal class Page
    {
        protected IWebDriver driver;
        protected WebDriverWait wait;

        public Page(IWebDriver driver)
        {
            this.driver = driver;
            this.wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        }
    }
}