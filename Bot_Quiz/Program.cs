using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Quiz
{
    class Program
    {
        static private DiscordClient _pClient;
        static CommandsNextModule _pCommands;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            Strix.CBot.DoInitClient(out _pClient, out _pCommands);
            _pCommands.RegisterCommands<Command_Quiz>();

            await _pClient.ConnectAsync();
            await Task.Delay(-1);
            //while (true)
            //{
            //    await UpdateCheckTime(DateTime.Now);
            //}
        }
    }
}
