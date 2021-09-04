using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Common
{
    public static class DebugLogger
    {
        public enum ImportanceLevel
        {
            Debug = 0,
            Info = 1,
            Notification = 2,
            Warning = 3,
            Exception = 4
        }

        private static int NestingLevel = 0;

        public static ImportanceLevel MinimalImportanceLevel = ImportanceLevel.Debug;


        public static void Warning(Exception exception)
        {
            Log(exception, ImportanceLevel.Warning);
        }

        public static void Warning(string warning)
        {
            Log(warning, ImportanceLevel.Warning);
        }

        public static void Exception(Exception exception)
        {
            Log(exception, ImportanceLevel.Exception);
        }

        public static void Log(object obj, ImportanceLevel importance = ImportanceLevel.Debug)
        {
            Log(obj.ToString());
        }

        public static void Log(string line, ImportanceLevel importance = ImportanceLevel.Debug)
        {
            if (importance < MinimalImportanceLevel)
                return;

            if (NestingLevel == 0)
                Debug.WriteLine(line);
            else
                Debug.WriteLine("┃".Repeat(NestingLevel - 1) + $"┣[{importance}]{line}");
        }

        public static void StartBlock(string line, ImportanceLevel importance = ImportanceLevel.Debug)
        {
            if (importance < MinimalImportanceLevel)
            {
                NestingLevel++;
                return;
            }

            Debug.WriteLine("┃".Repeat(++NestingLevel - 1) + $"┏{line}");
        }

        public static void FinishBlock(string line, ImportanceLevel importance = ImportanceLevel.Debug)
        {
            if (importance < MinimalImportanceLevel)
            {
                NestingLevel--;
                return;
            }

            Debug.WriteLine("┃".Repeat(NestingLevel-- - 1) + $"┗{line}");
        }

        public class MinimalImportanceScope : IDisposable
        {
            private ImportanceLevel _previousMinimalImportanceLevel;
            public MinimalImportanceScope(ImportanceLevel importance)
            {
                _previousMinimalImportanceLevel = MinimalImportanceLevel;
                MinimalImportanceLevel = importance;
            }

            public void Dispose()
            {
                MinimalImportanceLevel = _previousMinimalImportanceLevel;
            }
        }
    }
}
