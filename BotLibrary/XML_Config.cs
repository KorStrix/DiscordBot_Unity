using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Strix
{
    public class XML_Config
    {
        public class SConfig
        {
            [XmlElement("Token")]
            public string strBotToken;

            [XmlElement("CallID")]
            public string strCall_ID;

            [XmlElement("CallChannel")]
            public string strCall_Channel;

            [XmlElement("LobbyChannelID")]
            public ulong strLobbyChannelID;

            [XmlElement("BootingMessage")]
            public string strBootingMessage;

            [XmlRoot("Tutorial")]
            public class STutorial
            {
                [XmlAttribute("Title")]
                public string strTitle;
                [XmlArray("Fields"), XmlArrayItem("Field")]
                public STutorial_Field[] arrField;
            }

            public class STutorial_Field
            {
                public STutorial_Field() { }

                public STutorial_Field(string strFieldName, string strFieldValue)
                {
                    this.strFieldName = strFieldName;
                    this.strFieldValue = strFieldValue;
                }

                [XmlAttribute("FieldName")]
                public string strFieldName;
                [XmlAttribute("FieldValue")]
                public string strFieldValue;
            }

            static public SConfig CreateDummy()
            {
                SConfig pDummy = new SConfig();
                pDummy.strBotToken = "토큰";
                pDummy.strCall_ID = "호출 명령어";
                pDummy.strCall_Channel = "제한 채널";
                pDummy.strLobbyChannelID = 0;
                pDummy.strBootingMessage = "부팅메세지";

                pDummy.pTutorial.strTitle = "로봇이름 - 튜토리얼";
                pDummy.pTutorial.arrField = new STutorial_Field[1]
                { new STutorial_Field("기능", "설명") };

                return pDummy;
            }


            [XmlElement("Tutorial")]
            public STutorial pTutorial = new STutorial();
        }

        static public SConfig pConfig { get; private set; }

        static public SConfig Load()
        {
            pConfig = Strix.CManagerXMLParser.LoadXML("Config.xml", SConfig.CreateDummy);
            return pConfig;
        }
    }
}
