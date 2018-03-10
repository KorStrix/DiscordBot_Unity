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

            _pCommands.RegisterCommands<Command_Crawling_GameNews>();
            _pCommands.RegisterCommands<Command_Crawling_NaverCafe>();

            await _pClient.ConnectAsync();
            //await Task.Delay(-1);
            while (true)
            {
                await UpdateCheckTime();
            }
        }

        static public async Task UpdateCheckTime()
        {
            DateTime sDateTime = DateTime.Now;

            var arrConfig = XML_Paper.pConfig.arrConfig;
            for(int i = 0; i < arrConfig.Length; i++)
            {
                var pConfig = arrConfig[i];
                if (pConfig.bIsUsage == false) continue;

                if(CheckIsCorrectTime(sDateTime.Hour, pConfig.iHour) &&
                   CheckIsCorrectTime(sDateTime.Minute, pConfig.iMinute) &&
                   CheckIsCorrectTime(sDateTime.Second, pConfig.iSecond))
                {
                    if(_mapCrawling.ContainsKey(pConfig.eReportChannelID_GameNews) == false)
                    {
                        Console.WriteLine($"{pConfig.eReportChannelID_GameNews} is Not Contain !!");
                        continue;
                    }

#if DEBUG
                    Console.WriteLine($"{pConfig.eReportChannelID_GameNews} is Excute !!");
#else
                    Console.WriteLine($"{pConfig.eReportChannelID_GameNews} is Excute !!");
                    await _mapCrawling[pConfig.eReportChannelID_GameNews](_pClient.GetChannelAsync(pConfig.iReportChannelID).Result);
#endif
                }
            }

            Console.WriteLine($"Working... [{sDateTime.Hour}:{sDateTime.Minute}:{sDateTime.Second}.{sDateTime.Millisecond}]");
            await Task.Delay(1000);
        }

        static private bool CheckIsCorrectTime(int iTimeCurrent, int iTimeCheck)
        {
            if (iTimeCheck == 0) return true;

            return (iTimeCurrent % iTimeCheck == 0);
        }

        static public DiscordEmbedBuilder Crawling_Find(DiscordColor pColor, ReadOnlyCollection<IWebElement> arrElement, string strTitle, string strURL, bool bUseNumbering)
        {
            DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
            pEmbed.
                WithColor(pColor).
                WithTitle(strTitle).
                WithDescription(strURL);

            if (bUseNumbering)
            {
                int iRanking = 1;
                foreach (IWebElement pElement in arrElement)
                {
                    if (pElement.Displayed == false)
                        continue;

                    string strName = $"{iRanking++}. {pElement.Text}";
                    string strValue = pElement.GetAttribute("href");
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
                    string strValue = pElement.GetAttribute("href");
                    pEmbed.AddField(strName, strValue);
                }
            }

            return pEmbed;
        }
    }
}
