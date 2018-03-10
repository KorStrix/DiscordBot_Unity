using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Strix
{
	public class CBot
	{
        static public DiscordClient pClient { get; private set; }
        static public CommandsNextModule pCommands { get; private set; }

        static public DiscordChannel pLobbyChannel { get; private set; }

        static public void DoInitClient(out DiscordClient pClient, out CommandsNextModule pCommandModule)
        {
            XML_Config.Load();
            CBot.pClient = new DiscordClient(new DiscordConfiguration
            {
                Token = XML_Config.pConfig.strBotToken,
                TokenType = TokenType.Bot,

                UseInternalLogHandler = true,
                LogLevel = DSharpPlus.LogLevel.Debug
            });

            CBot.pClient.Ready += PClinet_Ready;

            CommandsNextConfiguration pConfiguration = new CommandsNextConfiguration();
            if (string.IsNullOrEmpty(XML_Config.pConfig.strCall_ID) == false)
                pConfiguration.StringPrefix = XML_Config.pConfig.strCall_ID;
            //else
            //    ProcNotStringPrefix(pConfiguration);

            pCommands = CBot.pClient.UseCommandsNext(pConfiguration);
            pCommands.RegisterCommands<Commands_Tutorial>();


            pClient = CBot.pClient;
            pCommandModule = pCommands;
        }

#pragma warning disable 1998
        private static async Task PClinet_Ready(DSharpPlus.EventArgs.ReadyEventArgs e)
        {
            pLobbyChannel = await (pClient.GetChannelAsync(XML_Config.pConfig.strLobbyChannelID).ConfigureAwait(false));
            if (pLobbyChannel == null)
                Console.WriteLine($"Error pLobbyChannel == null");

            if (XML_Config.pConfig.strLobbyChannelID == 0 ||
                string.IsNullOrEmpty(XML_Config.pConfig.strBootingMessage)) return;

#if !DEBUG
            await (
                await pClient.GetChannelAsync(XML_Config.pConfig.strLobbyChannelID).ConfigureAwait(false))
                .SendMessageAsync(XML_Config.pConfig.strBootingMessage);
#endif
        }
#pragma warning restore 1998

        //static private IReadOnlyDictionary<string, Command> _mapCommand;

        //static private void ProcNotStringPrefix(CommandsNextConfiguration pConfiguration)
        //{
        //    pConfiguration.StringPrefix = "SE";
        //    pConfiguration.SelfBot = true;

        //    pClient.MessageCreated += async eMessageArgs =>
        //    {
        //        if (CheckIsRespond(eMessageArgs.Channel) == false) return;

        //        if(_mapCommand == null)
        //            _mapCommand = pCommands.RegisteredCommands;

        //        string strCommand = eMessageArgs.Message.Content;
        //        string[] arrCommand = strCommand.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        //        Command pCommand = null;
        //        if (_mapCommand.TryGetValue(arrCommand[0], out pCommand) == false)
        //            return;

        //        await eMessageArgs.Channel.SendMessageAsync("SE " + strCommand);
        //    };
        //}

        static public bool CheckIsRespond(DiscordChannel pChannel)
        {
            if (string.IsNullOrEmpty(XML_Config.pConfig.strCall_Channel)) return true;

            return pChannel.Name.ToLower().Contains(XML_Config.pConfig.strCall_Channel);
        }
    }
}
