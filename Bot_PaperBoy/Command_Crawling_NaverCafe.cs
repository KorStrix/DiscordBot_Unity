using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_PaperBoy
{
    class Command_Crawling_NaverCafe
    {
        public readonly string const_UnityHub_Hire = "http://cafe.naver.com/unityhub?iframe_url=/ArticleList.nhn%3Fsearch.clubid=26377973%26search.menuid=31%26search.boardtype=L";
        public readonly string const_UnityHub_Study = "http://cafe.naver.com/unityhub?iframe_url=/ArticleList.nhn%3Fsearch.clubid=26377973%26search.menuid=112%26search.boardtype=L";

        //[Command("test")]
        public async Task Crawling_ThisIsGame(CommandContext pContext)
        {
            IWebDriver pDriver = new ChromeDriver();
            pDriver.Url = const_UnityHub_Hire;

            //IWebElement pElement_LoginButton = pDriver.FindElement(By.XPath("//*[@id='gnb_login_button']"));
            //pElement_LoginButton.Click();

            //IWebElement pElement_Login = pDriver.FindElement(By.XPath("//*[@id='id_area']/span"));
            //pElement_Login.SendKeys("strix13");

            IWebElement pElement_ListParentsParents = pDriver.FindElement(By.Id("main-area"));

            IWebElement pElement_ListParents = pElement_ListParentsParents.FindElement(By.ClassName("article-board.m-tcol-c"));

            //IWebElement pElement_ListParents = pElement_ListParentsParents.FindElement(By.TagName("tbody"));
            //var arrElementRanking = pElement_ListParents.FindElements(By.TagName("tr"));
            //var pEmbedBuilder = Program.Crawling_Find(DiscordColor.Blue, arrElementRanking, "[네이버카페]유니티허브 구인구직/협업/외주 게시판입니다.", const_UnityHub_Hire, false);

            //await pContext.Channel.SendMessageAsync(null, false, pEmbedBuilder);
            pDriver.Close();
        }
    }
}
