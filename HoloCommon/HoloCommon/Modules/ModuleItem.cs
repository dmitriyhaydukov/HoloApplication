using System.Xml.Serialization;

namespace HoloCommon.Modules
{
    public class ModuleItem
    {
        [XmlAttribute("path")]
        public string Path { get; set; }

        [XmlAttribute("arguments")]
        public string Arguments { get; set; }

        [XmlAttribute("waitForExit")]
        public bool WaitForExit { get; set; }
    }
}