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
    class Program
    {
        static private DiscordClient _pClient;
        static CommandsNextModule _pCommands;

        static public List<SQuiz> listQuiz;
        static public List<SQuiz_NonRegistered> listQuiz_NonRegistered;
        static public Dictionary<ulong, SQuizMember> mapQuizMember = new Dictionary<ulong, SQuizMember>();
        static public Dictionary<EUserRole, SQuizRole> mapQuizRole = new Dictionary<EUserRole, SQuizRole>();

        static void Main(string[] args)
        {
            XML_Quiz.Load();

            SyncDB();
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        static void SyncDB()
        {
            listQuiz = new List<SQuiz>(SCPHPConnector.Get<SQuiz>());
            listQuiz_NonRegistered = new List<SQuiz_NonRegistered>(SCPHPConnector.Get<SQuiz_NonRegistered>());

            mapQuizMember.AddRange(SCPHPConnector.Get<SQuizMember>());
            mapQuizRole.AddRange(SCPHPConnector.Get<SQuizRole>());
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
