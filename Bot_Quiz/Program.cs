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

    class Program
    {
        static private DiscordClient _pClient;
        static CommandsNextModule _pCommands;

        static public List<SQuiz> listQuiz;

        static void Main(string[] args)
        {
            SyncDB();
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static void SyncDB()
        {
            listQuiz = Strix.SCPHPConnector.Get<SQuiz>();
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
