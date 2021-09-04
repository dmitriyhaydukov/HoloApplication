using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class LogMessage : Logger.LogMessageBase
    {
    }

    public class TaskProgressMessage : Logger.LogMessageBase
    {
        public string TaskTitle { get; set; }
        // TODO: Добавить проверки при присвоении
        public float Progress { get; set; }
        public float ProgressMinimum { get; set; }
        public float ProgressMaximum { get; set; }

        public float NormalizedProgress => Progress / (ProgressMaximum - ProgressMinimum);

        public override Logger.LogType Type
        {
            get => Logger.LogType.Progress;
            set => throw new InvalidOperationException($"Нельзя задать свойство {nameof(Type)} для {nameof(TaskProgressMessage)}.");
        }
    }
}