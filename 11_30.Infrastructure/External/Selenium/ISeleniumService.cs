using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace _11_30.Infrastructure.External.Selenium
{
    public interface ISeleniumService
    {
        public IWebDriver CreateDriver(string driverName);
        public WebDriverWait CreateDriverWait(IWebDriver driver);
        public Task GotoUrlAsync(IWebDriver driver, string url);
        public void LxLogin(WebDriverWait driverWait, string userName, string passWord);
        public void LxStartAnsweringQuestions(WebDriverWait driverWait, IWebDriver driver);
        public void LxChoiceAnswer(string questionType, List<int> answers, ReadOnlyCollection<IWebElement> options, IWebDriver driver, WebDriverWait driverWait);
        public void LxNextRoundQuestion(WebDriverWait driverWait);
        public void EJiaLogin(WebDriverWait driverWait, string userName, string passWord);
        public IWebElement EJiaEnterMsg(WebDriverWait driverWait, IWebDriver driver);
        public void EJiaPrase(WebDriverWait driverWait);
        public void EJiaStartReadNews(WebDriverWait driverWait, IWebDriver driver, string newsType, DateTime startDate, DateTime endDate, IWebElement iFrame);
    }
}
