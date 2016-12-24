using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace mission19
{
    public class Application
    {
        private IWebDriver driver;
     
        private ProductListPage productListPage;
        private ProductPage productPage;
        private CartPage cartPage;

        public Application()
        {
            driver = new ChromeDriver();
            
            productListPage = new ProductListPage(driver);
            productPage = new ProductPage(driver);
            cartPage = new CartPage(driver);
        }

        public void Quit()
        {
            driver.Quit();
        }

        
        
        internal ISet<string> GetMostPopularProductLinks()
        {
            return productListPage.Open().GetMostPopularLinks();
        }

        internal ProductListPage GetProductList()
        {
            return productListPage.Open() ;
        }

        internal ProductPage GetProduct()
        {
            return productPage;
        }

        internal CartPage GetCartPage()
        {
            return cartPage.Open();
        }

    }
}