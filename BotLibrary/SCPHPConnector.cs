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
    public interface IDBInsertAble
    {
        NameValueCollection IDBInsertAble_GetInsertParameter();
    }

    public enum EPHPName
    {
        Get,
        Get_OrInsert,
        Insert,
    }

    static public class SCPHPConnector
    {
        static XML_PHPConfig.SPHPConfig _pConfig;

        static public T[] Get<T>()
        {
            return GetValue<T>(EPHPName.Get);
        }

        static public T[] Get_OrInsert<T>()
        {
            return GetValue<T>(EPHPName.Get_OrInsert);
        }

        static public T Insert<T>(T pInsert)
            where T : class, IDBInsertAble
        {
            return GetValue<T>(EPHPName.Insert, pInsert.IDBInsertAble_GetInsertParameter())[0];
        }


        static private T[] GetValue<T>(EPHPName ePHPName, NameValueCollection arrPostAdd = null)
        {
            ProcCheck_And_UpdateConfig();

            NameValueCollection arrPost = new NameValueCollection()
                {
                    { "dbname", _pConfig.strDBName },  //order: {"parameter name", "parameter value"}
                    { "table", typeof(T).Name },
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

            using (WebClient pWebClient = new WebClient())
            {
                string strReturn = Encoding.UTF8.GetString(pWebClient.UploadValues(string.Format(XML_PHPConfig.pConfig.strPHP_Address_Prefix, ePHPName.ToString()), arrPost));
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

        static private void ProcCheck_And_UpdateConfig()
        {
            if(_pConfig == null)
                _pConfig = XML_PHPConfig.Load();
        }
    }
}
