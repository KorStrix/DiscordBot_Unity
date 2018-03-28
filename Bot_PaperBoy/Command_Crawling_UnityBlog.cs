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
    class Command_Crawling_UnityBlog
    {
        static public readonly string const_UnityBlog = "https://blogs.unity3d.com/kr/";

        [Command("test")]
        public async Task Comnad_Crawling_UnityBlog(CommandContext pContext)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            await DoCrawling_UnityBlog(pContext.Channel);
        }

        static public async Task DoCrawling_UnityBlog(DiscordChannel pChannel)
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");

            IWebDriver pDriver = new ChromeDriver(options);
            pDriver.Url = const_UnityBlog;

            await ProcCrawling_UnityBlog(pChannel, pDriver.FindElement(By.ClassName("lg-post-group")));
            await ProcCrawling_UnityBlog(pChannel, pDriver.FindElement(By.XPath("//*[@id='content-wrapper']/div/div[3]/div[1]")));
            await ProcCrawling_UnityBlog(pChannel, pDriver.FindElement(By.XPath("//*[@id='content-wrapper']/div/div[3]/div[2]")));
            await ProcCrawling_UnityBlog(pChannel, pDriver.FindElement(By.XPath("//*[@id='content-wrapper']/div/div[3]/div[3]")));

            pDriver.Close();
        }

        static private async Task ProcCrawling_UnityBlog(DiscordChannel pChannel, IWebElement pElement_Parents)
        {
            if (pElement_Parents == null)
                return;

            IWebElement pElement_TitleParents = pElement_Parents.FindElement(By.TagName("h4"));
            IWebElement pElement_Title = pElement_TitleParents.FindElement(By.TagName("a"));

            IWebElement pElement_ImageParents = pElement_Parents.FindElement(By.TagName("a"));
            IWebElement pElement_Image = pElement_ImageParents.FindElement(By.TagName("div"));

            string strImageURL = pElement_Image.GetAttribute("style");
            int iCutString_StartIndex = strImageURL.IndexOf("https");
            int iCutString_FinishIndex = strImageURL.IndexOf(")");
            strImageURL = strImageURL.Substring(iCutString_StartIndex, iCutString_FinishIndex - iCutString_StartIndex - 1);

            IWebElement pElement_AuthorAndDate = pElement_Parents.FindElement(By.TagName("span"));
            IWebElement pElement_Author = pElement_AuthorAndDate.FindElement(By.TagName("a"));
            IWebElement pElement_Date = pElement_AuthorAndDate.FindElement(By.ClassName("c-mg"));

            string strDate = pElement_Date.Text;
            strDate = strDate.Replace("월", ",");

            IWebElement pElement_Contents = pElement_Parents.FindElement(By.TagName("p"));
            string strContents = pElement_Contents.Text;

            string strCategory = "";
            var arrElement_TagNameIs_Div = pElement_Parents.FindElements(By.TagName("a"));
            foreach(IWebElement pElement in arrElement_TagNameIs_Div)
            {
                try
                {
                    if (pElement.GetAttribute("class").StartsWith("category-tag"))
                    {
                        strCategory = pElement.Text;
                        break;
                    }
                }
                catch
                {
                    continue;
                }
            }

            if (Strix.CBot.CheckIsOverDay(strDate, 1) == false)
            {
                if (string.IsNullOrWhiteSpace(strContents))
                    strContents = pElement_ImageParents.GetAttribute("href");

            strContents = strContents.Replace("더 읽기",
                    $"[더 읽기]({pElement_ImageParents.GetAttribute("href")})");

                DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
                pEmbed.
                    WithTitle("유니티 블로그 최신 글입니다.").
                    WithUrl(const_UnityBlog).
                    WithImageUrl(strImageURL).
                    AddField(pElement_Title.Text, strContents).
                    WithFooter($"{pElement_Author.Text} {pElement_Date.Text} ({strCategory})");

                await pChannel.SendMessageAsync(null, false, pEmbed);
            }
        }
    }
}
