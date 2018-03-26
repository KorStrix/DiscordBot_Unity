using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Strix
{
    public class XML_PHPConfig
    {
        public class SPHPConfig
        {
            [XmlElement("PHP_Address_Prefix")]
            public string strPHP_Address_Prefix;

            [XmlElement("DBName")]
            public string strDBName;


            static public SPHPConfig CreateDummy()
            {
                SPHPConfig pDummy = new SPHPConfig();
                pDummy.strPHP_Address_Prefix = "http://www.Test.com/php/{0}.php";
                pDummy.strDBName = "DB Name";

                return pDummy;
            }
        }

        static public SPHPConfig pConfig { get; private set; }

        static public SPHPConfig Load()
        {
            pConfig = Strix.CManagerXMLParser.LoadXML("PHPConfig.xml", SPHPConfig.CreateDummy);
            return pConfig;
        }
    }
}
