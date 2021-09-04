using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Processing.Computing;

namespace HolographicInterferometryVNext
{
    public class OpenClSourcesProvider : IOpenClSourcesProvider
    {
        private const string SourcesExtension = "h";
        private const string SourcesFolderName = "OpenClSources";
        private const string DebugEnvironmentProgramPath = "ImageProcessing\\HolographicInterferometryVNext\\bin";

        private string sourcesFolderPath;

        public OpenClSourcesProvider()
        {
            var programStartupPath = Application.StartupPath;

            if (!programStartupPath.Contains(DebugEnvironmentProgramPath))
            {
                sourcesFolderPath = programStartupPath + "\\" + SourcesFolderName + "\\";

                if (!Directory.Exists(sourcesFolderPath))
                    Directory.CreateDirectory(sourcesFolderPath);

                return;
            }

            var projectFolderPath = programStartupPath.Substring(0, programStartupPath.IndexOf("HolographicInterferometryVNext\\bin"));
            sourcesFolderPath = projectFolderPath + "Processing\\" + SourcesFolderName + "\\";
        }

        public string GetProgram(string name, bool throwErrorIfNotExists = true)
        {
            var path = GetFileName(name);
            if (!File.Exists(path))
            {
                if (throwErrorIfNotExists)
                    throw new FileNotFoundException("Файл не найден", path);

                return "";
            }

            return File.ReadAllText(path);
        }

        public void SaveProgram(string program, string name)
        {
            var path = GetFileName(name);
            using (var streamWriter = new StreamWriter(path))
            {
                streamWriter.Write(program);
            }
        }

        private string GetFileName(string programName)
        {
            return $"{sourcesFolderPath}{programName}.{SourcesExtension}";
        }
    }
}
