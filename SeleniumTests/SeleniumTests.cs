using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace SeleniumTests
{
    [TestFixture]
    public class SeleniumTests
    {



        protected IWebDriver driver;



        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            driver = new ChromeDriver(options);
        }

        [Test]
        public void LoginForm_InvalidUsername()
        {

            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            driver.FindElement(By.Id("username")).SendKeys("invalidUsername");
            driver.FindElement(By.Id("password")).SendKeys("SuperSecretPassword!");
            driver.FindElement(By.CssSelector("button.radius")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
            var message = wait.Until(d => d.FindElement(By.Id("flash")).Text);

            Assert.That(message, Does.Contain("Your username is invalid!"));

        }


        [Test]
        public void FooterElementsCount()
        {
            driver.Navigate().GoToUrl("https://elementalselenium.com/");

            var elementsValue =driver.FindElements(By.ClassName("footer__link-item")).Count();

            Assert.That(elementsValue, Is.EqualTo(7));

        }


        [Test]
        public void LoginForm_FindAndClickLink()
        {



            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            var originalWindow = driver.CurrentWindowHandle;

            driver.FindElement(By.LinkText("Elemental Selenium")).Click();

            
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            wait.Until(d => d.WindowHandles.Count == 2);

            
            var newWindow = driver.WindowHandles.First(h => h != originalWindow);
            driver.SwitchTo().Window(newWindow);

            
            Assert.That(driver.Url, Is.EqualTo("https://elementalselenium.com/"));

        }


        [Test]
        public void TitleCheck()
        {

            driver.Navigate().GoToUrl("https://example.com");

            Assert.That(driver.Title, Is.EqualTo("Example Domain"));

            driver.Quit();
        }

        [Test]
        public void LoginForm_FillAndSubmit()
        {

            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            driver.FindElement(By.Id("username")).SendKeys("tomsmith");
            driver.FindElement(By.Id("password")).SendKeys("SuperSecretPassword!");
            driver.FindElement(By.CssSelector("button.radius")).Click();

            var message = driver.FindElement(By.Id("flash")).Text;
            Assert.That(message, Does.Contain("You logged into a secure area"));

        }





        [Test]
        public void WaitForText_AfterLoading()
        {
            driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_loading/2");

            driver.FindElement(By.TagName("button")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var text = wait.Until(d => d.FindElement(By.Id("finish")).Text);

            Assert.That(text, Is.EqualTo("Hello World!"));

        }

        [TearDown]
        public void TearDown()
        {
           
            driver?.Quit();
            driver?.Dispose(); 
        }
    }
}
