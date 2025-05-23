using Serilog;
using System;
using System.IO;
using System.Reflection;

namespace FruitSA.API.Helper
{
    public static class SerilogLogger
    {
        public static void Configure()
        {
            var projectName = Assembly.GetExecutingAssembly().GetName().Name;
            var dateStamp = DateTime.Now.ToString("yyyy-MM-dd");

            var logsDir = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            Directory.CreateDirectory(logsDir);
            var logFilePath = Path.Combine(logsDir, $"{projectName}_{dateStamp}.log");


            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .Enrich.FromLogContext()
               .Enrich.WithCorrelationId()
               .WriteTo.File(
                   path: logFilePath,
                   rollingInterval: RollingInterval.Infinite,
                   shared: true,
                   outputTemplate: "[{Timestamp:yyyy-MM-dd} {Level:u3}] {Message:lj}{NewLine}{Exception}"
               ).CreateLogger();
        }
    }
}
