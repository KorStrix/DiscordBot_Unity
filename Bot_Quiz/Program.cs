using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Strix;

namespace Bot_Quiz
{
    [System.Serializable]
    public class SQuiz
    {
        public ulong ulQuizID;
        public string strQuizMaker;
        public string strQuizLevel;
        public string strQuiz;
        public string strAnswer;
    }

    [System.Serializable]
    public class SQuiz_NonRegistered : IDBInsertAble
    {
        public ulong ulQuizID;
        public string strQuizMaker;
        public string strQuiz;
        public string strAnswer;

        public SQuiz_NonRegistered() { }

        public SQuiz_NonRegistered(DiscordUser pUser, string strQuiz, string strAnswer)
        {
            strQuizMaker = pUser.Username;
            this.strQuiz = strQuiz;
            this.strAnswer = strAnswer;
        }

        public NameValueCollection IDBInsertAble_GetInsertParameter()
        {
            NameValueCollection arrInsertParameter = new NameValueCollection();
            arrInsertParameter.Add("strQuizMaker", strQuizMaker);
            arrInsertParameter.Add("strQuiz", strQuiz);
            arrInsertParameter.Add("strAnswer", strAnswer);

            return arrInsertParameter;
        }
    }

    [System.Serializable]
    public class SQuizMember : IDictionaryItem<ulong>
    {
        public ulong ulUserID;
        public string strNickName;
        public string strGrade;
        public ulong ulQuizPoint;
        public ulong ulQuizGenreateCount;
        public ulong ulQuizWinCount;

        public ulong IDictionaryItem_GetKey()
        {
            return ulUserID;
        }
    }

    class Program
    {
        static private DiscordClient _pClient;
        static CommandsNextModule _pCommands;

        static public List<SQuiz> listQuiz;
        static public List<SQuiz_NonRegistered> listQuiz_NonRegistered;
        static public Dictionary<ulong, SQuizMember> mapQuizMember = new Dictionary<ulong, SQuizMember>();

        static void Main(string[] args)
        {
            XML_Quiz.Load();

            SyncDB();
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static void SyncDB()
        {
            listQuiz = new List<SQuiz>(Strix.SCPHPConnector.Get<SQuiz>());
            listQuiz_NonRegistered = new List<SQuiz_NonRegistered>(Strix.SCPHPConnector.Get<SQuiz_NonRegistered>());
            mapQuizMember.AddRange(Strix.SCPHPConnector.Get<SQuizMember>());
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
