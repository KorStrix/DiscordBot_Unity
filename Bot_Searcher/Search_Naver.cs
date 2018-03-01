using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;
using Strix;

namespace Bot_Searcher
{
	partial class Command_Search
	{
		static string strURL_Naver = "https://search.naver.com/search.naver?sm=tab_hty.top&query={0}";

		[Command( "naver" )]
		public async Task SearchStart_Naver( CommandContext pContext, string strSearchWord )
		{
			if (BotLibrary.CheckIsRespond( pContext ) == false) return;

			await Event_SearchStart( pContext, strSearchWord, Search_Naver, string.Format( strURL_Naver, strSearchWord ), "Naver" );
		}

		static DiscordEmbedBuilder Search_Naver( string strURL, string strSearchWord, DateTime pDateTimeStart )
		{
			// 본래 이렇게 해야 정상이지만, 빠른 속도를 위해 주석처리
			//IWebElement pElement_Search = pDriver.FindElement( By.Id( "query" ) );
			//pElement_Search.SendKeys( strSearchWord );
			//pDriver.FindElement( By.Id( "search_btn" ) ).Click();

			DiscordEmbedBuilder pEmbedBuilder = new DiscordEmbedBuilder();
			try
			{
				ProcWrite_Result_Success( pEmbedBuilder, pDateTimeStart, "검색결과", strSearchWord, strURL );
			}
			catch
			{
				ProcWrite_Result_Fail( pEmbedBuilder, strSearchWord );
			}

			return pEmbedBuilder;
		}
	}
}
