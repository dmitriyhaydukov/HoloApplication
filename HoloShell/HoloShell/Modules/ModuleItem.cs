using System.Xml.Serialization;

namespace HoloShell.Modules
{
    public class ModuleItem
    {
        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("arguments")]
        public string Arguments { get; set; }
    }
}