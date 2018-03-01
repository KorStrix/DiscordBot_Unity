using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;

namespace Bot_Searcher
{
	class Program
	{
		static private DiscordClient _pClient;
		static CommandsNextModule _pCommands_Search;

		static void Main( string[] args )
		{
			Strix.BotLibrary.CXMLParser.Load();
			MainAsync( args ).ConfigureAwait( false ).GetAwaiter().GetResult();
		}

		static async Task MainAsync( string[] args )
		{
			_pClient = new DiscordClient( new DiscordConfiguration
			{
				Token = Strix.BotLibrary.CXMLParser.pConfig.strBotToken,
				TokenType = TokenType.Bot,

				UseInternalLogHandler = true,
				LogLevel = DSharpPlus.LogLevel.Debug
			} );

			_pCommands_Search = _pClient.UseCommandsNext( new CommandsNextConfiguration
			{
				StringPrefix = Strix.BotLibrary.CXMLParser.pConfig.strCall_ID
			} );
			
			_pCommands_Search.RegisterCommands<Command_Search>();

			Console.WriteLine( "Searcher Is Ready" );

			await _pClient.ConnectAsync();
			await Task.Delay( -1 );
		}
	}
}
