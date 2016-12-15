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
    public class Mission9Tests
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
        public void Test_CountrySorting()
        {
            /*
             * Tect выполняет следующие действия в учебном приложении litecart.
             * 1) на странице http://localhost/litecart/admin/?app=countries&doc=countries
             * а) проверить, что страны расположены в алфавитном порядке
             * б) для тех стран, у которых количество зон отлично от нуля -- открыть страницу этой страны и там проверить, 
             *    что зоны расположены в алфавитном порядке
             * 
             */

            OpenCountriesPage();

            // part a.
            // find Countries from first <td> with <a href=> - this <td> keeps country name, the second <td> with <a href> has 'title=Edit' 
            // so to exclude second <td> we use cssselector with NOT ':not(title=Edit)' 
            // IList<IWebElement> productList = _driver.FindElements(By.CssSelector("td#content table.dataTable tr.row td a:not([title=Edit])"));
            // get td with href without title=Edit
            IList<IWebElement> countryList = _driver.FindElements(By.CssSelector("td#content table.dataTable tr.row td a:not([title=Edit])"));

            IsListSorted(countryList);

            // part b.
            // find Countrires with TZs, the sixth <td> is a column with numbers of tz 
            IList<IWebElement> tzList = _driver.FindElements(By.CssSelector("td#content table.dataTable tr.row td:nth-child(6)"));
            // find element with TZ != '0' and keep href links to avoid re-reading countryList
            int i=0;
            var hRefs = new List<string>();
            foreach (var item in tzList )
            {
                if (item.Text != "0")
                    hRefs.Add(countryList[i].GetAttribute("href"));
                i++;
            }
            Console.WriteLine("total hrefs={0}", hRefs.Count  );
            // iterate hRefs, open link in driver, find TZ list 
            foreach (var item in hRefs)
            {
                _driver.Url = item;
                countryList = _driver.FindElements(By.CssSelector("td#content table#table-zones tr td:nth-child(3)")).ToList();
                // NOT GOOD !!! last element has empty cell, need exclude it before test for sorting 
                if (countryList[countryList.Count-1].Text == "" )
                    countryList.RemoveAt(countryList.Count-1); 

                IsListSorted(countryList);
            }


        }

        [Test]
        public void Test_GeoZones()
        {
            /*
             * Tect выполняет следующие действия в учебном приложении litecart.
             * на странице http://localhost/litecart/admin/?app=geo_zones&doc=geo_zones
             * зайти в каждую из стран и проверить, что зоны расположены в алфавитном порядке
             */
            OpenGeoZonesPage();

            // find links to Edit Geo Zone pages for each country
            // links could be taken from 3th <td> with country name or from last <td> where <a href> has a 'title=Edit' 
            // get links from last <td>
            IList<IWebElement> hRefs = _driver.FindElements(By.CssSelector("td#content table.dataTable tr.row td:last-child a[title='Edit']"));
            // keep href links as text to avoid re-reading hRefs
            var links = hRefs.Select(item => item.GetAttribute("href").ToString( )).ToList();
 
            Console.WriteLine("total hrefs={0}", hRefs.Count);
            // iterate hRefs, open link in driver,  
            foreach (var item in links)
            {
                // open Edit Geo Zone page for selected country
                _driver.Url = item;
                // get TZ list with selected=true
                IList<IWebElement> tzList = _driver.FindElements(By.CssSelector("td#content table#table-zones tr td:nth-child(3) select option[selected]")).ToList();
                // copy tz names into list
                IList<IWebElement> selectedList = new List<IWebElement>();
                foreach (var item2 in tzList)
                {
                    // one more check selected tz has attribute seletcted=true
                    if (item2.GetAttribute("selected")=="true")
                    selectedList.Add(item2);
                }
                
                IsListSorted(selectedList);
                 
            }


        }

        private static void IsListSorted(IList<IWebElement> countryList)
        {
            // fill list 'unsorted' with country name from td->textContext 
            var unsorted = countryList.Select(item => item.Text); 
            // new empty list
            var sorted = new List<string>();
            // fill new list by sorted x list
            sorted.AddRange(unsorted.OrderBy(o => o));
            // compare 2 lists - must be equal
            Assert.IsTrue(unsorted.SequenceEqual(sorted));
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

        private void OpenCountriesPage()
        {
            _driver.Url = "http://litecart:81/admin/?app=countries&doc=countries";

            /*
            // login box is visible
            _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("box-login")));
            // fill the fields and click login
            _driver.FindElement(By.Name("username")).SendKeys("admin");
            _driver.FindElement(By.Name("password")).SendKeys("admin");
            _driver.FindElement(By.Name("login")).Click();
            */
            // wait login is sucessfull and logout icon is visible
            _wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("i.fa.fa-sign-out.fa-lg")));
        }

        private void OpenGeoZonesPage()
        {
            _driver.Url = "http://litecart:81/admin/?app=geo_zones&doc=geo_zones";

            /*
            // login box is visible
            _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("box-login")));
            // fill the fields and click login
            _driver.FindElement(By.Name("username")).SendKeys("admin");
            _driver.FindElement(By.Name("password")).SendKeys("admin");
            _driver.FindElement(By.Name("login")).Click();
            */
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
