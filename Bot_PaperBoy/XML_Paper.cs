using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bot_PaperBoy
{
    public class XML_Paper
    {
        public enum ECrawlingKey
        {
            [XmlEnum("게임메카")]
            게임메카,
            [XmlEnum("디스이즈게임즈")]
            디스이즈게임즈,
        }

        public class SPaperConfig
        {
            [XmlRoot("PaperConfigDetail")]
            public class SPaperConfigDetail
            {
                [XmlAttribute("CrawlingKey")]
                public ECrawlingKey eReportChannelID_GameNews = ECrawlingKey.게임메카;

                [XmlAttribute("IsUsage")]
                public bool bIsUsage = true;

                [XmlAttribute("Hour")]
                public int iHour = 0;

                [XmlAttribute("Minute")]
                public int iMinute = 0;

                [XmlAttribute("Second")]
                public int iSecond = 0;

                [XmlAttribute("ReportChannelID")]
                public ulong iReportChannelID;
            }

            [XmlArray("PaperConfigDetailList"), XmlArrayItem("PaperConfigDetail")]
            public SPaperConfigDetail[] arrConfig;

            static public SPaperConfig CreateDummy()
            {
                SPaperConfig pConfig = new SPaperConfig();
                pConfig.arrConfig = new SPaperConfigDetail[1] { new SPaperConfigDetail() };

                return pConfig;
            }
        }

        [XmlArray("PaperConfigDetailList"), XmlArrayItem("PaperConfigDetail")]
        static public SPaperConfig pConfig;

        static public SPaperConfig Load()
        {
            pConfig = Strix.CManagerXMLParser.LoadXML("SPaperConfig.xml", SPaperConfig.CreateDummy);
            return pConfig;
        }
    }
}
