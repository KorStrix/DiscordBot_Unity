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
    class Command_Crawling_UnityBlog
    {
        static public readonly string const_UnityBlog = "https://blogs.unity3d.com/kr/";

        static public async Task DoCrawling_UnityBlog(DiscordChannel pChannel)
        {
            IWebDriver pDriver = new ChromeDriver();
            pDriver.Url = const_UnityBlog;

            IWebElement pElement_RankWrap = pDriver.FindElement(By.ClassName("// //*[@id='post-64154']/div[1]/div[2]/h4"));
            IWebElement pElement_RankList = pElement_RankWrap.FindElement(By.ClassName("rank_list"));

            var arrElementRanking = pElement_RankList.FindElements(By.TagName("a"));

            DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
            pEmbed.
                WithTitle("유니티 블로그 최신 글입니다.").
                WithUrl(const_UnityBlog);

            await pChannel.SendMessageAsync(null, false, pEmbed);
            pDriver.Close();
        }
    }
}
