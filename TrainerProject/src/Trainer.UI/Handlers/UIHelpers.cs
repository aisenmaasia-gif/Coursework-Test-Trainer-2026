namespace Trainer.UI.Handlers;

public static class UIHelpers
{
    public static void PrintColored(string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static string Prompt(string message)
    {
        Console.Write($"{message}: ");
        return Console.ReadLine()?.Trim() ?? "";
    }

    public static void Wait()
    {
        Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
        Console.ReadKey();
    }
}