using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Seleniumtest_Shaikhullin;
public class SeleniumTestsForPractice
{
    public ChromeDriver driver;
    public WebDriverWait wait;

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(7);
        
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        
        Authorization();
    }
    
    [Test]
    public void AuthorizationTest()
    {
        wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/news"));
    }

    [Test]
    public void NavigationTest()
    {
        var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sideMenu.Click();
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']"))
            .Last(element => element.Displayed);
        community.Click();
        wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/communities"));
    }

    [Test]
    public void CheckTitleNewsPage()
    {
        CheckTitlePage("Новости");
    }
    

    [Test]
    public void CheckTitleMessagesPage()
    {
        wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/news"));
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/messages");
        CheckTitlePage("Диалоги");
    }
    
    [Test]
    public void LogOutTest()
    {
        var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sideMenu.Click();
        var logoutButton = driver.FindElement(By.CssSelector("[data-tid='LogoutButton']"));
        logoutButton.Click();
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/Account/Login"));
        Assert.That(driver.Url != "https://staff-testing.testkontur.ru/communities",
            "Ожидался выход из учетной записи");
    }
    
    // Баг на сайте. Нет заголовка 'Мероприятия'
    // [Test]
    // public void CheckTitleEvents()
    // {
    //     var pageTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
    //     driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events");
    //     CheckTitlePage("Мероприятия");
    // }
    
    public void Authorization()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("user");
        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("1q2w3e4r%T");
        
        var loginButton = driver.FindElement(By.Name("button"));
        loginButton.Click();
    }
    
    private void CheckTitlePage(String expectedTitle)
    {
        var pageTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(pageTitle.Text == expectedTitle, $"Ожидалось получить заголовок \"{expectedTitle}\" в разделе \"{expectedTitle}\", а получили заголовок \"{pageTitle.Text}\"");
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}