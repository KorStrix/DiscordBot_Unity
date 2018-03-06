using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Room_Manager
{
    class Program
    {
        static private DiscordClient _pClient;
        static CommandsNextModule _pCommands_Search;

        static void Main(string[] args)
        {
            Strix.XML_Config.Load();
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            Strix.CBot.DoInitClient(out _pClient, out _pCommands_Search);

            _pClient.GuildMemberAdded += _pClient_GuildMemberAdded;

            _pCommands_Search.RegisterCommands<Command_RoomManage>();

            await _pClient.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task _pClient_GuildMemberAdded(DSharpPlus.EventArgs.GuildMemberAddEventArgs e)
        {
            DiscordEmbedBuilder pEmbedBuilder_WellCome = new DiscordEmbedBuilder();
            pEmbedBuilder_WellCome.WithColor(DiscordColor.Cyan).
            WithTitle($"어서오세요. 유니티 개발자 모임에 오신것을 환영합니다!").
            WithDescription("시간 나실때 개인 메세지함을 확인해주세요.").WithDescription("test");

            await Strix.CBot.pLobbyChannel.SendMessageAsync(null, false, pEmbedBuilder_WellCome);
            await (e.Member.CreateDmChannelAsync().GetAwaiter().GetResult().SendMessageAsync("test"));
        }
    }
}
