using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Contracts
{
    public interface IConsoleWrapper<TCategory> : IConsoleWrapper
    {
        void WriteLine(string format, bool logToConsole = false, LogLevel logLevel = default, params object[] args);

        Task WriteAsync(string format, params object[] args);
        Task WriteLineAsync(string format, bool logToConsole = false, LogLevel logLevel = default, params object[] args);
    }

    public interface IConsoleWrapper
    {
        void Write(string format, params object[] args);
        void WriteLine<TCategory>(string format, bool logToConsole = false, LogLevel logLevel = default, params object[] args);

        Task WriteAsync<TCategory>(string format, params object[] args);
        Task WriteLineAsync<TCategory>(string format, bool logToConsole = false, LogLevel logLevel = default, params object[] args);
    }
}
