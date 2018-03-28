using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Strix
{
    public interface IDBHasKey
    {
        string IDBHasKey_GetKeyColumnName();
        string IDBHasKey_GetKeyColumnValue();
    }

    public interface IDBInsertAble
    {
        NameValueCollection IDBInsertAble_GetInsertParameter();
    }

    static public class Extension_DB
    {
        static public T DoInsert_ToDB<T>(this T pInsertAble)
            where T : class, IDBInsertAble
        {
            return SCPHPConnector.Insert(pInsertAble);
        }
    }


    public enum EPHPName
    {
        Update_Set_Custom,
        Get,
        Get_OrInsert,
        Insert,
        Sync,
    }

    static public class SCPHPConnector
    {
        static XML_PHPConfig.SPHPConfig _pConfig;

        static public bool Sync<T>(T pTarget)
             where T : class, IDBInsertAble
        {
            return ProcExcutePHP<T>(EPHPName.Sync, pTarget.IDBInsertAble_GetInsertParameter());
        }

        static public T[] Get<T>()
        {
            return GetValue<T>(EPHPName.Get);
        }

        static public T[] Get_OrInsert<T>()
        {
            return GetValue<T>(EPHPName.Get_OrInsert);
        }

        static public T Update_Set<T>(string strDBKey, string strDBValue, string strUpdateColumnName, string strUpdateColumnValue)
        {
            NameValueCollection arrPostAdd = new NameValueCollection()
            {
                { strDBKey, strDBValue },
                { strUpdateColumnName, strUpdateColumnValue }
            };

            return GetValue<T>(EPHPName.Update_Set_Custom, arrPostAdd)[0];
        }

        static public T Update_Set<T>(T pTarget, string strUpdateColumnName, string strUpdateColumnValue)
            where T : class, IDBHasKey
        {
            return Update_Set<T>(pTarget.IDBHasKey_GetKeyColumnName(), pTarget.IDBHasKey_GetKeyColumnValue(), strUpdateColumnName, strUpdateColumnValue);
        }

        static public T Insert<T>(T pTarget)
            where T : class, IDBInsertAble
        {
            return GetValue<T>(EPHPName.Insert, pTarget.IDBInsertAble_GetInsertParameter())[0];
        }


        static private T[] GetValue<T>(EPHPName ePHPName, NameValueCollection arrPostAdd = null)
        {
            ProcCheck_And_UpdateConfig();
            NameValueCollection arrPost = ProcGenerateParam(arrPostAdd, typeof(T).Name);

            using (WebClient pWebClient = new WebClient())
            {
                string strReturn = Encoding.UTF8.GetString(pWebClient.UploadValues(string.Format(XML_PHPConfig.pConfig.strPHP_Address_Prefix, ePHPName.ToString()), arrPost));
                if (strReturn.Equals("false"))
                    return null;

                JToken pTokenArray = JObject.Parse(strReturn)["array"];
                int iLoopIndex = 0;
                T[] arrReturn = new T[pTokenArray.Count()];
                foreach (JToken pToken in pTokenArray)
                {
                    arrReturn[iLoopIndex++] = pToken.ToObject<T>();
                }

                return arrReturn;
            }
        }

        static private bool ProcExcutePHP<T>(EPHPName ePHPName, NameValueCollection arrPostAdd = null)
        {
            ProcCheck_And_UpdateConfig();
            NameValueCollection arrPost = ProcGenerateParam(arrPostAdd, typeof(T).Name);

            using (WebClient pWebClient = new WebClient())
            {
                string strReturn = Encoding.UTF8.GetString(pWebClient.UploadValues(string.Format(XML_PHPConfig.pConfig.strPHP_Address_Prefix, ePHPName.ToString()), arrPost));
                return !strReturn.Equals("false");
            }
        }

        static private NameValueCollection ProcGenerateParam(NameValueCollection arrPostAdd, string strTableName)
        {
            NameValueCollection arrPost = new NameValueCollection()
                {
                    { "dbname", _pConfig.strDBName },  //order: {"parameter name", "parameter value"}
                    { "table", strTableName },
                    { "paramcount", arrPostAdd != null ? arrPostAdd.Count.ToString() : "0" }
                };

            if (arrPostAdd != null)
            {
                int iLoopCount = 0;
                foreach (string strKey in arrPostAdd)
                {
                    arrPost.Add($"key{iLoopCount}", strKey);
                    arrPost.Add($"value{iLoopCount}", arrPostAdd[strKey]);
                    iLoopCount++;
                }
            }

            return arrPost;
        }

        static private void ProcCheck_And_UpdateConfig()
        {
            if(_pConfig == null)
                _pConfig = XML_PHPConfig.Load();
        }
    }
}
