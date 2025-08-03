using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTests
{
    [TestFixture]
    internal class SeleniumAdvancedTests : BaseUiTest
    {
        private LoginPage _loginPage;

        [SetUp]
        public void InitPageObjects()
        {
            _loginPage = new LoginPage(Driver);
        }

        [Test]
        public void Login_InvalidCredentials_DoesNotRedirect()
        {
            Driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            var loginUrl = Driver.Url;
            _loginPage.Login("invalid", "invalid");

            Assert.That(Driver.Url, Is.EqualTo(loginUrl));
        }

        [Test]
        public void DynamicContent_LoadedBlocksContainText()
        {
            Driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dynamic_content");

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            var contents = wait.Until(d =>
                d.FindElements(By.CssSelector("#content .row")).Where(e => e.Text.Length > 0).ToList());

            Assert.That(contents.Count, Is.GreaterThan(0));
            Assert.That(contents.All(c => c.Text.Contains("Example") || c.Text.Length > 10));
        }

        public static IEnumerable<TestCaseData> DropdownOptions =>
            new[]
            {
        new TestCaseData("Option 1"),
        new TestCaseData("Option 2")
            };

        [Test, TestCaseSource(nameof(DropdownOptions))]
        public void Dropdown_SelectOption(string optionText)
        {
            Driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dropdown");

            var select = new SelectElement(Driver.FindElement(By.Id("dropdown")));
            select.SelectByText(optionText);

            Assert.That(select.SelectedOption.Text, Is.EqualTo(optionText));
        }

        [TestCase("tomsmith", "SuperSecretPassword!", true)]
        [TestCase("invalid", "SuperSecretPassword!", false)]
        [TestCase("tomsmith", "invalid", false)]
        [TestCase("", "", false)]
        public void LoginTest_VariousCredentials(string username, string password, bool isSuccessExpected)
        {
            Driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            _loginPage.Login(username, password);

            var message = Wait.Until(d => d.FindElement(By.Id("flash"))).Text;


            if (isSuccessExpected)
                Assert.That(message, Does.Contain("You logged into a secure area"));
            else
                Assert.That(message, Does.Contain("invalid"));
        }

        [Test]
        public void IFrame_DefaultTextIsCorrect()
        {
            Driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/iframe");
            Driver.SwitchTo().Frame("mce_0_ifr");
           
            var editorParagraph = Wait.Until(d => d.FindElement(By.CssSelector("#tinymce > p")));
            var placeHolderText = editorParagraph.Text;
            Driver.SwitchTo().DefaultContent();



            Assert.That(placeHolderText, Is.EqualTo("Your content goes here."));
        }


        [Test]
        public void Alert_Accept_ShouldWork()
        {
            Driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/javascript_alerts");
            Driver.FindElement(By.XPath("//button[text()='Click for JS Alert']")).Click();

            var alert = Driver.SwitchTo().Alert();
            Assert.That(alert.Text, Is.EqualTo("I am a JS Alert"));
            alert.Accept();

            var result = Driver.FindElement(By.Id("result")).Text;
            Assert.That(result, Is.EqualTo("You successfully clicked an alert"));
        }

        [Test]
        public void Select_Option2()
        {
            Driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/dropdown");

            var select = new SelectElement(Driver.FindElement(By.Id("dropdown")));
            select.SelectByText("Option 2");

            Assert.That(select.AllSelectedOptions.First().Text, Is.EqualTo("Option 2"));
        }

        [Test]
        public void Login_WithValidCredentials_ShowsSuccessMessage()
        {
            Driver.Navigate().GoToUrl("https://the-internet.herokuapp.com/login");

            _loginPage.Login("tomsmith", "SuperSecretPassword!");

            var message = Driver.FindElement(By.Id("flash")).Text;
            Assert.That(message, Does.Contain("You logged into a secure area"));
        }

    }

}
