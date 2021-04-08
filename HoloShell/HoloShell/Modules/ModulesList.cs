using System.Collections.Generic;
using System.Xml.Serialization;

namespace HoloShell.Modules
{
    [XmlRoot("modules")]
    public class ModulesList
    {
        [XmlElement("module")]
        public List<ModuleItem> ModuleItems { get; set; }  
    }
}