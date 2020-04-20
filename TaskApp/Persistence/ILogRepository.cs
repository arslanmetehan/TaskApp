using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApp.Enums;

namespace TaskApp.Persistence
{
    public interface ILogRepository
    {
        //void Log(LogType type, string message);
        void Log(LogType ınfo, string v);
    }
}
