using System.Configuration;

namespace HoloShell
{
    public static class ShellConfig
    {
        public static string ModulesDescriptorPath
        {
            get
            {
                return ConfigurationManager.AppSettings["modulesDescriptorPath"];
            }
        }
    }
}