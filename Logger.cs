public static class Logger
{
    public static void LogMessage(string message)
    {
        Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] INFO - {message}");
    }

    public static void LogError(string message)
    {
        Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] ERROR - {message}");
    }
}