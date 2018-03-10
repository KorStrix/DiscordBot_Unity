using DSharpPlus;
using DSharpPlus.CommandsNext;
using System;
using System.Threading.Tasks;

namespace Bot_Searcher
{
	class Program
	{
		static private DiscordClient _pClient;
		static CommandsNextModule _pCommands;

		static void Main( string[] args )
		{
			MainAsync( args ).ConfigureAwait( false ).GetAwaiter().GetResult();
		}

		static async Task MainAsync( string[] args )
		{
            Strix.CBot.DoInitClient(out _pClient, out _pCommands);
            _pCommands.RegisterCommands<Command_Search>();

			await _pClient.ConnectAsync();
			await Task.Delay( -1 );
		}
    }
}
