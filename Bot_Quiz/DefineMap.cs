using DSharpPlus.Entities;
using Strix;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot_Quiz
{
    [System.Serializable]
    public class SQuiz : IDBHasKey, IDBInsertAble
    {
        public ulong ulQuizID;
        public string strQuizMaker;
        public string strQuizLevel;
        public string strQuiz;
        public string strAnswer;

        public ulong ulQuizCount;
        public ulong ulWinCount;

        public void DoAdd_QuizCount()
        {
            SCPHPConnector.Update_Set(this, nameof(ulQuizCount), (++ulQuizCount).ToString());
        }

        public void DoAdd_WinCount()
        {
            SCPHPConnector.Update_Set(this, nameof(ulWinCount), (++ulWinCount).ToString());
        }

        public string IDBHasKey_GetKeyColumnName()
        {
            return nameof(ulQuizID);
        }

        public string IDBHasKey_GetKeyColumnValue()
        {
            return ulQuizID.ToString();
        }

        public NameValueCollection IDBInsertAble_GetInsertParameter()
        {
            return new NameValueCollection()
            {
                { nameof(strQuizMaker), strQuizMaker },
                { nameof(strQuizLevel), strQuizLevel },
                { nameof(strQuiz), strQuiz },
                { nameof(strAnswer), strAnswer },
            };
        }

        public string Print_WinPercentage()
        {
            return $"{((float)ulWinCount / ulQuizCount).ToString("F2")}% [정답 횟수({ulWinCount}) / 출제 횟수({ulQuizCount})]";
        }
    }

    [System.Serializable]
    public class SQuiz_NonRegistered : IDictionaryItem<ulong>, IDBInsertAble
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
            return new NameValueCollection()
            {
                { nameof(strQuizMaker), strQuizMaker},
                { nameof(strQuiz), strQuiz },
                { nameof(strAnswer), strAnswer }
            };
        }

        public ulong IDictionaryItem_GetKey()
        {
            return ulQuizID;
        }

        public void DoRegistQuiz()
        {
            SQuiz pQuizNew = new SQuiz();
            pQuizNew.strQuizMaker = strQuizMaker;
            pQuizNew.strQuiz = strQuiz;
            pQuizNew.strAnswer = strAnswer;

            pQuizNew.strQuizLevel = nameof(EUserRole.지망생);
            pQuizNew = pQuizNew.DoInsert_ToDB();
            Program.listQuiz.Add(pQuizNew);
        }
    }

    public enum EUserRole
    {
        지망생,
        사원,
        대리,
        과장,
        부장,
        이사,
    }

    [System.Serializable]
    public class SQuizRole : IDictionaryItem<EUserRole>
    {
        public string strGrade;
        public int iQuizPoint;

        public EUserRole IDictionaryItem_GetKey()
        {
            return strGrade.ConvertEnum<EUserRole>();
        }
    }


    [System.Serializable]
    public class SQuizMember : IDBInsertAble ,IDictionaryItem<ulong>, IDBHasKey
    {
        public ulong ulUserID;
        public string strNickName;
        public string strGrade;
        public ulong ulQuizPoint;
        public ulong ulQuizGenreateCount;
        public ulong ulQuizWinCount;
        public ulong ulQuizTryCount;

        public EUserRole p_pRole {  get { return strGrade.ConvertEnum<EUserRole>(); } }

        public SQuizMember() { }

        public SQuizMember(ulong ulUserID, string strNickName)
        {
            this.ulUserID = ulUserID;
            this.strNickName = strNickName;
            strGrade = nameof(EUserRole.지망생);
        }

        public ulong IDictionaryItem_GetKey()
        {
            return ulUserID;
        }

        public string DoPrint_Point(bool bIsPromotion = false)
        {
            SQuizRole pQuizRole = Program.mapQuizRole[p_pRole];

            if(bIsPromotion)
                return $"현재[ ({p_pRole.PrevEnum_String<EUserRole>()} -> {strGrade}) {ulQuizPoint} / 다음 역할({p_pRole.NextEnum_String<EUserRole>()}) {pQuizRole.iQuizPoint} ]";
            else
                return $"현재[ ({strGrade}) {ulQuizPoint} / 다음 역할({p_pRole.NextEnum_String<EUserRole>()}) {pQuizRole.iQuizPoint} ]";
        }

        public void DoAdd_QuizTryCount()
        {
            SCPHPConnector.Update_Set(this, nameof(ulQuizTryCount), (++ulQuizTryCount).ToString());
        }

        public bool DoAdd_QuizPoint(int iPoint)
        {
            ulQuizPoint += (ulong)iPoint;
            SCPHPConnector.Update_Set(this, nameof(ulQuizPoint), ulQuizPoint.ToString());

            ulQuizWinCount += 1;
            SCPHPConnector.Update_Set(this, nameof(ulQuizWinCount), ulQuizWinCount.ToString());

            SQuizRole pQuizRole = Program.mapQuizRole[p_pRole];
            bool bIsPromotion = ulQuizPoint >= (ulong)pQuizRole.iQuizPoint;
            if(bIsPromotion)
            {
                strGrade = p_pRole.NextEnum_String<EUserRole>();
                SCPHPConnector.Update_Set(this, nameof(strGrade), strGrade);
            }

            return bIsPromotion;
        }

        public string IDBHasKey_GetKeyColumnName()
        {
            return nameof(ulUserID);
        }

        public string IDBHasKey_GetKeyColumnValue()
        {
            return ulUserID.ToString();
        }

        public NameValueCollection IDBInsertAble_GetInsertParameter()
        {
            return new NameValueCollection()
            {
                { nameof(ulUserID), ulUserID.ToString() },
                { nameof(strNickName), strNickName },
                { nameof(strGrade), strGrade },
            };
        }

        public async void DoUpdateRole(DiscordGuild pGuild, DiscordMember pMember)
        {
            var arrRoles = pGuild.Roles;
            foreach (var pRole in arrRoles)
            {
                if (pRole.Name.Equals(p_pRole.ToString()))
                    await pGuild.GrantRoleAsync(pMember, pRole);

                if (pRole.Name.Equals(p_pRole.PrevEnum_String<EUserRole>()))
                    await pGuild.RevokeRoleAsync(pMember, pRole, "");
            }
        }
    }
}
