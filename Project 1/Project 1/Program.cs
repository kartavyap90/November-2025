using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

public class Program
{
    private static void Main(string[] args)
    {
        // ✅ Setup ChromeOptions to suppress password manager and use clean profile
        ChromeOptions options = new ChromeOptions();
        options.AddArgument("--disable-save-password-bubble");
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddArgument("--no-proxy-server");
        options.AddArgument("--disable-extensions");
        options.AddArgument("--start-maximized");
        options.AddArgument("--user-data-dir=C:\\Temp\\ChromeProfile"); // Make sure this folder exists

        IWebDriver driver = new ChromeDriver(options);

        // ✅ Use HTTPS to avoid connection reset
        driver.Navigate().GoToUrl("https://horse.industryconnect.io/Account/Login?ReturnUrl=%2f");

        // ✅ Login steps
        IWebElement usernameTextbox = driver.FindElement(By.Id("UserName"));
        usernameTextbox.SendKeys("hari");

        IWebElement passwordTextbox = driver.FindElement(By.Id("Password"));
        passwordTextbox.SendKeys("123123");

        IWebElement Signin = driver.FindElement(By.XPath("//*[@id=\"loginForm\"]/form/div[3]/input[1]"));
        Signin.Click();

        // ✅ Optional: wait for HTML modal popup (e.g., Change Password)
        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        try
        {
            // Adjust locator based on actual modal button
            IWebElement okButton = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//button[text()='OK']")));
            okButton.Click();
            Console.WriteLine("Popup handled successfully.");
        }
        catch (WebDriverTimeoutException)
        {
            Console.WriteLine("No popup appeared within timeout.");
        }

        // ✅ Verify login success
        IWebElement helloHari = driver.FindElement(By.XPath("//*[@id=\"logoutForm\"]/ul/li/a"));
        if (helloHari.Text == "Hello Hari!")
        {
            Console.WriteLine("Logged");
        }
        else
        {
            Console.WriteLine("Not Logged");
        }

        driver.Quit();
    }
}
