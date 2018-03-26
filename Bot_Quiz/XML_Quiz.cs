using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bot_Quiz
{
    public class XML_Quiz
    {
        public class SQuizConfig
        {
            [XmlElement("문제시작멘트")]
            public string strQuizStart;

            static public SQuizConfig CreateDummy()
            {
                SQuizConfig pDummy = new SQuizConfig();
                pDummy.strQuizStart = "을 위한 문제입니다!";

                return pDummy;
            }
        }

        static public SQuizConfig pConfig { get; private set; }

        static public SQuizConfig Load()
        {
            pConfig = Strix.CManagerXMLParser.LoadXML("QuizConfig.xml", SQuizConfig.CreateDummy);
            return pConfig;
        }
    }
}