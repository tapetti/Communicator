using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Communicator
{
    class XmlManager
    {
        public void XmlDataWriter(object obj, string filePath)
        {
            XmlSerializer sr = new XmlSerializer(obj.GetType());
            TextWriter writer = new StreamWriter(filePath);
            sr.Serialize(writer, obj);
            writer.Close();
        }

        public DataGeneralSettings XmlDataSettingsReader(string filePath)
        {
            DataGeneralSettings obj = new DataGeneralSettings();
            XmlSerializer xs = new XmlSerializer(typeof(DataGeneralSettings));
            FileStream reader = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            obj = (DataGeneralSettings)xs.Deserialize(reader);
            reader.Close();
            return obj;
        }
    }
}
