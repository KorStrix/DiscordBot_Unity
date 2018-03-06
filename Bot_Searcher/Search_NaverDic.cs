using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading.Tasks;
using Strix;

namespace Bot_Searcher
{
	partial class Command_Search
	{
		static string strURL_NaverDic_General = "http://dic.naver.com/search.nhn?dicQuery={0}&query={0}&target=dic&ie=utf8&query_utf=&isOnlyViewEE=";
		static string strURL_NaverDic_English = "http://endic.naver.com/search.nhn?sLn=kr&isOnlyViewEE=N&query={0}";
		static string strURL_NaverDic_Korea = "http://krdic.naver.com/search.nhn?query={0}&kind=all";

		[Command( "dic" )]
		public async Task SearchStart_NaverDic( CommandContext pContext, string strSearchWord )
		{
			if (CBot.CheckIsRespond( pContext ) == false) return;

			await Event_SearchStart( pContext, strSearchWord, Search_NaverDic, string.Format( strURL_NaverDic_General, strSearchWord ), "Naver 사전" );
		}

		[Command( "dicen" )]
		public async Task SearchStart_NaverDic_English( CommandContext pContext, string strSearchWord )
		{
			if (CBot.CheckIsRespond( pContext ) == false) return;

			await Event_SearchStart( pContext, strSearchWord, Search_NaverDic_En, string.Format( strURL_NaverDic_English, strSearchWord ), "Naver 영어사전", true );
		}

		[Command( "dicko" )]
		public async Task SearchStart_NaverDic_Korean( CommandContext pContext, string strSearchWord )
		{
			if (CBot.CheckIsRespond( pContext ) == false) return;

			await Event_SearchStart( pContext, strSearchWord, Search_NaverDic_Ko, string.Format( strURL_NaverDic_Korea, strSearchWord ), "Naver 국어사전", true );
		}

		static DiscordEmbedBuilder Search_NaverDic( string strURL, string strSearchWord, DateTime pDateTimeStart )
		{
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

		static DiscordEmbedBuilder Search_NaverDic_En( string strURL, string strSearchWord, DateTime pDateTimeStart )
		{
			IWebDriver pDriver = new ChromeDriver();
			pDriver.Url = strURL;

			DiscordEmbedBuilder pEmbedBuilder = new DiscordEmbedBuilder();
			try
			{
				IWebElement pElement_List = pDriver.FindElement( By.ClassName( "list_e2" ) );

				IWebElement pElement_Word_Parents = pElement_List.FindElement( By.TagName( "a" ) );
				IWebElement pElement_Word = pElement_Word_Parents.FindElement( By.TagName( "strong" ) );

				IWebElement pElement_Description_Parents_Parents = pElement_List.FindElement( By.TagName( "dd" ) );
				IWebElement pElement_Description_Parents = pElement_Description_Parents_Parents.FindElement( By.TagName( "p" ) );

				_pStrBuilder.Length = 0;
				var arrElement_Description = pElement_Description_Parents.FindElements( By.TagName( "span" ) );
				foreach (IWebElement pElement_Description in arrElement_Description)
				{
					_pStrBuilder.Append( pElement_Description.Text );
					_pStrBuilder.Append( " " );
				}
				
				ProcWrite_Result_Success( pEmbedBuilder, pDateTimeStart, pElement_Word.Text, strSearchWord, _pStrBuilder.ToString(), pElement_Word_Parents.GetAttribute("href"), strURL );
			}
			catch
			{
				ProcWrite_Result_Fail( pEmbedBuilder, strSearchWord );
			}

			pDriver.Close();
			return pEmbedBuilder;
		}

		static DiscordEmbedBuilder Search_NaverDic_Ko( string strURL, string strSearchWord, DateTime pDateTimeStart )
		{
			IWebDriver pDriver = new ChromeDriver();
			pDriver.Url = strURL;

			DiscordEmbedBuilder pEmbedBuilder = new DiscordEmbedBuilder();
			try
			{
				IWebElement pElement_List = pDriver.FindElement( By.ClassName( "lst3" ) );
				//IWebElement pElement_List_First_Parents_Parents = pElement_List.FindElement( By.TagName( "li" ) );

				IWebElement pElement_Word = pElement_List.FindElement( By.TagName( "div" ) );
				IWebElement pElement_Description_Parents = pElement_List.FindElement( By.TagName( "p" ) );

				ProcWrite_Result_Success( pEmbedBuilder, pDateTimeStart, pElement_Word.Text, strSearchWord, pElement_Description_Parents .Text, pElement_Word.GetAttribute( "href" ), strURL, false );
			}
			catch
			{
				ProcWrite_Result_Fail( pEmbedBuilder, strSearchWord );
			}

			pDriver.Close();
			return pEmbedBuilder;
		}
	}
}
