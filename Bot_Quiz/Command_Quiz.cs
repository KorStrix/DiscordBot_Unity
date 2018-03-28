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

            if(ProcCheck_And_AddQuizMember(pContext.User))
            {
                await pContext.Channel.SendMessageAsync($"첫 도전을 환영합니다! {pContext.User.Mention}");
            }

            bool bAlreadyExsist;
            var pEmbed = GenerateEmbedBuilder_Quiz(pContext.User, out bAlreadyExsist);
            if (bAlreadyExsist)
            {
                SQuiz pQuiz;
                if (TryGetPlayingQuiz(pContext.User, out pQuiz))
                {
                    await pContext.Channel.SendMessageAsync($"이미 푸시던 퀴즈가 있어 다시 내드리겠습니다. {pContext.User.Mention}", false, pEmbed);
                }
                else
                    await pContext.Channel.SendMessageAsync("에러!");
            }
            else
            {
                await pContext.Channel.SendMessageAsync($"{pContext.User.Mention} {XML_Quiz.pConfig.strQuizStart}", false, pEmbed);
            }
        }

        [Command("정답")]
        public async Task Command_Answer(CommandContext pContext, string strAnswer)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            SQuiz pQuiz;
            if(TryGetPlayingQuiz(pContext.User, out pQuiz))
            {
                SQuizMember pMember = Program.mapQuizMember[pContext.User.Id];
                pMember.DoAdd_QuizTryCount();

                if (pQuiz.strAnswer.Equals(strAnswer))
                {
                    pQuiz.DoAdd_WinCount();
                    if(pMember.DoAdd_QuizPoint(1))
                        await pContext.Channel.SendMessageAsync($"정답입니다! 진급을 축하드립니다! {pMember.DoPrint_Point(true)}");
                    else
                        await pContext.Channel.SendMessageAsync($"정답입니다! 포인트를 획득하셨습니다! {pMember.DoPrint_Point()}");
                }
                else
                {
                    await pContext.Channel.SendMessageAsync("오답입니다..");
                }

                _mapQuizPlayer.Remove(pContext.User);

            }
            else
                await pContext.Channel.SendMessageAsync("에러!");
        }


        [RequirePermissions(DSharpPlus.Permissions.ManageChannels)]
        [Command("퀴즈추가")]
        public async Task Command_Request_AddQuiz(CommandContext pContext, string strQuiz, string strAnswer)
        {
            if (Strix.CBot.CheckIsRespond(pContext.Channel) == false) return;

            SQuiz_NonRegistered pQuizNew = new SQuiz_NonRegistered(pContext.User, strQuiz, strAnswer);
            Program.listQuiz_NonRegistered.Add(pQuizNew.DoInsert_ToDB());

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

        // ==================================================================================== //

        private SQuiz ProcGenerateQuiz(DiscordUser pUser)
        {
            System.Random pRandom = new System.Random();
            int iRandomIndex = pRandom.Next(0, Program.listQuiz.Count);
            SQuiz pQuiz = Program.listQuiz[iRandomIndex];
            pQuiz.DoAdd_QuizCount();

            _mapQuizPlayer.Add(pUser, pQuiz);

            return pQuiz;
        }
       
        private DiscordEmbedBuilder GenerateEmbedBuilder_Quiz(DiscordUser pUser, out bool bAlreadyExist)
        {
            SQuiz pQuiz;
            bAlreadyExist = TryGetPlayingQuiz(pUser, out pQuiz);
            if (bAlreadyExist == false)
                pQuiz = ProcGenerateQuiz(pUser);

            SQuizMember pMember = Program.mapQuizMember[pUser.Id];

            DiscordEmbedBuilder pEmbed = new DiscordEmbedBuilder();
            pEmbed.
                AddField("문제", pQuiz.strQuiz).
                AddField("포인트 현황", $"{pMember.DoPrint_Point()}").
                AddField("이 문제의 정답률", $"{pQuiz.Print_WinPercentage()}" ).
                WithFooter($"[출제자 : {pQuiz.strQuizMaker}] [난이도 : {pQuiz.strQuizLevel}]");

            return pEmbed;
        }

        // ==================================================================================== //

        private bool ProcCheck_And_AddQuizMember(DiscordUser pUser)
        {
            if (Program.mapQuizMember.ContainsKey(pUser.Id)) return false;

            SQuizMember pQuizMember = new SQuizMember(pUser.Id, pUser.Username);
            Program.mapQuizMember.Add(pUser.Id, pQuizMember.DoInsert_ToDB());

            return true;
        }

        private bool TryGetPlayingQuiz(DiscordUser pUser, out SQuiz pQuiz)
        {
            return _mapQuizPlayer.TryGetValue(pUser, out pQuiz);
        }
    }
}
