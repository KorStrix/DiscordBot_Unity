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
    class Command_Crawling_RealTimeNews
    {
        static public readonly string const_ReaTimeNews_Naver = "https://datalab.naver.com/keyword/realtimeList.naver";
        static string strURL_Naver = "https://search.naver.com/search.naver?sm=tab_hty.top&query={0}";

        [Command("test")]
        public async Task Crawling_Naver_UnityHub_Study(CommandContext pContext)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            await DoCrawling_Naver_RealTimeNews(pContext.Channel);
        }

        static public async Task DoCrawling_Naver_RealTimeNews(DiscordChannel pChannel)
        {
            await Crawling_RealTime_Naver(pChannel, const_ReaTimeNews_Naver, DiscordColor.Green, "[네이버]현재 시각 뉴스입니다.");
        }

        static public async Task Crawling_RealTime_Naver(DiscordChannel pChannel, string strURL, DiscordColor pColor, string strTitle)
        {
            IWebDriver pDriver = new ChromeDriver();
            pDriver.Url = strURL;

            IWebElement pElement_CurrentTime = pDriver.FindElement(By.XPath("//*[@id='content']/div/div[3]/div/div/div[4]/div/strong"));

            var pElement_ListParents = pDriver.FindElement(By.XPath("//*[@id='content']/div/div[3]/div/div/div[4]/div/div/ul"));
            var arrElements = pElement_ListParents.FindElements(By.TagName("span"));

            var pEmbedBuilder = ProcGenerateEmbedBuilder(pColor, arrElements, strTitle, strURL, pElement_CurrentTime.Text);

            await pChannel.SendMessageAsync(null, false, pEmbedBuilder);
            pDriver.Close();
        }

        static public DiscordEmbedBuilder ProcGenerateEmbedBuilder(DiscordColor pColor, ReadOnlyCollection<IWebElement> arrElement, string strTitle, string strURL, string strDescription)
        {
            DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
            pEmbed.
                WithColor(pColor).
                WithTitle(strTitle).
                WithUrl(strURL).
                WithDescription(strDescription);

            if(arrElement != null)
            {
                int iOrder = 1;
                foreach (IWebElement pElement in arrElement)
                {
                    if (pElement.Displayed == false)
                        continue;

                    string strSearchWord = pElement.Text.Replace(" ", "+");

                    pEmbed.AddField(
                        $"{iOrder++}.{pElement.Text}",
                        $"ㄴ [Link 바로가기]({string.Format(strURL_Naver, strSearchWord)})");
                }
            }

            return pEmbed;
        }
    }
}
