using System;
using NUnit.Framework;
using OpenQA.Selenium;
// ReSharper disable once RedundantUsingDirective
using OpenQA.Selenium.Firefox;
// ReSharper disable once RedundantUsingDirective
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

// ReSharper disable once CheckNamespace
namespace missions
{
    [TestFixture]
    public class Mission11Tests
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
        public void Test_CreateNewUser()
        {
            /*
             * Tect выполняет следующие действия в учебном приложении litecart.
             * Сценарий должен состоять из следующих частей:
             *
             * 1) регистрация новой учётной записи с достаточно уникальным адресом электронной почты (чтобы не конфликтовало с ранее созданными пользователями),
             * 2) выход (logout), потому что после успешной регистрации автоматически происходит вход,
             * 3) повторный вход в только что созданную учётную запись,
             * 4) и ещё раз выход.
             * Проверки можно никакие не делать, только действия -- заполнение полей, нажатия на кнопки и ссылки. 
             * Если сценарий дошёл до конца, то есть созданный пользователь смог выполнить вход и выход -- значит создание прошло успешно.
             * 
             * В форме регистрации есть капча, её нужно отключить в админке учебного приложения на вкладке Settings -> Security.             
             */
            // find New customers click here
            var createNewUserBtn = _driver.FindElement(By.CssSelector("div#box-account-login table tr:last-child td a[href*=create_account]"));
            createNewUserBtn.Click();
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[name=create_account]")));

            // fill the form for new user
            // name and email are unique for each test run
            // find form
            var form = _driver.FindElement(By.Name("customer_form"));

            string name;
            string email = NewRandomEmailAndName(out name);
            string pass="1234";
            // fill new user form 
            FillNewUserForm(form, name, email, pass);

            // click Create Account
            form.FindElement(By.CssSelector("button[name=create_account]")).Click();
            // wait until Logout button be clickable
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div#box-account ul.list-vertical li:last-child a[href*=logout]")));

            // logout
            _driver.FindElement(By.CssSelector("div#box-account ul.list-vertical li:last-child a[href*=logout]")).Click();
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div#box-account-login table tr:last-child td a[href*=create_account]")));


            // login with newly created email and password
            var emailUserField = _driver.FindElement(By.CssSelector("div#box-account-login table tr:first-child td input[name=email]"));
            FillField(emailUserField, email);
            // press TAB to switch next field 
            emailUserField.SendKeys(Keys.Tab);
            // get current active field 
            var passElem = _driver.SwitchTo().ActiveElement();
            //  fill by password
            FillField(passElem, pass);
            // double TAB to switch to login button
            passElem.SendKeys(Keys.Tab);
            _driver.SwitchTo().ActiveElement().SendKeys(Keys.Tab);
            // click current button
            _driver .SwitchTo().ActiveElement().Click();
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div#box-account ul.list-vertical li:last-child a[href*=logout]")));

            // logout
            _driver.FindElement(By.CssSelector("div#box-account ul.list-vertical li:last-child a[href*=logout]")).Click();
            //_wait.Until(ExpectedConditions.TitleContains("Online Store"));
            _wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div#box-account-login table tr:last-child td a[href*=create_account]")));

        }

        private void FillNewUserForm(IWebElement form, string name, string email, string pass)
        {
            
            // name 
            var field = form.FindElement(By.CssSelector("input[name=firstname"));
            field.Clear();
            FillField(field, name);
            field.SendKeys(Keys.Home);
            field.SendKeys(Keys.Control + "A");
            field.SendKeys(Keys.Control + "C");
            // last name 
            field = form.FindElement(By.CssSelector("input[name=lastname"));
            field.SendKeys(Keys.Home);
            field.SendKeys(Keys.Control + "V");
            //FillField(field, "demoLastName");
            // address1
            field = form.FindElement(By.CssSelector("input[name=address1"));
            FillField(field, "address1Demo");
            // postcode
            field = form.FindElement(By.CssSelector("input[name=postcode"));
            field.SendKeys(Keys.Control + "A");
            field.SendKeys(Keys.Delete);
            FillField(field, "123456");
            // city
            field = form.FindElement(By.CssSelector("input[name=city"));
            FillField(field, "CityDemo");
            // email
            field = form.FindElement(By.CssSelector("input[name=email"));
            FillField(field, email);
            // phone
            field = form.FindElement(By.CssSelector("input[name=phone"));
            FillField(field, "+7495-777-77-77");
            // pass
            field = form.FindElement(By.CssSelector("input[name=password"));
            field.Clear();
            FillField(field, pass);
            // confirm pass
            field = form.FindElement(By.CssSelector("input[name=confirmed_password"));
            field.Clear();
            FillField(field, pass);
        }

        private static void FillField(IWebElement field, string value)
        {
            field.Click();
            field.SendKeys(value);
        }

        private string NewRandomEmailAndName(out string name)
        {
            Random randomGenerator = new Random();
            int randomInt = randomGenerator.Next( 1000);
            name = "username" + randomInt; // + "@demo.com";

            return name+"@demo.com";

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
