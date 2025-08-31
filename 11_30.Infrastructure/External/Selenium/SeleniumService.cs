using _11_30.Domain.Entities.QuestionBank;
using _11_30.Domain.Repositories;
using _11_30.Domain.Services;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace _11_30.Infrastructure.External.Selenium
{
    public class SeleniumService : ISeleniumService
    {
        private readonly IQuestionBankRepository _questionBankRepository;
        private readonly IQuestionBankDomainService _questionsService;
        private readonly IConfiguration _configuration;
        public SeleniumService(IQuestionBankRepository questionsRepository, IQuestionBankDomainService questionsService, IConfiguration configuration)
        {
            _questionBankRepository = questionsRepository;
            _questionsService = questionsService;
            _configuration = configuration;
        }

        /// <summary>
        /// 点击答案
        /// </summary>
        /// <param name="questionType">题目类型</param>
        /// <param name="answers">答案</param>
        /// <param name="options">题目选项</param>
        /// <param name="driver"></param>
        /// <param name="driverWait"></param>
        public void LxChoiceAnswer(string questionType, List<int> answers, ReadOnlyCollection<IWebElement> options, IWebDriver driver, WebDriverWait driverWait)
        {
            foreach (var answer in answers)
            {
                if ((int)answer <= options.Count() - 1)
                {
                    driver.ExecuteJavaScript("arguments[0].click();", options[answer]);
                }
            }
            //多选题点击确定
            if (questionType == "多选题")
            {
                driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"QuestionWrap\"]/div[2]/div/div/div/div/span"))).Click();
            }
        }

        /// <summary>
        /// 创建Driver
        /// </summary>
        /// <param name="driverName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IWebDriver CreateDriver(string driverName)
        {
            //获取浏览器配置文件
            var DriverSection = _configuration.GetSection("DriverOption");
            if (driverName == "Chrome")
            {
                var driver = DriverSection.GetSection("ChromeDriver").Value;
                var options = new ChromeOptions();
                //读取启动参数列表
                var args = DriverSection.GetSection("Arguments").Get<string[]>();
                if (args != null)
                {
                    //添加参数
                    options.AddArguments(args);
                }
                if (driver == string.Empty)
                    //测试环境不需要driver
                    return new ChromeDriver(options);
                ChromeDriver chromeDriver = new ChromeDriver(driver, options);
                return chromeDriver;
            }
            else if (driverName == "Edge")
            {
                var options = new EdgeOptions();
                EdgeDriver edgeDriver = new EdgeDriver(options);
                return edgeDriver;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 登录乐学网
        /// </summary>
        /// <param name="driverWait"></param>
        /// <param name="userName">用户名</param>
        /// <param name="passWord">密码</param>
        public void LxLogin(WebDriverWait driverWait, string userName, string passWord)
        {
            //获取账号输入框，输入账号
            IWebElement userInputEL = driverWait.Until(ExpectedConditions.ElementExists(By.Id("loginName")));
            userInputEL.SendKeys($"{userName}");
            //获取密码输入框，输入密码
            IWebElement passInputEL = driverWait.Until(ExpectedConditions.ElementExists(By.Id("password")));
            passInputEL.SendKeys($"{passWord}");
            //登录
            driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"div4\"]/input"))).Click();
            //验证
            Thread.Sleep(5000);
            driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"dialog-content\"]/div/input[1]"))).Click();
        }
        /// <summary>
        /// 进入下一轮答题
        /// </summary>
        /// <param name="driverWait"></param>
        public void LxNextRoundQuestion(WebDriverWait driverWait)
        {
            //进入下一轮答题
            Thread.Sleep(5000);
            driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"practise__wrap\"]/div/section")));
            //获取准确率
            string accuracy = driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"practise__wrap\"]/div/section/div[2]/div[1]/span[2]"))).Text;
            driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"practise__wrap\"]/div/section/div[3]"))).Click();
        }

        /// <summary>
        /// 打开URL
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task GotoUrlAsync(IWebDriver driver, string url)
        {
            await driver.Navigate().GoToUrlAsync(url);
        }

        /// <summary>
        /// 进入LX答题路径
        /// </summary>
        /// <param name="driverWait"></param>
        /// <param name="driver"></param>
        public void LxStartAnsweringQuestions(WebDriverWait driverWait, IWebDriver driver)
        {
            //切换iframe
            var iframe = driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"app\"]/div/div/div[1]/div/iframe")));
            driver.SwitchTo().Frame(iframe);
            //定位答题入口
            driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[contains(@src, '6833f59f30065e186813f6eb_0100')]"))).Click();
            driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"app\"]/div/footer/div[1]/i/i"))).Click();
            Thread.Sleep(2000);
        }

        /// <summary>
        /// 创建DriverWait
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        public WebDriverWait CreateDriverWait(IWebDriver driver)
        {
            WebDriverWait driverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            return driverWait;
        }

        public void EJiaLogin(WebDriverWait driverWait, string userName, string passWord)
        {
            driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"app\"]/div[1]/div[1]/div[2]/div[1]/span[2]"))).Click();
            IWebElement userInputEL = driverWait.Until(ExpectedConditions.ElementExists(By.Id("email")));
            userInputEL.SendKeys($"{userName}");
            IWebElement passInputEL = driverWait.Until(ExpectedConditions.ElementExists(By.Id("password")));
            passInputEL.SendKeys($"{passWord}");
            driverWait.Until(ExpectedConditions.ElementExists(By.Id("log-btn"))).Click();
            //寻找消息框检查是否登陆成功
            Thread.Sleep(5000);
            var msgEl = driverWait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/header/div[2]/div/div[3]/ul/li[2]/span")));
        }

        public void EJiaStartReadNews(WebDriverWait driverWait, IWebDriver driver, string newsType, DateTime startDate, DateTime endDate, IWebElement iFrame)
        {
            var chatPanel = driver.FindElement(By.XPath("//*[@id=\"msg-list\"]"));
            Thread.Sleep(3000);
            try
            {
                var newsButton = driverWait.Until(d => d.FindElement(By.XPath($"//span[text()='{newsType}']")));
                newsButton.Click();
            }
            catch
            {
                throw new Exception("检查一下你的新闻有没有置顶呀，我怎么找不到呢！");
            }
            Thread.Sleep(2000);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            int previousItemCount = 0;
            bool exit = false;
            while (true)
            {
                var chatItems = driver.FindElements(By.CssSelector(".chat-item"));
                if (chatItems.Count == previousItemCount)
                {
                    driver.SwitchTo().Frame(iFrame);
                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollBy(0, -100000);", chatPanel);
                }

                for (int i = chatItems.Count - 1 - previousItemCount; i >= 0; i--)
                {
                    var chatItem = chatItems[i];
                    try
                    {
                        var timeElem = chatItem.FindElement(By.CssSelector(".send-time"));
                        string timeText = timeElem.Text.Trim();
                        timeText = "2025-" + timeText;
                        var sendTime = DateTime.Parse(timeText);

                        if (sendTime >= startDate && sendTime < endDate)
                        {

                            // 查找 news-list 下第一个 news-li 并点击
                            var newsas = chatItem.FindElements(By.TagName("a"));
                            foreach (var newsa in newsas)
                            {
                                newsa.Click();
                                // 获取所有打开的窗口句柄
                                var allHandles1 = driver.WindowHandles;
                                var qty = allHandles1.Count;
                                string mainHandle = allHandles1[0];
                                string currentHandle = allHandles1[qty - 1];
                                driver.SwitchTo().Window(currentHandle);
                                // 滚动到底部触发懒加载
                                ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                                try
                                {
                                    // 等待点赞按钮可点击
                                    var praiseBtn = driverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.oper-area span.praise-num")));
                                    // 点击点赞按钮
                                    praiseBtn?.Click();
                                }
                                catch
                                {

                                }
                                // 切换回第一个窗口
                                driver.Close();
                                driver.SwitchTo().Window(mainHandle);
                                driver.SwitchTo().Frame(iFrame);
                            }
                        }
                        else
                        {
                            exit = true;
                            break;
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        continue;
                    }
                }
                if (exit)
                {
                    break;
                }
                Thread.Sleep(1000);
                // 获取所有打开的窗口句柄
                var allHandles = driver.WindowHandles;
                string firstHandle = allHandles[0];

                //关闭除第一个以外的所有窗口
                //for (int i = allHandles.Count - 1; i > 0; i--)
                //{
                //    driver.SwitchTo().Window(allHandles[i]);
                //    driver.Close();
                //}

                // 切换回第一个窗口
                driver.SwitchTo().Window(firstHandle);
                // 向上滚动加载更多

                driver.SwitchTo().Frame(iFrame);
                // 执行滚动
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollBy(0, -100000);", chatPanel);
                Thread.Sleep(1000);

                // 如果没有新增元素，跳出
                if (chatItems.Count == previousItemCount)
                    break;

                previousItemCount = chatItems.Count;
            }
        }

        public IWebElement EJiaEnterMsg(WebDriverWait driverWait, IWebDriver driver)
        {
            driverWait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/header/div[2]/div/div[3]/ul/li[2]/span"))).Click();
            var iFrame = driver.FindElement(By.XPath("/html/body/div[1]/iframe[2]"));
            driver.SwitchTo().Frame(iFrame);
            return iFrame;
        }

        public void EJiaPrase(WebDriverWait driverWait)
        {
            driverWait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"__nuxt\"]/section/div[2]/div[1]/div[2]/p/span[2]"))).Click();
        }
    }
}
