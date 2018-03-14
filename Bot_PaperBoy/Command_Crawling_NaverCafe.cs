using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_PaperBoy
{
    public enum ENaverIndex
    {
        글ID,
        글제목,
        퍼스나콘,
        아이디,
        작성일및조회수,
        좋아요개수,

        MAX,
    }

    class Command_Crawling_NaverCafe
    {
        static public readonly string const_UnityHub_Study = "http://cafe.naver.com/unityhub?iframe_url=/ArticleList.nhn%3Fsearch.clubid=26377973%26search.menuid=112%26search.boardtype=L";
        static public readonly string const_UnityHub_Hire = "http://cafe.naver.com/unityhub?iframe_url=/ArticleList.nhn%3Fsearch.clubid=26377973%26search.menuid=31%26search.boardtype=L";

        static public readonly string const_IndieTer_Hire = "http://cafe.naver.com/unityhub?iframe_url=/ArticleList.nhn%3Fsearch.clubid=28183931%26search.menuid=14%26search.boardtype=L";


        [Command("test_uhs")]
        public async Task Crawling_Naver_UnityHub_Study(CommandContext pContext)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            await DoCrawling_NaverCafe_UnityHub_Study(pContext.Channel);
        }

        [Command("test_uhh")]
        public async Task Crawling_Naver_UnityHub_Hire(CommandContext pContext)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            await DoCrawling_Naver_UnityHub_Hire(pContext.Channel);
        }

        [Command("test_ith")]
        public async Task Crawling_Naver_IndieTer_Hire(CommandContext pContext)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            await DoCrawling_Naver_IndieTer_Hire(pContext.Channel);
        }

        static public async Task DoCrawling_NaverCafe_UnityHub_Study(DiscordChannel pChannel)
        {
            await Crawling_NaverCafe(pChannel, const_UnityHub_Study, DiscordColor.Red, "[네이버카페]유니티허브 스터디 그룹 게시판입니다.");
        }

        static public async Task DoCrawling_Naver_UnityHub_Hire(DiscordChannel pChannel)
        {
            await Crawling_NaverCafe(pChannel, const_UnityHub_Hire, DiscordColor.Green, "[네이버카페]유니티허브 구인구직 게시판입니다.");
        }

        static public async Task DoCrawling_Naver_IndieTer_Hire(DiscordChannel pChannel)
        {
            await Crawling_NaverCafe(pChannel, const_IndieTer_Hire, DiscordColor.CornflowerBlue, "[네이버카페]인디터 팀원모집 게시판입니다.");
        }

        static public async Task Crawling_NaverCafe(DiscordChannel pChannel, string strURL, DiscordColor pColor, string strTitle)
        {
            IWebDriver pDriver = new ChromeDriver();
            pDriver.Url = strURL;
            pDriver.Navigate();
            pDriver.SwitchTo().Frame("cafe_main");

            IWebElement pElement_ListParents = pDriver.FindElement(By.XPath("//*[@id='main-area']/div[6]/form/table/tbody"));
            var arrElementRanking = pElement_ListParents.FindElements(By.TagName("tr"));
            var pEmbedBuilder = ProcGenerateEmbedBuilder(pColor, arrElementRanking, strTitle, strURL);

            await pChannel.SendMessageAsync(null, false, pEmbedBuilder);
            pDriver.Close();
        }

        static public DiscordEmbedBuilder ProcGenerateEmbedBuilder(DiscordColor pColor, ReadOnlyCollection<IWebElement> arrElement, string strTitle, string strURL)
        {
            DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
            pEmbed.
                WithColor(pColor).
                WithTitle(strTitle).
                WithUrl(strURL);

            foreach (IWebElement pElement in arrElement)
            {
                if (pElement.Displayed == false)
                    continue;

                string[] arrName = pElement.Text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                if (arrName.Length != (int)ENaverIndex.MAX)
                    continue;

                IWebElement pElementLink_Parents = pElement.FindElement(By.ClassName("aaa"));
                IWebElement pElementLink = pElementLink_Parents.FindElement(By.TagName("a"));

                string strLink = pElementLink.GetAttribute("href");
                string[] arrDateAndViewCount = arrName[(int)ENaverIndex.작성일및조회수].Split(new String[] { "." }, StringSplitOptions.None);

                pEmbed.AddField(
                    $"{arrName[(int)ENaverIndex.글제목]}",
                    $"ㄴ [Link 바로가기]({strLink}) 글쓴이 : [{arrName[(int)ENaverIndex.아이디]}][{arrDateAndViewCount[0]}.{arrDateAndViewCount[1]}.{arrDateAndViewCount[2]}]");
            }

            return pEmbed;
        }
    }
}
