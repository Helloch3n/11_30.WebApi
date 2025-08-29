using _11_30.Application.Interface;
using _11_30.Domain.Entities.QuestionBank;
using _11_30.Domain.Repositories;
using _11_30.Domain.Services;
using _11_30.Infrastructure.External.Selenium;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Text.RegularExpressions;

namespace _11_30.Application.Services
{
    internal class LxAppService : ILxAppService
    {
        private readonly ISeleniumService _seleniumService;
        private readonly IQuestionBankRepository _questionBankRepository;
        private readonly IQuestionBankDomainService _questionBankDomainService;

        public LxAppService(ISeleniumService seleniumService, IQuestionBankRepository questionBankRepository, IQuestionBankDomainService questionBankDomainService)
        {
            _seleniumService=seleniumService;
            _questionBankRepository=questionBankRepository;
            _questionBankDomainService=questionBankDomainService;
        }

        public async Task LxAutoAnswerAsync()
        {
            //创建Driver
            IWebDriver driver = _seleniumService.CreateDriver("Chrome");
            //创建DriverWait
            WebDriverWait driverWait = _seleniumService.CreateDriverWait(driver);
            //访问url
            await _seleniumService.GotoUrlAsync(driver, "https://el.21tb.com/login/login.init.do?returnUrl=https%3A%2F%2Fel.21tb.com%2Ffrontend-page%2FracePC");
            //登录乐学网我司达人
            _seleniumService.LxLogin(driverWait, "ss", "123");
            //进入答题路径,切换iframe
            _seleniumService.LxStartAnsweringQuestions(driverWait, driver);
            //进入20轮答题，每轮20题。TODO: 未来如有多页面抓取需求，可将此段封装
            for (int a = 1; a <= 20; a++)
            {
                for (int b = 1; b <= 20; b++)
                {
                    Thread.Sleep(5000);
                    //获取问题类型。TODO: 未来如有多页面抓取需求，可将此段封装
                    string questionType;
                    string text = driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"QuestionWrap\"]/div[1]/div/h2"))).Text;
                    Match matchTitle = Regex.Match(text, "第\\d+题【(.*?)】");
                    if (matchTitle.Success)
                    {
                        questionType = matchTitle.Groups[1].Value;
                    }
                    else
                    {
                        questionType="单选题";
                    }
                    //获取题目
                    string title = driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"QuestionWrap\"]/div[1]/div/p"))).Text;
                    //获取选项列表
                    var optionsEL = driverWait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("ul.question-subject-option span.rich-text-content")));
                    var options = optionsEL.Select(el => el.Text.Trim()).ToList();
                    //获取问题
                    Question question = await _questionBankRepository.GetQuestionByTitleAsync(title);
                    //获取答案
                    List<int> answers = _questionBankDomainService.GetTrueOptionsService(question, options);
                    //重新获取选项列表
                    var optionsEL1 = driverWait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.CssSelector("ul.question-subject-option span.rich-text-content")));
                    //选择答案
                    _seleniumService.LxChoiceAnswer(questionType, answers, optionsEL1, driver, driverWait);
                    //下一轮答题
                    _seleniumService.LxNextRoundQuestion(driverWait);
                }
            }
            driver.Quit();
        }
    }
}
