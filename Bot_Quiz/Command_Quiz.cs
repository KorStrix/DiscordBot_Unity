using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Strix;

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

            SQuiz_NonRegistered pQuizNew = new SQuiz_NonRegistered(pContext.User, strQuiz, strAnswer);
            pQuizNew = SCPHPConnector.Insert(pQuizNew);

            Program.listQuiz_NonRegistered.Add(pQuizNew);

            await pContext.Channel.SendMessageAsync("퀴즈추가요청완료");
        }

        [RequirePermissions(DSharpPlus.Permissions.ManageChannels)]
        [Command("퀴즈후보보기")]
        public async Task Command_Request_AddQuiz(CommandContext pContext)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
            pEmbed.WithTitle("퀴즈후보리스트입니다.");

            int iLoopCount = 1;
            foreach(SQuiz_NonRegistered pQuiz in Program.listQuiz_NonRegistered)
            {
                string strAnswer = pQuiz.strAnswer;
                if (strAnswer.Length > 20)
                    strAnswer = $"{strAnswer.Substring(0, 20)}...";
                pEmbed.AddField($"{iLoopCount++} . {pQuiz.strQuiz}",
                    $"ㄴ{pQuiz.strAnswer} 제출자 : {pQuiz.strQuizMaker}");
            }

            await pContext.Channel.SendMessageAsync(null, false, pEmbed);
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
            DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
            pEmbed.
                AddField("문제", pQuiz.strQuiz).
                WithFooter($"[출제자 : {pQuiz.strQuizMaker}] [난이도 : {pQuiz.strQuizLevel}]");

            return pEmbed;
        }
    }
}
