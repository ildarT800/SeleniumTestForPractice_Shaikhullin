using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Seleniumtest_Shaikhullin;
public class SeleniumTestsForPractice
{
    public ChromeDriver driver;

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox", "--start-maximized", "--disable-extensions");
        
        driver = new ChromeDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(7);
        
        Authorizationn();
    }
    
    [Test]
    public void Authorization()
    {
        // Проверяем, что находимся на нужной странице и авторизация прошла
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(
            SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains("https://staff-testing.testkontur.ru/news"));
    }

    [Test]
    public void NavigationTest()
    {
        var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sideMenu.Click();
        var community = driver.FindElements(By.CssSelector("[data-tid='Community']"))
            .First(element => element.Displayed);
        community.Click();
        Assert.That(driver.Url == "https://staff-testing.testkontur.ru/communities",
            "Ожидался Url https://staff-testing.testkontur.ru/communities, а получили " + driver.Url);
    }

    [Test]
    public void CheckTitleNews()
    {
        var pageTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(pageTitle.Text == "Новости", "Ожидалось получить заголовок 'Новости' в разделе 'Новости', а получили заголовок " + pageTitle.Text);
    }
    
    [Test]
    public void CheckTitleMessages()
    {
        var downloadFinish = driver.FindElement(By.CssSelector("[data-tid='News']"));
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/messages");
        var pageTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        Assert.That(pageTitle.Text == "Диалоги", "Ожидалось получить заголовок 'Диалоги' в разделе 'Диалоги', а получили заголовок " + pageTitle.Text);
    }
    
    [Test]
    public void ExitTest()
    {
        var sideMenu = driver.FindElement(By.CssSelector("[data-tid='SidebarMenuButton']"));
        sideMenu.Click();
        var logoutButton = driver.FindElement(By.CssSelector("[data-tid='LogoutButton']"));
        logoutButton.Click();
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/communities");
        var login = driver.FindElement(By.Id("Username"));
        Assert.That(driver.Url != "https://staff-testing.testkontur.ru/communities",
            "Ожидался выход из учетной записи");
    }
    // Баг на сайте. Нет заголовка 'Мероприятия'
    // [Test]
    // public void CheckTitleEvents()
    // {
    //     var pageTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
    //     driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/events");
    //     pageTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
    //     Assert.That(pageTitle.Text == "Мероприятия", "Ожидалось получить заголовок Мероприятия в разделе Мероприятия, а получили заголовок " + pageTitle.Text);
    // }
    public void Authorizationn()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("user");
        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys("1q2w3e4r%T");
        
        var loginButton = driver.FindElement(By.Name("button"));
        loginButton.Click();
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }
}