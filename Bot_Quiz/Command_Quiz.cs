using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot_Quiz
{
    class Command_Quiz
    {
        public Dictionary<DiscordUser, SQuiz> _mapQuizPlayer = new Dictionary<DiscordUser, SQuiz>();

        [Command("퀴즈")]
        public async Task Command_GenerateQuiz(CommandContext pContext)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            if(_mapQuizPlayer.ContainsKey(pContext.User) == false)
            {
                var pEmbed = GenerateEmbedBuilder(ProcGenerateQuiz(pContext.User));
                await pContext.Channel.SendMessageAsync($"{pContext.User.Mention} {XML_Quiz.pConfig.strQuizStart}", false, pEmbed);
            }
            else
            {

            }
        }

        [Command("정답")]
        public async Task Command_Answer(CommandContext pContext, string strAnswer)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            if(CheckIsCorrectQuiz(pContext.User, strAnswer))
            {
                await pContext.Channel.SendMessageAsync("정답입니다!");
            }
            else
            {
                await pContext.Channel.SendMessageAsync("오답입니다..");
            }
        }

        [RequirePermissions(DSharpPlus.Permissions.ManageChannels)]
        [Command("퀴즈추가")]
        public async Task Command_Request_AddQuiz(CommandContext pContext, string strQuiz, string strAnswer)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            await pContext.Channel.SendMessageAsync("퀴즈추가요청.. " + pContext.Message.Content);
        }

        private SQuiz ProcGenerateQuiz(DiscordUser pUser)
        {
            System.Random pRandom = new System.Random();
            int iRandomIndex = pRandom.Next(0, Program.listQuiz.Count);
            SQuiz pQuiz = Program.listQuiz[iRandomIndex];
            _mapQuizPlayer.Add(pUser, pQuiz);

            return pQuiz;
        }

        private bool CheckIsCorrectQuiz(DiscordUser pUser, string strAnswer)
        {
            return _mapQuizPlayer.ContainsKey(pUser) && _mapQuizPlayer[pUser].strAnswer.Equals(strAnswer);
        }

        private DiscordEmbedBuilder GenerateEmbedBuilder(SQuiz pQuiz)
        {
            XML_Quiz.Load();
            DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
            pEmbed.
                AddField("문제", pQuiz.strQuiz).
                WithFooter($"[출제자 : {pQuiz.strQuizMaker}] [난이도 : {pQuiz.strQuizLevel}]");

            return pEmbed;
        }
    }
}
