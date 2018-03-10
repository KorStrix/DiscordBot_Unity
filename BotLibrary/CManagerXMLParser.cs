using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Strix
{
    public static class CManagerXMLParser
    {
        static public T LoadXML<T>(string strFileName, Func<T> OnGenerateDummy)
            where T : class
        {
            var serializer = new XmlSerializer(typeof(T));

            string strFilePath = Directory.GetCurrentDirectory() + "//" + strFileName;
            if (File.Exists(strFilePath) == false)
            {
                Save(strFilePath, OnGenerateDummy);
            }

            using (var stream = new FileStream(Directory.GetCurrentDirectory() + "//" + strFileName, FileMode.Open))
            {
                return serializer.Deserialize(stream) as T;
            }
        }

        static public void Save<T>(string strFileName, T pConfig)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new FileStream(strFileName, FileMode.Create))
            {
                serializer.Serialize(stream, pConfig);
            }
        }
    }
}
