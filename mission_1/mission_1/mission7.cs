using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

// ReSharper disable once CheckNamespace
namespace missions
{
    [TestFixture]
    public class Mission7Tests
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
        }

        [TearDown]
        public void Stop()
        {
            _driver.Quit();
            _driver = null;
        }

        [Test]
        public void Test_ClickAllItemsInMenuAndSubMenu()
        {
            /*
             * Tect выполняет следующие действия в учебном приложении litecart.
            1) входит в панель администратора http://localhost/litecart/admin
            2) прокликивает последовательно все пункты меню слева, включая вложенные пункты
            3) для каждой страницы проверяет наличие заголовка
             */

            LoginToPanel();

            // find box-apps-menu
            // IList<IWebElement> menuItemsList = _driver.FindElements(By.CssSelector("ul#box-apps-menu li#app-"));
            //
            IList<IWebElement> menuItemsList = GetMainMenuList();
            
            int menuItemsListCount = menuItemsList.Count;

            Assert.True(menuItemsListCount == 17);

            for (int i=0; i < menuItemsListCount; i++)
            {
                // find link to click
                var hrefLink = GetFirstHRefLink(menuItemsList, i);

                var expectedUrl = hrefLink.GetAttribute("href");
                
                hrefLink.Click();

                // Browser URL  
                WaitUrlIs(expectedUrl);

                // find H1 with name of Title
                var expectedTitle = WaitH1IsVisibleAndReturnText();
                //var expectedTitle = _driver.FindElement(By.CssSelector("h1")).Text ;
                WaitTitleIs(expectedTitle);
                
                // re-read main menu items list after click
                menuItemsList = GetMainMenuList();

                Assert.True(menuItemsList.Count==menuItemsListCount);

                // check for submenus
                if (AreElementsPresent(menuItemsList[i], By.CssSelector("ul.docs")))
                {
                    menuItemsList = ClickSubMenuItems(menuItemsList, i);
                }
                
            }
        }

        private static IWebElement GetFirstHRefLink(IList<IWebElement> menuItemsList, int i)
        {
            return menuItemsList[i].FindElement(By.CssSelector("a[href*='http://litecart']"));
        }

        private ReadOnlyCollection<IWebElement> GetMainMenuList()
        {
            return _driver.FindElements(By.CssSelector("ul#box-apps-menu li#app-"));
        }

        private static ReadOnlyCollection<IWebElement> GetSubMenuList(IList<IWebElement> menuItemsList, int i)
        {
            return menuItemsList[i].FindElements(By.CssSelector("ul.docs li"));
        }

        private void WaitUrlIs(string expectedUrl)
        {
            Console.WriteLine("wait Url={0}",expectedUrl);
            _wait.Until(ExpectedConditions.UrlContains(expectedUrl));
        }

        private void WaitTitleIs(string expectedTitle)
        {
            _wait.Until(ExpectedConditions.TitleContains(expectedTitle));
        }

        private string WaitH1IsVisibleAndReturnText()
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("td#content h1")));
            return _driver.FindElement(By.CssSelector("td#content h1")).Text;
        }

        private IList<IWebElement> ClickSubMenuItems(IList<IWebElement> menuItemsList, int i)
        {
            //string expectedUrl;
            // click on submenu items
            IList<IWebElement> subMenuItemsList = GetSubMenuList(menuItemsList, i);
            var subMenuItemCounts = subMenuItemsList.Count;

            // we start loop from 1 as zero-index element is active already
            for (int j = 1; j < subMenuItemCounts; j++)
            {
                // get new Title
                var hrefLink = GetFirstHRefLink(subMenuItemsList, j); //subMenuItemsList[j].FindElement(By.CssSelector("a[href*='litecart']"));
                var expectedUrl = hrefLink.GetAttribute("href");
                
                hrefLink.Click();
                
                WaitUrlIs(expectedUrl);

                var expectedSubTitle = WaitH1IsVisibleAndReturnText();
                //wait new Title 
                WaitTitleIs(expectedSubTitle);

                //re-read main and submenu lists
                menuItemsList = GetMainMenuList();
                subMenuItemsList = GetSubMenuList(menuItemsList, i);
            }
            return menuItemsList;
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

        private bool AreElementsPresent(IWebElement driver, By locator)
        {
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
