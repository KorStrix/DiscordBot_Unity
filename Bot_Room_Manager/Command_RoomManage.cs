using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Room_Manager
{
    class Command_RoomManage
    {
        [Command("testwelcome")]
        public async Task Test_Welcome(CommandContext pContext)
        {
            await Program.ProcWelComeMessage(pContext.Member, pContext.Channel);
        }
    }
}
