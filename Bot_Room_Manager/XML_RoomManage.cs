using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bot_Room_Manager
{
    public class XML_RoomManage
    {
        [Serializable]
        public class SRoomManage
        {
            [XmlElement("WelcomeTitle")]
            public string strWelcomeTitle;

            [XmlElement("WelcomeText_ForNewMan")]
            public string strWelcomeText_ForNewMan;

            [XmlElement("WelcomeText_ForEveryone")]
            public string strWelcomeText_ForEveryone;

            [XmlElement("WelcomeTitle_DM")]
            public string strWelcomeTitle_DM;

            [XmlElement("WelcomeText_DM")]
            public string strWelcomeText_DM;


            static public SRoomManage CreateDummy()
            {
                SRoomManage pDummy = new SRoomManage();
                pDummy.strWelcomeTitle = "웰컴 타이틀";
                pDummy.strWelcomeText_ForNewMan = "웰컴 개인메세지";
                pDummy.strWelcomeText_ForEveryone = "웰컴 모두에게";

                pDummy.strWelcomeTitle_DM = "웰컴 DM 타이틀";
                pDummy.strWelcomeText_DM = "웰컴 DM 개인메세지";

                return pDummy;
            }
        }

        static public SRoomManage pConfig { get; private set; }

        static public SRoomManage Load()
        {
            pConfig = Strix.CManagerXMLParser.LoadXML("RoomManage.xml", SRoomManage.CreateDummy);
            return pConfig;
        }
    }
}
