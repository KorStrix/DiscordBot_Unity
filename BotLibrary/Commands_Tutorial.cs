using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Strix.CBot;

namespace Strix
{
    public class Commands_Tutorial
    {
        [Command("tutorial")]
        public async Task Start_Tutorial_1(CommandContext pContext) { if (CBot.CheckIsRespond(pContext) == false) return; await DoStartTutorial(pContext); }
        [Command("hi")]
        public async Task Start_Tutorial_2(CommandContext pContext) { if (CBot.CheckIsRespond(pContext) == false) return; await DoStartTutorial(pContext); }
        [Command("hello")]
        public async Task Start_Tutorial_3(CommandContext pContext) { if (CBot.CheckIsRespond(pContext) == false) return; await DoStartTutorial(pContext); }
        [Command("안녕")]
        public async Task Start_Tutorial_4(CommandContext pContext) { if (CBot.CheckIsRespond(pContext) == false) return; await DoStartTutorial(pContext); }
        [Command("튜토리얼")]
        public async Task Start_Tutorial_5(CommandContext pContext) { if (CBot.CheckIsRespond(pContext) == false) return; await DoStartTutorial(pContext); }

        static public async Task DoStartTutorial(CommandContext pContext)
        {
            var pConfig = XML_Config.Load();
            var pTutorial = pConfig.pTutorial;

            DiscordEmbedBuilder pEmbedBuilder = new DiscordEmbedBuilder();
            pEmbedBuilder.WithAuthor(pTutorial.strTitle);

            for (int i = 0; i < pTutorial.arrField.Length; i++)
                pEmbedBuilder.AddField(pTutorial.arrField[i].strFieldName, pTutorial.arrField[i].strFieldValue);

            await pContext.RespondAsync(null, false, pEmbedBuilder);
        }
    }
}
