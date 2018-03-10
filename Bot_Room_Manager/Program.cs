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
        static CommandsNextModule _pCommands;

        static XML_RoomManage.SRoomManage _pRoomManager;

        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            _pRoomManager = XML_RoomManage.Load();
            Strix.CBot.DoInitClient(out _pClient, out _pCommands);

            _pClient.GuildMemberAdded += _pClient_GuildMemberAdded;

            _pCommands.RegisterCommands<Command_RoomManage>();

            await _pClient.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task _pClient_GuildMemberAdded(DSharpPlus.EventArgs.GuildMemberAddEventArgs e)
        {
            await ProcWelComeMessage(e.Member, Strix.CBot.pLobbyChannel);
        }

        public static async Task ProcWelComeMessage(DiscordMember pMember, DiscordChannel pChannel)
        {
            DiscordEmbedBuilder pEmbedBuilder_WellCome = new DiscordEmbedBuilder();
            pEmbedBuilder_WellCome.WithColor(DiscordColor.Cyan).
            WithTitle(_pRoomManager.strWelcomeTitle).
            WithDescription(_pRoomManager.strWelcomeText_ForNewMan);

            await pChannel.SendMessageAsync($"{pMember.Mention} {_pRoomManager.strWelcomeText_ForEveryone}");
            await pChannel.SendMessageAsync(null, false, pEmbedBuilder_WellCome);

            await (pMember.CreateDmChannelAsync().GetAwaiter().GetResult().SendMessageAsync(null, false, CreateWelcomeDM(pMember)));
        }

        static public DiscordEmbedBuilder CreateWelcomeDM(DiscordMember pMember)
        {
            var pConfig = XML_RoomManage.Load();

            DiscordEmbedBuilder pEmbedBuilder = new DiscordEmbedBuilder();
            pEmbedBuilder
                .WithColor(DiscordColor.Green)
                .WithAuthor(pConfig.strWelcomeTitle_DM)
                .WithDescription(pConfig.strWelcomeText_DM);

            return pEmbedBuilder;
        }
    }
}
