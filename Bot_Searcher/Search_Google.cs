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
		static string strURL_Google = "https://www.google.co.kr/search?num=40&hl=ko&source=hp&ei=VNyXWsmKJYK08QXNnqLoBQ&q={0}&oq={0}";

		[Command( "google" )]
		public async Task SearchStart_Google( CommandContext pContext, string strSearchWord )
		{
			if (BotLibrary.CheckIsRespond( pContext ) == false) return;

			await Event_SearchStart( pContext, strSearchWord, Search_Google, strURL_Google, "Google" );
		}

		static DiscordEmbedBuilder Search_Google( string strURL, string strSearchWord, DateTime pDateTimeStart )
		{
			string strTargetURL = string.Format( strURL, strSearchWord );
			DiscordEmbedBuilder pEmbedBuilder = new DiscordEmbedBuilder();
			try
			{
				ProcWrite_Result_Success( pEmbedBuilder, pDateTimeStart, "검색결과", strSearchWord, strTargetURL );
			}
			catch
			{
				ProcWrite_Result_Fail( pEmbedBuilder, strSearchWord );
			}

			return pEmbedBuilder;
		}
	}
}
