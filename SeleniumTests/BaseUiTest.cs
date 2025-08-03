using NUnit.Framework.Interfaces;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace SeleniumTests
{
    public abstract class BaseUiTest
    {
        protected IWebDriver Driver;
        protected WebDriverWait Wait;

        [SetUp]
        public void BaseSetUp()
        {
            var options = new ChromeOptions();
            Driver = new ChromeDriver(options);
            Driver.Manage().Window.Maximize();
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
        }

        [TearDown]
        public void BaseTearDown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var ts = (ITakesScreenshot)Driver;
                var screenshot = ts.GetScreenshot();
                screenshot.SaveAsFile($"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            }
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
