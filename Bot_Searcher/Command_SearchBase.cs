using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Text;
using System.Threading.Tasks;
using Strix;

namespace Bot_Searcher
{
	partial class Command_Search
	{
		delegate DiscordEmbedBuilder OnSearch( string strURL, string strSearchWord, DateTime pDateTimeStart );

		static StringBuilder _pStrBuilder = new StringBuilder();

		[Command( "tutorial" )]
		public async Task Start_Tutorial_1( CommandContext pContext) { if(BotLibrary.CheckIsRespond(pContext) == false) return; await BotLibrary.DoStartTutorial( pContext ); }
		[Command( "hi" )]
		public async Task Start_Tutorial_2( CommandContext pContext ) { if (BotLibrary.CheckIsRespond( pContext ) == false) return; await BotLibrary.DoStartTutorial( pContext ); }
		[Command( "hello" )]
		public async Task Start_Tutorial_3( CommandContext pContext ) { if (BotLibrary.CheckIsRespond( pContext ) == false) return; await BotLibrary.DoStartTutorial( pContext ); }
		[Command( "안녕" )]
		public async Task Start_Tutorial_4( CommandContext pContext ) { if (BotLibrary.CheckIsRespond( pContext ) == false) return; await BotLibrary.DoStartTutorial( pContext ); }
		[Command( "튜토리얼" )]
		public async Task Start_Tutorial_5( CommandContext pContext ) { if (BotLibrary.CheckIsRespond( pContext ) == false) return; await BotLibrary.DoStartTutorial( pContext ); }

		static async Task Event_SearchStart( CommandContext pContext, string strSearchWorld, OnSearch OnSearch, string strSearchURL, string strCategoryName, bool bPrintSearchURL = false )
		{
			DiscordEmbedBuilder pResult = null;
			System.DateTime sDateTimeCurrent = DateTime.Now;
			await pContext.RespondAsync( $"접수했습니다. {pContext.User.Mention} [ {strCategoryName} - {strSearchWorld} ]를 검색하겠습니다. 핑 [{pContext.Client.Ping} ms ]" );
			if (bPrintSearchURL)
			{
				await pContext.RespondAsync( "오래 걸릴 수 있어 URL부터 먼저 드립니다.." );
				await pContext.RespondAsync( strSearchURL );
			}

			pResult = OnSearch( strSearchURL, strSearchWorld, sDateTimeCurrent );

			await pContext.RespondAsync( $"{pContext.User.Mention}을 위한 결과입니다. " );
			await pContext.RespondAsync( null, false, pResult );

		}

		static void ProcWrite_Result_Success( DiscordEmbedBuilder pEmbedBuilderTarget, DateTime sDateTimeStart, string strTitle, string strSearchWord, string strURL_List )
		{
			ProcWrite_Result_Success( pEmbedBuilderTarget, sDateTimeStart, strTitle, strSearchWord, null, null, strURL_List, false );
		}

		static void ProcWrite_Result_Success( DiscordEmbedBuilder pEmbedBuilderTarget, DateTime sDateTimeStart, string strTitle, string strSearchWord, string strDescrption, string strURL_Result, string strURL_List, bool bCheckProbably_Meant = true )
		{
			if (strDescrption != null)
				pEmbedBuilderTarget.AddField( "Description", strDescrption );

			if(strURL_Result != null)
				pEmbedBuilderTarget.AddField( "URL", strURL_Result );

			if(bCheckProbably_Meant)
				bCheckProbably_Meant = CheckIsProbably_Meant( strTitle, strSearchWord );

			if (bCheckProbably_Meant)
			{
				pEmbedBuilderTarget.
					WithColor( DiscordColor.Yellow ).
					WithTitle( $"(Probably meant) {strTitle}" ).
					AddField( "검색 URL입니다.", strURL_List );
			}
			else
			{
				pEmbedBuilderTarget.
					WithColor( DiscordColor.Green ).
					WithTitle( strTitle ).
					AddField( "검색 URL입니다.", strURL_List );
			}

			TimeSpan pTimeSpanGap = DateTime.Now.Subtract( sDateTimeStart );
			pEmbedBuilderTarget.AddField( "경과 시간", $"{pTimeSpanGap.Seconds}.{pTimeSpanGap.Milliseconds}초" );
		}

		static void ProcWrite_Result_Fail( DiscordEmbedBuilder pEmbedBuilderTarget, string strSearchWord )
		{
			pEmbedBuilderTarget.
				WithColor( DiscordColor.Red ).
				WithTitle( $"{strSearchWord} 을 찾지 못했습니다.." );
		}

		static bool CheckIsProbably_Meant( string strText, string strSearchWord )
		{
			strSearchWord = strSearchWord.ToLower();
			string[] arrSplit = strText.Split( '.' );
			for (int i = 0; i < arrSplit.Length; i++)
			{
				if (arrSplit[i].ToLower().Equals( strSearchWord ))
					return false;
			}

			return true;
		}
	}
}
