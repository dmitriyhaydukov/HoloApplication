using System.Xml.Serialization;
using System.IO;

namespace HoloCommon.Modules
{
    public class ModulesReader
    {
        public ModulesList ReadModules(string filePath)
        {
            string data = File.ReadAllText(filePath);
            XmlSerializer serializer = new XmlSerializer(typeof(ModulesList));
            using (TextReader reader = new StringReader(data))
            {
                ModulesList modulesList = (ModulesList)serializer.Deserialize(reader);
                return modulesList;
            }
        }
    }
}