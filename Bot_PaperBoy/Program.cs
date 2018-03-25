using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_PaperBoy
{
    class Program
    {
        static private DiscordClient _pClient;
        static CommandsNextModule _pCommands;

        static Dictionary<XML_Paper.ECrawlingKey, Func<DiscordChannel, Task>> _mapCrawling = new Dictionary<XML_Paper.ECrawlingKey, Func<DiscordChannel, Task>>();

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            Strix.CBot.DoInitClient(out _pClient, out _pCommands);
            XML_Paper.Load();
            _mapCrawling.Add(XML_Paper.ECrawlingKey.게임메카, Command_Crawling_GameNews.DoCrawling_GameMeca);
            _mapCrawling.Add(XML_Paper.ECrawlingKey.디스이즈게임즈, Command_Crawling_GameNews.DoCrawling_ThisIsGame);

            _mapCrawling.Add(XML_Paper.ECrawlingKey.네이버카페_유니티허브_스터디, Command_Crawling_NaverCafe.DoCrawling_NaverCafe_UnityHub_Study);
            _mapCrawling.Add(XML_Paper.ECrawlingKey.네이버카페_유니티허브_구인구직, Command_Crawling_NaverCafe.DoCrawling_Naver_UnityHub_Hire);
            _mapCrawling.Add(XML_Paper.ECrawlingKey.네이버카페_유니티허브_뉴스, Command_Crawling_NaverCafe.DoCrawling_Naver_UnityHub_News);

            _mapCrawling.Add(XML_Paper.ECrawlingKey.네이버카페_인디터_팀원모집, Command_Crawling_NaverCafe.DoCrawling_Naver_IndieTer_Hire);

            
            _mapCrawling.Add(XML_Paper.ECrawlingKey.네이버실시간뉴스, Command_Crawling_RealTimeNews.DoCrawling_Naver_RealTimeNews);

            _mapCrawling.Add(XML_Paper.ECrawlingKey.유니티블로그, Command_Crawling_UnityBlog.DoCrawling_UnityBlog);            

            _pCommands.RegisterCommands<Command_Crawling_GameNews>();
            _pCommands.RegisterCommands<Command_Crawling_NaverCafe>();
            _pCommands.RegisterCommands<Command_Crawling_RealTimeNews>();
            _pCommands.RegisterCommands<Command_Crawling_UnityBlog>();

            await _pClient.ConnectAsync();
            //await Task.Delay(-1);
            while (true)
            {
                await UpdateCheckTime(DateTime.Now);
            }
        }

        static public async Task UpdateCheckTime(DateTime sDateTime)
        {
            var arrConfig = XML_Paper.pConfig.arrConfig;
            for(int i = 0; i < arrConfig.Length; i++)
            {
                var pConfig = arrConfig[i];
                if (pConfig.bIsUsage == false) continue;

                bool bIsCorrect = false;
                for(int j = 0; j < pConfig.arrTime.Length; j++)
                {
                    if (CheckIsCorrectTime_Hour(sDateTime.Hour, pConfig.arrTime[j].iHour) &&
                        CheckIsCorrectTime(sDateTime.Minute, pConfig.arrTime[j].iMinute) &&
                        CheckIsCorrectTime(sDateTime.Second, pConfig.arrTime[j].iSecond))
                    {
                        bIsCorrect = true;
                        break;
                    }
                }

                if(bIsCorrect)
                {
                    if (_mapCrawling.ContainsKey(pConfig.eReportChannelID_GameNews) == false)
                    {
                        Console.WriteLine($"{pConfig.eReportChannelID_GameNews} is Not Contain !!");
                        continue;
                    }

                    Console.WriteLine($"{pConfig.eReportChannelID_GameNews} is Excute !!" + sDateTime.ToString());
                    await _mapCrawling[pConfig.eReportChannelID_GameNews](_pClient.GetChannelAsync(pConfig.iReportChannelID).Result);
                }
            }

            await Task.Delay(1000);
        }

        static private bool CheckIsCorrectTime_Hour(int iTimeCurrent, int iTimeCheck)
        {
            if (iTimeCheck == -1)
                return true;

            bool bCurrentIsOver12 = iTimeCurrent > 12;
            bool bCheckIsOver12 = iTimeCheck > 12;

            if (bCurrentIsOver12 == bCheckIsOver12)
                return CheckIsCorrectTime(iTimeCurrent, iTimeCheck);
            else
                return false;
        }

        static private bool CheckIsCorrectTime(int iTimeCurrent, int iTimeCheck)
        {
            if (iTimeCheck == -1)
                return true;

            if(iTimeCheck == 0)
                return iTimeCurrent == 0;
            else
                return (iTimeCurrent % iTimeCheck == 0);
        }

        static public DiscordEmbedBuilder DoGenerateEmbedBuilder(DiscordColor pColor, ReadOnlyCollection<IWebElement> arrElement, string strTitle, string strURL, bool bUseNumbering)
        {
            DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
            pEmbed.
                WithColor(pColor).
                WithTitle(strTitle).
                WithUrl(strURL);

            if (bUseNumbering)
            {
                int iRanking = 1;
                foreach (IWebElement pElement in arrElement)
                {
                    if (pElement.Displayed == false)
                        continue;

                    string strName = $"{iRanking++}. {pElement.Text}";
                    string strValue = $"ㄴ[Link 바로가기]({pElement.GetAttribute("href")})";
                    pEmbed.AddField(strName, strValue);
                }
            }
            else
            {
                foreach (IWebElement pElement in arrElement)
                {
                    if (pElement.Displayed == false)
                        continue;

                    string strName = pElement.Text;
                    string strValue = $"ㄴ[Link 바로가기]({pElement.GetAttribute("href")})";
                    pEmbed.AddField(strName, strValue);
                }
            }

            return pEmbed;
        }
    }
}
