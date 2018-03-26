using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Quiz
{
    class Command_Quiz
    {
        [Command("퀴즈")]
        public async Task Command_GenerateQuiz(CommandContext pContext)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            await pContext.Channel.SendMessageAsync(pContext.Message.Content);
        }

        [Command("답변")]
        public async Task Command_Answer(CommandContext pContext, int iAnswer)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            await pContext.Channel.SendMessageAsync("답변 : " + iAnswer);
        }

        [Command("퀴즈추가")]
        public async Task Command_Request_AddQuiz(CommandContext pContext)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            await pContext.Channel.SendMessageAsync(pContext.Message.Content);
        }
    }
}
