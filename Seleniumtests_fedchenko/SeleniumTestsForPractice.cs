using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace Seleniumtests_fedchenko;

public class SeleniumTestsForPractice
{
    public ChromeDriver driver;
    
    public void Authorization()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        
        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys("fedchenko.valeria2010@yandex.ru");

        var password = driver.FindElement(By.Name("Password"));
        password.SendKeys("0306.Marina");

        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
        wait.Until(ExpectedConditions.UrlToBe("https://staff-testing.testkontur.ru/news"));
    }

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArguments("--no-sandbox","--start-maximized", "--disable-extensions");

        driver = new ChromeDriver(options);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
        
        Authorization();
    }
        
    [Test] // Проверка авторизации
    public void AuthorizationTest()
    {
        var news = driver.FindElement(By.CssSelector("[data-tid='Title']"));
        var currentUrl = driver.Url;
        currentUrl.Should().Be("https://staff-testing.testkontur.ru/news");
    }

    [Test] // Проверка перехода на страницу "Сообщества"
    public void NavigationTest()
    {
      var community = driver.FindElements(By.CssSelector("[data-tid='Community']")).First(element => element.Displayed);
      community.Click();
      
      var communityTitle = driver.FindElement(By.CssSelector("[data-tid='Title']"));
      communityTitle.Should().NotBeNull();
      
      var currentUrl = driver.Url;
      currentUrl.Should().Be("https://staff-testing.testkontur.ru/communities");
    }

    [Test] // Проверка поиска
    public void SearchTest()
    {
        var search = driver.FindElement(By.CssSelector("[data-tid='SearchBar']"));
        search.Click();
        
        var input = driver.FindElement(By.ClassName("react-ui-1oilwm3"));
        input.SendKeys("Агапова Алиса Алексеевна");

        var combobox = driver.FindElement(By.CssSelector("[data-tid='ComboBoxMenu__item']"));
        combobox.Click();

        var currentUrl = driver.Url;
        currentUrl.Should().Be("https://staff-testing.testkontur.ru/profile/f23f7980-6b93-4959-9fc0-dbc3359c0dbb");
    }
    

    [Test] // Проверка применения новогодней темы
    public void NewYearThemeTest()
    {
        var profile1 = driver.FindElement(By.CssSelector("[data-tid='PopupMenu__caption']"));
        profile1.Click();
        
        var settings = driver.FindElement(By.CssSelector("[data-tid='Settings']"));
        settings.Click();
        
        var themeButton = driver.FindElement(By.ClassName("react-ui-1jxed06"));
        themeButton.Click();
        
        var saveButton = driver.FindElement(By.ClassName("react-ui-1m5qr6w"));
        saveButton.Click();
        
        var garland = driver.FindElement(By.CssSelector("[class='sc-dvUynV eIUTfe']"));
        garland.Should().NotBeNull();
    }

    [Test] // Проверка открытия модального окна при нажатии кнопки "Создать" в разделе "Документы"
    public void DocumentsTest()
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/documents");

        var buttonCreate = driver.FindElement(By.CssSelector("[class='sc-juXuNZ sc-ecQkzk WTxfS vPeNx']"));
        buttonCreate.Click();
        
        var modalWindowCreate = driver.FindElement(By.CssSelector("[data-tid='ModalPageHeader']"));
        modalWindowCreate.Should().NotBeNull();
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Close();
        driver.Quit();
    }
}