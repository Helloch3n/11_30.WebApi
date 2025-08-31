using _11_30.Application.Interface;
using _11_30.Infrastructure.External.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Globalization;

namespace _11_30.Application.Services
{
    public class EJiaAppService : IEJiaAppService
    {
        private readonly ISeleniumService _seleniumService;

        public EJiaAppService(ISeleniumService seleniumService)
        {
            _seleniumService = seleniumService;
        }

        public async Task AutoNewsAsync(string userName, string passWord, DateTime startDate, DateTime endDate, string newsType)
        {
            //创建Driver
            IWebDriver driver = _seleniumService.CreateDriver("Chrome");
            //创建DriverWait
            WebDriverWait driverWait = _seleniumService.CreateDriverWait(driver);
            //访问url
            await _seleniumService.GotoUrlAsync(driver, "https://ejia.tbea.com/home/");
            try
            {
                //登陆账号
                _seleniumService.EJiaLogin(driverWait, userName, passWord);
            }
            catch (Exception ex)
            {
                //记录日志
                //日志。。。
                driver.Quit();
                throw new Exception("我的心好累，你好好检查一下账号密码!");
            }
            //获取消息IFrame，切换进入消息
            var iFrame = _seleniumService.EJiaEnterMsg(driverWait, driver);
            //开始新闻阅读
            _seleniumService.EJiaStartReadNews(driverWait, driver, newsType, startDate, endDate, iFrame);
            //退出浏览器
            driver.Quit();

        }
    }
}
