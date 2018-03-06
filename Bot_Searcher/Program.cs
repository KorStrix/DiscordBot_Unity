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
        static Strix.XML_Config.SConfig _pConfig;

		static void Main( string[] args )
		{
            _pConfig = Strix.CManagerXMLParser.LoadXML<Strix.XML_Config.SConfig>("Config.xml", Strix.XML_Config.SConfig.CreateDummy);
			MainAsync( args ).ConfigureAwait( false ).GetAwaiter().GetResult();
		}

		static async Task MainAsync( string[] args )
		{
            Strix.CBot.DoInitClient(out _pClient, out _pCommands_Search);
            _pCommands_Search.RegisterCommands<Command_Search>();

			await _pClient.ConnectAsync();
			await Task.Delay( -1 );
		}
    }
}
