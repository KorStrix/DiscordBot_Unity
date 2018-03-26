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
    static public class SCPHPConnector
    {
        static public List<T> Get<T>()
        {
            XML_PHPConfig.Load();
            List<T> list = new List<T>();
            using (WebClient client = new WebClient())
            {
                NameValueCollection postData = new NameValueCollection()
                {
                    { "dbname", "Discord" },  //order: {"parameter name", "parameter value"}
                    { "table", typeof(T).Name },
                    { "paramcount", "0" }

                };

                string strReturn = Encoding.UTF8.GetString(client.UploadValues(string.Format(XML_PHPConfig.pConfig.strPHP_Address_Prefix, "Get"), postData));

                JObject pObject = JObject.Parse(strReturn);
                foreach(JToken pToken in pObject["array"])
                {
                    list.Add(pToken.ToObject<T>());
                }

                return list;
            }
        }
    }
}
