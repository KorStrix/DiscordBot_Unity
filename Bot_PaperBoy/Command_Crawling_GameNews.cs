using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_PaperBoy
{
    public class Command_Crawling_GameNews
    {
        static public readonly string const_strGameMeca = "http://www.gamemeca.com/news.php";
        static public readonly string const_strThisIsGame = "http://www.thisisgame.com/";

        [Command("test11")]
        public async Task Crawling_GameMesca(CommandContext pContext)
        {
            await DoCrawling_GameMeca(pContext.Channel);
        }

        [Command("test12")]
        public async Task Crawling_ThisIsGame(CommandContext pContext)
        {
            await DoCrawling_ThisIsGame(pContext.Channel);
        }

        static public async Task DoCrawling_GameMeca(DiscordChannel pChannel)
        {
            IWebDriver pDriver = new ChromeDriver();
            pDriver.Url = const_strGameMeca;

            IWebElement pElement_RankWrap = pDriver.FindElement(By.ClassName("rank_wrap"));
            IWebElement pElement_RankList = pElement_RankWrap.FindElement(By.ClassName("rank_list"));

            var arrElementRanking = pElement_RankList.FindElements(By.TagName("a"));
            var pEmbedBuilder = Program.Crawling_Find(DiscordColor.Red, arrElementRanking, "게임메카 뉴스 리스트입니다.", const_strGameMeca, true);

            await pChannel.SendMessageAsync(null, false, pEmbedBuilder);
            pDriver.Close();
        }

        static public async Task DoCrawling_ThisIsGame(DiscordChannel pChannel)
        {
            IWebDriver pDriver = new ChromeDriver();
            pDriver.Url = const_strThisIsGame;

            IWebElement pElement_ListParents = pDriver.FindElement(By.ClassName("side-comp-body"));

            var arrElementRanking = pElement_ListParents.FindElements(By.TagName("a"));
            var pEmbedBuilder = Program.Crawling_Find(DiscordColor.Green, arrElementRanking, "디스이스게임 많이본 기사 리스트입니다.", const_strThisIsGame, false);

            await pChannel.SendMessageAsync(null, false, pEmbedBuilder);
            pDriver.Close();
        }
    }
}
