using OpenQA.Selenium;

public class LoginPage
{
    private IWebDriver driver;
    public LoginPage(IWebDriver driver) => this.driver = driver;

    private IWebElement UsernameInput => driver.FindElement(By.Id("username"));
    private IWebElement PasswordInput => driver.FindElement(By.Id("password"));
    private IWebElement LoginButton => driver.FindElement(By.CssSelector("button"));

    public void Login(string username, string password)
    {
        UsernameInput.SendKeys(username);
        PasswordInput.SendKeys(password);
        LoginButton.Click();
    }
}