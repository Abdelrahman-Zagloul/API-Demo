
namespace API_Demo.Logger
{
    public class Logging : ILogging
    {
        public void Log(LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Debug:
                    PrintMessage(LogLevel.Debug, message, ConsoleColor.DarkBlue);
                    break;
                case LogLevel.Error:
                    PrintMessage(LogLevel.Error, message, ConsoleColor.Red);
                    break;
                case LogLevel.Information:
                    PrintMessage(LogLevel.Information, message, ConsoleColor.Green);
                    break;
                case LogLevel.Warning:
                    PrintMessage(LogLevel.Warning, message, ConsoleColor.DarkRed);
                    break;
                case LogLevel.Critical:
                    Console.BackgroundColor = ConsoleColor.Red;
                    PrintMessage(LogLevel.Critical, message, ConsoleColor.DarkBlue);
                    break;
            }
        }

        private void PrintMessage(LogLevel logLevel, string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"[{logLevel}] At: {DateTime.Now} => {message}");
            Console.ResetColor();
        }
    }
}
