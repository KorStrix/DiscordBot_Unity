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
                SConfig pConfigDummy = new SConfig();
                pConfigDummy.strBotToken = "토큰";
                pConfigDummy.strCall_ID = "호출 명령어";
                pConfigDummy.strCall_Channel = "제한 채널";
                pConfigDummy.strLobbyChannelID = 0;
                pConfigDummy.strBootingMessage = "부팅메세지";

                pConfigDummy.pTutorial.strTitle = "로봇이름 - 튜토리얼";
                pConfigDummy.pTutorial.arrField = new STutorial_Field[1]
                { new STutorial_Field("기능", "설명") };

                return pConfigDummy;
            }


            [XmlElement(ElementName = "Tutorial")]
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
