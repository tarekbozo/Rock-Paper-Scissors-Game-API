using Serilog;

namespace RockPaperScissorsGame.Infrastructure.Logging
{
    public static class Logger
{
        public static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() 
                .WriteTo.Console() 
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day) // Write logs to a file
                .CreateLogger();
        }

        public static void LogInformation(string message, params object[] args)
    {
        Log.Information(message, args);
    }

    public static void LogError(string message, params object[] args)
    {
        Log.Error(message, args);
    }
}

}
