using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bot_Room_Manager
{
    class XML_RoomManage
    {
        public class SConfig
        {
            [XmlElement("Token")]
            public string strBotToken;

            [XmlElement("CallID")]
            public string strCall_ID;
        }

        static public SConfig pConfig { get; private set; }
    }
}
