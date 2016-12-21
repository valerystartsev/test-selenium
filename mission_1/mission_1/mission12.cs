using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
// ReSharper disable once RedundantUsingDirective
using OpenQA.Selenium .Chrome; 
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

// ReSharper disable once CheckNamespace
namespace missions
{
    [TestFixture]
    public class Mission12Tests
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

            LoginToPanel();
        }

        [TearDown]
        public void Stop()
        {
            _driver.Quit();
            _driver = null;
        }

        [Test]
        public void Test_AddNewProduct()
        {
            /*
             * Tect выполняет следующие действия в учебном приложении litecart.
             * Сделайте сценарий для добавления нового товара (продукта) в учебном приложении litecart (в админке).
            * Для добавления товара нужно открыть меню Catalog, в правом верхнем углу нажать кнопку "Add New Product", 
             * заполнить поля с информацией о товаре и сохранить.
            * Достаточно заполнить только информацию на вкладках General, Information и Prices. 
             * Скидки (Campains) на вкладке Prices можно не добавлять.
             * После сохранения товара нужно убедиться, что он появился в каталоге (в админке). 
             * Клиентскую часть магазина можно не проверять.
             */

            // find and click Catalog in menu
            ClickCatalogMenuItem();

            // find and click Add New Product
            _driver.FindElement( By.CssSelector("div#content-wrapper tr td#content a.button[href*=edit_product]")).Click();
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("td#content form span.button-set button[name=save]")));

            // click on General Tab
            ClickTab("General");
            // fill General Tab fields and remember newly created product name
            var newProductName = FillGeneralTabFields();

            // click Information Tab
            ClickTab("Information");
            FillInformationTabFields();

            // click on Prices Tab
            ClickTab("Prices");
            FillPrices();

            // click on Save button
            _driver.FindElement(By.CssSelector("td#content form span.button-set button[name=save]")).Click(); 
                
            // click Catalog menu item again to refresh product list
            ClickCatalogMenuItem();
            // get all products 
            IList<IWebElement> productList = _driver.FindElements(By.CssSelector("td#content table.dataTable td a:not([title=Edit]"));
            bool isProductAdded = false;
            foreach (var item in productList)
            {
                if (item.Text == newProductName)
                {
                    isProductAdded = true;
                    break;
                }
            }
            Assert.True(isProductAdded);

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

        private void FillPrices()
        {
            // fill Purchase Price by random price
            var purPrice = _driver.FindElement(By.CssSelector("div#tab-prices input[name=purchase_price]"));
            purPrice.Clear();
            purPrice.SendKeys(GetRandomNumber());

            // find first Price input and fill by random price 
            var price = _driver.FindElement(By.CssSelector("div#tab-prices input[name*=prices]"));
            price.Clear();
            price.SendKeys(GetRandomNumber());

        }

        private string GetRandomNumber()
        {
            Random randomGenerator = new Random();
            return randomGenerator.Next(1000).ToString();
        }

        private void FillInformationTabFields()
        {
            // fill manufacturer select element
            var manufacturers = _driver.FindElement(By.CssSelector("div#tab-information select[name=manufacturer_id]"));
            var select = new SelectElement(manufacturers);
            select.SelectByIndex(1);


            // fill short description
            var shortDesc = _driver.FindElement(By.CssSelector("div#tab-information input[name*=short_description]"));
            shortDesc.Clear();
            shortDesc.SendKeys(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse sollicitudin ante massa, eget ornare libero porta congue.");
            // try copy&paste
            shortDesc.SendKeys(Keys.Control + "a");
            Thread.Sleep(500);
            shortDesc.SendKeys(Keys.Control + "c"); 
            shortDesc.Clear();
            shortDesc.SendKeys(Keys.Control + "v");

            // fill Desc
            // first switch to text view
            _driver.FindElement(By.CssSelector("div#tab-information button.trumbowyg-viewHTML-button")).Click(); 
            // find text area element
            var longDesc = _driver.FindElement(By.CssSelector("div#tab-information textarea.trumbowyg-textarea[name*=description]"));
            //longDesc.Click();
            longDesc.Clear();
            longDesc.SendKeys(Keys.Control + "v" );
            Thread.Sleep(500);
            // switch to HTML view
            _driver.FindElement(By.CssSelector("div#tab-information button.trumbowyg-viewHTML-button")).Click(); 
            
        }

        private string FillGeneralTabFields()
        {
            // set status = enabled
            IList<IWebElement> radioBtnList = _driver.FindElements(By.CssSelector( "label input[type=radio][name=status]"));
            if (!radioBtnList[0].Selected)
            { // not active, do click
                radioBtnList[0].Click();
                _wait.Until(ExpectedConditions.ElementSelectionStateToBe(By.CssSelector("label input:first-child"), true ));
            }
            // set name = 'RandomDuck_xxx' where xxx is random
            int rndCode;
            string name = NewRandomName(out rndCode);
            _driver.FindElement(By.CssSelector("span.input-wrapper input[name*=name]")).SendKeys(Keys.Home + name + Keys.Tab);
            
            // switch next field 'Code' and set code=rndXXX
            _driver.SwitchTo().ActiveElement().SendKeys("rnd" + rndCode.ToString()); 
            
            // find element for categories
            // set all catagories ON
            IList<IWebElement> categories = _driver.FindElements(By.CssSelector("div#tab-general input[name*=categories]"));
            foreach (var item in categories)
            {
                if (string.IsNullOrEmpty(item.GetAttribute("checked")))
                    item.Click();
            }
            
            // find element for default category
            // 
            IList<IWebElement> defaults = _driver.FindElements(By.CssSelector("div#tab-general select[name*=default_category_id] option"));
            foreach (var item in defaults)
            {
                if (string.IsNullOrEmpty(item.GetAttribute("selected")))
                    item.Click();
            }

            // find quantity, set it as Random
            var quantity = _driver.FindElement(By.CssSelector("div#tab-general input[name=quantity]"));
            quantity.Click();
            quantity.SendKeys(Keys.Control + "A");
            quantity.SendKeys(Keys.Delete);
            quantity.SendKeys(GetRandomNumber() );


            return name;
        }

        private string NewRandomName(out int rndCode)
        {
            Random randomGenerator = new Random();
            int randomInt = randomGenerator.Next( 1000);

            string name = "Custom Duck_" + randomInt;
            rndCode = randomInt;
 
            return name;
        }


        private void ClickTab(string tabName)
        {
            string cssString = "td#content div.tabs ul.index li a[href*="+tabName.ToLower()+"]";
            var tabButton = _driver.FindElement(By.CssSelector(cssString));
            tabButton.Click();
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("td#content div.tabs ul.index li.active a")));
            Assert.IsTrue(
                _driver.FindElement(By.CssSelector("td#content div.tabs ul.index li.active a")).GetAttribute("textContent") ==
                tabName);
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

        // ReSharper disable once UnusedMember.Local
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
