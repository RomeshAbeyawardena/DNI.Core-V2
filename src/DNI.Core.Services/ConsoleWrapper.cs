using DNI.Core.Contracts;
using DNI.Core.Contracts.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Core.Services
{
    public class ConsoleWrapper<TCategory> : ConsoleWrapper, IConsoleWrapper<TCategory>
    {
        public ConsoleWrapper(ILoggerCacheFactory loggerFactory)
            : base(loggerFactory)
        {
            
        }

        public Task WriteAsync(string format, params object[] args)
        {
            return WriteAsync<TCategory>(format, args);
        }

        public void WriteLine(string format, bool logToConsole = false, LogLevel logLevel = LogLevel.Trace, params object[] args)
        {
            WriteLine<TCategory>(format, logToConsole, logLevel, args);
        }

        public Task WriteLineAsync(string format, bool logToConsole = false, LogLevel logLevel = LogLevel.Trace, params object[] args)
        {
            return WriteLineAsync<TCategory>(format, logToConsole, logLevel, args);
        }
    }

    public class ConsoleWrapper : IConsoleWrapper
    {
        public ConsoleWrapper(ILoggerCacheFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
            writingPoolStringBuilder = new StringBuilder();
        }

        public void Write(string format, params object[] args)
        {
            writingPoolStringBuilder.AppendFormat(format, args);
            Console.Write(format, args);
        }

        public Task WriteAsync<TCategory>(string format, params object[] args)
        {
            Write(format, args);
            return Task.CompletedTask;
        }

        public void WriteLine<TCategory>(string format, bool logToConsole = false, LogLevel logLevel = LogLevel.Information, params object[] args)
        {
            Console.WriteLine(format, args);
            var logger = GetLogger<TCategory>();
            if(logToConsole)
            {
                logger.Log(logLevel, writingPoolStringBuilder.ToString());
                writingPoolStringBuilder.Clear();
                logger.Log(logLevel, format, args);
            }
            
        }

        public Task WriteLineAsync<TCategory>(string format, bool logToConsole = false, LogLevel logLevel = LogLevel.Information, params object[] args)
        {
            WriteLine<TCategory>(format, logToConsole, logLevel, args);
            return Task.CompletedTask;
        }
        
        public Task<string> ReadSecureStringAsync(bool interceptKeyPresses)
        {
            var stringBuilder = new StringBuilder();
            ConsoleKeyInfo currentKey;

            while((currentKey = Console.ReadKey(interceptKeyPresses)).Key != ConsoleKey.Enter)
            {
                if(currentKey.Key == ConsoleKey.Backspace)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                }
                stringBuilder.Append(currentKey.KeyChar);
            }

            return Task.FromResult(stringBuilder.ToString());
        }

        private ILogger<TCategory> GetLogger<TCategory>()
        {
            return loggerFactory.GetOrCreateLogger<TCategory>();
        }

        private readonly StringBuilder writingPoolStringBuilder;
        private readonly ILoggerCacheFactory loggerFactory;
    }
}
