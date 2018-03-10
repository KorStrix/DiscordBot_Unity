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
		static string strURL_UnityScriptAPI = "https://docs.unity3d.com/ScriptReference/30_search.html?q={0}";
		
		[Command( "unity" )]
		public async Task SearchStart_Unity( CommandContext pContext, string strSearchWord )
		{
			if (CBot.CheckIsRespond( pContext.Channel) == false) return;

			await Event_SearchStart( pContext, strSearchWord, Search_UnityAPI, string.Format( strURL_UnityScriptAPI, strSearchWord ), "UnityAPI", true );
		}

		static DiscordEmbedBuilder Search_UnityAPI( string strURL, string strSearchWord, DateTime pDateTimeStart )
		{
			IWebDriver pDriver = new ChromeDriver();
			pDriver.Url = strURL;

			// 본래 이렇게 해야 정상이지만, 빠른 속도를 위해 주석처리
			// pDriver.Url = strURL_UnityScriptAPI;
			// pDriver.Manage().Window.Maximize();
			// IWebElement pElement_SearchInput = pDriver.FindElement( By.Id( "q" ) );
			// pElement_SearchInput.SendKeys( strSearchWord );
			// pDriver.FindElement( By.ClassName( "submit" ) ).Click();

			DiscordEmbedBuilder pEmbedBuilder = new DiscordEmbedBuilder();
			IWebElement pElement_ResultList = pDriver.FindElement( By.ClassName( "search-results" ) );
			try
			{
				IWebElement pElement_ListFirst = pElement_ResultList.FindElement( By.ClassName( "result" ) );
				IWebElement pElement_ChildNodeHref = pElement_ListFirst.FindElement( By.ClassName( "title" ) );
				string strResultURL = pElement_ChildNodeHref.GetAttribute( "href" );
				IWebElement pElement_ChildNode_Description = pElement_ListFirst.FindElement( By.TagName( "p" ) );

				ProcWrite_Result_Success( pEmbedBuilder, pDateTimeStart, pElement_ChildNodeHref.Text, strSearchWord, pElement_ChildNode_Description.Text, strResultURL, strURL );
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
